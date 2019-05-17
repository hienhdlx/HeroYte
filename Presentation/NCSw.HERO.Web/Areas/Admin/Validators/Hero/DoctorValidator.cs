using FluentValidation;
using FluentValidation.Validators;
using NCSw.HERO.Services.Localization;
using NCSw.HERO.Web.Areas.Admin.Models;
using NCSw.HERO.Web.Framework.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NCSw.HERO.Web.Areas.Admin.Validators
{
    public partial class DoctorValidator : BaseNopValidator<DoctorModel>
    {
        public DoctorValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Code).NotEmpty()
                .WithMessage(string.Format(localizationService.GetResource("Hero.Validators.InputFields.Required"), localizationService.GetResource("Hero.Admin.Doctors.Fields.Code")));
            RuleFor(x => x.Code).SetValidator(new MaximumLengthValidator(50))
                .WithMessage(string.Format(localizationService.GetResource("Hero.Validators.Characters.MaxLength"), localizationService.GetResource("Hero.Admin.Doctors.Fields.Code"), 50));

            RuleFor(x => x.FullName).NotEmpty()
                .WithMessage(string.Format(localizationService.GetResource("Hero.Validators.InputFields.Required"), localizationService.GetResource("Hero.Admin.Doctors.Fields.FullName")));
            RuleFor(x => x.FullName).SetValidator(new MaximumLengthValidator(255))
                .WithMessage(string.Format(localizationService.GetResource("Hero.Validators.Characters.MaxLength"), localizationService.GetResource("Hero.Admin.Doctors.Fields.FullName"), 255));

            RuleFor(x => x.IdentityCard).NotEmpty()
                .WithMessage(string.Format(localizationService.GetResource("Hero.Validators.InputFields.Required"), localizationService.GetResource("Hero.Admin.Doctors.Fields.IdentityCard")));
            RuleFor(x => x.IdentityCard).SetValidator(new MaximumLengthValidator(50))
                .WithMessage(string.Format(localizationService.GetResource("Hero.Validators.Characters.MaxLength"), localizationService.GetResource("Hero.Admin.Doctors.Fields.IdentityCard"), 50));

            RuleFor(x => x.PhoneNumber).NotEmpty()
                .WithMessage(string.Format(localizationService.GetResource("Hero.Validators.InputFields.Required"), localizationService.GetResource("Hero.Admin.Doctors.Fields.PhoneNumber")));
            RuleFor(x => x.PhoneNumber).SetValidator(new MaximumLengthValidator(255))
                .WithMessage(string.Format(localizationService.GetResource("Hero.Validators.Characters.MaxLength"), localizationService.GetResource("Hero.Admin.Doctors.Fields.PhoneNumber"), 255));

            RuleFor(x => x.Email)
                .EmailAddress()
                .WithMessage(string.Format(localizationService.GetResource("Hero.Validators.InputFields.IsValid"), localizationService.GetResource("Hero.Admin.Doctors.Fields.Email")));

            RuleFor(x => x.IdentityCard).Must((x, context) =>
            {
                return IsOnlyNumber(x.IdentityCard);
            }).WithMessage(string.Format(localizationService.GetResource("Hero.Validators.InputFields.IsOnlyNumber"), localizationService.GetResource("Hero.Admin.Doctors.Fields.IdentityCard")));

            RuleFor(x => x.Code).Must((x, context) =>
            {
                return IsOnlyNumberAndLetters(x.Code);
            }).WithMessage(string.Format(localizationService.GetResource("Hero.Validators.InputFields.IsOnlyNumberAndLetters"), localizationService.GetResource("Hero.Admin.Doctors.Fields.Code")));
        }

        public static bool IsOnlyNumber(string value)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(value, @"^[0-9-]*$"))
                //nếu hợp lệ return true còn ngược lại thì false
                return true;
            return false;
        }
        public static bool IsOnlyNumberAndLetters(string value)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(value, @"[^A-Za-z0-9]+"))
                //nếu hợp lệ return true còn ngược lại thì false
                return true;
            return false;
        }
    }
}
