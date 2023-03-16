using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Marmara.Models;

namespace Marmara.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class QuestionsController : Controller
    {
        private MarmaraEntities db = new MarmaraEntities();

        // GET: Admin/Questions
        public ActionResult Index(int Id)
        {
            var tblQuestions = db.tblQuestions.Where(q=>q.ExamId == Id).Include(t => t.tblExam);
            ViewBag.ExamName = db.tblExams.Find(Id).Name;
            ViewBag.ExamId = Id;
            return View(tblQuestions.ToList());
        }

        // GET: Admin/sadfasd/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblQuestion tblQuestion = db.tblQuestions.Find(id);
            if (tblQuestion == null)
            {
                return HttpNotFound();
            }
            return View(tblQuestion);
        }

        // POST: Admin/sadfasd/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            
            tblQuestion tblQuestion = db.tblQuestions.Find(id);
            int? ExamId = tblQuestion.ExamId;
            db.tblQuestions.Remove(tblQuestion);
            db.SaveChanges();
            return RedirectToAction("index", new { Id = ExamId });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public JsonResult addQuestion(tblQuestion data)
        {
            tblQuestion questions = new tblQuestion();
            if (data != null)
            {
                questions.Description = data.Description;
                questions.ExamId = data.ExamId;
                questions.Row = data.Row;
                questions.TrueAnswer = data.TrueAnswer;
                db.tblQuestions.Add(questions);
                db.SaveChanges();
            }

            return Json(questions, JsonRequestBehavior.AllowGet);
        }
    }
}
