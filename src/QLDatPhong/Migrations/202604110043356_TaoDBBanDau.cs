namespace QLDatPhong.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TaoDBBanDau : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BookingDetails",
                c => new
                    {
                        BookingDetailID = c.Int(nullable: false, identity: true),
                        BookingID = c.Int(nullable: false),
                        RoomID = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.BookingDetailID)
                .ForeignKey("dbo.Bookings", t => t.BookingID, cascadeDelete: true)
                .ForeignKey("dbo.Rooms", t => t.RoomID, cascadeDelete: true)
                .Index(t => t.BookingID)
                .Index(t => t.RoomID);
            
            CreateTable(
                "dbo.Bookings",
                c => new
                    {
                        BookingID = c.Int(nullable: false, identity: true),
                        CustomerName = c.String(nullable: false),
                        CustomerPhone = c.String(nullable: false),
                        CheckInDate = c.DateTime(nullable: false),
                        CheckOutDate = c.DateTime(nullable: false),
                        TotalAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        BookingStatus = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BookingID);
            
            CreateTable(
                "dbo.Rooms",
                c => new
                    {
                        RoomID = c.Int(nullable: false, identity: true),
                        RoomNumber = c.String(nullable: false),
                        RoomTypeID = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.RoomID)
                .ForeignKey("dbo.RoomTypes", t => t.RoomTypeID, cascadeDelete: true)
                .Index(t => t.RoomTypeID);
            
            CreateTable(
                "dbo.RoomTypes",
                c => new
                    {
                        RoomTypeID = c.Int(nullable: false, identity: true),
                        TypeName = c.String(nullable: false),
                        Description = c.String(),
                        BasePrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        MaxGuests = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.RoomTypeID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Rooms", "RoomTypeID", "dbo.RoomTypes");
            DropForeignKey("dbo.BookingDetails", "RoomID", "dbo.Rooms");
            DropForeignKey("dbo.BookingDetails", "BookingID", "dbo.Bookings");
            DropIndex("dbo.Rooms", new[] { "RoomTypeID" });
            DropIndex("dbo.BookingDetails", new[] { "RoomID" });
            DropIndex("dbo.BookingDetails", new[] { "BookingID" });
            DropTable("dbo.RoomTypes");
            DropTable("dbo.Rooms");
            DropTable("dbo.Bookings");
            DropTable("dbo.BookingDetails");
        }
    }
}
