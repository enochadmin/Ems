using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using EMPMGMT.Models;

namespace EMPMGMT.Controllers
{
    public class DashboardController : Controller
    {
        EMPMGMTEntities _entity = new EMPMGMTEntities();
        // GET: Dashboard
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Admin()
        {
            int? empId = (int?)Session["id"];
            int TotalEmployees = _entity.Employees.Count();
            ViewBag.TotalEmployees = TotalEmployees;
            return View();
        }
        public ActionResult HR()
        {
            int? empId = (int?)Session["id"];
            int TotalLeave = _entity.Leaves.Count();
            ViewBag.TotalEmployees = TotalLeave;

            int NewRequestedLeave = _entity.Leaves.Where(l => l.Status == 1).Count();
            ViewBag.NewRequestedLeave = NewRequestedLeave;


            return View();
        }
        public ActionResult Employee()
        {

            int? empId = (int?)Session["id"];
            int caseAssignments = _entity.CaseAssignments.Where(c => c.AssignedTo == empId && c.CustomerCas.CompletionStatus == 1).Count();
            ViewBag.pendingTasks = caseAssignments;
            
            int completedTasks=_entity.CaseAssignments.Where(c => c.AssignedTo == empId && c.CustomerCas.CompletionStatus == 2).Count();
            ViewBag.completedTasks = completedTasks;
            return View();
        }
        public ActionResult Head()
        {
            return View();
        }
        public ActionResult CustomerService()
        {
            int? empId = (int?)Session["id"];
            int TotalCustomers = _entity.Customers.Count();
            ViewBag.TotalCustomers = TotalCustomers;

            int CustomerCases = _entity.CustomerCases.Where(c=> c.CompletionStatus == 0).Count();
            ViewBag.CustomerCases = CustomerCases;
            return View();
        }
        

    }
}