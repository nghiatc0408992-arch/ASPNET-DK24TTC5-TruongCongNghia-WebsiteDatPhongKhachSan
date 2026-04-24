using QLDatPhong.Models; // Gọi namespace Models để dùng DbContext
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace QLDatPhong.Controllers
{
    public class HomeController : Controller
    {
        // Khởi tạo DbContext
        private HotelDbContext db = new HotelDbContext();

        public ActionResult Index()
        {
            // Lấy danh sách loại phòng, truyền sang View
            var roomTypes = db.RoomTypes.ToList();
            return View(roomTypes);
        }

        [HttpGet]
        public ActionResult Search(DateTime checkIn, DateTime checkOut)
        {
            // Kiểm tra hợp lệ: Ngày trả phải sau ngày nhận
            if (checkIn >= checkOut)
            {
                ViewBag.ErrorMessage = "Ngày trả phòng phải sau ngày nhận phòng!";
                return View(new List<RoomType>()); // Trả về danh sách rỗng
            }

            // 1. Tìm các ID phòng "đang bận" trong khoảng thời gian khách chọn
            // Điều kiện: Đơn hàng không bị hủy (Khác 3) VÀ thời gian giao nhau
            var busyRoomIds = db.BookingDetails
                .Where(bd => bd.Booking.BookingStatus != 3
                          && bd.Booking.CheckInDate < checkOut
                          && bd.Booking.CheckOutDate > checkIn)
                .Select(bd => bd.RoomID)
                .Distinct()
                .ToList();

            // 2. Lấy danh sách các phòng "Trống" 
            // Điều kiện: Trạng thái phòng là 0 (Sẵn sàng) VÀ không nằm trong danh sách bận
            var availableRooms = db.Rooms
                .Where(r => r.Status == 0 && !busyRoomIds.Contains(r.RoomID))
                .ToList();

            // 3. Gom nhóm lại thành Loại Phòng (RoomType) để hiển thị ra giao diện cho khách chọn
            var availableRoomTypeIds = availableRooms.Select(r => r.RoomTypeID).Distinct().ToList();

            var roomTypes = db.RoomTypes
                .Where(rt => availableRoomTypeIds.Contains(rt.RoomTypeID))
                .ToList();

            // Lưu lại thông tin vào ViewBag để hiển thị trên View và mang sang trang Đặt phòng
            ViewBag.CheckIn = checkIn.ToString("yyyy-MM-dd");
            ViewBag.CheckOut = checkOut.ToString("yyyy-MM-dd");
            ViewBag.TotalAvailable = availableRooms.Count;

            return View(roomTypes);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }
    }
}