using Microsoft.AspNetCore.Mvc.Rendering;

namespace ePTS.Web.Extensions
{
    public static class HtmlHelperExtensions
    {
        public static string IsActive(this IHtmlHelper htmlHelper, string? area, string controller, string action)
        {
            var routeData = htmlHelper.ViewContext.RouteData;

            var routeAction = (string?)routeData.Values["action"];
            var routeController = (string?)routeData.Values["controller"];
            var routeArea = (string?)routeData.Values["area"];

            var isActive = string.Equals(controller, routeController, StringComparison.OrdinalIgnoreCase)
                && string.Equals(action, routeAction, StringComparison.OrdinalIgnoreCase)
                && string.Equals(area, routeArea, StringComparison.OrdinalIgnoreCase);

            return isActive ? "active" : "";
        }

    }
}
