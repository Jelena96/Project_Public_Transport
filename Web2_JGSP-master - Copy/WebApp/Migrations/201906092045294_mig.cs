namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PriceLists", "From", c => c.String());
            AlterColumn("dbo.PriceLists", "To", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PriceLists", "To", c => c.DateTime(nullable: false));
            AlterColumn("dbo.PriceLists", "From", c => c.DateTime(nullable: false));
        }
    }
}
