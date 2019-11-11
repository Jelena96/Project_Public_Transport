namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class paypalmig : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PayPals",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Cart = c.String(),
                        CreateTime = c.String(),
                        PayPalId = c.String(),
                        Email = c.String(),
                        FirstName = c.String(),
                        LastName = c.String(),
                        PayerId = c.String(),
                        PaymentMethod = c.String(),
                        Status = c.String(),
                        State = c.String(),
                        Currency = c.String(),
                        Total = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.PayPals");
        }
    }
}
