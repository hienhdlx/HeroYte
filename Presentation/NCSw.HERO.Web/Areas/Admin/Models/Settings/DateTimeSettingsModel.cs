﻿using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using NCSw.HERO.Web.Framework.Models;
using NCSw.HERO.Web.Framework.Mvc.ModelBinding;

namespace NCSw.HERO.Web.Areas.Admin.Models.Settings
{
    /// <summary>
    /// Represents a date time settings model
    /// </summary>
    public partial class DateTimeSettingsModel : BaseNopModel, ISettingsModel
    {
        #region Ctor

        public DateTimeSettingsModel()
        {
            AvailableTimeZones = new List<SelectListItem>();
        }

        #endregion

        #region Properties

        public int ActiveStoreScopeConfiguration { get; set; }

        [NopResourceDisplayName("Admin.Configuration.Settings.CustomerUser.AllowCustomersToSetTimeZone")]
        public bool AllowCustomersToSetTimeZone { get; set; }

        [NopResourceDisplayName("Admin.Configuration.Settings.CustomerUser.DefaultStoreTimeZone")]
        public string DefaultStoreTimeZoneId { get; set; }

        [NopResourceDisplayName("Admin.Configuration.Settings.CustomerUser.DefaultStoreTimeZone")]
        public IList<SelectListItem> AvailableTimeZones { get; set; }

        #endregion
    }
}