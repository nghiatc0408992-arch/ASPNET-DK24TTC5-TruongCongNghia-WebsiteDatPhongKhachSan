using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QLDatPhong.Models
{
    public class RoomType
    {
        [Key]
        public int RoomTypeID { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên loại phòng")]
        [Display(Name = "Tên loại phòng")]
        public string TypeName { get; set; }

        [Display(Name = "Mô tả")]
        public string Description { get; set; }

        [Display(Name = "Giá cơ bản")]
        public decimal BasePrice { get; set; }

        [Display(Name = "Số khách tối đa")]
        public int MaxGuests { get; set; }

        // Navigation property
        public virtual ICollection<Room> Rooms { get; set; }
    }
}