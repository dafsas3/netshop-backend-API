using System.ComponentModel.DataAnnotations;

namespace NetShopAPI.DTOs.User.ResponseDTO
{
    public class UserResponse
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string SurName { get; set; } = null!;
        public string NickName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Phone { get; set; }
        public string Role { get; set; } = null!;
    }
}
