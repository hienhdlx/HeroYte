using Microsoft.AspNetCore.Mvc;
using NCSw.HERO.Web.Areas.Admin.Factories;
using NCSw.HERO.Web.Framework.Components;

namespace NCSw.HERO.Web.Areas.Admin.Components
{
    /// <summary>
    /// Represents a view component that displays the nopCommerce news
    /// </summary>
    public class NopCommerceNewsViewComponent : NopViewComponent
    {
        #region Fields

        private readonly IHomeModelFactory _homeModelFactory;

        #endregion

        #region Ctor

        public NopCommerceNewsViewComponent(IHomeModelFactory homeModelFactory)
        {
            this._homeModelFactory = homeModelFactory;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Invoke view component
        /// </summary>
        /// <returns>View component result</returns>
        public IViewComponentResult Invoke()
        {
            try
            {
                //prepare model
                var model = _homeModelFactory.PrepareNopCommerceNewsModel();

                return View(model);
            }
            catch
            {
                return Content(string.Empty);
            }
        }

        #endregion
    }
}