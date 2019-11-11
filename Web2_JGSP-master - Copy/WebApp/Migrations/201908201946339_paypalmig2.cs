namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class paypalmig2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BaseTickets", "PayPalId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BaseTickets", "PayPalId");
        }
    }
}
