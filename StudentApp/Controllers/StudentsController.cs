using Rotativa;
using StudentApp.Attributes;
using StudentApp.Models;
using StudentApp.Models.Entity;
//using StudentApp.ObjectModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
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
                var App = db.Applications.Where(x => x.UserId == application.ID).FirstOrDefault();
                var bgEducation = db.BgEducations.Where(x => x.AppID == App.ID).ToList();
                Session["userID"] = App.UserId;
                var ftEducation = db.FtEducations.Where(x => x.AppId == App.ID).FirstOrDefault();
                var workExp = db.WorkExps.Where(x => x.AppId == App.ID).ToList();
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
                    BgEducation1 = bgEducation,
                    Upload1 = uploaded,
                    WorkExp = workExp,
                    FtEducation = ftEducation

                };
                ViewBag.Email = db.Users.Find(application.ID).Email;
                ViewBag.StApplication = StApplication;
                Session["ApplicationID"] = StApplication.ID;
            }
            catch (Exception ex)
            {
                Response.Write(@"<script language='javascript'>alert('Message: \n" + ex.Message + " .');</script>");
            }

            return View(StApplication);
        }

        public ActionResult DownloadAsZip()
        {
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

        public List<UploadedFiles> ReturnUploadFilesData(int APPID)
        {
            var Upload1 = (from p in db.Uploads
                           where p.AppId == APPID
                           select new UploadedFiles
                           {
                               FilePath = p.FilePath,
                               FileType = p.FileType,
                               FileName = p.FileName,
                               Id = db.Uploads.Where(x => x.AppId == APPID && x.FileType == p.FileType).FirstOrDefault().Id
                           }).ToList();


            return Upload1;
        }

         
        // [MultipleButton(Argument = "DownloadViewAsPDF", Name = "Action")
        public ActionResult DownloadViewAsPDF()
        {
            int fr = (int)Session["ApplicationID"];
            var App = db.Applications.Where(x => x.ID == fr).FirstOrDefault();
            var bgEducation = db.BgEducations.Where(x => x.AppID == App.ID).ToList();
            var ftEducation = db.FtEducations.Where(x => x.AppId == App.ID).FirstOrDefault();
            var workExp = db.WorkExps.Where(x => x.AppId == App.ID).ToList();
            var LangCert = db.LanguageCerts.Where(x => x.AppId == App.ID).FirstOrDefault();
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
                BgEducation1 = bgEducation,
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
                    langPoints.Add("TestDate", StApplication.LanguageCert.TestDateTOEFL.Value.ToString("dd-MM-yyyy"));
                    break;
                case "IELTS Academic":
                    langPoints.Add("Listening", StApplication.LanguageCert.ListeningIELTS.ToString());
                    langPoints.Add("Speaking", StApplication.LanguageCert.SpeakingIELTS.ToString());
                    langPoints.Add("Writing", StApplication.LanguageCert.WritingIELTS.ToString());
                    langPoints.Add("Reading", StApplication.LanguageCert.ReadingIELTS.ToString());
                    langPoints.Add("OverallScore", StApplication.LanguageCert.OverallScoreIELTS.ToString());
                    langPoints.Add("CertNo", StApplication.LanguageCert.CertNoIELTS.ToString());
                    langPoints.Add("CertOther", StApplication.LanguageCert.CertOtherIELTS.ToString());
                    langPoints.Add("TestDate", StApplication.LanguageCert.TestDateIELTS.Value.ToString("dd-MM-yyyy"));
                    break;
                case "Other":
                    break;
                default:
                    break;
            }
            using (var fs = new StreamReader(Server.MapPath("~/Models/PDFTemplate.html")))
            {
                StringBuilder sbBG = new StringBuilder();
                StringBuilder sbWE = new StringBuilder();
                var WEList = workExp.ToList();
                var bgList = bgEducation.ToList();
                sbBG.Append(Utils.GetHTMLBG(bgList));
                sbWE.Append(Utils.GetHTMLWorkExp(WEList));
                var getUploadData = ReturnUploadFilesData((int)App.ID);

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
                    .Replace("@@DateofBirth", StApplication.DateofBirth.Value.ToString("dd-MM-yyyy"))
                    .Replace("@@CountryofBirth", StApplication.CountryofBirth)
                    .Replace("@@CitizenshipMain", StApplication.CitizenshipMain)
                    .Replace("@@PlaceofBirth", StApplication.PlaceofBirth)
                    .Replace("@@FatherName", StApplication.FatherName)
                    .Replace("@@FatherSurname", StApplication.FatherSurname)
                    .Replace("@@MotherName", StApplication.MotherName)
                    .Replace("@@MotherSurname", StApplication.MotherSurname)
                    .Replace("@@Email", StApplication.Email)
                    .Replace("@@PhoneNumber", StApplication.PhoneNumber)
                    .Replace("@@PassaportNumber", StApplication.PassaportNumber.ToString())
                    .Replace("@@PassStartDate", StApplication.PassStartDate.Value.ToString("dd-MM-yyyy"))
                    .Replace("@@PassExpireDate", StApplication.PassExpireDate.Value.ToString("dd-MM-yyyy"))
                    .Replace("@@IssuingAuthority", StApplication.IssuingCountry)
                    .Replace("@@IssuingCountry", StApplication.IssuingCountry)
                    .Replace("@@NationalId", StApplication.NationalId)
                    .Replace("@@IdStartDate", StApplication.IdStartDate.Value.ToString("dd-MM-yyyy"))
                    .Replace("@@IdExpireDate", StApplication.IdExpireDate.Value.ToString("dd-MM-yyyy"))
                    .Replace("@@CountryCitizenship", StApplication.CountryCitizenship)
                    .Replace("@@FtCountry1", StApplication.FtEducation.Country1)
                    .Replace("@@FtCountry2", StApplication.FtEducation.Country2)
                    .Replace("@@FieldOfStudy", StApplication.FtEducation.FieldOfStudy)
                    .Replace("@@FtEducationLevel", StApplication.FtEducation.EducationLevel)
                    .Replace("@@Ftintake", StApplication.FtEducation.Intake)
                    .Replace("@@FtAcademicYear", StApplication.FtEducation.AcademicYear.ToString())
                    .Replace("@@sbHtmlBG", sbBG.ToString())



                    // FİLE TO PDF PART PROGRESS.....

                    .Replace("@@passport", getUploadData.Where(v => v.FileType == "passport").Select(v => v.FileName).FirstOrDefault())
                    .Replace("@@nationalid", getUploadData.Where(v => v.FileType == "NationalID").Select(v => v.FileName).FirstOrDefault())
                    .Replace("@@diploma", getUploadData.Where(v => v.FileType == "Diploma").Select(v => v.FileName).FirstOrDefault())
                    .Replace("@@transcript", getUploadData.Where(v => v.FileType == "Transcript").Select(v => v.FileName).FirstOrDefault())
                    .Replace("@@cv", getUploadData.Where(v => v.FileType == "CV").Select(v => v.FileName).FirstOrDefault())
                    .Replace("@@ieltsortoeflcertificate", getUploadData.Where(v => v.FileName == "IELTSorToeflCertificate").Select(v => v.FileName).FirstOrDefault())
                    .Replace("@@extradocument1", getUploadData.Where(v => v.FileType == "Extradocument1").Select(v => v.FileName).FirstOrDefault())
                    .Replace("@@extradocument2", getUploadData.Where(v => v.FileType == "Extradocument2").Select(v => v.FileName).FirstOrDefault())
                    .Replace("@@extradocument3", getUploadData.Where(v => v.FileType == "Extradocument3").Select(v => v.FileName).FirstOrDefault())
                    .Replace("@@extradocument4", getUploadData.Where(v => v.FileType == "Extradocument4").Select(v => v.FileName).FirstOrDefault())


                    //.Replace("@@InstitutionName", StApplication.BgEducation1.InstitutionName)
                    //.Replace("@@Faculty", StApplication.BgEducation1.Faculty)
                    //.Replace("@@InsturactionLang", StApplication.BgEducation1.InsturactionLang)
                    //.Replace("@@InsCountry", StApplication.BgEducation1.InsCountry)
                    //.Replace("@@BgEducationLevel", StApplication.BgEducation1.EducationLevel)
                    //.Replace("@@EducationStDate", StApplication.BgEducation1.EducationStDate.ToString())
                    //.Replace("@@EducationCompDate", StApplication.BgEducation1.EducationCompDate.ToString())
                    //.Replace("@@Awarded", StApplication.BgEducation1.Awarded.ToString())
                    //.Replace("@@AvarageGrade", StApplication.BgEducation1.AvarageGrade.ToString())
                    //.Replace("@@CompanyName", StApplication.WorkExp.CompanyName)
                    //.Replace("@@Position", StApplication.WorkExp.Position)
                    //.Replace("@@JobType", StApplication.WorkExp.JobType)
                    //.Replace("@@EmployeeAdress", StApplication.WorkExp.EmployeeAdress)
                    //.Replace("@@ManagerName", StApplication.WorkExp.ManagerName)
                    //.Replace("@@EmployeePhone", StApplication.WorkExp.EmployeePhone)
                    //.Replace("@@EmployeeMail", StApplication.WorkExp.EmployeeMail)
                    .Replace("@@sbHtmlWorkExp", sbWE.ToString())
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

                fs.Close();
            }
            System.IO.File.WriteAllText(Server.MapPath($"~/Content/{StApplication.ID}_Application.html"), xm);

            return new ActionAsPdf($"../Content/{StApplication.ID}_Application.html")
            {
                FileName = "Application.pdf"

            };
        }
        public PartialViewResult UploadFileReturnAdmin()    // Automatic File table refreshing with this Function
        {
            var AppID = (int)Session["ApplicationID"];
            var Upload1 = (from p in db.Uploads
                           where p.AppId == AppID
                           select new UploadedFiles
                           {
                               FilePath = p.FilePath,
                               FileType = p.FileType,
                               FileName = p.FileName,
                               Id = db.Uploads.Where(x => x.AppId == AppID && x.FileType == p.FileType).FirstOrDefault().Id
                           }).ToList();
            return PartialView(@"~/Views/Students/UploadFiles.cshtml", Upload1.AsEnumerable());
        }
        [HttpPost]
        [MultipleButton(Argument = "DownloadFiles", Name = "Action")]
        public ActionResult Download()
        {
            return View();
        }
        public PartialViewResult EduBGAdmin()
        {
            var userID = (int)Session["userID"];
            var AppID = db.Applications.Where(X => X.UserId == userID).FirstOrDefault().ID;
            var BgEducation1 = (from p in db.BgEducations
                                where p.AppID == AppID
                                select new BgEducationRecords
                                {
                                    ID = p.ID,
                                    InsCountry = p.InsCountry,
                                    InstitutionName = p.InstitutionName,
                                    InsturactionLang = p.InsturactionLang,
                                    AppID = p.AppID,
                                    AvarageGrade = p.AvarageGrade,
                                    Awarded = p.Awarded,
                                    EducationCompDate = p.EducationCompDate,
                                    EducationLevel = p.EducationLevel,
                                    EducationStDate = p.EducationStDate,
                                    Faculty = p.Faculty,
                                    StudyMode = p.StudyMode
                                }).ToList();

            return PartialView("/Views/Students/EduBGAdmin.cshtml", BgEducation1.AsEnumerable());
        }
        public PartialViewResult UploadFiles()
        {
            var userID = (int)Session["userID"];
            var AppID = db.Applications.Where(X => X.UserId == userID).FirstOrDefault().ID;
            var UplaodFiles = (from p in db.Uploads
                               where p.AppId == AppID
                               select new UploadedFiles
                               {
                                   Id = p.Id,
                                   File_AppID = p.AppId,
                                   FileName = p.FileName,
                                   FileType = p.FileType
                               }).ToList();

            return PartialView("/Views/Students/UploadFiles.cshtml", UplaodFiles.AsEnumerable());
        }
        public PartialViewResult WorkExpAdmin()
        {
            var userID = (int)Session["userID"];
            var AppID = db.Applications.Where(X => X.UserId == userID).FirstOrDefault().ID;
            var WorkExp = (from p in db.WorkExps
                           where p.AppId == AppID
                           select new WorkExperiences
                           {
                               ID = p.ID,
                               CompanyName = p.CompanyName,
                               AppId = p.AppId,
                               EmployeeAdress = p.EmployeeAdress,
                               EmployeeMail = p.EmployeeMail,
                               EmployeePhone = p.EmployeePhone,
                               JobDescription = p.JobDescription,
                               JobType = p.JobType,
                               ManagerName = p.ManagerName,
                               Position = p.Position
                           }).ToList();
            return PartialView("~/Views/Students/WorkExpAdmin.cshtml", WorkExp.AsEnumerable());
        }


    }
}
