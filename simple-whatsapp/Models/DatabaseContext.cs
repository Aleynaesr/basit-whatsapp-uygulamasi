using AgProgramlamaOdev2.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace AgProgramlamaOdev2.Models
{
    public class DatabaseContext:DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<PrivateMessage> PrivateMessages { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupMessage> GroupMessages { get; set; }
        public DbSet<Online> Onlines { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

        }
    }
}