using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace api.Data
{
    // inherit from DbContext 
    public class ApplicationDBContext : IdentityDbContext<AppUser>
    {
        //ctor tab
        // derived class constructor calling the base class construtor i.e DbContext constructor by passing the dpContext options
        public ApplicationDBContext(DbContextOptions dbContextOptions):base(dbContextOptions)
        {
            
        }
        public DbSet<Stock> Stock{
            get; set;
        }
        public DbSet<Comment> Comments {
            get; set;
        }
        public DbSet<Portfolio> Portfolios{
            get; set;
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Portfolio> (x=>x.HasKey(p=> new{p.AppUserId,p.StockId}));//foreign key declaration
            //connecting into the table
            builder.Entity<Portfolio>()
            .HasOne(u=>u.AppUser)
            .WithMany(u=>u.Portfolios)
            .HasForeignKey(p=>p.AppUserId);
            builder.Entity<Portfolio>()
            .HasOne(u=>u.Stock)
            .WithMany(u=>u.Portfolios)
            .HasForeignKey(p=>p.StockId);
            List<IdentityRole> Roles = new List <IdentityRole>
            {
                new IdentityRole{
                    Name="Admin",
                    NormalizedName="ADMIN"
                },
                 new IdentityRole{
                    Name="User",
                    NormalizedName="USER"
                },
            };
            builder.Entity<IdentityRole>().HasData(Roles);

        }
    }

}