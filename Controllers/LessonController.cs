using Marmara.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Marmara.Controllers
{
    [Authorize(Roles = "User")]
    public class LessonController : Controller
    {
        private MarmaraEntities db = new MarmaraEntities();


        // GET: Lessons
        //public ActionResult Live(int? Id)
        //{
        //    if (Id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    User user = db.Users.Find(Id);

        //    if (user != null)
        //    {
        //        if (!isUserLogged(user.Id))
        //        {
        //            return RedirectToAction("login", "home");
        //        }
        //    }
        //    else
        //    {
        //        return HttpNotFound();
        //    }

        //    if (user.OnlineLessonPermission == null || user.OnlineLessonPermission == "")
        //    {
        //        return RedirectToAction("PermissionError");
        //    }
        //    DateTime PermissionTime = Convert.ToDateTime(user.OnlineLessonPermission);
        //    TimeSpan Now_Subtract_Permission = DateTime.Now.Subtract(PermissionTime);
        //    int RemainingTime = Convert.ToInt32(Now_Subtract_Permission.TotalSeconds);
        //    if (RemainingTime <= 0)
        //    {
        //        RedirectToAction("PermissionError");
        //    }

        //    tblLesson liveLesson = db.tblLessons.Find(1);
        //    if (liveLesson.IsLiveStreamActive == false)
        //    {
        //        return RedirectToAction("Error");
        //    }
        //    List<tblMessage> messages = db.tblMessages.ToList();
        //    ViewBag.NameSurname = user.Surname + " " + user.Name;

        //    return View(messages);
        //}

        public ActionResult Error()
        {
            return View();
        }

        public ActionResult PermissionError()
        {
            return View();
        }

        //public ActionResult Video(int? Id)
        //{
        //    if (Id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    User user = db.Users.Find(Id);

        //    if (user != null)
        //    {
        //        if (!isUserLogged(user.Id))
        //        {
        //            return RedirectToAction("login", "home");
        //        }
        //    }
        //    else
        //    {
        //        return HttpNotFound();
        //    }

        //    if (user.OnlineLessonPermission == null || user.OnlineLessonPermission == "")
        //    {
        //        return RedirectToAction("PermissionError");
        //    }
        //    DateTime PermissionTime = Convert.ToDateTime(user.OnlineLessonPermission);
        //    TimeSpan Now_Subtract_Permission = DateTime.Now.Subtract(PermissionTime);
        //    int RemainingTime = Convert.ToInt32(Now_Subtract_Permission.TotalSeconds);
        //    if (RemainingTime <= 0)
        //    {
        //        RedirectToAction("PermissionError");
        //    }


        //    return View();
        //}

        [HttpPost]
        public ActionResult CreateMsg(string name, string message, string msgtime)
        {
            tblMessage msg = new tblMessage();
            msg.NameSurname = name;
            msg.Message = message;
            msg.MessageTime = msgtime;
            db.tblMessages.Add(msg);
            db.SaveChanges();

            return Content(message);
        }
    }
}