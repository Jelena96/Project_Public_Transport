namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class idtypeoftiicket2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.BaseTickets", "IdTicketType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.BaseTickets", "IdTicketType", c => c.Int());
        }
    }
}
