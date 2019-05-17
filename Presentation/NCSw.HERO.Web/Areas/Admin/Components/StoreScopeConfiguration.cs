using Microsoft.AspNetCore.Mvc;
using NCSw.HERO.Web.Areas.Admin.Factories;
using NCSw.HERO.Web.Framework.Components;

namespace NCSw.HERO.Web.Areas.Admin.Components
{
    /// <summary>
    /// Represents a view component that displays the store scope configuration
    /// </summary>
    public class StoreScopeConfigurationViewComponent : NopViewComponent
    {
        #region Fields

        private readonly ISettingModelFactory _settingModelFactory;

        #endregion

        #region Ctor

        public StoreScopeConfigurationViewComponent(ISettingModelFactory settingModelFactory)
        {
            this._settingModelFactory = settingModelFactory;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Invoke view component
        /// </summary>
        /// <returns>View component result</returns>
        public IViewComponentResult Invoke()
        {
            //prepare model
            var model = _settingModelFactory.PrepareStoreScopeConfigurationModel();

            if (model.Stores.Count < 2)
                return Content(string.Empty);
            
            return View(model);
        }

        #endregion
    }
}