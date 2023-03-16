using Marmara.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Marmara.Controllers
{
    public class CertificateController : Controller
    {
        private MarmaraEntities db = new MarmaraEntities();

        [AllowAnonymous]
        public ActionResult Single(int? Id)
        {
            var userExamId = Id;
            if (userExamId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblUserExam userExam = db.tblUserExams.Find(userExamId);
            if (userExam == null || userExam.IsSertificateHas != true || userExam.IsPaid != true || userExam.IsExamStarted != true || userExam.IsExamEnd != true)
            {
                return HttpNotFound();
            }
            return View(userExam);
        }
    }
}