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
    public class CustomerCasController : Controller
    {
        private EMPMGMTEntities db = new EMPMGMTEntities();

        // GET: CustomerCas
        public ActionResult Index()
        {
            var customerCases = db.CustomerCases.Include(c => c.Customer).Include(c => c.Service);
            return View(customerCases.ToList());
        }
        public ActionResult CaseCompletionApproval()
        {
            var customerCases = db.CaseAssignments.Include(c => c.CustomerCas).Include(c => c.Employee).Where(c=>c.CustomerCas.CompletionStatus==1);
            return View(customerCases.ToList());
        }
        public ActionResult ApproveCase(int? id)
        {
            CustomerCas customerCase = db.CustomerCases.Where(c => c.Id == id).FirstOrDefault();
            customerCase.CompletionStatus = 2;//case completed
            db.Entry(customerCase).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("CaseCompletionApproval", "CustomerCas");
          
        }
        // GET: CustomerCas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerCas customerCas = db.CustomerCases.Find(id);
            if (customerCas == null)
            {
                return HttpNotFound();
            }
            return View(customerCas);
        }

        // GET: CustomerCas/Create
        public ActionResult Create()
        {
            var customer = db.Customers.Select(s => new
            {
                Text = s.FName + " " + s.LName + " (" + s.Phone + ")",
                Value = s.Id
            }
                ).ToList();
            ViewBag.CustomerId = new SelectList(customer, "Value", "Text");
            ViewBag.ServiceId = new SelectList(db.Services, "Id", "Name");
            return View();
        }

        // POST: CustomerCas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CustomerId,ServiceId,RequestDate,OrderNumber,CompletionStatus,CreatedBy,Remark")] CustomerCas customerCas)
        {
            if (ModelState.IsValid)
            {
                customerCas.CreatedBy = (int?)Session["id"];
                customerCas.CompletionStatus = 0;// if completion status=0  case not assigned , if status =1 case assigned(pending) if status=2 case completed

                db.CustomerCases.Add(customerCas);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "FName", customerCas.CustomerId);
            ViewBag.ServiceId = new SelectList(db.Services, "Id", "Name", customerCas.ServiceId);
            return View(customerCas);
        }

        // GET: CustomerCas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerCas customerCas = db.CustomerCases.Find(id);
            if (customerCas == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "FName", customerCas.CustomerId);
            ViewBag.ServiceId = new SelectList(db.Services, "Id", "Name", customerCas.ServiceId);
            return View(customerCas);
        }

        // POST: CustomerCas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CustomerId,ServiceId,RequestDate,OrderNumber,CompletionStatus,CreatedBy,Remark")] CustomerCas customerCas)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customerCas).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "FName", customerCas.CustomerId);
            ViewBag.ServiceId = new SelectList(db.Services, "Id", "Name", customerCas.ServiceId);
            return View(customerCas);
        }

        // GET: CustomerCas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerCas customerCas = db.CustomerCases.Find(id);
            if (customerCas == null)
            {
                return HttpNotFound();
            }
            return View(customerCas);
        }

        // POST: CustomerCas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CustomerCas customerCas = db.CustomerCases.Find(id);
            db.CustomerCases.Remove(customerCas);
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
