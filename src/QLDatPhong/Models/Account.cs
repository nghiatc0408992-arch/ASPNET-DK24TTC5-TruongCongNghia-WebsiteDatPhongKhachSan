using System.ComponentModel.DataAnnotations;

namespace QLDatPhong.Models
{
    public class Account
    {
        [Key]
        public int AccountID { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tài khoản")]
        [Display(Name = "Tên đăng nhập")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }

        [Display(Name = "Vai trò")]
        // Vai trò có thể là "Admin" hoặc "Employee"
        public string Role { get; set; }
    }
}