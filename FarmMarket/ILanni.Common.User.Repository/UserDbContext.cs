using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ILanni.Common.User.Repository
{
    public class UserDbContext:DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User.DbModel.User>().ToTable("user_User");
            //modelBuilder.Entity<User.DbModel.Role>().ToTable("user_Role");
            modelBuilder.Entity<DbModel.UserRole>().ToTable("user_UserRole");
        }

        public DbSet<User.DbModel.User> Users { get; set; }

        public DbSet<User.DbModel.Role> Roles { get; set; }

    }
}
