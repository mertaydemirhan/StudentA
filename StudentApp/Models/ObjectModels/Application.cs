using StudentApp.Models.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StudentApp.ObjectModels
{

    public class Application
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public int ID { get; set; }
        public int userId { get; set; }
        public string Gender { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public Nullable<System.DateTime> DateofBirth { get; set; }
        public string CountryofBirth { get; set; }
        public string CitizenshipMain { get; set; }
        public string PlaceofBirth { get; set; }
        public string FatherName { get; set; }
        public string FatherSurname { get; set; }
        public string MotherName { get; set; }
        public string MotherSurname { get; set; }
        public string PhoneNumber { get; set; }
        public string maritalStatus { get; set; }
        public string Email { get; set; }
        public string PassaportNumber { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public Nullable<System.DateTime> PassStartDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public Nullable<System.DateTime> PassExpireDate { get; set; }
        public string IssuingCountry { get; set; }
        public string IssuingAuthority { get; set; }
        public string NationalId { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public Nullable<System.DateTime> IdStartDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public Nullable<System.DateTime> IdExpireDate { get; set; }
        public string CountryCitizenship { get; set; }
        public  List< Models.Entity.BgEducation> BgEducation1 { get; set; }
        public  FtEducation FtEducation { get; set; }
        public  List<WorkExp> WorkExp { get; set; }
        public  LanguageCert LanguageCert { get; set; }
        public Models.UploadedFiles UploadedFiles { get; set; }
        public Models.Entity.Upload Upload1 { get; set; }

    }
}