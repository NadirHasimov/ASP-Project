using Marmara.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Marmara.Controllers
{
    [Authorize(Roles ="User")]
    public class ProfileController : Controller
    {
        readonly MarmaraEntities db = new MarmaraEntities();

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

        // GET: Profile
        public ActionResult Index(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
          
            if (user != null)
            {
                if (!isUserLogged(user.Id))
                {
                    return RedirectToAction("login", "home");
                }
            }
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index([Bind(Include = "Id,Name,Surname,Email,ImageId,Password,Phone,BirthDate,IsMale,Address,Class")] User user, HttpPostedFileBase imageFile)
        {
            if (!isUserLogged(user.Id))
            {
                return RedirectToAction("login", "home");
            }
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.Entry(user).Property(x => x.ImageId).IsModified = false;
                db.Entry(user).Property(x => x.IsAdmin).IsModified = false;
                db.Entry(user).Property(x => x.Status).IsModified = false;
                db.Entry(user).Property(x => x.Token).IsModified = false;
                db.Entry(user).Property(x => x.Password).IsModified = false;
                //db.Entry(user).Property(x => x.IsCourseStudent).IsModified = false;
                db.Entry(user).Property(x => x.Role).IsModified = false;
                if (user.Password != null)
                {
                    user.Password = System.Web.Helpers.Crypto.HashPassword(user.Password);
                    Logout();
                }
                if (imageFile != null)
                {
                    string pic = System.IO.Path.GetFileName(imageFile.FileName);
                    string path = System.IO.Path.Combine(
                                           Server.MapPath("~/Uploads"), pic);
                    // file is uploaded
                    imageFile.SaveAs(path);
                }
                if (imageFile != null && imageFile.ContentLength > 0)
                    try
                    {
                        if (user.ImageId != null)
                        {
                            tblImage oldImg = db.tblImages.Find(user.ImageId);
                            string fullPath = Request.MapPath("~/Uploads/" + oldImg.Path);
                            if (System.IO.File.Exists(fullPath))
                            {
                                System.IO.File.Delete(fullPath);
                            }
                            oldImg.Path = imageFile.FileName;
                        }
                        else
                        {
                            tblImage newimg = new tblImage();
                            newimg.Path = imageFile.FileName;
                            db.tblImages.Add(newimg);
                            db.SaveChanges();
                            user.ImageId = newimg.Id;
                        }
                        
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Message = "ERROR:" + ex.Message.ToString();
                    }

                db.SaveChanges();
                Session["Success"] = "Məlumatlar yeniləndi!";
                return RedirectToAction("Index");
            }
            ViewBag.ImageId = new SelectList(db.tblImages, "Id", "Path", user.ImageId);
            return View(user.Id);
        }

        public ActionResult Logout()
        {
            Session["Login"] = null;
            Session["User"] = null;
            System.Web.Security.FormsAuthentication.SignOut();
            return RedirectToAction("index", "home");
        }

        public ActionResult MyExams(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            User user = db.Users.Find(id);
            if (user != null)
            {
                if (!isUserLogged(user.Id))
                {
                    return RedirectToAction("login", "home");
                }
            }
            else
            {
                return HttpNotFound();
            }
            ViewBag.User = user;

            var UserExams = db.tblUserExams.Where(t => t.UserId == id).Include(t => t.tblExam).Include(t => t.tblImage).Include(t => t.User).OrderByDescending(t=>t.Id).ToList();
            if (UserExams == null)
            {
                return HttpNotFound();
            }

            foreach (var item in UserExams)
            {
                if (item.IsExamStarted == true)
                {
                    DateTime examStartTime = item.AttemptDate.Value;
                    DateTime examEndTime = examStartTime.AddSeconds(Convert.ToDouble(item.tblExam.ExamTimeLimit));
                    TimeSpan Now_Subtract_EndTime = DateTime.Now.Subtract(examEndTime);
                    int RemainingTime = Convert.ToInt32(Now_Subtract_EndTime.TotalSeconds);
                    if (RemainingTime >= 0)
                    {
                    if(item.IsExamStarted == false || item.IsExamStarted == null || item.IsExamEnd == false || item.IsExamEnd == null)
                        {
                            item.IsExamStarted = true;
                            item.IsExamEnd = true;
                            item.EndScore = 0;
                            item.FalseAnswerCount = 0;
                            item.TrueAnswerCount = 0;
                            item.IsSertificateHas = false;
                            db.SaveChanges();
                        }
                    }
                }
            }
            return View(UserExams);
        }

        public ActionResult Sertificates(int? Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(Id);

            if (user != null)
            {
                if (!isUserLogged(user.Id))
                {
                    return RedirectToAction("login", "home");
                }
            }

            var sertificates = db.tblUserExams.Where(
                s => s.IsExamStarted == true &&
                s.IsExamEnd == true && 
                s.IsSertificateHas == true && 
                s.UserId == user.Id
                ).OrderByDescending(t => t.Id);
            tblImage image = db.tblImages.FirstOrDefault(i => i.Id == user.ImageId);
            ViewBag.Image = image;
            ViewBag.User = user;
            return View(sertificates.ToList());
        }

        public ActionResult MyReceipts(int? Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(Id);

            if (user != null)
            {
                if (!isUserLogged(user.Id))
                {
                    return RedirectToAction("login", "home");
                }
            }

            var userExam = db.tblUserExams.Where(u=>u.UserId == user.Id).Include(u=>u.tblExam).Include(u => u.tblImage).OrderByDescending(t => t.Id);
            if (userExam == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.User = user;
            return View(userExam.ToList());
        }

        public ActionResult Schedule(int? Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(Id);

            if (user != null)
            {
                if (!isUserLogged(user.Id))
                {
                    return RedirectToAction("login", "home");
                }
            }

            var userExam = db.tblUserExams.Where(u => u.UserId == user.Id).Include(u => u.tblExam).Include(u => u.tblImage).OrderByDescending(t => t.Id);
            if (userExam == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblImage image = db.tblImages.FirstOrDefault(i => i.Id == user.ImageId);
            ViewBag.Image = image;
            ViewBag.User = user;
            return View(userExam.ToList());
        }
    }
}