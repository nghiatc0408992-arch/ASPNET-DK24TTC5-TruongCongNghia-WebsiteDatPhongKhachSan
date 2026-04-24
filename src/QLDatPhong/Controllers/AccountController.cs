using System.Linq;
using System.Web.Mvc;
using System.Web.Security; // Thư viện bắt buộc cho FormsAuthentication
using QLDatPhong.Models;

namespace QLDatPhong.Controllers
{
    public class AccountController : Controller
    {
        private HotelDbContext db = new HotelDbContext();

        // GET: Account/Login
        public ActionResult Login()
        {
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string Username, string Password)
        {
            if (ModelState.IsValid)
            {
                // Tìm user trong DB (Lưu ý: Thực tế đồ án nên dùng mã hóa MD5 cho Password)
                var user = db.Accounts.FirstOrDefault(a => a.Username == Username && a.Password == Password);

                if (user != null)
                {
                    // Đăng nhập thành công: Lưu Cookie
                    FormsAuthentication.SetAuthCookie(user.Username, false);

                    // Bạn có thể lưu thêm Role vào Session để hiển thị menu phù hợp
                    Session["UserRole"] = user.Role;
                    Session["UserName"] = user.Username;

                    return RedirectToAction("Dashboard", "Admin");
                }
                else
                {
                    ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng.");
                }
            }
            return View();
        }

        // GET: Account/Logout
        public ActionResult Logout()
        {
            // Xóa Cookie và Session
            FormsAuthentication.SignOut();
            Session.Clear();
            return RedirectToAction("Login", "Account");
        }
    }
}