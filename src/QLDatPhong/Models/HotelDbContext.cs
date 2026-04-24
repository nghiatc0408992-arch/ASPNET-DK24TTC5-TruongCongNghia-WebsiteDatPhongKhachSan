using System.Collections.Generic;
using System.Data.Entity;

namespace QLDatPhong.Models
{
    public class HotelDbContext : DbContext
    {
        // Tên chuỗi kết nối (Connection String) trong file Web.config
        public HotelDbContext() : base("name=HotelDbConnectionString")
        {
        }

        public DbSet<RoomType> RoomTypes { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<BookingDetail> BookingDetails { get; set; }
        public DbSet<Account> Accounts { get; set; }
    }
}