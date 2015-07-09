using HinttechPractice.Data;
using HinttechPractice.Data.DataContext;
using HinttechPractice.Service.Intefaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HinttechPractice.Service
{
   public class UsersService:ICommonOp
    {
        private readonly DataContext context;
        public UsersService()
        {
            if (context == null)
            {
                context = new DataContext();
            }
        }
        public void Create(object pom)
        {
                context.Users.Add((User)pom);
                context.SaveChanges();
        }

        public object FindById(int id)
        {
            User pom = new User();
                pom = context.Users.Find(id);

            return pom;
        }

        public User FindUserByUsername(String username)
        {
            foreach (User pom in context.Users) {
                if (pom.Username.Equals(username)) {
                    return pom;
                }
            }
            return null;
        }

        public User FindUserByEmail(String email)
        {
            foreach (User pom in context.Users)
            {
                if (pom.Email.Equals(email))
                {
                    return pom;
                }
            }
            return null;
        }

        public List<User> FindAll()
        {
            return context.Users.ToList();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public void Edit(object user)
        {
            User old = new User();
                User newUser = (User)user;
                old = context.Users.Find(newUser.UserId);
                fillFields(old, newUser);
                context.Entry(old).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
        }
        private void fillFields(User old, User newUSer)
        {
            old.Email = newUSer.Email;
            old.FirstName = newUSer.FirstName;
            old.LastName = newUSer.LastName;
            old.Password = newUSer.Password;
            old.Username = newUSer.Username;
            old.ProfilePicture = newUSer.ProfilePicture;
        }
    }
}
