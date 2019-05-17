using FluentValidation;
using NCSw.HERO.Core.Domain.Common;
using NCSw.HERO.Services.Localization;
using NCSw.HERO.Web.Framework.Validators;
using NCSw.HERO.Web.Models.Common;

namespace NCSw.HERO.Web.Validators.Common
{
    public partial class ContactVendorValidator : BaseNopValidator<ContactVendorModel>
    {
        public ContactVendorValidator(ILocalizationService localizationService, CommonSettings commonSettings)
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage(localizationService.GetResource("ContactVendor.Email.Required"));
            RuleFor(x => x.Email).EmailAddress().WithMessage(localizationService.GetResource("Common.WrongEmail"));
            RuleFor(x => x.FullName).NotEmpty().WithMessage(localizationService.GetResource("ContactVendor.FullName.Required"));
            if (commonSettings.SubjectFieldOnContactUsForm)
            {
                RuleFor(x => x.Subject).NotEmpty().WithMessage(localizationService.GetResource("ContactVendor.Subject.Required"));
            }
            RuleFor(x => x.Enquiry).NotEmpty().WithMessage(localizationService.GetResource("ContactVendor.Enquiry.Required"));
        }
    }
}