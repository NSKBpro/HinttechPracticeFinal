using HinttechPractice.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HinttechPractice.Service.Intefaces
{
    public interface HolidayIntefrace
    {
        DbSet<Holiday> GetHolidays();
        List<Holiday> GetHolidaysForDate(DateTime dt);
        void RemoveHoliday(int holidayId);
        void AddHoliday(Holiday h);
        void EditHoliday(Holiday h);
    }
}
