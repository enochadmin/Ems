using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EMPMGMT.Models;

namespace EMPMGMT.Controllers
{
    public class LeaveReasonsController : Controller
    {
        private EMPMGMTEntities db = new EMPMGMTEntities();

        // GET: LeaveReasons
        public ActionResult Index()
        {
            return View(db.LeaveReasons.ToList());
        }

        // GET: LeaveReasons/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LeaveReason leaveReason = db.LeaveReasons.Find(id);
            if (leaveReason == null)
            {
                return HttpNotFound();
            }
            return View(leaveReason);
        }

        // GET: LeaveReasons/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LeaveReasons/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] LeaveReason leaveReason)
        {
            if (ModelState.IsValid)
            {
                db.LeaveReasons.Add(leaveReason);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(leaveReason);
        }

        // GET: LeaveReasons/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LeaveReason leaveReason = db.LeaveReasons.Find(id);
            if (leaveReason == null)
            {
                return HttpNotFound();
            }
            return View(leaveReason);
        }

        // POST: LeaveReasons/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] LeaveReason leaveReason)
        {
            if (ModelState.IsValid)
            {
                db.Entry(leaveReason).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(leaveReason);
        }

        // GET: LeaveReasons/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LeaveReason leaveReason = db.LeaveReasons.Find(id);
            if (leaveReason == null)
            {
                return HttpNotFound();
            }
            return View(leaveReason);
        }

        // POST: LeaveReasons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            LeaveReason leaveReason = db.LeaveReasons.Find(id);
            db.LeaveReasons.Remove(leaveReason);
            db.SaveChanges();
            return RedirectToAction("Index");
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
