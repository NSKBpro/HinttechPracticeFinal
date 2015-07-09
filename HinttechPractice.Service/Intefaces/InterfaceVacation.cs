using HinttechPractice.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HinttechPractice.Service.Intefaces
{
    public interface InterfaceVacation
    {
        DbSet<Vacation> GetVacations();
    }
}
