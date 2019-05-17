using Microsoft.AspNetCore.Mvc;

namespace NCSw.HERO.Web.Areas.Admin.Controllers
{
    public partial class FunctionController : BaseAdminController
    {
        #region Const


        #endregion

        #region Fields


        #endregion

        #region Ctor

        
        #endregion

        #region Utilities
        
        #endregion

        #region Actions

        public virtual IActionResult Index()
        {
            return View();
        }

        #endregion
    }
}