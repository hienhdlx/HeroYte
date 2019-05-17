using System.Collections.Generic;
using NCSw.HERO.Web.Framework.Models;
using NCSw.HERO.Web.Models.Common;

namespace NCSw.HERO.Web.Models.Catalog
{
    public partial class CustomerBackInStockSubscriptionsModel : BaseNopModel
    {
        public CustomerBackInStockSubscriptionsModel()
        {
            this.Subscriptions = new List<BackInStockSubscriptionModel>();
        }

        public IList<BackInStockSubscriptionModel> Subscriptions { get; set; }
        public PagerModel PagerModel { get; set; }

        #region Nested classes

        public partial class BackInStockSubscriptionModel : BaseNopEntityModel
        {
            public int ProductId { get; set; }
            public string ProductName { get; set; }
            public string SeName { get; set; }
        }

        #endregion
    }
}