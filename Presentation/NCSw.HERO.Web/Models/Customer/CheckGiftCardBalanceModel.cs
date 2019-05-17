using FluentValidation.Attributes;
using NCSw.HERO.Web.Framework.Models;
using NCSw.HERO.Web.Framework.Mvc.ModelBinding;
using NCSw.HERO.Web.Validators.Customer;
using System.ComponentModel.DataAnnotations;

namespace NCSw.HERO.Web.Models.Customer
{
    [Validator(typeof(GiftCardValidator))]
    public partial class CheckGiftCardBalanceModel : BaseNopModel
    {
        public string Result { get; set; }

        public string Message { get; set; }
        
        [NopResourceDisplayName("ShoppingCart.GiftCardCouponCode.Tooltip")]
        public string GiftCardCode { get; set; }
    }
}
