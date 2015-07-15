using HinttechPractice.Data;
using HinttechPractice.Security;
using HinttechPractice.Service;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;



namespace HinttechPractice.Controllers
{
    [MyAuthorizeAtribute(Roles = "User")]
    public class LoadVacationsController : Controller
    {
        private static string s1;
        private static string s2;
        public static int flag=0;
        private static int daniZaVracanje;
        private static DateTime datumProveraZaEdit;
        private HolidayService db2 = new HolidayService();
        TestService db = new TestService();

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetVacations()
        {

            ViewBag.Title = "CalendarView";
            ViewBag.initHolidays = db2.GetHolidays();
            ViewBag.initVacations = db.GetVacations();
            flag = 0;
            return View("InitCalendar");
        }

        public ActionResult RegistracijaOdmora(String parameterdatum1, String parameterdatum2)
        {
            UsersService users = new UsersService();
            User u = users.FindUserByUsername(HttpContext.User.Identity.Name);
            String datum = DateTime.Now.ToString("yyyy-MM-dd");
            ViewBag.Datum = datum;   
            s1 = parameterdatum1;
            s2 = parameterdatum2;
            ViewBag.Parameterdatum1 = parameterdatum1;
            ViewBag.Parameterdatum2 = parameterdatum2;
            ViewBag.UserId = u.UserId;
            ViewBag.BrDana = u.VacationDays;
            var vac = new Vacation();
            vac.UserId = Int32.Parse(u.UserId.ToString());
            vac.DateFrom = Convert.ToDateTime(parameterdatum1);
            vac.DateTo = Convert.ToDateTime(parameterdatum2);
            flag = 0;


            return View(vac);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegistracijaOdmora(Vacation vacation)
        {
            flag = 0;
            UsersService users = new UsersService();
            User u = (User)users.FindById(vacation.UserId);
             String datum = DateTime.Now.ToString("yyyy-MM-dd");
            ViewBag.Datum = datum;
            Double numDays = (vacation.DateTo - vacation.DateFrom).TotalDays;
             List<Vacation> vacations=db.GetVacationsForCurrentUser(u.UserId);
            numDays -= DaysIsntCountHoliday(vacation);

            if (vacation.IsSickLeave && (DateTime.Parse(vacation.DateTo.ToString("yyyy-MM-dd")) > (DateTime.Parse(datum))) && (DateTime.Parse(vacation.DateTo.ToString("yyyy-MM-dd")) > (DateTime.Parse(vacation.DateFrom.ToString("yyyy-MM-dd")))))
            {
                int brojac = 0;
                if (ModelState.IsValid)
                {
                   
                    foreach (Vacation v in vacations)
                    {
                        if ((vacation.DateFrom > v.DateFrom && vacation.DateFrom < v.DateTo) || (vacation.DateTo > v.DateFrom && vacation.DateTo < v.DateTo) || (vacation.DateFrom <= v.DateFrom && vacation.DateTo >= v.DateTo))
                        {
                            flag = 1;

                        }
                        else
                        {
                            brojac++;
                        }
                    }
                    if (brojac == vacations.Count)
                    {
                        flag = 0;
                        vacation.DateFrom = vacation.DateFrom;
                        vacation.DateTo = vacation.DateTo;
                        db.AddVacation(vacation);
                        return RedirectToAction("initHolidays", "LoadHolidays");
                    }


                }
                return View();
            }
            else
            {
                if (Convert.ToInt32(numDays) < u.VacationDays && (DateTime.Parse(vacation.DateTo.ToString("yyyy-MM-dd")) > (DateTime.Parse(datum))) && (DateTime.Parse(vacation.DateTo.ToString("yyyy-MM-dd")) > (DateTime.Parse(vacation.DateFrom.ToString("yyyy-MM-dd")))))
                {
                    int brojac = 0;
                    if (ModelState.IsValid)
                    {
                         foreach (Vacation v in vacations)
                        {
                            if ((vacation.DateFrom > v.DateFrom && vacation.DateFrom < v.DateTo) || (vacation.DateTo > v.DateFrom && vacation.DateTo < v.DateTo)||(vacation.DateFrom<=v.DateFrom && vacation.DateTo>=v.DateTo))
                            {
                                flag = 1;
                                
                            }
                            else
                            {
                                
                                brojac++; 
                            }
                        }

                         if (brojac == vacations.Count)
                        {
                            flag = 0;
                            vacation.DateFrom = vacation.DateFrom;
                            vacation.DateTo = vacation.DateTo;
                            db.AddVacation(vacation);
                            int days = u.VacationDays - Convert.ToInt32(numDays);
                            u.VacationDays = days;
                            users.Edit(u);
                            return RedirectToAction("initHolidays", "LoadHolidays");
                        }
                         else
                         {
                             return RedirectToAction("initHolidays", "LoadHolidays");
                         }

                    }
                    return View();
                }
                else
                {
                    return RedirectToAction("initHolidays", "LoadHolidays");
                }
                //provera
            }
        }

        private double DaysIsntCountHoliday(Vacation vacation)
        {
            HolidayService holidaysService = new HolidayService();
            List<Holiday> holidays = holidaysService.GetHolidays().ToList();
            double holidayDaysIsntCount = 0;
            if (vacation.IsSickLeave) return holidayDaysIsntCount;
            foreach (Holiday h in holidays)
            {
                //kada je praznik izmedju pocetka i kraja odmora
               if (h.DateFrom >= vacation.DateFrom && h.DateFrom <= vacation.DateTo)
                {
                    if (h.DateTo <= vacation.DateTo)
                    {
                        holidayDaysIsntCount = (h.DateTo - h.DateFrom).TotalDays;
                    }
                    else if (h.DateTo >= vacation.DateTo)
                    {
                        holidayDaysIsntCount = (vacation.DateTo - h.DateFrom).TotalDays;
                    }
                }
                //kada je praznik poceo pre pocetka odmora i traje posle
                else if (h.DateFrom <= vacation.DateFrom && h.DateTo >= vacation.DateTo)
                {
                    holidayDaysIsntCount = (vacation.DateTo - vacation.DateFrom).TotalDays;
                }
                //zavrsi se izmedju
                else if (h.DateFrom <= vacation.DateFrom && h.DateTo <= vacation.DateTo && h.DateTo >= vacation.DateFrom)
                {
                    holidayDaysIsntCount = (h.DateTo - vacation.DateFrom).TotalDays;
                }
            }
            return holidayDaysIsntCount;
        }

        public ActionResult EditVacation(int vacationId)
        {
            UsersService users = new UsersService();
            User u = users.FindUserByUsername(HttpContext.User.Identity.Name);
            ViewBag.BrDana = u.VacationDays;
            TestService ser = new TestService();
            if (ser.FindVacationByUserId(u.UserId, vacationId) == true)
            {
                Vacation vac = (Vacation)db.FindById(vacationId);
                if (vac == null)
                {
                    //...
                    return RedirectToAction("SeeVacations");
                }
                Double numDays = (vac.DateTo - vac.DateFrom).TotalDays;
                daniZaVracanje = Convert.ToInt32(numDays);
                String datum = DateTime.Now.ToString("yyyy-MM-dd");
                ViewBag.Datum = datum;
                ViewBag.Bolestan = vac.IsSickLeave;
                datumProveraZaEdit = vac.DateTo;
                ViewBag.editDateFrom = vac.DateFrom.ToString("yyyy-MM-dd");
                ViewBag.editDateTo = vac.DateTo.ToString("yyyy-MM-dd");
                return View(vac);
            }
            else
            {
                return RedirectToAction("SeeVacations");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditVacation(Vacation vacation, int? page)
        {
            String datum = DateTime.Now.ToString("yyyy-MM-dd");
            ViewBag.Datum = datum;
            UsersService users = new UsersService();
            User u = (User)users.FindById(vacation.UserId);
            Double numDays = (vacation.DateTo - vacation.DateFrom).TotalDays;
            Double povecavanjeOdmora = (vacation.DateTo - datumProveraZaEdit).TotalDays;
            Vacation oldVacation = (Vacation)db.FindById(vacation.VacationPeriodId);
            List<Vacation> vacations = db.GetVacationsForCurrentUser(u.UserId);
            double oldHolidayVacationCount = DaysIsntCountHoliday(oldVacation);
            double totalHolidayDays = oldHolidayVacationCount - DaysIsntCountHoliday(vacation);
            numDays += totalHolidayDays;
            if (vacation.IsSickLeave && (DateTime.Parse(vacation.DateTo.ToString("yyyy-MM-dd")) > (DateTime.Parse(datum))) && (DateTime.Parse(vacation.DateTo.ToString("yyyy-MM-dd")) > (DateTime.Parse(vacation.DateFrom.ToString("yyyy-MM-dd")))))
            {
                int brojac = 0;
                foreach (Vacation v in vacations)
                {
                    if (vacation.VacationPeriodId == v.VacationPeriodId)
                    {
                        brojac++;
                        continue;
                    }
                    if ((vacation.DateFrom >= v.DateFrom && vacation.DateFrom <= v.DateTo) || (vacation.DateTo >= v.DateFrom && vacation.DateTo <= v.DateTo) || (vacation.DateFrom <= v.DateFrom && vacation.DateTo >= v.DateTo))
                    {

                        flag = 1;

                    }
                    else
                    {
                        brojac++;
                    }
                }
                if (brojac == vacations.Count)
                {
                    db.EditVacation(vacation);
                    return SeeVacations(page);
                }
                else
                {
                    return SeeVacations(page);
                }

            }
            else
            {

                if (Convert.ToInt32(povecavanjeOdmora) <= u.VacationDays && (DateTime.Parse(vacation.DateTo.ToString("yyyy-MM-dd")) > (DateTime.Parse(datum))) && (DateTime.Parse(vacation.DateTo.ToString("yyyy-MM-dd")) > (DateTime.Parse(vacation.DateFrom.ToString("yyyy-MM-dd")))))
                {
                    int brojac = 0;
                    if (ModelState.IsValid)
                    {
                        foreach (Vacation v in vacations)
                        {
                            if(vacation.VacationPeriodId==v.VacationPeriodId)
                            {
                                brojac++;
                                continue;
                            }
                            if ((vacation.DateFrom >= v.DateFrom && vacation.DateFrom <= v.DateTo) || (vacation.DateTo >= v.DateFrom && vacation.DateTo <= v.DateTo) || (vacation.DateFrom <= v.DateFrom && vacation.DateTo >= v.DateTo))
                            {

                                flag = 1;

                            }
                            else
                            {
                                brojac++;
                            }
                        }
                        if (brojac == vacations.Count)
                        {
                            int days = u.VacationDays - Convert.ToInt32(numDays) + daniZaVracanje;
                            u.VacationDays = days;
                            users.Edit(u);
                            db.EditVacation(vacation);
                            daniZaVracanje = Convert.ToInt32(numDays);
                            return SeeVacations(page);
                        }
                        else
                        {
                            return SeeVacations(page);
                        }

                    }
                    return View();
                }
                else
                {
                    return SeeVacations(page);
                }
            }
        }

        public ActionResult DeleteVacation(int vacationId)
        {
            Vacation vac = (Vacation)db.FindById(vacationId);
            return View(vac);
        }

        [HttpPost]
        public ActionResult DeleteVacation(Vacation vacation, int? page)
        {
            Vacation vac = (Vacation)db.FindById(vacation.VacationPeriodId);
            double numDays = (vac.DateTo - vac.DateFrom).TotalDays;
            
            UsersService users = new UsersService();
            User u = (User)users.FindUserByUsername(HttpContext.User.Identity.Name);
             if (!vac.IsSickLeave)
            {
            int days = u.VacationDays + Convert.ToInt32(numDays);
            u.VacationDays = days;
           }
           
            users.Edit(u);
            db.DeleteVacation(vac.VacationPeriodId);
            return SeeVacationsDelete(page);
        }

        public ActionResult SeeVacations(int? page)
        {
            UsersService users = new UsersService();
            int userId = users.FindUserByUsername(HttpContext.User.Identity.Name).UserId;
            List<Vacation> currentUserVacations = db.GetVacationsForCurrentUser(userId);
            double tempPaginationValue = currentUserVacations.Count() / 6;
            if (currentUserVacations.Count() % 6 != 0) tempPaginationValue++;
            if (tempPaginationValue < page) page = 1;
            //if (page != 1 && page != tempPaginationValue && 6 * tempPaginationValue == currentUserVacations.Count()) page--;
            String datum = DateTime.Now.ToString("yyyy-MM-dd");
            ViewBag.Datum = datum;
            return View("SeeVacations", currentUserVacations.ToList().ToPagedList(page ?? 1, 6));
        }

        public ActionResult SeeVacationsDelete(int? page)
        {
            UsersService users = new UsersService();
            int userId = users.FindUserByUsername(HttpContext.User.Identity.Name).UserId;
            List<Vacation> currentUserVacations = db.GetVacationsForCurrentUser(userId);
            double tempPaginationValue = currentUserVacations.Count() / 6;
            if (currentUserVacations.Count() % 6 != 0) tempPaginationValue++;
            if (tempPaginationValue < page) page--;
            //if (page != 1 && page != tempPaginationValue && 6 * tempPaginationValue == currentUserVacations.Count()) page--;
            String datum = DateTime.Now.ToString("yyyy-MM-dd");
            ViewBag.Datum = datum;
            return View("SeeVacations", currentUserVacations.ToList().ToPagedList(page ?? 1, 6));
        }
    }
}