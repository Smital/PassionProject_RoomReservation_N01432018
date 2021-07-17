namespace RoomReservation.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class amenities : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Amenities",
                c => new
                    {
                        AmenityID = c.Int(nullable: false, identity: true),
                        AmenityName = c.String(),
                    })
                .PrimaryKey(t => t.AmenityID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Amenities");
        }
    }
}
