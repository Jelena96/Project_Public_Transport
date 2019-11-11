namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addIdTypeOfUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BaseTickets", "IdTypeOfUser", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.BaseTickets", "IdTypeOfUser");
        }
    }
}
