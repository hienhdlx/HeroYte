using FluentValidation;
using FluentValidation.Validators;
using NCSw.HERO.Services.Localization;
using NCSw.HERO.Web.Areas.Admin.Models;
using NCSw.HERO.Web.Framework.Validators;

namespace NCSw.HERO.Web.Areas.Admin.Validators
{
    public partial class DepartmentValidator : BaseNopValidator<DepartmentModel>
    {
        public DepartmentValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Code).NotEmpty()
                .WithMessage(string.Format(localizationService.GetResource("Hero.Validators.InputFields.Required"), localizationService.GetResource("Hero.Admin.Department.Fields.Code")));
            RuleFor(x => x.Code).SetValidator(new MaximumLengthValidator(50))
                .WithMessage(string.Format(localizationService.GetResource("Hero.Validators.Characters.MaxLength"), localizationService.GetResource("Hero.Admin.Department.Fields.Code"), 50));

            RuleFor(x => x.Name).NotEmpty()
                .WithMessage(string.Format(localizationService.GetResource("Hero.Validators.InputFields.Required"), localizationService.GetResource("Hero.Admin.Department.Fields.Name")));
            RuleFor(x => x.Name).SetValidator(new MaximumLengthValidator(255))
                .WithMessage(string.Format(localizationService.GetResource("Hero.Validators.Characters.MaxLength"), localizationService.GetResource("Hero.Admin.Department.Fields.Name"), 255));

            RuleFor(x => x.Description).SetValidator(new MaximumLengthValidator(500))
                .WithMessage(string.Format(localizationService.GetResource("Hero.Validators.Characters.MaxLength"), localizationService.GetResource("Hero.Admin.Department.Fields.Description"), 500));
        }
    }
}
