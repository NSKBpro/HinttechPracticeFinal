using HinttechPractice.Data;
using HinttechPractice.Data.DataContext;
using HinttechPractice.Service.Intefaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;

namespace HinttechPractice.Service
{
    public class TestService: InterfaceVacation
    {
        private readonly DataContext dataContext;

        public TestService()
        {
            if (dataContext == null)
            {
                dataContext = new DataContext();
            }
        }

        public void Add(User testModel)
        {
            dataContext.Users.Add(testModel);
            dataContext.SaveChanges();
        }

        public DbSet<Vacation> GetVacations()
        {
            var vacations = dataContext.Vacations;
            if (vacations != null)
            {
                return vacations;
            }
            else
                return null;
        }

        public List<Vacation> GetVacationsForCurrentUser(int userID)
        {
            List<Vacation> retVal = new List<Vacation>();
            foreach (Vacation v in dataContext.Vacations)
            {
                if (v.UserId == userID)
                {
                    retVal.Add(v);
                }
            }
            return retVal;
        }

        public void AddVacation(Vacation testModel)
        {
            dataContext.Vacations.Add(testModel);
            dataContext.SaveChanges();

        }
        public void EditVacation(Vacation testModel)
        {
            dataContext.Entry(dataContext.Vacations.Find(testModel.VacationPeriodId)).CurrentValues.SetValues(testModel);
            dataContext.SaveChanges();


        }
        public object FindById(int id)
        {
            Vacation pom = new Vacation();
            pom = dataContext.Vacations.Find(id);

            return pom;
        }

        public void DeleteVacation(int vacationId)
        {
            dataContext.Vacations.Remove(dataContext.Vacations.Find(vacationId));
            dataContext.SaveChanges();
        }
        
    }
}
