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
    public class ExamsController : Controller
    {
        MarmaraEntities db = new MarmaraEntities();

        // GET: Admin/Exams
        public ActionResult Index()
        {
            var tblExams = db.tblExams.Include(t => t.tblLesson).Include(t => t.tblQuestions);
            return View(tblExams.ToList());
        }


        // GET: Admin/Exams/Create
        public ActionResult Create()
        {
            ViewBag.LessonId = new SelectList(db.tblLessons, "Id", "Name");
            return View();
        }

        // POST: Admin/Exams/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,LessonId,Status,StartPage,EndPage,ExamTimeLimit,Score,FourFalseOneTrue,Price,FiveAnswerOrNot,SertificateScore,Class")] tblExam tblExam, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (file != null && file.ContentLength > 0)
                {
                    string path = System.IO.Path.Combine(Server.MapPath("~/Uploads"), System.IO.Path.GetFileName(file.FileName));
                    file.SaveAs(path);
                    tblExam.PdfName = file.FileName;
                }

                db.tblExams.Add(tblExam);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.LessonId = new SelectList(db.tblLessons, "Id", "Name", tblExam.LessonId);
            return View(tblExam);
        }

        // GET: Admin/Exams/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblExam tblExam = db.tblExams.Find(id);
            if (tblExam == null)
            {
                return HttpNotFound();
            }
            ViewBag.LessonId = new SelectList(db.tblLessons, "Id", "Name", tblExam.LessonId);
            return View(tblExam);
        }

        // POST: Admin/Exams/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,LessonId,Status,StartPage,EndPage,ExamTimeLimit,Score,FourFalseOneTrue,Price,FiveAnswerOrNot,SertificateScore,Class")] tblExam tblExam, HttpPostedFileBase file)
        {

            if (ModelState.IsValid)
            {
                tblExam exam = db.tblExams.Find(tblExam.Id);
                exam.Name = tblExam.Name;
                exam.LessonId = tblExam.LessonId;
                exam.EndPage = tblExam.EndPage;
                exam.ExamTimeLimit = tblExam.ExamTimeLimit;
                exam.FiveAnswerOrNot = tblExam.FiveAnswerOrNot;
                exam.FourFalseOneTrue = tblExam.FourFalseOneTrue;
                exam.Price = tblExam.Price;
                exam.Score = tblExam.Score;
                exam.StartPage = tblExam.StartPage;
                exam.Status = tblExam.Status;
                exam.SertificateScore = tblExam.SertificateScore;
                //exam.Class = tblExam.Class;

                if (file != null && file.ContentLength > 0)
                    try
                    {
                        var oldPdf = db.tblExams.Find(tblExam.Id).PdfName;

                        if (oldPdf != null)
                        {
                            string fullPath = Request.MapPath("~/Uploads/" + oldPdf);
                            if (System.IO.File.Exists(fullPath))
                            {
                                System.IO.File.Delete(fullPath);
                            }
                        }
                        string path = System.IO.Path.Combine(Server.MapPath("~/Uploads"), System.IO.Path.GetFileName(file.FileName));
                        file.SaveAs(path);
                        exam.PdfName = file.FileName;
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Message = "ERROR:" + ex.Message.ToString();
                    }
                else
                {
                    exam.PdfName = exam.PdfName;
                }


                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LessonId = new SelectList(db.tblLessons, "Id", "Name", tblExam.LessonId);
            return View(tblExam);
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
