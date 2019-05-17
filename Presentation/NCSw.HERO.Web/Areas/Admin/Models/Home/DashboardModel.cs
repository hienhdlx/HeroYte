using NCSw.HERO.Web.Areas.Admin.Models.Common;
using NCSw.HERO.Web.Areas.Admin.Models.Reports;
using NCSw.HERO.Web.Framework.Models;

namespace NCSw.HERO.Web.Areas.Admin.Models.Home
{
    /// <summary>
    /// Represents a dashboard model
    /// </summary>
    public partial class DashboardModel : BaseNopModel
    {
        #region Ctor

        public DashboardModel()
        {
            this.PopularSearchTerms = new PopularSearchTermSearchModel();
            this.BestsellersByAmount = new BestsellerBriefSearchModel();
            this.BestsellersByQuantity = new BestsellerBriefSearchModel();
        }

        #endregion

        #region Properties

        public bool IsLoggedInAsVendor { get; set; }

        public PopularSearchTermSearchModel PopularSearchTerms { get; set; }

        public BestsellerBriefSearchModel BestsellersByAmount { get; set; }

        public BestsellerBriefSearchModel BestsellersByQuantity { get; set; }

        #endregion
    }
}