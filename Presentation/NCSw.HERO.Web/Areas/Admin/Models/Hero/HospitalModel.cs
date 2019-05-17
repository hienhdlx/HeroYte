using FluentValidation.Attributes;
using NCSw.HERO.Web.Areas.Admin.Models.Customers;
using NCSw.HERO.Web.Areas.Admin.Validators;
using NCSw.HERO.Web.Framework.Models;
using NCSw.HERO.Web.Framework.Mvc.ModelBinding;
using System;
using System.Collections.Generic;

namespace NCSw.HERO.Web.Areas.Admin.Models
{
    [Validator(typeof(HospitalValidator))]
    public partial class HospitalModel : BaseNopEntityModel, ILocalizedModel<HospitalLocalizedModel>
    {
        /// <summary>
        /// Mã CSYT
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Tên CSYT (Đa ngôn ngữ)
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Logo
        /// </summary>
        public string Logo { get; set; }

        /// <summary>
        /// Mô tả/Giới thiệu (Đa ngôn ngữ)
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Địa chỉ
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Kích hoạt
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Đã xóa
        /// </summary>
        public bool Deleted { get; set; }

        /// <summary>
        /// Người tạo
        /// </summary>
        public int CreatedBy { get; set; }

        /// <summary>
        /// Ngày tạo
        /// </summary>
        public DateTime CreatedOnUtc { get; set; }

        /// <summary>
        /// Người sửa
        /// </summary>
        public int? UpdatedBy { get; set; }

        /// <summary>
        /// Ngày sửa
        /// </summary>
        public DateTime? UpdatedOnUtc { get; set; }

        /// <summary>
        /// Người tạo
        /// </summary>
        public CustomerModel CreatedByUser { get; set; }

        /// <summary>
        /// Người sửa
        /// </summary>
        public CustomerModel UpdatedByUser { get; set; }

        public IList<HospitalLocalizedModel> Locales { get; set; }

        public HospitalModel()
        {
            Locales = new List<HospitalLocalizedModel>();
        }
    }

    public partial class HospitalLocalizedModel : ILocalizedLocaleModel
    {
        public int LanguageId { get; set; }

        [NopResourceDisplayName("Hero.Admin.Hospitals.Fields.Name")]
        public string Name { get; set; }

        [NopResourceDisplayName("Hero.Admin.Hospitals.Fields.Description")]
        public string Description { get; set; }
    }
}
