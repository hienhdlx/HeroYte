using FluentValidation.Attributes;
using Microsoft.AspNetCore.Mvc.Rendering;
using NCSw.HERO.Web.Areas.Admin.Models.Customers;
using NCSw.HERO.Web.Areas.Admin.Validators;
using NCSw.HERO.Web.Framework.Models;
using NCSw.HERO.Web.Framework.Mvc.ModelBinding;
using System;
using System.Collections.Generic;

namespace NCSw.HERO.Web.Areas.Admin.Models
{
    [Validator(typeof(DepartmentValidator))]
    public partial class DepartmentModel : BaseNopEntityModel, ILocalizedModel<DepartmentLocalizedModel>
    {
        [NopResourceDisplayName("Hero.Admin.Department.Fields.Path")]
        public string Path { get; set; }
        public string PathName { get; set; }

        [NopResourceDisplayName("Hero.Admin.Department.Fields.ParentId")]
        public int ParentId { get; set; }
        /// <summary>
        /// Bệnh viện/CSYT (FK: Hospital)
        /// </summary>
        [NopResourceDisplayName("Hero.Admin.Department.Fields.HospitalName")]
        public int HospitalId { get; set; }

        /// <summary>
        /// Mã dịch vụ
        /// </summary>
        [NopResourceDisplayName("Hero.Admin.Department.Fields.Code")]
        public string Code { get; set; }

        /// <summary>
        /// Tên dịch vụ (Đa ngôn ngữ)
        /// </summary>
        [NopResourceDisplayName("Hero.Admin.Department.Fields.Name")]
        public string Name { get; set; }

        /// <summary>
        /// Mô tả (Đa ngôn ngữ)
        /// </summary>
        [NopResourceDisplayName("Hero.Admin.Department.Fields.Description")]
        public string Description { get; set; }

        /// <summary>
        /// Dia chi 
        /// </summary>
        [NopResourceDisplayName("Hero.Admin.Department.Fields.Address")]
        public string Address { get; set; }

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
        [NopResourceDisplayName("Hero.Admin.Department.Fields.Hospital")]
        public HospitalModel Hospital { get; set; }

        /// <summary>
        /// Người tạo
        /// </summary>
        public CustomerModel CreatedByUser { get; set; }

        /// <summary>
        /// Người sửa
        /// </summary>
        public CustomerModel UpdatedByUser { get; set; }

        public IList<SelectListItem> DepartmentListTemplates { get; set; }

        public IList<DepartmentLocalizedModel> Locales { get; set; }

        public DepartmentModel()
        {
            Locales = new List<DepartmentLocalizedModel>();
            DepartmentListTemplates = new List<SelectListItem>();
        }
    }

    public partial class DepartmentLocalizedModel : ILocalizedLocaleModel
    {
        public int LanguageId { get; set; }

        [NopResourceDisplayName("Hero.Admin.Department.Fields.Name")]
        public string Name { get; set; }

        [NopResourceDisplayName("Hero.Admin.Department.Fields.Description")]
        public string Description { get; set; }

        [NopResourceDisplayName("Hero.Admin.Department.Fields.Address")]
        public string Address { get; set; }
        
    }
}
