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
    public class LiveStreamController : Controller
    {
        private MarmaraEntities db = new MarmaraEntities();


        // GET: Admin/LiveStream/Index/5
        public ActionResult Index(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblLesson tblLesson = db.tblLessons.Find(id);
            if (tblLesson == null)
            {
                return HttpNotFound();
            }

            List<tblMessage> messages = db.tblMessages.ToList();
            ViewBag.Messages = messages;

            return View(tblLesson);
        }

        // POST: Admin/LiveStream/Index/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index([Bind(Include = "Id,IsLiveStreamActive")] tblLesson tblLessons)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblLessons).State = EntityState.Modified;
                db.Entry(tblLessons).Property(x => x.Name).IsModified = false;
                db.SaveChanges();
                if (tblLessons.IsLiveStreamActive == true)
                {
                    List<tblMessage> messages = db.tblMessages.ToList();
                    ViewBag.Messages = messages;
                }
                else
                {
                    db.tblMessages.RemoveRange(db.tblMessages.ToList());
                    db.SaveChanges();
                }
                
                return View(tblLessons);
            }
            return HttpNotFound();
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
