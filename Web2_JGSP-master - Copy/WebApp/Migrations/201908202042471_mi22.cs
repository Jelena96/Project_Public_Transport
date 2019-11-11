namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mi22 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.BaseTickets", "PayPalId", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.BaseTickets", "PayPalId", c => c.Int(nullable: false));
        }
    }
}
