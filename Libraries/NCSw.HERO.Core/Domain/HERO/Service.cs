using NCSw.HERO.Core.Domain.Customers;
using NCSw.HERO.Core.Domain.Localization;
using System;

namespace NCSw.HERO.Core.Domain
{
    /// <summary>
    /// Danh mục Dịch vụ
    /// </summary>
    public partial class Service : BaseEntity, ILocalizedEntity
    {
        /// <summary>
        /// Bệnh viện/CSYT (FK: Hospital)
        /// </summary>
        public int HospitalId { get; set; }

        /// <summary>
        /// Mã dịch vụ
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Tên dịch vụ (Đa ngôn ngữ)
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Mô tả (Đa ngôn ngữ)
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Thời gian thực hiện dịch vụ (phút)
        /// </summary>
        public int ProcessTime { get; set; }

        /// <summary>
        /// Thứ tự
        /// </summary>
        public int DisplayOrder { get; set; }

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
        /// Bệnh viện/CSYT (FK: Hospital)
        /// </summary>
        public virtual Hospital Hospital { get; set; }

        /// <summary>
        /// Người tạo
        /// </summary>
        public virtual Customer CreatedByUser { get; set; }

        /// <summary>
        /// Người sửa
        /// </summary>
        public virtual Customer UpdatedByUser { get; set; }
    }
}
