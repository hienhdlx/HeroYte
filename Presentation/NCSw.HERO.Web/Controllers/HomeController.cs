using Microsoft.AspNetCore.Mvc;
using NCSw.HERO.Web.Framework.Mvc.Filters;
using NCSw.HERO.Web.Framework.Security;

namespace NCSw.HERO.Web.Controllers
{
    public partial class HomeController : BasePublicController
    {
        [HttpsRequirement(SslRequirement.No)]
        public virtual IActionResult Index()
        {
            return RedirectToAction("Index", "Home", new { area = "Admin" });
        }
    }
}