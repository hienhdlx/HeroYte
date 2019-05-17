using System.Collections.Generic;
using NCSw.HERO.Web.Areas.Admin.Models.Directory;
using NCSw.HERO.Web.Framework.Models;

namespace NCSw.HERO.Web.Areas.Admin.Models.Payments
{
    /// <summary>
    /// Represents a payment method restriction model
    /// </summary>
    public partial class PaymentMethodRestrictionModel : BaseNopModel
    {
        #region Ctor

        public PaymentMethodRestrictionModel()
        {
            AvailablePaymentMethods = new List<PaymentMethodModel>();
            AvailableCountries = new List<CountryModel>();
            Resticted = new Dictionary<string, IDictionary<int, bool>>();
        }

        #endregion

        #region Properties

        public IList<PaymentMethodModel> AvailablePaymentMethods { get; set; }

        public IList<CountryModel> AvailableCountries { get; set; }

        //[payment method system name] / [customer role id] / [restricted]
        public IDictionary<string, IDictionary<int, bool>> Resticted { get; set; }

        #endregion
    }
}