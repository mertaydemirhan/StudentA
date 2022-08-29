using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.HtmlControls;
using StudentApp.Models.Entity;

namespace StudentApp
{
    public class Utils
    {
        public static string GetHTMLBG(List<BgEducation> obj)
        {
            StringBuilder sbHtml = new StringBuilder();
            var tr = new HtmlTableRow();
            var td = new HtmlTableCell();
            foreach (var item in obj)
            {
                sbHtml.Append("<tr>");
                sbHtml.Append("<td>"+item.InstitutionName.ToString()+"</td>");
                //td.InnerText = item.Faculty.ToString();
                sbHtml.Append("<td>" + item.Faculty.ToString() + "</td>");
               // td.InnerText = item.InsturactionLang.ToString();
                sbHtml.Append("<td>"+ item.InsturactionLang.ToString()+"</td>");
                //td.InnerText = item.InsCountry.ToString();
                sbHtml.Append("<td>"+ item.InsCountry.ToString()+"</td>");
               //td.InnerText = item.StudyMode.ToString();
                sbHtml.Append("<td>"+ item.StudyMode.ToString()+"</td>");
               // td.InnerText = item.EducationLevel.ToString();
                sbHtml.Append("<td>"+ item.EducationLevel.ToString()+"</td>");
               // td.InnerText = item.EducationStDate.ToString();
                sbHtml.Append("<td>"+ item.EducationStDate.Value.ToString("dd-MM-yyy")+"</td>");
               // td.InnerText = item.EducationCompDate.ToString();
                sbHtml.Append("<td>"+ item.EducationCompDate.Value.ToString("dd-MM-yyy") +"</td>");
               // td.InnerText = item.Awarded.ToString();
                //sbHtml.Append("<td>"+ item.Awarded.ToString()+"</td>");
               // td.InnerText = item.AvarageGrade.ToString();
                sbHtml.Append("<td>"+ item.AvarageGrade.ToString()+"</td>");
                sbHtml.Append("</tr>");
            }
            
            return sbHtml.ToString();
        }

        public static string GetHTMLWorkExp(List<WorkExp> obj)
        {
            StringBuilder sbHtml = new StringBuilder();
            foreach(var item in obj)
            {
                sbHtml.Append("<tr>");
                sbHtml.Append("<td>" + item.CompanyName.ToString() + "</td>");
                sbHtml.Append("<td>" + item.Position.ToString() + "</td>");
                sbHtml.Append("<td>" + item.JobType.ToString() + "</td>");
                sbHtml.Append("<td>" + item.EmployeeAdress.ToString() + "</td>");
                sbHtml.Append("<td>" + item.ManagerName.ToString() + "</td>");
                sbHtml.Append("<td>" + item.EmployeeMail.ToString() + "</td>");
                sbHtml.Append("<td>" + item.EmployeePhone.ToString() + "</td>");
                sbHtml.Append("<td>" + item.JobDescription.ToString() + "</td>");
                sbHtml.Append("</tr>");
            }
            return sbHtml.ToString();
        }
    }
}