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
    public class CaseAssignmentsController : Controller
    {
        private EMPMGMTEntities db = new EMPMGMTEntities();

        // GET: CaseAssignments
        public ActionResult Index()
        {
            var caseAssignments = db.CaseAssignments.Include(c => c.CustomerCas).Include(c => c.Employee).Where(c => c.CustomerCas.CompletionStatus ==1);
            return View(caseAssignments.ToList());
        }
        public ActionResult CompletedTask()
        {
            int? empId = (int?)Session["id"];
            var caseAssignments = db.CaseAssignments.Include(c => c.CustomerCas).Include(c => c.Employee).Where(c => c.AssignedTo == empId && c.CustomerCas.CompletionStatus == 2);
            return View(caseAssignments.ToList());
        }
        public ActionResult TotalCompletedTask()
        {
            var caseAssignments = db.CaseAssignments.Include(c => c.CustomerCas).Include(c => c.Employee).Where(c => c.CustomerCas.CompletionStatus == 2);
            return View(caseAssignments.ToList());
        }
        public ActionResult AssignedTask()
        {
            int? empId = (int?)Session["id"];
            var caseAssignments = db.CaseAssignments.Include(c => c.CustomerCas).Include(c => c.Employee).Where(c=>c.AssignedTo== empId&&c.CustomerCas.CompletionStatus==1);
            return View(caseAssignments.ToList());
        }
        // GET: CaseAssignments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CaseAssignment caseAssignment = db.CaseAssignments.Find(id);
            if (caseAssignment == null)
            {
                return HttpNotFound();
            }
            return View(caseAssignment);
        }

        // GET: CaseAssignments/Create
        public ActionResult Create()
        {
            var customer = db.CustomerCases.Include(c=>c.Customer).Include(s=>s.Service).Where(c=>c.CompletionStatus==0).Select(s => new
            {
                Text = s.Customer.FName + " " + s.Customer.LName + " (" + s.Service.Name + ")",
                Value = s.Id
            }
             ).ToList();
            var employee = db.Employees.Where(e=>e.RoleId==4).Select(s => new
            {
                Text = s.FName + " " + s.LName,
                Value = s.Id
            }
        ).ToList();
            ViewBag.CustomerCaseId = new SelectList(customer, "Value", "Text");
            ViewBag.AssignedTo = new SelectList(employee, "Value", "Text");
            return View();
        }

        // POST: CaseAssignments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,AssignedBy,AssignedTo,CustomerCaseId,CompeletionDate,Remark")] CaseAssignment caseAssignment)
        {
            if (ModelState.IsValid)
            {
                caseAssignment.AssignedBy = (int?)Session["id"];
                db.CaseAssignments.Add(caseAssignment);
                db.SaveChanges();

                CustomerCas customerCase = db.CustomerCases.Where(c => c.Id == caseAssignment.CustomerCaseId).FirstOrDefault();
                customerCase.CompletionStatus = 1; //Case assigned to employee
                db.Entry(customerCase).State = EntityState.Modified;  
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CustomerCaseId = new SelectList(db.CustomerCases, "Id", "OrderNumber", caseAssignment.CustomerCaseId);
            ViewBag.AssignedTo = new SelectList(db.Employees, "Id", "FName", caseAssignment.AssignedTo);
            return View(caseAssignment);
        }

        // GET: CaseAssignments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CaseAssignment caseAssignment = db.CaseAssignments.Find(id);
            if (caseAssignment == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerCaseId = new SelectList(db.CustomerCases, "Id", "OrderNumber", caseAssignment.CustomerCaseId);
            ViewBag.AssignedTo = new SelectList(db.Employees, "Id", "FName", caseAssignment.AssignedTo);
            return View(caseAssignment);
        }

        // POST: CaseAssignments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,AssignedBy,AssignedTo,CustomerCaseId,CompeletionDate,Remark")] CaseAssignment caseAssignment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(caseAssignment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerCaseId = new SelectList(db.CustomerCases, "Id", "OrderNumber", caseAssignment.CustomerCaseId);
            ViewBag.AssignedTo = new SelectList(db.Employees, "Id", "FName", caseAssignment.AssignedTo);
            return View(caseAssignment);
        }

        // GET: CaseAssignments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CaseAssignment caseAssignment = db.CaseAssignments.Find(id);
            if (caseAssignment == null)
            {
                return HttpNotFound();
            }
            return View(caseAssignment);
        }

        // POST: CaseAssignments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CaseAssignment caseAssignment = db.CaseAssignments.Find(id);
            db.CaseAssignments.Remove(caseAssignment);
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
