using AdLineup.Migrations;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace AdLineup.Models
{
    public class AdLineupContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public AdLineupContext() : base("name=AdLineupContext")
        {
            Database.SetInitializer<AdLineupContext>(new MigrateDatabaseToLatestVersion<AdLineupContext, Configuration>());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }


        public System.Data.Entity.DbSet<AdLineup.Models.Billboard> Billboards { get; set; }

        public System.Data.Entity.DbSet<AdLineup.Models.Ad> Ads { get; set; }

        public System.Data.Entity.DbSet<AdLineup.Models.Customer> Customers { get; set; }

        public System.Data.Entity.DbSet<AdLineup.Models.AdBillboards> AdBillboards { get; set; }
    } // public class AdLineupContext : DbContext
}
