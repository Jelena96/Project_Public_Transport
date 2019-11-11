namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class forTransaction : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PassengerTypes", "RowVersion", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AddColumn("dbo.TicketTypes", "RowVersion", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TicketTypes", "RowVersion");
            DropColumn("dbo.PassengerTypes", "RowVersion");
        }
    }
}
