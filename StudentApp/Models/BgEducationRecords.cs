﻿using StudentApp.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace StudentApp.Models
{
    public class BgEducationRecords
    {
        public int ID { get; set; }
        public string InstitutionName { get; set; }
        public string Faculty { get; set; }
        public string InsturactionLang { get; set; }
        public string InsCountry { get; set; }
        public string StudyMode { get; set; }
        public string EducationLevel { get; set; }
        public Nullable<System.DateTime> EducationStDate { get; set; }
        public Nullable<System.DateTime> EducationCompDate { get; set; }
        public Nullable<double> AvarageGrade { get; set; }
        public bool Awarded { get; set; }
        public Nullable<int> AppID { get; set; }
        public IEnumerable<Entity.BgEducation> BgEducations { get; set; }
    }
}