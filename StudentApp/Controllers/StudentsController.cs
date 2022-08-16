using NReco.PdfGenerator;
using Rotativa;
using StudentApp.Attributes;
using StudentApp.Models.Entity;
//using StudentApp.ObjectModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;


namespace StudentApp.Controllers
{
    public class StudentsController : Controller
    {
        readonly StudentAppEntities db = new StudentAppEntities();

        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }
        public ActionResult ApplicationDetails(Application application)
        {
            var StApplication = new ObjectModels.Application();
            try
            {
                var  App = db.Applications.Where(x => x.UserId == application.ID).FirstOrDefault();
                var bgEducation = db.BgEducations.Where(x => x.AppID == App.ID).FirstOrDefault();
                var ftEducation = db.FtEducations.Where(x => x.AppId == App.ID).FirstOrDefault();
                var workExp = db.WorkExps.Where(x => x.AppId == App.ID).FirstOrDefault();
                var LangCert = db.LanguageCerts.Where(x => x.AppId == App.ID).FirstOrDefault();
                var uploaded = db.Uploads.Where(x => x.AppId == App.ID).FirstOrDefault();
                StApplication = new ObjectModels.Application
                {
                    ID = App.ID,
                    Name = App.Name,
                    Surname = App.Surname,
                    AddressLine1 = App.AddressLine1,
                    AddressLine2 = App.AddressLine2,
                    IssuingAuthority = App.IssuingAuthority,
                    State = App.State,
                    CitizenshipMain = App.CitizenshipMain,
                    City = App.City,
                    Country = App.Country,
                    CountryCitizenship = App.CountryCitizenship,
                    CountryofBirth = App.CountryofBirth,
                    DateofBirth = App.DateofBirth,
                    FatherName = App.FatherName,
                    FatherSurname = App.FatherSurname,
                    Gender = App.Gender,
                    Email = App.Email,
                    IdExpireDate = App.IdExpireDate,
                    IdStartDate = App.IdStartDate,
                    IssuingCountry = App.IssuingCountry,
                    LanguageCert = LangCert,
                    maritalStatus = App.maritalStatus,
                    MotherName = App.MotherName,
                    MotherSurname = App.MotherSurname,
                    NationalId = App.NationalId,
                    PassaportNumber = App.PassaportNumber,
                    PassExpireDate = App.PassExpireDate,
                    PassStartDate = App.PassStartDate,
                    PlaceofBirth = App.PlaceofBirth,
                    PhoneNumber = App.PhoneNumber,
                    userId = (int)App.UserId,
                    Zip = App.Zip,
                    WorkExp = workExp,
                    FtEducation = ftEducation
                };
                ViewBag.Email = db.Users.Find(application.ID).Email;
                ViewBag.StApplication = StApplication;
                Session["ApplicationID"] = StApplication.ID;
            }catch(Exception ex)
            {
                Response.Write(@"<script language='javascript'>alert('Message: \n" + ex.Message + " .');</script>");
            }

            return View(StApplication);
        }

        public ActionResult DownloadAsZip() {
            int fr = (int)Session["ApplicationID"];
             // zip dosyası ismi

            var temp = Server.MapPath($"~/UploadedFiles/") + +fr + @"\";
            var ziptopath = Server.MapPath($"~/UploadedFiles/DownloadZip");

            if (!Directory.Exists(ziptopath))
                Directory.CreateDirectory(ziptopath);



            // temp klasörü yoksa oluştur
            if (!Directory.Exists(temp))
                Directory.CreateDirectory(temp);

            var files = Directory.GetFiles(temp).ToList();


            // gelen pdf leri temp klasörüne atıyor.
            files.ForEach(f => System.IO.File.Copy(f, System.IO.Path.Combine(ziptopath, System.IO.Path.GetFileName(f))));

            var zipname = fr + "_archive.zip";
            //// o track numarası ismindeki dosyaları sorgula Hepsini sil
            //var fileList = Directory.GetFiles(parentDirectory)
            //    .Where(a => a.Contains(trackingNo))// && !a.EndsWith(".zip")
            //    .ToList();

            //// sorguladığın dosyaları sil
            //foreach (var file in fileList)
            //    File.Delete(file);

            ziptopath += zipname;
            System.IO.Compression.ZipFile.CreateFromDirectory(temp, ziptopath);
            Response.ContentType = "application/zip";
            Response.AddHeader("Content-Disposition", $"attachment; filename={zipname}");
            Response.TransmitFile(ziptopath);
            Response.Flush();
            //Directory.GetFiles(temp).ToList().ForEach(System.IO.File.Delete); directory silmek içindi, pasife alındı.
            //Directory.Delete(temp);

            Response.End();
            var ExistZipFile = Directory.GetFiles(Server.MapPath($"~/UploadedFiles/"));
            Directory.GetFiles(Server.MapPath($"~/UploadedFiles/DownloadZip/")).ToList().ForEach(System.IO.File.Delete); // dosyaları sildirdik.
            Directory.Delete(Server.MapPath($"~/UploadedFiles/DownloadZip/"));
            foreach (var f in ExistZipFile)
            {
                var fi = new FileInfo(f);
                if (fi.Name == "DownloadZip" + zipname)
                    fi.Delete();
            }
            return null;
        }


