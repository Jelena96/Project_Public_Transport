namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addCheckInTicket : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BaseTickets", "CheckIn", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.BaseTickets", "CheckIn");
        }
    }
}
