using System;
using System.Linq;
using System.Web.Mvc;
using QLDatPhong.Models;
using System.Data.Entity;

namespace QLDatPhong.Controllers
{
    [Authorize]
    public class BookingController : Controller
    {
        private HotelDbContext db = new HotelDbContext();

        // GET: Booking/Index (Dành cho Admin xem danh sách)
        public ActionResult Index()
        {
            // Lấy danh sách đơn hàng mới nhất lên đầu
            var bookings = db.Bookings.OrderByDescending(b => b.BookingID).ToList();
            return View(bookings);
        }

        // GET: Booking/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            // Lấy đơn hàng cùng với các bảng liên quan (Chi tiết, Phòng, Loại phòng)
            var booking = db.Bookings
                .Include(b => b.BookingDetails.Select(bd => bd.Room.RoomType))
                .FirstOrDefault(b => b.BookingID == id);

            if (booking == null)
            {
                return HttpNotFound();
            }

            return View(booking);
        }

        // POST: Booking/UpdateStatus
        [HttpPost]
        public ActionResult UpdateStatus(int id, int status)
        {
            var booking = db.Bookings.Find(id);
            if (booking != null)
            {
                booking.BookingStatus = status;

                // Nếu chuyển sang trạng thái "Hoàn thành" (2) hoặc "Hủy" (3)
                // Bạn có thể viết thêm logic cập nhật lại trạng thái phòng (Room.Status) tại đây

                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        // GET: Booking/Create
        public ActionResult Create(int roomTypeId, string checkIn, string checkOut)
        {
            // Chuyển đổi string date sang DateTime
            DateTime dIn = DateTime.Parse(checkIn);
            DateTime dOut = DateTime.Parse(checkOut);

            // Tìm loại phòng để lấy giá
            var roomType = db.RoomTypes.Find(roomTypeId);

            // Tính số đêm
            int nights = (dOut - dIn).Days;
            if (nights <= 0) nights = 1;

            // Truyền dữ liệu sang View để hiển thị
            ViewBag.RoomTypeName = roomType.TypeName;
            ViewBag.BasePrice = roomType.BasePrice;
            ViewBag.Nights = nights;
            ViewBag.Total = nights * roomType.BasePrice;

            var model = new Booking
            {
                CheckInDate = dIn,
                CheckOutDate = dOut,
                TotalAmount = nights * roomType.BasePrice
            };

            // Lưu RoomTypeId vào hidden field hoặc ViewBag để dùng khi POST
            ViewBag.RoomTypeId = roomTypeId;

            return View(model);
        }

        // POST: Booking/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Booking booking, int roomTypeId)
        {
            if (ModelState.IsValid)
            {
                // Bước quan trọng: Tìm 1 phòng trống cụ thể thuộc loại phòng này
                // Thuật toán: Giống trang Search nhưng lấy ra Top 1
                var busyRoomIds = db.BookingDetails
                    .Where(bd => bd.Booking.BookingStatus != 3
                              && bd.Booking.CheckInDate < booking.CheckOutDate
                              && bd.Booking.CheckOutDate > booking.CheckInDate)
                    .Select(bd => bd.RoomID).ToList();

                var availableRoom = db.Rooms
                    .FirstOrDefault(r => r.RoomTypeID == roomTypeId
                                      && r.Status == 0
                                      && !busyRoomIds.Contains(r.RoomID));

                if (availableRoom == null)
                {
                    ModelState.AddModelError("", "Rất tiếc, loại phòng này vừa mới hết phòng trống!");
                    return View(booking);
                }

                // Lưu Đơn đặt phòng (Booking)
                booking.BookingStatus = 0; // Chờ duyệt
                db.Bookings.Add(booking);
                db.SaveChanges();

                // Lưu Chi tiết đặt phòng (BookingDetail)
                var detail = new BookingDetail
                {
                    BookingID = booking.BookingID,
                    RoomID = availableRoom.RoomID,
                    Price = availableRoom.RoomType.BasePrice
                };
                db.BookingDetails.Add(detail);
                db.SaveChanges();

                return RedirectToAction("Success");
            }
            return View(booking);
        }

        public ActionResult Success()
        {
            return View();
        }
    }
}