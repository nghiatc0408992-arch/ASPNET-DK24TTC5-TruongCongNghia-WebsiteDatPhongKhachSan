using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QLDatPhong.Models
{
    public class Booking
    {
        [Key]
        public int BookingID { get; set; }

        [Required]
        [Display(Name = "Tên khách hàng")]
        public string CustomerName { get; set; }

        [Required]
        [Display(Name = "Số điện thoại")]
        public string CustomerPhone { get; set; }

        [Display(Name = "Ngày Check-in")]
        [DataType(DataType.Date)]
        public DateTime CheckInDate { get; set; }

        [Display(Name = "Ngày Check-out")]
        [DataType(DataType.Date)]
        public DateTime CheckOutDate { get; set; }

        [Display(Name = "Tổng tiền")]
        public decimal TotalAmount { get; set; }

        [Display(Name = "Trạng thái đơn")]
        // 0 - Chờ duyệt, 1 - Đã xác nhận, 2 - Hoàn thành, 3 - Đã hủy
        public int BookingStatus { get; set; }

        // Navigation property
        public virtual ICollection<BookingDetail> BookingDetails { get; set; }
    }
}