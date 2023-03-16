using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;
using Marmara.Models;

namespace Marmara.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private MarmaraEntities db = new MarmaraEntities();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(User user)
        {

            if (user.Email == null || user.Password == null)
            {
                Session["loginError"] = "Zəhmət olmasa tələb olunan sahələri doldurun!";
                return RedirectToAction("login", "home", new { Area = string.Empty });
            }

            User loginned = db.Users.FirstOrDefault(u => u.Email == user.Email);
            if (loginned != null)
            {
                if (System.Web.Helpers.Crypto.VerifyHashedPassword(loginned.Password, user.Password))
                {
                    FormsAuthentication.SetAuthCookie(loginned.Id.ToString(), false);
                    Session["UserId"] = loginned.Id;
                    Session["User"] = loginned;
                    Session["Login"] = true;

                    if (loginned.IsAdmin == true && loginned.Status == true)
                    {
                        return RedirectToAction("index", "dashboard", new { Area = "Admin" });
                    }
                    else
                    {
                        return RedirectToAction("index", "profile", new { Area = string.Empty, loginned.Id });
                    }

                }
            }

            Session["loginError"] = "Yanlış e-poçt və ya şifrə daxil etmisiniz. Zəhmət olmasa bir daha cəhd edin.";
            return RedirectToAction("login", "home", new { Area = string.Empty });
        }

        public ActionResult Logout()
        {
            Session["Login"] = null;
            Session["User"] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("index", "home");
        }

        public ActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registration([Bind(Include = "Id,Name,Surname,Email,Password,Phone,BirthDate,IsMale,Address,Class")] User user)
        {
            if (ModelState.IsValid)
            {
                user.Password = System.Web.Helpers.Crypto.HashPassword(user.Password);
                user.IsAdmin = false;
                user.Status = true;
                user.Role = "User";
                db.Users.Add(user);
                db.SaveChanges();

                FormsAuthentication.SetAuthCookie(user.Id.ToString(), false);
                Session["UserId"] = user.Id;
                Session["User"] = user;
                Session["Login"] = true;

                int LastUserId = db.Users.OrderByDescending(p => p.Id).First().Id;

                return RedirectToAction("index", "profile", new { Area = string.Empty, user.Id });
            }

            return View(user);
        }

        public ActionResult ForgotPassword()
        {
            return View();
        }

        public ActionResult SentEmail(string Email)
        {
            User SelectedUser = db.Users.FirstOrDefault(e => e.Email == Email);
            SelectedUser.Token = System.Web.Helpers.Crypto.SHA1(DateTime.Now.ToLongDateString() + SelectedUser.Email);
            db.SaveChanges();
            SendMailRecover(SelectedUser);
            return View(SelectedUser);
        }

        private void SendMailRecover(User SelectedUser)
        {
            SmtpClient client = new SmtpClient()
            {
                Host = "smtp.gmail.com",
                Port = 587,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new System.Net.NetworkCredential("example@gmail.com", "pass"),
                EnableSsl = true
            };

            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("example@gmail.com");
            mailMessage.To.Add(SelectedUser.Email);
            mailMessage.Subject = "example password recover";
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = "Hi " + SelectedUser.Name + " " + SelectedUser.Surname + " ! To change your password please click link below. <br/><br/> <a href = '" + Request.Url.GetLeftPart(UriPartial.Authority) + "/home/changepassword?token/" + SelectedUser.Token + "'>Click on the link!</a>";

            client.Send(mailMessage);

        }

        public ActionResult ChangePassword(string token)
        {
            User user = db.Users.FirstOrDefault(u => u.Token == token);
            if (user == null)
            {
                return HttpNotFound();
            }

            return View(user);
        }

        [HttpPost]
        public ActionResult ChangePassword(string Password, string PasswordConfirm, string url)
        {
            User SelectedUser = db.Users.FirstOrDefault(e => e.Token == url);
            PasswordConfirm = null;
            url = null;
            SelectedUser.Password = Crypto.HashPassword(Password);
            SelectedUser.Token = null;
            db.Entry(SelectedUser).State = System.Data.Entity.EntityState.Modified;
            db.Entry(SelectedUser).Property(u => u.Email).IsModified = false;
            db.Entry(SelectedUser).Property(u => u.Phone).IsModified = false;
            db.Entry(SelectedUser).Property(u => u.Name).IsModified = false;
            db.Entry(SelectedUser).Property(u => u.Surname).IsModified = false;
            db.Entry(SelectedUser).Property(u => u.IsAdmin).IsModified = false;
            db.SaveChanges();

            Session["Login"] = true;
            Session["User"] = SelectedUser;
            if (SelectedUser.IsAdmin == true)
            {
                return RedirectToAction("index", "dashboard", new { Area = "Admin" });
            }
            else
            {
                return RedirectToAction("index", "profile", new { Area = string.Empty, id = SelectedUser.Id });
            }
        }

        [HttpPost]
        public ActionResult ChangePasswordProfile(string Password, string PasswordConfirm, int Id)
        {
            User user = db.Users.Find(Id);
            PasswordConfirm = null;
            user.Password = Crypto.HashPassword(Password);
            user.Token = null;
            db.Entry(user).State = System.Data.Entity.EntityState.Modified;
            db.Entry(user).Property(u => u.Email).IsModified = false;
            db.Entry(user).Property(u => u.Phone).IsModified = false;
            db.Entry(user).Property(u => u.Name).IsModified = false;
            db.Entry(user).Property(u => u.Surname).IsModified = false;
            db.Entry(user).Property(u => u.IsAdmin).IsModified = false;
            db.SaveChanges();

            Session["Login"] = null;
            Session["User"] = null;
            return RedirectToAction("index", "profile", new { Area = string.Empty, id = user.Id });
        }



        [HttpPost]
        public JsonResult CheckExisting(string Email)
        {
            if (db.Users.FirstOrDefault(u => u.Email == Email) != null)
            {
                return Json(new
                {
                    valid = true
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                valid = false,
                message = "Bu e-poçt mövcud deyil!"
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Check(string Email)
        {
            if (db.Users.FirstOrDefault(u => u.Email == Email) == null)
            {
                return Json(new
                {
                    valid = true
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                valid = false,
                message = "Bu e-poçt artıq istifadə edilmişdir!"
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ProfileCheck(string Email, int Id)
        {
            User existedEmail = db.Users.FirstOrDefault(e => e.Id == Id);
            if (db.Users.FirstOrDefault(u => u.Email == Email) == null || existedEmail.Email == Email)
            {
                return Json(new
                {
                    valid = true
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                valid = false,
                message = "Bu e-poçt artıq istifadə edilmişdir!"
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ModalSession()
        {
            Session["Agreement"] = "true";

            return Json(Session["Agreement"], JsonRequestBehavior.AllowGet);
        }

    }
}