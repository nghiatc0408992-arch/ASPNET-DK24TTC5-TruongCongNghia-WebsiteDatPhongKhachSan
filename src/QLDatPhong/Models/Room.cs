using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLDatPhong.Models
{
    public class Room
    {
        [Key]
        public int RoomID { get; set; }

        [Required]
        [Display(Name = "Số phòng")]
        public string RoomNumber { get; set; }

        [Display(Name = "Loại phòng")]
        public int RoomTypeID { get; set; }

        [Display(Name = "Trạng thái")]
        // Trạng thái: 0 - Trống, 1 - Đang dọn, 2 - Bảo trì
        public int Status { get; set; }

        // Navigation properties
        [ForeignKey("RoomTypeID")]
        public virtual RoomType RoomType { get; set; }
        public virtual ICollection<BookingDetail> BookingDetails { get; set; }
    }
}