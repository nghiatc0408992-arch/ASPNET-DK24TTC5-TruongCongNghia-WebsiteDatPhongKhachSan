using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLDatPhong.Models
{
    public class BookingDetail
    {
        [Key]
        public int BookingDetailID { get; set; }

        public int BookingID { get; set; }
        public int RoomID { get; set; }

        [Display(Name = "Giá tại thời điểm đặt")]
        public decimal Price { get; set; }

        // Navigation properties
        [ForeignKey("BookingID")]
        public virtual Booking Booking { get; set; }

        [ForeignKey("RoomID")]
        public virtual Room Room { get; set; }
    }
}