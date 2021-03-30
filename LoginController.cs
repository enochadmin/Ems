using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EMPMGMT.Models;

namespace EMPMGMT.Controllers
{
    public class LoginController : Controller
    {
        EMPMGMTEntities _entity = new EMPMGMTEntities();
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
      
        public ActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ChangePass(string PrevPass, string NewPass,string ConfirmPass)
        {
            int? empId = (int)Session["id"];
            var isPrevPassMatch = _entity.Accounts.Where(a => a.EmployeeId == empId && a.Password == PrevPass).FirstOrDefault();
            if (isPrevPassMatch != null)
            {
                if (NewPass.Equals(ConfirmPass))
                {
                    Account acc = _entity.Accounts.Where(a => a.Id == isPrevPassMatch.Id).FirstOrDefault();
                    acc.Password = NewPass;
                    _entity.Entry(acc).State = EntityState.Modified;
                    _entity.SaveChanges();
                }
            }
            return RedirectToAction("Index", "Login");
        }
        [HttpPost]
        public ActionResult Index(Account accounts)
        {
            var user = _entity.Accounts.Where(a => a.UserName == accounts.UserName && a.Password == accounts.Password).FirstOrDefault();
            if (user != null)
            {
                Session["role"] = user.Employee.Role.Name;
                Session["id"] = user.Employee.Id;
                Session["FirstName"] = user.Employee.FName;
                Session["LastName"] = user.Employee.LName;

                Session["phone"] = user.Employee.Phone;

                Session["email"] = user.Employee.Email;

                Session["address"] = user.Employee.Address;
                return RedirectToAction(user.Employee.Role.Name, "Dashboard");
            }
            else
            {
                ViewBag.Error = "Invalid User Name or Password";
                return PartialView("Index");
            }
           
        }
    }
}