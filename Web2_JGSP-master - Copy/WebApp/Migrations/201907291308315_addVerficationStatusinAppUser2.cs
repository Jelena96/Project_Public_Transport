namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addVerficationStatusinAppUser2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "VerificationStatus", c => c.String());
        }

        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "VerificationStatus");
        }
    }
}
