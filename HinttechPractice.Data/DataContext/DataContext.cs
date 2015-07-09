﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HinttechPractice.Data.DataContext
{
    public class DataContext :DbContext
    {
        public DataContext()
            : base("DataContext")
        {

        }
        public virtual DbSet<Holiday> Holidays { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Vacation> Vacations { get; set; }
    }
  
}

