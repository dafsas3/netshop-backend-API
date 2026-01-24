namespace NetShopAPI.Common
{
    public record ApiError(string Code, string Message, string? TraceId = null);
}
