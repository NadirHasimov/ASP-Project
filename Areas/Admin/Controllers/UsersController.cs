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
    public class UsersController : Controller
    {
        private MarmaraEntities db = new MarmaraEntities();

        // GET: Admin/Users
        public ActionResult Index()
        {
            var users = db.Users.Where(u=>u.Role == "User" && u.IsAdmin != true).ToList();
            return View(users);
        }

       
        // GET: Admin/Users/Create
        public ActionResult Create()
        {
            ViewBag.ImageId = new SelectList(db.tblImages, "Id", "Path");
            return View();
        }

        // POST: Admin/Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Surname,Email,Password,Phone,BirthDate,IsMale,Status,IsCourseStudent,Address,Class")] User user, string NewImagePath)
        {
            if (ModelState.IsValid)
            {
                if (NewImagePath != null)
                {
                    tblImage img = db.tblImages.FirstOrDefault(i => i.Path == NewImagePath);
                    user.ImageId = img.Id;
                    db.SaveChanges();
                }
                user.Password = System.Web.Helpers.Crypto.HashPassword(user.Password);
                user.IsAdmin = false;
                user.Role = "User";
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ImageId = new SelectList(db.tblImages, "Id", "Path", user.ImageId);
            return View(user);
        }

        // GET: Admin/Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.ImageId = new SelectList(db.tblImages, "Id", "Path", user.ImageId);
            tblImage image = db.tblImages.FirstOrDefault(i => i.Id == user.ImageId);
            ViewBag.Image = image;
            return View(user);
        }

        // POST: Admin/Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Surname,Email,Phone,BirthDate,IsMale,Status,IsCourseStudent,Address,Class")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.Entry(user).Property(x => x.Password).IsModified = false;
                db.Entry(user).Property(x => x.ImageId).IsModified = false;
                db.Entry(user).Property(x => x.IsAdmin).IsModified = false;
                db.Entry(user).Property(x => x.Role).IsModified = false;

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ImageId = new SelectList(db.tblImages, "Id", "Path", user.ImageId);
            return View(user);
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
