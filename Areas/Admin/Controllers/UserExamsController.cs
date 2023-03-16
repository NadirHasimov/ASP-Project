using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Marmara.Models;

namespace Marmara.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserExamsController : Controller
    {
        private MarmaraEntities db = new MarmaraEntities();

        // GET: Admin/UserExams
        public ActionResult Index()
        {
            var tblUserExams = db.tblUserExams.Include(t => t.tblExam).Include(t => t.tblImage).Include(t => t.User).OrderByDescending(t=>t.Id);
            return View(tblUserExams.ToList());
        }


        //// GET: Admin/UserExams/Create
        //public ActionResult Create()
        //{
        //    ViewBag.ExamId = new SelectList(db.tblExams, "Id", "Name");
        //    ViewBag.ProofImageId = new SelectList(db.tblImages, "Id", "Path");
        //    ViewBag.UserId = new SelectList(db.Users, "Id", "Name");
        //    return View();
        //}

        //// POST: Admin/UserExams/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "Id,UserId,ExamId,ProofImageId,IsExamStarted,AttemptDate,IsExamEnd,PaymentDate,IsSertificateHas,IsPaid,Attempts,EndScore,TrueAnswerCount,FalseAnswerCount,EmptyAnswerCount")] tblUserExam tblUserExam)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.tblUserExams.Add(tblUserExam);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.ExamId = new SelectList(db.tblExams, "Id", "Name", tblUserExam.ExamId);
        //    ViewBag.ProofImageId = new SelectList(db.tblImages, "Id", "Path", tblUserExam.ProofImageId);
        //    ViewBag.UserId = new SelectList(db.Users, "Id", "Name", tblUserExam.UserId);
        //    return View(tblUserExam);
        //}

        // GET: Admin/UserExams/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblUserExam tblUserExam = db.tblUserExams.Find(id);
            if (tblUserExam == null)
            {
                return HttpNotFound();
            }
            ViewBag.ExamId = new SelectList(db.tblExams, "Id", "Name", tblUserExam.ExamId);
            ViewBag.ProofImageId = new SelectList(db.tblImages, "Id", "Path", tblUserExam.ProofImageId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Name", tblUserExam.UserId);
            return View(tblUserExam);
        }

        // POST: Admin/UserExams/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,IsPaid")] tblUserExam tblUserExam)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblUserExam).State = EntityState.Modified;
                db.Entry(tblUserExam).Property(x => x.UserId).IsModified = false;
                db.Entry(tblUserExam).Property(x => x.ExamId).IsModified = false;
                db.Entry(tblUserExam).Property(x => x.ProofImageId).IsModified = false;
                db.Entry(tblUserExam).Property(x => x.IsExamStarted).IsModified = false;
                db.Entry(tblUserExam).Property(x => x.AttemptDate).IsModified = false;
                db.Entry(tblUserExam).Property(x => x.IsExamEnd).IsModified = false;
                db.Entry(tblUserExam).Property(x => x.PaymentDate).IsModified = false;
                db.Entry(tblUserExam).Property(x => x.IsSertificateHas).IsModified = false;
                db.Entry(tblUserExam).Property(x => x.Attempts).IsModified = false;
                db.Entry(tblUserExam).Property(x => x.EndScore).IsModified = false;
                db.Entry(tblUserExam).Property(x => x.TrueAnswerCount).IsModified = false;
                db.Entry(tblUserExam).Property(x => x.FalseAnswerCount).IsModified = false;
                db.Entry(tblUserExam).Property(x => x.EmptyAnswerCount).IsModified = false;
                db.Entry(tblUserExam).Property(x => x.Attempts).IsModified = false;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ExamId = new SelectList(db.tblExams, "Id", "Name", tblUserExam.ExamId);
            ViewBag.ProofImageId = new SelectList(db.tblImages, "Id", "Path", tblUserExam.ProofImageId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Name", tblUserExam.UserId);
            return View(tblUserExam);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
