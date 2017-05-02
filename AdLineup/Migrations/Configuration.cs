namespace AdLineup.Migrations
{
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<AdLineup.Models.AdLineupContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            ContextKey = "AdLineup.Models.AdLineupContext";
        }

        protected override void Seed(AdLineup.Models.AdLineupContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            // seed data for AdLineup

            if (context.Billboards.Count() == 0)
            {
                context.Billboards.AddOrUpdate(
                    b => b.Id,
                    new Billboard { Id = 1, Name = "#112345 I12 @ O'Neal", Location = ".5 Miles South of O'Neal on I1", Latitude = "30.4212", Longitude = "-91.1542", AdFlowchartDiagramData = "AdFlowchartDiagramData1" },
                    new Billboard { Id = 2, Name = "#113542 S Harrells Ferry @ Mi2", Location = ".21 Miles East of Harrells Fe2", Latitude = "30.4232", Longitude = "-91.1242", AdFlowchartDiagramData = "AdFlowchartDiagramData2" },
                    new Billboard { Id = 3, Name = "#113224 S Sherwood @ Old Hamm3", Location = ".11 Miles North of S Sherwood3", Latitude = "30.4252", Longitude = "-91.1572", AdFlowchartDiagramData = "AdFlowchartDiagramData3" }
                    );
                context.SaveChanges();
            }  // save seed data for Billboard

            if (context.Ads.Count() == 0)
            {
                context.Ads.AddOrUpdate(
                    a => a.Id,
                    new Ad { Id = 1, Name = "Panera Bread #A3372.1", Index = 1, FlightStart = Convert.ToDateTime("5/1/2017 01:00PM"), FlightEnd = "5/5/2017 01:00PM", ImageFilename = "/Content/Images/SampleImage1Small.jpg" },
                    new Ad { Id = 2, Name = "McDonalds Chicken #44-732B", Index = 2, FlightStart = Convert.ToDateTime("5/10/2017 03:00PM"), FlightEnd = "5/15/2017 03:00PM", ImageFilename = "/Content/Images/SampleImage2Small.jpg" },
                    new Ad { Id = 3, Name = "Krispy Kreme Hot Sign #5", Index = 3, FlightStart = Convert.ToDateTime("5/20/2017 06:00PM"), FlightEnd = "5/25/2017 06:00PM", ImageFilename = "/Content/Images/SampleImage3Small.jpg" }
                    );
                context.SaveChanges();
            }  // save seed data for Ad

            if (context.Customers.Count() == 0)
            {
                context.Customers.AddOrUpdate(
                    c => c.Id,
                    new Customer { Id = 1, Name = "John O'Connor", Email = "texas.toast@email.com", CellPhone = "(888) 555-1111", WorkPhone = "(888) 555-1111" },
                    new Customer { Id = 2, Name = "Mary Martha", Email = "rocket.man@email.com", CellPhone = "(877) 555-2222", WorkPhone = "(877) 555-2222" },
                    new Customer { Id = 3, Name = "Steve Windstone", Email = "cargo.ship@email.com", CellPhone = "(866) 555-3333", WorkPhone = "(866) 555-3333" }
                    );
                context.SaveChanges();
            }  // save seed data for Customer

            if (context.AdBillboards.Count() == 0)
            {
                context.AdBillboards.AddOrUpdate(
                    a => a.Id,
                    new AdBillboards { Id = 1, BillboardId = 1, AdId = 1 },
                    new AdBillboards { Id = 2, BillboardId = 2, AdId = 2 },
                    new AdBillboards { Id = 3, BillboardId = 3, AdId = 3 }
                    );
                context.SaveChanges();
            }  // save seed data for AdBillboards


        }
    }
}
