using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace LibreriaVirtual.Filter
{
    public class AuthorizeUsuariosAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;

            var id = context.RouteData.Values["id"];
            var personal = context.RouteData.Values["personal"];
            var favs = context.RouteData.Values["favs"];

            ITempDataProvider provider = context.HttpContext.RequestServices.GetService<ITempDataProvider>();
            var temp = provider.LoadTempData(context.HttpContext);

            if (id != null)
            {
                temp["id"] = id.ToString();
            }
            else
            {
                temp.Remove("id");
            }

            if (personal != null)
            {
                temp["personal"] = personal.ToString();
            }
            else
            {
                temp.Remove("personal");
            }

            if (favs != null)
            {
                temp["favs"] = favs.ToString();
            }
            else
            {
                temp.Remove("favs");
            }

            provider.SaveTempData(context.HttpContext, temp);

            if (!user.Identity.IsAuthenticated)
            {
                context.Result = GetRoute("Account", "Login");
            }
        }

        private RedirectToRouteResult GetRoute(string controller, string action)
        {
            RouteValueDictionary ruta = new RouteValueDictionary
                (
                    new
                    {
                        controller = controller,
                        action = action
                    }
                );
            RedirectToRouteResult result = new RedirectToRouteResult(ruta);
            return result;
        }
    }
}
