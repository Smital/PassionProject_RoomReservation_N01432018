namespace RoomReservation.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class amenitiesrooms : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RoomAmenities",
                c => new
                    {
                        Room_RoomID = c.Int(nullable: false),
                        Amenity_AmenityID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Room_RoomID, t.Amenity_AmenityID })
                .ForeignKey("dbo.Rooms", t => t.Room_RoomID, cascadeDelete: true)
                .ForeignKey("dbo.Amenities", t => t.Amenity_AmenityID, cascadeDelete: true)
                .Index(t => t.Room_RoomID)
                .Index(t => t.Amenity_AmenityID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RoomAmenities", "Amenity_AmenityID", "dbo.Amenities");
            DropForeignKey("dbo.RoomAmenities", "Room_RoomID", "dbo.Rooms");
            DropIndex("dbo.RoomAmenities", new[] { "Amenity_AmenityID" });
            DropIndex("dbo.RoomAmenities", new[] { "Room_RoomID" });
            DropTable("dbo.RoomAmenities");
        }
    }
}
