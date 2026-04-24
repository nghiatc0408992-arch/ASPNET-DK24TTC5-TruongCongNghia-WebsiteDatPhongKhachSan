using System;
using System.Linq;
using System.Web.Mvc;
using QLDatPhong.Models;

namespace QLDatPhong.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private HotelDbContext db = new HotelDbContext();

        public ActionResult Dashboard()
        {
            // 1. Thống kê số lượng phòng
            ViewBag.TotalRooms = db.Rooms.Count();
            ViewBag.AvailableRooms = db.Rooms.Count(r => r.Status == 0); // Trạng thái 0 là trống

            // 2. Thống kê đơn hàng
            ViewBag.PendingBookings = db.Bookings.Count(b => b.BookingStatus == 0);
            ViewBag.ConfirmedBookings = db.Bookings.Count(b => b.BookingStatus == 1);

            // 3. Tính tổng doanh thu từ các đơn đã "Hoàn thành" (Status = 2)
            // Dùng (decimal?) để tránh lỗi nếu chưa có đơn nào trong DB
            ViewBag.TotalRevenue = db.Bookings
                                     .Where(b => b.BookingStatus == 2)
                                     .Sum(b => (decimal?)b.TotalAmount) ?? 0;

            // 4. Lấy 5 đơn hàng mới nhất để hiển thị bảng tóm tắt
            var recentBookings = db.Bookings
                                   .OrderByDescending(b => b.BookingID)
                                   .Take(5)
                                   .ToList();

            return View(recentBookings);
        }
    }
}