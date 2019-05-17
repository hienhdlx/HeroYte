using NCSw.HERO.Core.Domain.Customers;
using NCSw.HERO.Core.Domain.Localization;
using System;

namespace NCSw.HERO.Core.Domain
{
    /// <summary>
    /// Danh mục tong dai
    /// </summary>
    public partial class Appointment : BaseEntity, ILocalizedEntity
    {
        public int CustomerId { get; set; }

        public string RegisterName { get; set; }

        public string RegisterPhoneNumber { get; set; }

        public string RegisterEmail { get; set; }

        public int RegisterGender { get; set; }

        public string FullName { get; set; }

        public DateTime BirthDay { get; set; }

        public int Gender { get; set; }

        public string IdentityCard { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Description { get; set; }

        public string Address { get; set; }

        public int CountryId { get; set; }

        public int ProvinceId { get; set; }

        public int DistrictId { get; set; }

        public int WardId { get; set; }

        public string AppointmentCode { get; set; }

        public DateTime AppointmentDay { get; set; }

        public DateTime AppointmentHour { get; set; }

        public int ServiceId { get; set; }

        public int DepartmentId { get; set; }

        public int DoctorId { get; set; }

        public int Status { get; set; }

        public int Source { get; set; }

        public string Note { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedOnUtc { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime? UpdatedOnUtc { get; set; }

        public virtual Customer CreatedByUser { get; set; }

        public virtual Customer UpdatedByUser { get; set; }
    }
}
