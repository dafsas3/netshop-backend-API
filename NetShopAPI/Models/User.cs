using System.ComponentModel.DataAnnotations;


namespace NetShopAPI.Models
{
    public class User
    {
        public Guid Id { get; set; }

        [MaxLength(100)]
        public string FirstName { get; set; } = null!;

        [MaxLength(100)]
        public string LastName { get; set; } = null!;

        [MaxLength(320)]
        public string Email { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string SurName { get; set; } = null!;

        [Required]
        [MaxLength(60)]
        public string NickName { get; set; } = null!;

        [Phone]
        [MaxLength(30)]
        public string Phone {  get; set; }

        public string PasswordHash { get; set; } = null!;

        [MaxLength(30)]
        public string Role {  get; set; } = "User";

        public DateTime CreatedAtUtc { get; set; }
    }
}
