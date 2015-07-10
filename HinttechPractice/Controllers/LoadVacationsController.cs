using HinttechPractice.Data;
using HinttechPractice.Security;
using HinttechPractice.Service;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HinttechPractice.Controllers
{
     [MyAuthorizeAtribute(Roles = "User")]
    public class LoadVacationsController : Controller
    {
        private static string s1;
        private static string s2;
        private HolidayService db2 = new HolidayService();
        TestService db = new TestService();
        //
        // GET: /LoadVacations/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetVacations()
        {
            
            ViewBag.Title = "CalendarView";
            ViewBag.initHolidays = db2.GetHolidays();
            ViewBag.initVacations = db.GetVacations();
            return View("InitCalendar");
        }

        public ActionResult RegistracijaOdmora(String parameterdatum1, String parameterdatum2)
        {
            UsersService users = new UsersService();
            User u = users.FindUserByUsername(HttpContext.User.Identity.Name);
           
            s1 = parameterdatum1;
            s2 = parameterdatum2;
            ViewBag.Parameterdatum1 = parameterdatum1;
            ViewBag.Parameterdatum2 = parameterdatum2;
            ViewBag.UserId = u.UserId;
            var vac = new Vacation();
            vac.UserId = Int32.Parse(u.UserId.ToString());
            vac.DateFrom = Convert.ToDateTime(parameterdatum1);
            vac.DateTo = Convert.ToDateTime(parameterdatum2);


            return View(vac);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegistracijaOdmora(Vacation vacation)
        {
            if (ModelState.IsValid)
            {
                vacation.DateFrom = Convert.ToDateTime(s1);
                vacation.DateTo = Convert.ToDateTime(s2);
                db.AddVacation(vacation);
                return RedirectToAction("initHolidays","LoadHolidays"); 

            }
            return View();
        }

        public ActionResult EditVacation(int vacationId,String datum1,String datum2,String opis,String isSick)
        {
            UsersService users = new UsersService();
            User u = users.FindUserByUsername(HttpContext.User.Identity.Name);
            TestService ser = new TestService();
            if (ser.FindVacationByUserId(u.UserId, vacationId) == true)
            {



                ViewBag.UserId = u.UserId;
                ViewBag.Datum1 = datum1;
                ViewBag.Datum2 = datum2;
                ViewBag.Opis = opis;
                ViewBag.IsSick = isSick;

                Vacation vac = new Vacation();
                vac.VacationPeriodId = vacationId;
                vac.UserId = u.UserId;
                vac.DateFrom = Convert.ToDateTime(datum1);
                vac.DateTo = Convert.ToDateTime(datum2);
                vac.Description = opis;
                vac.IsSickLeave = Convert.ToBoolean(isSick);
                return View(vac);
            }
            else
            {
                return RedirectToAction("initHolidays", "LoadHolidays");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditVacation(Vacation vacation)
        {
            if (ModelState.IsValid)
            {
                db.EditVacation(vacation);

                return RedirectToAction("initHolidays", "LoadHolidays");
            }
            return View();
        }

        public ActionResult DeleteVacation(int vacationId, int userId, String datum1, String datum2, String opis, String isSick)
        {
           
            Vacation vac = new Vacation();
            vac.VacationPeriodId = vacationId;
            vac.UserId = userId;
            vac.DateFrom = Convert.ToDateTime(datum1);
            vac.DateTo = Convert.ToDateTime(datum2);
            vac.Description = opis;
            vac.IsSickLeave = Convert.ToBoolean(isSick);
            return View(vac);


        }

        [HttpPost] 
        public ActionResult DeleteVacation(int vacationId)
        {
            if (ModelState.IsValid)
            {
                db.DeleteVacation(vacationId);

                return RedirectToAction("initHolidays", "LoadHolidays");
            }
            return View();
        }


       

       
	}
}