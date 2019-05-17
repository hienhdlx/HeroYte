using Microsoft.AspNetCore.Mvc;
using NCSw.HERO.Web.Framework.Controllers;
using NCSw.HERO.Web.Framework.Mvc.Filters;
using NCSw.HERO.Web.Framework.Security;

namespace NCSw.HERO.Web.Controllers
{
    [HttpsRequirement(SslRequirement.NoMatter)]
    [WwwRequirement]
    [CheckAccessPublicStore]
    [CheckAccessClosedStore]
    [CheckLanguageSeoCode]
    [CheckDiscountCoupon]
    [CheckAffiliate]
    public abstract partial class BasePublicController : BaseController
    {
        protected virtual IActionResult InvokeHttp404()
        {
            Response.StatusCode = 404;
            return new EmptyResult();
        }
    }
}