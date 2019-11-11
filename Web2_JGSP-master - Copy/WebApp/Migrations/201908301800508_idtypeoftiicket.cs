namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class idtypeoftiicket : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BaseTickets", "IdTicketType", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.BaseTickets", "IdTicketType");
        }
    }
}
