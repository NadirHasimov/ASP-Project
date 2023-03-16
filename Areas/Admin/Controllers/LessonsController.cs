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
    [Authorize(Roles ="Admin")]
    public class LessonsController : Controller
    {
        private MarmaraEntities db = new MarmaraEntities();

        // GET: Admin/tblCategories
        public ActionResult Index()
        {
            return View(db.tblLessons.ToList());
        }


        // GET: Admin/tblCategories/Create
        public ActionResult Create()
        {
           
            return View();
        }

        // POST: Admin/tblCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] tblLesson tblLessons)
        {
            if (ModelState.IsValid)
            {
                tblLessons.IsLiveStreamActive = false;
                db.tblLessons.Add(tblLessons);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tblLessons);
        }

        // GET: Admin/tblCategories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblLesson tblLessons = db.tblLessons.Find(id);
            if (tblLessons == null)
            {
                return HttpNotFound();
            }
            return View(tblLessons);
        }

        // POST: Admin/tblCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] tblLesson tblLessons)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblLessons).State = EntityState.Modified;
                db.Entry(tblLessons).Property(x => x.IsLiveStreamActive).IsModified = false;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tblLessons);
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
