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
    public class LeavesController : Controller
    {
        private EMPMGMTEntities db = new EMPMGMTEntities();

        // GET: Leaves
        public ActionResult Index()
        {
            var leaves = db.Leaves.Include(l => l.Employee);
            return View(leaves.ToList());
        }

        // GET: Leaves/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Leaf leaf = db.Leaves.Find(id);
            if (leaf == null)
            {
                return HttpNotFound();
            }
            return View(leaf);
        }

        // GET: Leaves/Create
        public ActionResult Create()
        {
            ViewBag.EmployeeId = new SelectList(db.Employees, "Id", "FName");
            ViewBag.LeaveReasonId = new SelectList(db.LeaveReasons, "Id", "Name");
            return View();
        }

        // POST: Leaves/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,EmployeeId,DateFrom,DateTo,LeaveReasonId,Remark,Status")] Leaf leaf)
        {
            
            if (ModelState.IsValid)
            {
                leaf.EmployeeId = (int)Session["id"];
                db.Leaves.Add(leaf);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EmployeeId = new SelectList(db.Employees, "Id", "FName", leaf.EmployeeId);
            ViewBag.LeaveReasonId = new SelectList(db.LeaveReasons, "Id", "Name", leaf.LeaveReasonId);
            return View(leaf);
        }

        // GET: Leaves/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Leaf leaf = db.Leaves.Find(id);
            if (leaf == null)
            {
                return HttpNotFound();
            }
            ViewBag.EmployeeId = new SelectList(db.Employees, "Id", "FName", leaf.EmployeeId);
            ViewBag.LeaveReasonId = new SelectList(db.LeaveReasons, "Id", "Name", leaf.LeaveReasonId);
            return View(leaf);
        }

        // POST: Leaves/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,EmployeeId,DateFrom,DateTo,LeaveReasonId,Remark,Status")] Leaf leaf)
        {
            if (ModelState.IsValid)
            {
                db.Entry(leaf).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EmployeeId = new SelectList(db.Employees, "Id", "FName", leaf.EmployeeId);
            ViewBag.LeaveReasonId = new SelectList(db.LeaveReasons, "Id", "Name", leaf.LeaveReasonId);
            return View(leaf);
        }

        // GET: Leaves/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Leaf leaf = db.Leaves.Find(id);
            if (leaf == null)
            {
                return HttpNotFound();
            }
            return View(leaf);
        }

        // POST: Leaves/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Leaf leaf = db.Leaves.Find(id);
            db.Leaves.Remove(leaf);
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
