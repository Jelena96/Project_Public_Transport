namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class typeofpassanger : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "TypeOfPassenger", c => c.String());
            DropColumn("dbo.AspNetUsers", "Approved");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "Approved", c => c.String());
            DropColumn("dbo.AspNetUsers", "TypeOfPassenger");
        }
    }
}
