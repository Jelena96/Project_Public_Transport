namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class timeissuedbt : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.BaseTickets", "TimeIssued", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.BaseTickets", "TimeIssued", c => c.String(nullable: false, maxLength: 20));
        }
    }
}
