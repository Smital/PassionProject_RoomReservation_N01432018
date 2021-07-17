namespace RoomReservation.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class bookingroom : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bookings", "RoomID", c => c.Int(nullable: false));
            CreateIndex("dbo.Bookings", "RoomID");
            AddForeignKey("dbo.Bookings", "RoomID", "dbo.Rooms", "RoomID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Bookings", "RoomID", "dbo.Rooms");
            DropIndex("dbo.Bookings", new[] { "RoomID" });
            DropColumn("dbo.Bookings", "RoomID");
        }
    }
}
