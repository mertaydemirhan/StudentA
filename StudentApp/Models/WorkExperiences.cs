using StudentApp.ObjectModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentApp.Models
{
    public class WorkExperiences
    {
        public int ID { get; set; }
        public string CompanyName { get; set; }
        public string Position { get; set; }
        public string JobType { get; set; }
        public string EmployeeAdress { get; set; }
        public string ManagerName { get; set; }
        public string EmployeeMail { get; set; }
        public string EmployeePhone { get; set; }
        public string JobDescription { get; set; }
        public Nullable<int> AppId { get; set; }

        public virtual Application Application { get; set; }
        public IEnumerable<Entity.WorkExp> workExps { get; set; } 
    }
}