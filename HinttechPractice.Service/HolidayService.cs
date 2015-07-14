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
            dataContext.Holidays.Remove(dataContext.Holidays.Find(holidayId));
            dataContext.SaveChanges();
        }

        public DbSet<Holiday> GetHolidays()
        {
            var holidays = dataContext.Holidays;
            if (holidays != null)
            {
                
                return holidays;
            }
            else
                return null;
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
            dataContext.Entry(dataContext.Holidays.Find(h.HolidayId)).CurrentValues.SetValues(h);
            dataContext.SaveChanges();
        }
    }
}
