﻿using System;
using NCSw.HERO.Web.Framework.Models;

namespace NCSw.HERO.Web.Areas.Admin.Models.Orders
{
    /// <summary>
    /// Represents a shipment event model
    /// </summary>
    public partial class ShipmentStatusEventModel : BaseNopModel
    {
        #region Properties

        public string EventName { get; set; }

        public string Location { get; set; }

        public string Country { get; set; }

        public DateTime? Date { get; set; }

        #endregion
    }
}