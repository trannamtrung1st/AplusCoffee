using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FranCoffee.Website.Models
{
    public class IdentityContext : IdentityDbContext<AppUsers, AppRoles, string>
    {
        public IdentityContext(DbContextOptions<IdentityContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

    }

    public class AppUsers : IdentityUser
    {
        [StringLength(100)]
        public override string Id { get; set; }
        public string FullName { get; set; }
    }

    public class AppRoles : IdentityRole
    {
        [StringLength(100)]
        public override string Id { get; set; }
    }

    public class DbContextFactory : IDesignTimeDbContextFactory<IdentityContext>
    {

        public IdentityContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<IdentityContext>();
            optionsBuilder.UseSqlServer(Startup.Configuration.GetConnectionString("FranCoffeeContext"));

            return new IdentityContext(optionsBuilder.Options);
        }
    }
}