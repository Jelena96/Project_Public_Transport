namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migracija1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Vehicles", "VehicleLine_Id", "dbo.Lines");
            DropIndex("dbo.Vehicles", new[] { "VehicleLine_Id" });
            AddColumn("dbo.Vehicles", "VehicleLineId", c => c.Int(nullable: false));
            DropColumn("dbo.Vehicles", "VehicleLine_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Vehicles", "VehicleLine_Id", c => c.Int());
            DropColumn("dbo.Vehicles", "VehicleLineId");
            CreateIndex("dbo.Vehicles", "VehicleLine_Id");
            AddForeignKey("dbo.Vehicles", "VehicleLine_Id", "dbo.Lines", "Id");
        }
    }
}
