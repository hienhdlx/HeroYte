using FluentValidation.Attributes;
using Microsoft.AspNetCore.Mvc.Rendering;
using NCSw.HERO.Core.Domain.Common;
using NCSw.HERO.Web.Areas.Admin.Validators;
using NCSw.HERO.Web.Framework.Models;
using NCSw.HERO.Web.Framework.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NCSw.HERO.Web.Areas.Admin.Models
{
    [Validator(typeof(DoctorValidator))]
    public partial class DoctorModel : BaseNopEntityModel
    {
        #region Ctor

        #endregion

        /// <summary>
        /// Mã bác sĩ
        /// </summary>
        [NopResourceDisplayName("Hero.Admin.Doctors.Fields.Code")]
        public string Code { get; set; }

        /// <summary>
        /// Tên bác sĩ (Đa ngôn ngữ)
        /// </summary>
        [NopResourceDisplayName("Hero.Admin.Doctors.Fields.FullName")]
        public string FullName { get; set; }

        /// <summary>
        /// Ngày sinh bác sĩ (Đa ngôn ngữ)
        /// </summary>
        [UIHint("DateNullable")]
        [NopResourceDisplayName("Hero.Admin.Doctors.Fields.BirthDay")]
        public DateTime BirthDay { get; set; }

        /// <summary>
        /// Giới tính (Đa ngôn ngữ)
        /// </summary>
        [NopResourceDisplayName("Hero.Admin.Doctors.Fields.Gender")]
        public int Gender { get; set; }
        public IList<SelectListItem> AvailableGenders { get; set; }

        /// <summary>
        /// CMND (Đa ngôn ngữ)
        /// </summary>
        [NopResourceDisplayName("Hero.Admin.Doctors.Fields.IdentityCard")]
        public string IdentityCard { get; set; }

        /// <summary>
        /// Email (Đa ngôn ngữ)
        /// </summary>
        [NopResourceDisplayName("Hero.Admin.Doctors.Fields.Email")]
        public string Email { get; set; }

        /// <summary>
        /// Số điện thoại (Đa ngôn ngữ)
        /// </summary>
        [NopResourceDisplayName("Hero.Admin.Doctors.Fields.PhoneNumber")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Địa chỉ (Đa ngôn ngữ)
        /// </summary>
        [NopResourceDisplayName("Hero.Admin.Doctors.Fields.Address")]
        public string Address { get; set; }
    }
}
