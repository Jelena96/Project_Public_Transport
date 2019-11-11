namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migracija : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Stations", "Line_Id", "dbo.Lines");
            DropIndex("dbo.Stations", new[] { "Line_Id" });
            CreateTable(
                "dbo.StationLines",
                c => new
                    {
                        Station_Id = c.Int(nullable: false),
                        Line_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Station_Id, t.Line_Id })
                .ForeignKey("dbo.Stations", t => t.Station_Id, cascadeDelete: true)
                .ForeignKey("dbo.Lines", t => t.Line_Id, cascadeDelete: true)
                .Index(t => t.Station_Id)
                .Index(t => t.Line_Id);
            
            DropColumn("dbo.Stations", "Line_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Stations", "Line_Id", c => c.Int());
            DropForeignKey("dbo.StationLines", "Line_Id", "dbo.Lines");
            DropForeignKey("dbo.StationLines", "Station_Id", "dbo.Stations");
            DropIndex("dbo.StationLines", new[] { "Line_Id" });
            DropIndex("dbo.StationLines", new[] { "Station_Id" });
            DropTable("dbo.StationLines");
            CreateIndex("dbo.Stations", "Line_Id");
            AddForeignKey("dbo.Stations", "Line_Id", "dbo.Lines", "Id");
        }
    }
}
