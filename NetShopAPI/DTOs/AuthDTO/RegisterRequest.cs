using System.ComponentModel.DataAnnotations;

namespace NetShopAPI.DTOs.AuthDTO
{
    public class RegisterRequest
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string SurName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Phone { get; set; }
        public string NickName { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
