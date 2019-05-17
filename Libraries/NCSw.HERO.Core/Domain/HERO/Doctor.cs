using NCSw.HERO.Core.Domain.Localization;
using System;
using System.Collections.Generic;
using System.Text;

namespace NCSw.HERO.Core.Domain
{
    public partial class Doctor : BaseEntity, ILocalizedEntity
    {
        /// <summary>
        /// Mã bác sĩ
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Tên bác sĩ (Đa ngôn ngữ)
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Ngày sinh bác sĩ (Đa ngôn ngữ)
        /// </summary>
        public DateTime BirthDay { get; set; }

        /// <summary>
        /// Giới tính (Đa ngôn ngữ)
        /// </summary>
        public int Gender { get; set; }

        /// <summary>
        /// CMND (Đa ngôn ngữ)
        /// </summary>
        public string IdentityCard { get; set; }

        /// <summary>
        /// Email (Đa ngôn ngữ)
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Số điện thoại (Đa ngôn ngữ)
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Địa chỉ (Đa ngôn ngữ)
        /// </summary>
        public string Address { get; set; }
    }
}
