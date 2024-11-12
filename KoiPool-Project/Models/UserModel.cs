using System.ComponentModel.DataAnnotations;

namespace KoiPool_Project.Models
{
    public class UserModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên đăng nhập")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập email"), EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; }

        [DataType(DataType.Password), Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        public string Password { get; set; }

    }
}