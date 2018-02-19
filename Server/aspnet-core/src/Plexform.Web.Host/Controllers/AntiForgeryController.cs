using Microsoft.AspNetCore.Antiforgery;
using Plexform.Controllers;

namespace Plexform.Web.Host.Controllers
{
    public class AntiForgeryController : PlexformControllerBase
    {
        private readonly IAntiforgery _antiforgery;

        public AntiForgeryController(IAntiforgery antiforgery)
        {
            _antiforgery = antiforgery;
        }

        public void GetToken()
        {
            _antiforgery.SetCookieTokenAndHeader(HttpContext);
        }
    }
}
