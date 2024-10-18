using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Core.Models;

namespace ToDoList.Infrastrucure.Context
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> optios) : base(optios) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>()
                        .ToTable("Users");
                        //.HasOne(X=>X.ToDolist)
                        //.WithOne();

            modelBuilder.Entity<task>()
                        .ToTable("Tasks")
                        .HasOne(T => T.ToDolist)
                        .WithMany(C=>C.Tasks)
                        .HasForeignKey(FK => FK.ToDolistId)
                        .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ToDolist>()
                        .HasOne(S => S.ApplicationUser)
                        .WithOne(X=>X.ToDolist)
                        .HasForeignKey<ToDolist>(m => m.ApplicationUserId)
                        .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<IdentityRole>()
                        .ToTable("Roles");
            modelBuilder.Entity<IdentityUserRole<string>>()
                        .ToTable("UserRoles");
            modelBuilder.Entity<IdentityUserClaim<string>>()
                        .ToTable("UserClaims");
            modelBuilder.Entity<IdentityUserLogin<string>>()
                        .ToTable("UserLogins");
            modelBuilder.Entity<IdentityRoleClaim<string>>()
                        .ToTable("RoleClaims");
            modelBuilder.Entity<IdentityUserToken<string>>()
                        .ToTable("UserTokens");
        }
        DbSet<ToDolist> ToDolist { get; set; }
        DbSet<task> Tasks { get; set; }
    }
}
