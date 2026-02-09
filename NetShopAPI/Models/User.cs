using System.ComponentModel.DataAnnotations;


namespace NetShopAPI.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string SurName { get; set; } = null!;
        public string NickName { get; set; } = null!;
        public string? Phone { get; set; }
        public string PasswordHash { get; set; } = null!;
        public string Role {  get; set; } = "User";
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
        public DateTime? LastAuthorizedtUtc { get; set; }
    }
}
