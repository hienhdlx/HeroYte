using FluentValidation;
using FluentValidation.Validators;
using NCSw.HERO.Services.Localization;
using NCSw.HERO.Web.Areas.Admin.Models;
using NCSw.HERO.Web.Framework.Validators;

namespace NCSw.HERO.Web.Areas.Admin.Validators
{
    public partial class ServiceValidator : BaseNopValidator<ServiceModel>
    {
        public ServiceValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Code).NotEmpty()
                .WithMessage(string.Format(localizationService.GetResource("Hero.Validators.InputFields.Required"), localizationService.GetResource("Hero.Admin.Services.Fields.Code")));
            RuleFor(x => x.Code).SetValidator(new MaximumLengthValidator(50))
                .WithMessage(string.Format(localizationService.GetResource("Hero.Validators.Characters.MaxLength"), localizationService.GetResource("Hero.Admin.Services.Fields.Code"), 50));

            RuleFor(x => x.Name).NotEmpty()
                .WithMessage(string.Format(localizationService.GetResource("Hero.Validators.InputFields.Required"), localizationService.GetResource("Hero.Admin.Services.Fields.Name")));
            RuleFor(x => x.Name).SetValidator(new MaximumLengthValidator(255))
                .WithMessage(string.Format(localizationService.GetResource("Hero.Validators.Characters.MaxLength"), localizationService.GetResource("Hero.Admin.Services.Fields.Name"), 255));

            RuleFor(x => x.Description).SetValidator(new MaximumLengthValidator(500))
                .WithMessage(string.Format(localizationService.GetResource("Hero.Validators.Characters.MaxLength"), localizationService.GetResource("Hero.Admin.Services.Fields.Description"), 500));

            RuleFor(x => x.ProcessTime).GreaterThan(0)
                .WithMessage(string.Format(localizationService.GetResource("Hero.Validators.Objects.GreaterThan"), localizationService.GetResource("Hero.Admin.Services.Fields.DissolvedDate"), localizationService.GetResource("Hero.Common.Fields.ProcessTime")));

            RuleFor(x => x.DisplayOrder).NotNull()
                .WithMessage(string.Format(localizationService.GetResource("Hero.Validators.InputFields.Required"), localizationService.GetResource("Hero.Common.Fields.DisplayOrder")));
        }
    }
}
