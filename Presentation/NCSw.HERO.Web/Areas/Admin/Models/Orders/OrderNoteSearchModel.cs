﻿using NCSw.HERO.Web.Framework.Models;

namespace NCSw.HERO.Web.Areas.Admin.Models.Orders
{
    /// <summary>
    /// Represents an order note search model
    /// </summary>
    public partial class OrderNoteSearchModel : BaseSearchModel
    {
        #region Properties

        public  int OrderId { get; set; }

        #endregion
    }
}