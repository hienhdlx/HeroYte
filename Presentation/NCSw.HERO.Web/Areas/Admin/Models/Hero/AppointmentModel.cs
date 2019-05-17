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
    public partial class AppointmentModel : BaseNopEntityModel
    {
        [NopResourceDisplayName("Admin.SwitchBoard.PatientCode")]
        public int CustomerId { get; set; }

        [NopResourceDisplayName("Admin.SwitchBoard.PersonContact")]
        public string RegisterName { get; set; }

        [NopResourceDisplayName("Admin.SwitchBoard.PhoneContact")]
        public string RegisterPhoneNumber { get; set; }

        public string RegisterEmail { get; set; }

        public int RegisterGender { get; set; }

        [NopResourceDisplayName("Admin.SwitchBoard.Name")]
        public string FullName { get; set; }

        [NopResourceDisplayName("Admin.SwitchBoard.Birthday")]
        public DateTime BirthDay { get; set; }

        [NopResourceDisplayName("Admin.SwitchBoard.Gender")]
        public int Gender { get; set; }

        [NopResourceDisplayName("Admin.SwitchBoard.IdentityCard")]
        public string IdentityCard { get; set; }

        public string Email { get; set; }

        [NopResourceDisplayName("Admin.SwitchBoard.PhoneNumber")]
        public string PhoneNumber { get; set; }

        [NopResourceDisplayName("Admin.SwitchBoard.DescribeDiseases")]
        public string Description { get; set; }

        [NopResourceDisplayName("Admin.SwitchBoard.SpecificAddress")]
        public string Address { get; set; }

        [NopResourceDisplayName("Admin.SwitchBoard.Nationality")]
        public int CountryId { get; set; }

        [NopResourceDisplayName("Admin.SwitchBoard.Province")]
        public int ProvinceId { get; set; }

        [NopResourceDisplayName("Admin.SwitchBoard.District")]
        public int DistrictId { get; set; }

        [NopResourceDisplayName("Admin.SwitchBoard.Ward")]
        public int WardId { get; set; }

        [NopResourceDisplayName("Admin.SwitchBoard.PatientCode")]
        public string AppointmentCode { get; set; }

        [NopResourceDisplayName("Admin.SwitchBoard.CheckupDate")]
        public DateTime AppointmentDay { get; set; }

        [NopResourceDisplayName("Admin.SwitchBoard.CheckupHours")]
        public DateTime AppointmentHour { get; set; }

        [NopResourceDisplayName("Admin.SwitchBoard.Service")]
        public int ServiceId { get; set; }

        [NopResourceDisplayName("Admin.SwitchBoard.Room")]
        public int DepartmentId { get; set; }

        [NopResourceDisplayName("Admin.SwitchBoard.DoctorName")]
        public int DoctorId { get; set; }

        public int Status { get; set; }

        public int Source { get; set; }

        public string Note { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedOnUtc { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime? UpdatedOnUtc { get; set; }

        public CustomerModel CreatedByUser { get; set; }

        public CustomerModel UpdatedByUser { get; set; }

        public IList<SelectListItem> DepartmentListTemplates { get; set; }

        public IList<SelectListItem> ServiceListTemplates { get; set; }

        public IList<SelectListItem> DoctorListTemplates { get; set; }

        public AppointmentModel()
        {
            DepartmentListTemplates = new List<SelectListItem>();
            ServiceListTemplates = new List<SelectListItem>();
            DoctorListTemplates = new List<SelectListItem>();
        }
    }
}
