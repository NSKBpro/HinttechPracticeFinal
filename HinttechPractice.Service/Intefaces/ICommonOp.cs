using HinttechPractice.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HinttechPractice.Service.Intefaces
{
    /// <summary>
    /// Generic interface for common DAL operations.
    /// </summary>
    interface ICommonOp
    {
        void Create(Object hol);

        Object FindById(int id);

        void Delete(int id);

        void Edit(Object obj);


    }
}