        // [MultipleButton(Argument = "DownloadViewAsPDF", Name = "Action")
        public ActionResult DownloadViewAsPDF()
        {
            int fr = (int)Session["ApplicationID"];
            var App = db.Applications.Where(x => x.ID == fr).FirstOrDefault();
            var bgEducation = db.BgEducations.Where(x => x.AppID == App.ID).FirstOrDefault();
            var ftEducation = db.FtEducations.Where(x => x.AppId == App.ID).FirstOrDefault();
            var workExp = db.WorkExps.Where(x => x.AppId == App.ID).FirstOrDefault();
            var LangCert = db.LanguageCerts.Where(x => x.AppId == App.ID).FirstOrDefault();
            var uploaded = db.Uploads.Where(x => x.AppId == App.ID).FirstOrDefault();
            var StApplication = new ObjectModels.Application
            {
                ID = App.ID,
                Name = App.Name,
                Surname = App.Surname,
                AddressLine1 = App.AddressLine1,
                AddressLine2 = App.AddressLine2,
                IssuingAuthority = App.IssuingAuthority,
                State = App.State,
                CitizenshipMain = App.CitizenshipMain,
                City = App.City,
                Country = App.Country,
                CountryCitizenship = App.CountryCitizenship,
                CountryofBirth = App.CountryofBirth,
                DateofBirth = App.DateofBirth,
                FatherName = App.FatherName,
                FatherSurname = App.FatherSurname,
                Gender = App.Gender,
                Email = App.Email,
                IdExpireDate = App.IdExpireDate,
                IdStartDate = App.IdStartDate,
                IssuingCountry = App.IssuingCountry,
                LanguageCert = LangCert,
                maritalStatus = App.maritalStatus,
                MotherName = App.MotherName,
                MotherSurname = App.MotherSurname,
                NationalId = App.NationalId,
                PassaportNumber = App.PassaportNumber,
                PassExpireDate = App.PassExpireDate,
                PassStartDate = App.PassStartDate,
                PlaceofBirth = App.PlaceofBirth,
                PhoneNumber = App.PhoneNumber,
                userId = (int)App.UserId,
                Zip = App.Zip,
                WorkExp = workExp,
                FtEducation = ftEducation
            };
            string xm = "";
            var langPoints = new Dictionary<string, string>();
            switch (StApplication.LanguageCert.CertType.ToString())
            {
                case "TOEFL IBT":
                    langPoints.Add("Listening", StApplication.LanguageCert.ListeningTOEFL.ToString());
                    langPoints.Add("Speaking", StApplication.LanguageCert.SpeakingTOEFL.ToString());
                    langPoints.Add("Writing", StApplication.LanguageCert.WritingTOEFL.ToString());
                    langPoints.Add("Reading", StApplication.LanguageCert.ReadingTOEFL.ToString());
                    langPoints.Add("OverallScore", StApplication.LanguageCert.OverallScoreTOEFL.ToString());
                    langPoints.Add("CertNo", StApplication.LanguageCert.CertNoTOEFL.ToString());
                    langPoints.Add("CertOther", StApplication.LanguageCert.CertOtherTOEFL.ToString());
                    langPoints.Add("TestDate", StApplication.LanguageCert.TestDateTOEFL.ToString());
                 break;
                case "IELTS Academic":
                    langPoints.Add("Listening", StApplication.LanguageCert.ListeningIELTS.ToString());
                    langPoints.Add("Speaking", StApplication.LanguageCert.SpeakingIELTS.ToString());
                    langPoints.Add("Writing", StApplication.LanguageCert.WritingIELTS.ToString());
                    langPoints.Add("Reading", StApplication.LanguageCert.ReadingIELTS.ToString());
                    langPoints.Add("OverallScore", StApplication.LanguageCert.OverallScoreIELTS.ToString());
                    langPoints.Add("CertNo", StApplication.LanguageCert.CertNoIELTS.ToString());
                    langPoints.Add("CertOther", StApplication.LanguageCert.CertOtherIELTS.ToString());
                    langPoints.Add("TestDate", StApplication.LanguageCert.TestDateIELTS.ToString());
                    break;
                case "Other":
                    break;
                default:
                    break;
            }
            using (var fs = new StreamReader(Server.MapPath("~/Models/PDFTemplate.html")))
            {
                xm = fs.ReadToEnd();
                xm = xm.Replace("@@Name", StApplication.Name)
                    .Replace("@@Surname", StApplication.Surname)
                    .Replace("@@Gender", StApplication.Gender)
                    .Replace("@@maritalStatus", StApplication.maritalStatus)
                    .Replace("@@AddressLine1", StApplication.AddressLine1)
                    .Replace("@@AddressLine2", StApplication.AddressLine2)
                    .Replace("@@City", StApplication.City)
                    .Replace("@@State", StApplication.State)
                    .Replace("@@Zip", StApplication.Zip)
                    .Replace("@@Country", StApplication.Country)
                    .Replace("@@DateofBirth", StApplication.DateofBirth.ToString())
                    .Replace("@@CountryofBirth", StApplication.CountryofBirth)
                    .Replace("@@CitizenshipMain", StApplication.CitizenshipMain)
                    .Replace("@@PlaceofBirth", StApplication.PlaceofBirth)
                    .Replace("@@FatherName", StApplication.FatherName)
                    .Replace("@@FatherSurname", StApplication.FatherSurname)
                    .Replace("@@MotherName", StApplication.MotherName)
                    .Replace("@@MotherSurname", StApplication.MotherSurname)
                    .Replace("@@Email", ViewBag.Email)
                    .Replace("@@PhoneNumber", StApplication.PhoneNumber)
                    .Replace("@@PassaportNumber", StApplication.PassaportNumber.ToString())
                    .Replace("@@PassStartDate", StApplication.PassStartDate.ToString())
                    .Replace("@@PassExpireDate", StApplication.PassExpireDate.ToString())
                    .Replace("@@IssuingAuthority", StApplication.IssuingCountry)
                    .Replace("@@IssuingCountry", StApplication.IssuingCountry)
                    .Replace("@@NationalId", StApplication.NationalId)
                    .Replace("@@IdStartDate", StApplication.IdStartDate.ToString())
                    .Replace("@@IdExpireDate", StApplication.IdExpireDate.ToString())
                    .Replace("@@CountryCitizenship", StApplication.CountryCitizenship)
                    .Replace("@@FtCountry1", StApplication.FtEducation.Country1)
                    .Replace("@@FtCountry2", StApplication.FtEducation.Country2)
                    .Replace("@@FieldOfStudy", StApplication.FtEducation.FieldOfStudy)
                    .Replace("@@FtEducationLevel", StApplication.FtEducation.EducationLevel)
                    .Replace("@@Ftintake", StApplication.FtEducation.Intake)
                    .Replace("@@FtAcademicYear", StApplication.FtEducation.AcademicYear.ToString())
                    .Replace("@@StudyMode", StApplication.BgEducation1.StudyMode)
                    .Replace("@@InstitutionName", StApplication.BgEducation1.InstitutionName)
                    .Replace("@@Faculty", StApplication.BgEducation1.Faculty)
                    .Replace("@@InsturactionLang", StApplication.BgEducation1.InsturactionLang)
                    .Replace("@@InsCountry", StApplication.BgEducation1.InsCountry)
                    .Replace("@@BgEducationLevel", StApplication.BgEducation1.EducationLevel)
                    .Replace("@@EducationStDate", StApplication.BgEducation1.EducationStDate.ToString())
                    .Replace("@@EducationCompDate", StApplication.BgEducation1.EducationCompDate.ToString())
                    .Replace("@@Awarded", StApplication.BgEducation1.Awarded.ToString())
                    .Replace("@@AvarageGrade", StApplication.BgEducation1.AvarageGrade.ToString())
                    .Replace("@@CompanyName", StApplication.WorkExp.CompanyName)
                    .Replace("@@Position", StApplication.WorkExp.Position)
                    .Replace("@@JobType", StApplication.WorkExp.JobType)
                    .Replace("@@EmployeeAdress", StApplication.WorkExp.EmployeeAdress)
                    .Replace("@@ManagerName", StApplication.WorkExp.ManagerName)
                    .Replace("@@EmployeePhone", StApplication.WorkExp.EmployeeMail)
                    .Replace("@@EmployeeMail", StApplication.WorkExp.EmployeeMail)
                    .Replace("@@JobDescription", StApplication.WorkExp.JobDescription)
                    .Replace("@@LangCert", StApplication.LanguageCert.LangCert.ToString())
                    .Replace("@@CertType", StApplication.LanguageCert.CertType)
                    .Replace("@@Listening", langPoints["Listening"])
                    .Replace("@@Speaking", langPoints["Speaking"])
                    .Replace("@@Writing", langPoints["Writing"])
                    .Replace("@@Reading", langPoints["Reading"])
                    .Replace("@@OverallScore", langPoints["OverallScore"])
                    .Replace("@@CertNo", langPoints["CertNo"])
                    .Replace("@@CertOther", langPoints["CertOther"])
                    .Replace("@@TestDate", langPoints["TestDate"]);

                //path = ;
                fs.Close();
            }
            System.IO.File.WriteAllText(Server.MapPath($"~/Content/{StApplication.ID}_Application.html"), xm);
            return new ActionAsPdf($"../Content/{StApplication.ID}_Application.html")
            {
                FileName = "Application.pdf"
                    
            };
        }

        [HttpPost]
        [MultipleButton(Argument = "DownloadFiles", Name = "Action")]
        public ActionResult Download()
        {
            return View();
        }


    }
}