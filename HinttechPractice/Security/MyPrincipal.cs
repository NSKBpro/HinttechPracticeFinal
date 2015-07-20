using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Principal;
using HinttechPractice.Data;

namespace HinttechPractice.Security
{
    ///<summary>
    ///Principal class, for role-based system. 
    ///</summary>
    public class MyPrincipal : IPrincipal
    {
        private User account = new User();

        public User Account
        {
            get { return account; }
            set { account = value; }
        }
        public MyPrincipal(String username)
        {
            this.Identity = new GenericIdentity(username);
        }

        public IIdentity Identity
        {
            get;
            set;
        }

        ///<summary>
        /// How to behave on role param.
        ///</summary>
        public bool IsInRole(string role)
        {
            if (role.Equals("Admin") && account.IsUserAdmin && !account.IsUserRegistered)
            {
                return true;
            }
            else if (role.Equals("User") && account.IsUserRegistered && !account.IsUserAdmin)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}