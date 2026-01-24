using System.Security.Claims;

namespace NetShopAPI.Services.CurrentUserServices
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _contexAccessor;

        public CurrentUserService(IHttpContextAccessor accessor)
        {
            _contexAccessor = accessor;
        }


        public bool IsAuthenticated => _contexAccessor.HttpContext?.User.Identity?.IsAuthenticated == true;

        public Guid? UserId
        {
            get
            {
                var idStr = _contexAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
                return Guid.TryParse(idStr, out var id) ? id : null;
            }
        }

        public string? Email => _contexAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Email);

    }
}
