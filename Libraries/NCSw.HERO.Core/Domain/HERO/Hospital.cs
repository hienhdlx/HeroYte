using NCSw.HERO.Core.Domain.Customers;
using NCSw.HERO.Core.Domain.Localization;
using System;
using System.Collections.Generic;

namespace NCSw.HERO.Core.Domain
{
    /// <summary>
    /// Bệnh viện/Cơ sở y tế
    /// </summary>
    public partial class Hospital : BaseEntity, ILocalizedEntity
    {
        private ICollection<Service> _services;

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
        public virtual Customer CreatedByUser { get; set; }

        /// <summary>
        /// Người sửa
        /// </summary>
        public virtual Customer UpdatedByUser { get; set; }

        public virtual ICollection<Service> Services
        {
            get => _services ?? (_services = new List<Service>());
            protected set => _services = value;
        }
    }
}
