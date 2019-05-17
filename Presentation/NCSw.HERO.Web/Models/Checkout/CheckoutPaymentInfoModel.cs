using NCSw.HERO.Web.Framework.Models;

namespace NCSw.HERO.Web.Models.Checkout
{
    public partial class CheckoutPaymentInfoModel : BaseNopModel
    {
        public string PaymentViewComponentName { get; set; }

        /// <summary>
        /// Used on one-page checkout page
        /// </summary>
        public bool DisplayOrderTotals { get; set; }
    }
}