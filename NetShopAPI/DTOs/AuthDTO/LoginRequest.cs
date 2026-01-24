using System.ComponentModel.DataAnnotations;

namespace NetShopAPI.DTOs.AuthDTO
{
    public class LoginRequest
    {
        public string Login { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
