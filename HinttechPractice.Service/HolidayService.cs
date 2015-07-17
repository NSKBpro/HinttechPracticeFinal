using HinttechPractice.Data;
using HinttechPractice.Data.DataContext;
using HinttechPractice.Service.Intefaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HinttechPractice.Service
{
    public class HolidayService : HolidayIntefrace
    {

        private readonly DataContext dataContext;

        public HolidayService()
        {
            if (dataContext == null)
            {
                dataContext = new DataContext();
            }
        }

        public void AddHoliday(Holiday h)
        {
            dataContext.Holidays.Add(h);
            dataContext.SaveChanges();
        }

        public void RemoveHoliday(int holidayId)
        {
            Holiday h = dataContext.Holidays.Find(holidayId);
            if(h!=null)
                dataContext.Holidays.Remove(h);
            dataContext.SaveChanges();
        }

        public DbSet<Holiday> GetHolidays()
        {
            var holidays = dataContext.Holidays;
                return holidays;
        }

        public List<Holiday> GetHolidaysForDate(DateTime dt)
        {
            List<Holiday> holidays = new List<Holiday>();
            foreach (Holiday h in GetHolidays())
            {
                if (h.DateFrom.Equals(dt))
                {
                    holidays.Add(h);
                }
            }
            return holidays;
        }

        public object FindById(int id)
        {
            Holiday pom = new Holiday();
            pom = dataContext.Holidays.Find(id);

            return pom;
        }

        public void EditHoliday(Holiday h)
        {
            Holiday oldHoliday = dataContext.Holidays.Find(h.HolidayId);
            if(oldHoliday!=null)
             dataContext.Entry(oldHoliday).CurrentValues.SetValues(h);
            dataContext.SaveChanges();
        }
    }
}
