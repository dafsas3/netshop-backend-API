using Microsoft.AspNetCore.Mvc;

namespace NetShopAPI.Common
{
    public static class ResultToActionResult
    {

        private static ApiError Internal() => new("INTERNAL_SERVER_ERROR", "Внутренняя ошибка сервера!");

        public static IActionResult ToActionResult<T>(this ControllerBase controller, Result<T> result)
        {
            return result.Status switch
            {
                ResultStatus.Ok => result.Data is null
                ? controller.NoContent()
                : controller.Ok(result.Data),

                ResultStatus.Created => result.Data is null
                ? controller.StatusCode(500, Internal())
                : controller.Created("", result.Data),


                ResultStatus.BadRequest => controller.BadRequest(result.Error ?? Internal()),
                ResultStatus.NotFound => controller.NotFound(result.Error ?? Internal()),
                ResultStatus.Conflict => controller.Conflict(result.Error ?? Internal()),
                ResultStatus.Unauthorized => controller.Unauthorized(result.Error ?? Internal()),

                _ => controller.StatusCode(500, Internal())
            };
        }
    }
}