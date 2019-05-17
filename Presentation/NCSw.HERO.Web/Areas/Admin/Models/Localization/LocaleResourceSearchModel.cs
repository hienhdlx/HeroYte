﻿using NCSw.HERO.Web.Framework.Models;
using NCSw.HERO.Web.Framework.Mvc.ModelBinding;

namespace NCSw.HERO.Web.Areas.Admin.Models.Localization
{
    /// <summary>
    /// Represents a locale resource search model
    /// </summary>
    public partial class LocaleResourceSearchModel : BaseSearchModel
    {
        #region Properties
        
        public int LanguageId { get; set; }

        [NopResourceDisplayName("Admin.Configuration.Languages.Resources.SearchResourceName")]
        public string SearchResourceName { get; set; }

        [NopResourceDisplayName("Admin.Configuration.Languages.Resources.SearchResourceValue")]
        public string SearchResourceValue { get; set; }

        #endregion
    }
}