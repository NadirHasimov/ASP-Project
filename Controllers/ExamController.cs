using Marmara.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace Marmara.Controllers
{
    [Authorize(Roles ="User")]
    public class ExamController : Controller
    {
        private MarmaraEntities db = new MarmaraEntities();

        private bool isUserLogged(int? Id)
        {
            if (Id != Convert.ToInt32(Session["UserId"]))
            {
                Session["Login"] = null;
                Session["User"] = null;
                System.Web.Security.FormsAuthentication.SignOut();
                return (false);
            }
            else
            {
                return (true);
            }
        }

        public ActionResult All()
        {
            var exams = db.tblExams.Where(e => e.Status == true);
            if (exams == null)
            {
                return HttpNotFound();
            }
            return View(exams.ToList());
        }

        public ActionResult Create(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var exams = db.tblExams.Where(e => e.Status == true);
            User user = Session["User"] as User;

            ViewBag.Exams = exams;
            ViewBag.ExamId = id;
            ViewBag.User = user;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int? UserId, int? ExamId, HttpPostedFileBase imageFile)
        {

            if (UserId == null || ExamId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            User examUser = db.Users.Find(UserId);
            if (examUser != null)
            {
                if (!isUserLogged(examUser.Id))
                {
                    return RedirectToAction("login", "home");
                }
            }
            else
            {
                return HttpNotFound();
            }

            tblUserExam model = new tblUserExam();
            if (imageFile != null && imageFile.ContentLength != 0)
            {
                var pic = examUser.Name + "_" + examUser.Surname + "_" + DateTime.Now.ToString("ddmmyyyyMM") + "_" + System.IO.Path.GetFileName(imageFile.FileName);

                string path = System.IO.Path.Combine(
                                       Server.MapPath("~/Uploads/Payments/"), pic);
                imageFile.SaveAs(path);

                tblImage newimg = new tblImage();
                newimg.Path = pic;
                db.tblImages.Add(newimg);
                db.SaveChanges();

                model.ProofImageId = newimg.Id;
            }
           
            
            model.UserId = UserId;
            model.ExamId = ExamId;
            model.IsExamStarted = false;
            model.IsExamEnd = false;
            model.PaymentDate = DateTime.Now;
            model.IsSertificateHas = false;
            //if (examUser.IsCourseStudent == true)
            //{
            //    model.IsPaid = true;
            //}
            //else
            //{
            //    model.IsPaid = false;
            //}
            db.tblUserExams.Add(model);
            db.SaveChanges();

            //if (examUser.IsCourseStudent != true)
            //{
            //    SendExamCreateMail(examUser);
            //}

            return RedirectToAction("myexams", "profile", new { id = UserId });
        }

        private void SendExamCreateMail(User user)
        {
            System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient()
            {
                Host = "smtp.gmail.com",
                Port = 587,
                DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new System.Net.NetworkCredential("marmaraeduaz@gmail.com", "4sl4v4zg3cm3"),
                EnableSsl = true
            };

            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("marmaraeduaz@gmail.com");
            mailMessage.To.Add("marmaraeduaz@gmail.com");
            mailMessage.Subject = "Yeni müraciət formu";
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = "Salam! Yeni imtahan əlavə olunmuşdur.  <br/><br/> <hr/> " +
                "Mesajı göndərən: " + user.Name + user.Surname + "  <br/><br/>" +
                "Nömrəsi: " + user.Phone + "  <br/><br/> " +
                "Emaili: " + user.Email + "  <br/><br/> ";

            client.Send(mailMessage);

        }


        public ActionResult StartPage(int? Id)
        {
            var userExamId = Id;
            if (userExamId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblUserExam userExam = db.tblUserExams.Find(userExamId);
            if (userExam.IsPaid == false || userExam.IsExamStarted != false || userExam.IsExamEnd != false)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (!isUserLogged(userExam.UserId))
            {
                return RedirectToAction("login", "home");
            }
            tblExam exam = db.tblExams.Find(userExam.ExamId);
            ViewBag.userExamId = userExam.Id;
            return View(exam);
        }


        public FileResult Download(string ImageName)
        {
            var FileVirtualPath =  System.Web.HttpContext.Current.Server.MapPath("~/Uploads/Payments/" + ImageName);
            return File(FileVirtualPath, "application/force-download", Path.GetFileName(FileVirtualPath));
        }


        public ActionResult Quiz(int? Id)
        {
            var userExamId = Id;
            if (userExamId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblUserExam userExam = db.tblUserExams.Find(userExamId);
            if (userExam == null)
            {
                return HttpNotFound();
            }

            if (!isUserLogged(userExam.UserId))
            {
                return RedirectToAction("login", "home");
            }

            if (userExam.IsPaid == false)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (userExam.IsExamEnd == true)
            {
                return RedirectToAction("result", new { id = userExamId });
            }

            int Attempts = 0;
            if (userExam.IsExamStarted == false)
            {
                userExam.IsExamStarted = true;
                Attempts += 1;
                userExam.Attempts = Attempts;
                userExam.AttemptDate = DateTime.Now;
                userExam.IsExamEnd = false;
                db.SaveChanges();
            }

            

            ViewBag.userExamId = userExamId;
            tblExam exam = db.tblExams.Find(userExam.ExamId);

            DateTime examStartTime = userExam.AttemptDate.Value;
            DateTime examEndTime = examStartTime.AddSeconds(Convert.ToDouble(exam.ExamTimeLimit));
            TimeSpan Now_Subtract_EndTime = DateTime.Now.Subtract(examEndTime);
            int RemainingTime = Convert.ToInt32(Now_Subtract_EndTime.TotalSeconds);
            if (RemainingTime >= 0)
            {
                if (userExam.IsExamStarted != true || userExam.IsExamEnd != true)
                {
                    userExam.IsExamStarted = true;
                    userExam.IsExamEnd = true;
                    userExam.EndScore = 0;
                    userExam.FalseAnswerCount = 0;
                    userExam.TrueAnswerCount = 0;
                    userExam.IsSertificateHas = false;
                    db.SaveChanges();
                    return RedirectToAction("result", new { id = userExamId });
                }
            }
            ViewBag.RemainingTime = RemainingTime;
            ViewBag.Exam = exam;
            var questions = db.tblQuestions.Where(q => q.ExamId == exam.Id).Include(t => t.tblExam);
            return View(questions);
        }

        [HttpPost]
        public JsonResult PostFinishExam(ExamModel model)
        {
            var userExamId = model.userExamId;
            if (userExamId == null)
            {
                return Json(new HttpStatusCodeResult(HttpStatusCode.BadRequest), JsonRequestBehavior.AllowGet);
            }
            tblUserExam userExam = db.tblUserExams.Find(userExamId);
            tblExam exam = db.tblExams.Find(userExam.ExamId);
            if (userExam == null || exam == null)
            {
                return Json(false, JsonRequestBehavior.AllowGet);

            }

            if (!isUserLogged(userExam.UserId))
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }

            if (userExam.IsPaid == false || userExam.IsExamEnd == true)
            {
                return Json(false, JsonRequestBehavior.AllowGet);

            }

            List<tblQuestion> questions = db.tblQuestions.Where(q => q.ExamId == userExam.ExamId).ToList();
            int TrueAnswerCount = 0;
            int FalseAnswerCount = 0;
            string FalseAnswers = "";
            foreach (var item in questions)
            {
                if (model.QuestionAndAnswers != null)
                {
                    foreach (var item2 in model.QuestionAndAnswers)
                    {
                        string[] splitted = item2.Split('-');
                        int rowNumb = Convert.ToInt32(splitted[splitted.Length - 2]);
                        string answer = splitted[splitted.Length - 1];

                        if (item.Row == rowNumb)
                        {
                            if (item.TrueAnswer == answer)
                            {
                                TrueAnswerCount += 1;
                            }
                            else
                            {
                                FalseAnswerCount += 1;
                                userExam.FalseAnswerCount = FalseAnswerCount;

                                var allfalses = rowNumb + "-" + item.TrueAnswer + ",";
                                FalseAnswers += allfalses;
                            }
                        }
                    }
                }
            }
            //userExam.FalseAnswers = FalseAnswers;

            if (FalseAnswerCount != 0)
            {
                userExam.FalseAnswerCount = FalseAnswerCount;
            }
            else
            {
                userExam.FalseAnswerCount = 0;
            }
            if (TrueAnswerCount != 0)
            {
                userExam.TrueAnswerCount = TrueAnswerCount;
            }
            else
            {
                userExam.TrueAnswerCount = 0;

            }

            userExam.EmptyAnswerCount = questions.Count - (FalseAnswerCount + TrueAnswerCount);
            decimal scoreByQuestion = Decimal.Divide((decimal)exam.Score, questions.Count);
            userExam.IsExamEnd = true;

            if (exam.FourFalseOneTrue == true)
            {
                if (userExam.FalseAnswerCount >= 4)
                {
                    if (userExam.TrueAnswerCount == 0 || userExam.TrueAnswerCount <= (userExam.FalseAnswerCount / 4))
                    {
                        userExam.EndScore = 0;
                    }
                    else
                    {
                        userExam.EndScore = (decimal)(scoreByQuestion * (userExam.TrueAnswerCount - (userExam.FalseAnswerCount / 4)));
                    }
                }
                else
                {
                    if (TrueAnswerCount == 0)
                    {
                        userExam.EndScore = 0;
                    }
                    else
                    {
                        userExam.EndScore = (decimal)(scoreByQuestion * userExam.TrueAnswerCount);
                    }
                }
            }
            else
            {
                if (TrueAnswerCount == 0)
                {
                    userExam.EndScore = 0;
                }
                else
                {
                    userExam.EndScore = (decimal)(scoreByQuestion * userExam.TrueAnswerCount);
                }
            }


            if (userExam.EndScore >= (exam.Score / 100 * exam.SertificateScore))
            {
                userExam.IsSertificateHas = true;
            }
            else
            {
                userExam.IsSertificateHas = false;
            }

            db.SaveChanges();

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Result(int? Id)
        {
            var userExamId = Id;
            if (userExamId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblUserExam userExam = db.tblUserExams.Find(userExamId);
            if (!isUserLogged(userExam.UserId))
            {
                return RedirectToAction("login", "home");
            }
           
            tblExam exam = db.tblExams.Find(userExam.ExamId);
            ViewBag.Exam = exam;
            ViewBag.ExamPdfName = exam.PdfName;
            ViewBag.User = db.Users.Find(userExam.UserId);
            return View(userExam);
        }
    }
}