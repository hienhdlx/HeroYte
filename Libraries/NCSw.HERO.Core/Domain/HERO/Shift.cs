using NCSw.HERO.Core.Domain.Localization;
using System;
using System.Collections.Generic;
using System.Text;

namespace NCSw.HERO.Core.Domain
{
    public partial class Shift : BaseEntity, ILocalizedEntity
    {
        /// <summary>
        /// Mã ca làm việc
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Tên ca làm việc (Đa ngôn ngữ)
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Mô tả (Đa ngôn ngữ)
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Giờ làm việc - Từ giờ (Đa ngôn ngữ)
        /// </summary>
        public TimeSpan FromHour { get; set; }

        /// <summary>
        /// Giờ làm việc - Từ giờ (Đa ngôn ngữ)
        /// </summary>
        public TimeSpan ToHour { get; set; }

        /// <summary>
        /// Nghỉ giữa ca - Từ giờ (Đa ngôn ngữ)
        /// </summary>
        public TimeSpan? BreakTimeFrom { get; set; }

        /// <summary>
        /// Nghỉ giữa ca - Từ giờ (Đa ngôn ngữ)
        /// </summary>
        public TimeSpan? BreakTimeTo { get; set; }

        /// <summary>
        /// Kích hoạt (Đa ngôn ngữ)
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Đã xóa (Đa ngôn ngữ)
        /// </summary>
        public bool Deleted { get; set; }

        /// <summary>
        /// Người tạo (Đa ngôn ngữ)
        /// </summary>
        public int CreatedBy { get; set; }

        /// <summary>
        /// Ngày tạo (Đa ngôn ngữ)
        /// </summary>
        public DateTime CreatedOnUtc { get; set; }

        /// <summary>
        /// Người sửa (Đa ngôn ngữ)
        /// </summary>
        public int? UpdatedBy { get; set; }

        /// <summary>
        /// Ngày sửa (Đa ngôn ngữ)
        /// </summary>
        public DateTime? UpdatedOnUtc { get; set; }
    }
}
