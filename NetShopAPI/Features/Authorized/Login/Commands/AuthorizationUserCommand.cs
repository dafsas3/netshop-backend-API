namespace NetShopAPI.Features.Authorized.Login.Query
{
    public class AuthorizationUserCommand
    {
        public string Email { get; set; } = null!;
        public string? Phone { get; set; }
        public string Password { get; set; } = null!;
    }
}
