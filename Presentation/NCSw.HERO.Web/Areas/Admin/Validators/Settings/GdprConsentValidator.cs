using FluentValidation;
using NCSw.HERO.Core.Domain.Gdpr;
using NCSw.HERO.Data;
using NCSw.HERO.Services.Localization;
using NCSw.HERO.Web.Areas.Admin.Models.Settings;
using NCSw.HERO.Web.Framework.Validators;

namespace NCSw.HERO.Web.Areas.Admin.Validators.Settings
{
    public partial class GdprConsentValidator : BaseNopValidator<GdprConsentModel>
    {
        public GdprConsentValidator(ILocalizationService localizationService, IDbContext dbContext)
        {
            RuleFor(x => x.Message).NotEmpty().WithMessage(localizationService.GetResource("Admin.Configuration.Settings.Gdpr.Consent.Message.Required"));
            RuleFor(x => x.RequiredMessage)
                .NotEmpty()
                .WithMessage(localizationService.GetResource("Admin.Configuration.Settings.Gdpr.Consent.RequiredMessage.Required"))
                .When(x => x.IsRequired);

            SetDatabaseValidationRules<GdprConsent>(dbContext);
        }
    }
}