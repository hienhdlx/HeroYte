using FluentValidation;
using NCSw.HERO.Core.Domain.Customers;
using NCSw.HERO.Services.Localization;
using NCSw.HERO.Web.Framework.Validators;
using NCSw.HERO.Web.Models.Customer;

namespace NCSw.HERO.Web.Validators.Customer
{
    public partial class LoginValidator : BaseNopValidator<LoginModel>
    {
        public LoginValidator(ILocalizationService localizationService, CustomerSettings customerSettings)
        {
            if (!customerSettings.UsernamesEnabled)
            {
                //login by email
                RuleFor(x => x.Email).NotEmpty().WithMessage(localizationService.GetResource("Account.Login.Fields.Email.Required"));
                RuleFor(x => x.Email).EmailAddress().WithMessage(localizationService.GetResource("Common.WrongEmail"));
            }
        }
    }
}