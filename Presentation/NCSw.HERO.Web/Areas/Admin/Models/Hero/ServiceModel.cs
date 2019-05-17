using FluentValidation.Attributes;
using NCSw.HERO.Web.Areas.Admin.Models.Customers;
using NCSw.HERO.Web.Areas.Admin.Validators;
using NCSw.HERO.Web.Framework.Models;
using NCSw.HERO.Web.Framework.Mvc.ModelBinding;
using System;
using System.Collections.Generic;

namespace NCSw.HERO.Web.Areas.Admin.Models
{
    [Validator(typeof(ServiceValidator))]
    public partial class ServiceModel : BaseNopEntityModel, ILocalizedModel<ServiceLocalizedModel>
    {
        /// <summary>
        /// Bệnh viện/CSYT (FK: Hospital)
        /// </summary>
        [NopResourceDisplayName("Hero.Admin.Services.Fields.Hospital")]
        public int HospitalId { get; set; }

        /// <summary>
        /// Mã dịch vụ
        /// </summary>
        [NopResourceDisplayName("Hero.Admin.Services.Fields.Code")]
        public string Code { get; set; }

        /// <summary>
        /// Tên dịch vụ (Đa ngôn ngữ)
        /// </summary>
        [NopResourceDisplayName("Hero.Admin.Services.Fields.Name")]
        public string Name { get; set; }

        /// <summary>
        /// Mô tả (Đa ngôn ngữ)
        /// </summary>
        [NopResourceDisplayName("Hero.Common.Fields.Description")]
        public string Description { get; set; }

        /// <summary>
        /// Thời gian thực hiện dịch vụ (phút)
        /// </summary>
        [NopResourceDisplayName("Hero.Admin.Services.Fields.ProcessTime")]
        public int ProcessTime { get; set; }

        /// <summary>
        /// Thứ tự
        /// </summary>
        [NopResourceDisplayName("Hero.Common.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        /// <summary>
        /// Kích hoạt
        /// </summary>
        [NopResourceDisplayName("Hero.Common.Fields.Active")]
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
        /// Bệnh viện/CSYT (FK: Hospital)
        /// </summary>
        [NopResourceDisplayName("Hero.Admin.Services.Fields.Hospital")]
        public HospitalModel Hospital { get; set; }

        /// <summary>
        /// Người tạo
        /// </summary>
        public CustomerModel CreatedByUser { get; set; }

        /// <summary>
        /// Người sửa
        /// </summary>
        public CustomerModel UpdatedByUser { get; set; }

        public IList<ServiceLocalizedModel> Locales { get; set; }

        public ServiceModel()
        {
            Locales = new List<ServiceLocalizedModel>();
        }
    }

    public partial class ServiceLocalizedModel : ILocalizedLocaleModel
    {
        public int LanguageId { get; set; }

        [NopResourceDisplayName("Hero.Admin.Services.Fields.Name")]
        public string Name { get; set; }

        [NopResourceDisplayName("Hero.Common.Fields.Description")]
        public string Description { get; set; }
    }
}
