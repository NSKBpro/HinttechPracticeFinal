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
    /// <summary>
    /// DAL class for comunication with database.
    /// </summary>
    public class UsersService : ICommonOp
    {
        private readonly DataContext context;

        public UsersService()
        {
            if (context == null)
            {
                context = new DataContext();
            }
        }

        public void Create(object userObject)
        {
            context.Users.Add((User)userObject);
            context.SaveChanges();
        }

        public object FindById(int userId)
        {
            User user = new User();
            user = context.Users.Find(userId);
            return user;
        }

        public User FindUserByUsername(String username)
        {
            foreach (User user in context.Users)
            {
                if (user.Username.Equals(username))
                {
                    return user;
                }
            }
            return null;
        }

        public User FindUserByEmail(String email)
        {
            foreach (User user in context.Users)
            {
                if (user.Email.Equals(email))
                {
                    return user;
                }
            }
            return null;
        }

        public List<User> FindAll()
        {
            return context.Users.ToList();
        }

        public void Delete(int userId)
        {
            throw new NotImplementedException();
        }

        public void Edit(object userObject)
        {
            User oldUser = new User();
            User newUser = (User)userObject;
            oldUser = context.Users.Find(newUser.UserId);
            if (oldUser != null)
            {
                FillFields(oldUser, newUser);
                context.Entry(oldUser).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            }
        }

        private void FillFields(User oldUser, User newUSer)
        {
            oldUser.Email = newUSer.Email;
            oldUser.FirstName = newUSer.FirstName;
            oldUser.LastName = newUSer.LastName;
            oldUser.Password = newUSer.Password;
            oldUser.Username = newUSer.Username;
            oldUser.ProfilePicture = newUSer.ProfilePicture;
        }
    }
}
