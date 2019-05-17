using FluentValidation.Attributes;
using NCSw.HERO.Web.Areas.Admin.Validators;
using NCSw.HERO.Web.Framework.Models;
using NCSw.HERO.Web.Framework.Mvc.ModelBinding;
using System;

namespace NCSw.HERO.Web.Areas.Admin.Models
{
    [Validator(typeof(ShiftValidator))]
    public class ShiftModel : BaseNopEntityModel
    {
        /// <summary>
        /// Mã ca làm việc
        /// </summary>
        [NopResourceDisplayName("Hero.Admin.Shifts.Fields.Code")]
        public string Code { get; set; }

        /// <summary>
        /// Tên ca làm việc (Đa ngôn ngữ)
        /// </summary>
        [NopResourceDisplayName("Hero.Admin.Shifts.Fields.Name")]
        public string Name { get; set; }

        /// <summary>
        /// Mô tả (Đa ngôn ngữ)
        /// </summary>
        [NopResourceDisplayName("Hero.Admin.Shifts.Fields.Description")]
        public string Description { get; set; }

        /// <summary>
        /// Giờ làm việc - Từ giờ (Đa ngôn ngữ)
        /// </summary>
        [NopResourceDisplayName("Hero.Admin.Shifts.Fields.FromHour")]
        public TimeSpan FromHour { get; set; }

        /// <summary>
        /// Giờ làm việc - Từ giờ (Đa ngôn ngữ)
        /// </summary>
        [NopResourceDisplayName("Hero.Admin.Shifts.Fields.ToHour")]
        public TimeSpan ToHour { get; set; }

        /// <summary>
        /// Nghỉ giữa ca - Từ giờ (Đa ngôn ngữ)
        /// </summary>
        [NopResourceDisplayName("Hero.Admin.Shifts.Fields.BreakTimeFrom")]
        public TimeSpan? BreakTimeFrom { get; set; }

        /// <summary>
        /// Nghỉ giữa ca - Từ giờ (Đa ngôn ngữ)
        /// </summary>
        [NopResourceDisplayName("Hero.Admin.Shifts.Fields.BreakTimeTo")]
        public TimeSpan? BreakTimeTo { get; set; }

        /// <summary>
        /// Kích hoạt (Đa ngôn ngữ)
        /// </summary>
        [NopResourceDisplayName("Hero.Admin.Shifts.Fields.Active")]
        public bool Active { get; set; }

        /// <summary>
        /// Đã xóa (Đa ngôn ngữ)
        /// </summary>
        [NopResourceDisplayName("Hero.Admin.Shifts.Fields.Deleted")]
        public bool Deleted { get; set; }

        /// <summary>
        /// Người tạo (Đa ngôn ngữ)
        /// </summary>
        [NopResourceDisplayName("Hero.Admin.Shifts.Fields.CreatedBy")]
        public int CreatedBy { get; set; }

        /// <summary>
        /// Ngày tạo (Đa ngôn ngữ)
        /// </summary>
        [NopResourceDisplayName("Hero.Admin.Shifts.Fields.CreatedOnUtc")]
        public DateTime CreatedOnUtc { get; set; }

        /// <summary>
        /// Người sửa (Đa ngôn ngữ)
        /// </summary>
        [NopResourceDisplayName("Hero.Admin.Shifts.Fields.UpdatedBy")]
        public int UpdatedBy { get; set; }

        /// <summary>
        /// Ngày sửa (Đa ngôn ngữ)
        /// </summary>
        [NopResourceDisplayName("Hero.Admin.Shifts.Fields.UpdatedOnUtc")]
        public DateTime UpdatedOnUtc { get; set; }

    }
}