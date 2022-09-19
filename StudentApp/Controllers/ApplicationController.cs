using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StudentApp.Models;
using StudentApp.Models.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace StudentApplication.Controllers
{
    [Authorize]
    public class ApplicationController : Controller
    {
        readonly StudentAppEntities db = new StudentAppEntities();

        // BgEducation PART ------------------------------------------------------------------------------------
        public PartialViewResult BgEducationRecords()
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
                                });

            return PartialView(@"~/Views/Application/BackgroundEducation.cshtml", BgEducation1.AsEnumerable());
        }
        public JsonResult BgDelete(int ID) // Background Education when delete button was triggered...
        {
            //var userID = (int)Session["userID"];
            //var AppID = db.Applications.Where(X => X.UserId == userID).FirstOrDefault().ID;
            //var ListOfEducation = db.BgEducations.Where(x => x.AppID == AppID);
            try
            {
                var ItemRemove = db.BgEducations.Find(ID);
                db.BgEducations.Remove(ItemRemove);
                db.SaveChanges();
                TempData["Message"] = " Your Background Education has been successfully deleted";
            }
            catch (Exception)
            {
                TempData["Message"] = " Encountered an error";
            }

            //return Json(TempData["Message"], JsonRequestBehavior.AllowGet);
            return Json(TempData["Message"], JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult BgAdd(BgEducationRecords ajaxData1)  // Bg Education Adding part
        {
            var userID = (int)Session["userID"];
            var AppID = db.Applications.Where(X => X.UserId == userID).FirstOrDefault().ID;

            // test

            db.BgEducations.Add(new BgEducation
            {
                AppID = AppID,
                Awarded = ajaxData1.Awarded,
                AvarageGrade = ajaxData1.AvarageGrade,
                EducationCompDate = ajaxData1.EducationCompDate,
                EducationLevel = ajaxData1.EducationLevel,
                EducationStDate = ajaxData1.EducationStDate,
                Faculty = ajaxData1.Faculty,
                InsCountry = ajaxData1.InsCountry,
                InstitutionName = ajaxData1.InstitutionName,
                InsturactionLang = ajaxData1.InsturactionLang,
                StudyMode = ajaxData1.StudyMode,
            });
            db.SaveChanges();
            return Json("", JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult EducationView(BgEducationRecords bgEducationInfo)
        {
            var bgeducation = db.BgEducations.Where(x => x.ID == bgEducationInfo.ID).ToList();

            return Json(bgeducation);
        }

        // Future Education PART -------------------------------------------------------------
        public PartialViewResult FutureEducationView()
        {
            return PartialView(@"~/Views/Application/FutureEducation.cshtml");
        }



        // Work experiences PART ----------------------------------------------------------------------------
        public PartialViewResult WorkExperiences()
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
                           });

            return PartialView(@"~/Views/Application/WorkExperience.cshtml", WorkExp.AsEnumerable());
        }
        [HttpPost]
        public JsonResult WorkExpAdd(WorkExperiences ajaxData2)  // Bg Education Adding part
        {
            var userID = (int)Session["userID"];
            var AppID = db.Applications.Where(X => X.UserId == userID).FirstOrDefault().ID;

            // test

            db.WorkExps.Add(new WorkExp
            {
                AppId = AppID,
                CompanyName = ajaxData2.CompanyName,
                EmployeeAdress = ajaxData2.EmployeeAdress,
                EmployeeMail = ajaxData2.EmployeeMail,
                EmployeePhone = ajaxData2.EmployeePhone,
                JobDescription = ajaxData2.JobDescription,
                ManagerName = ajaxData2.ManagerName,
                JobType = ajaxData2.JobType,
                Position = ajaxData2.Position
            });
            db.SaveChanges();
            return Json("", JsonRequestBehavior.AllowGet);
        }
        public JsonResult WorkExpDelete(int ID) // Work Experiences when delete button was triggered...
        {
            //var userID = (int)Session["userID"];
            //var AppID = db.Applications.Where(X => X.UserId == userID).FirstOrDefault().ID;
            //var ListOfEducation = db.BgEducations.Where(x => x.AppID == AppID);
            try
            {
                var ItemRemove = db.WorkExps.Find(ID);
                db.WorkExps.Remove(ItemRemove);
                db.SaveChanges();
                TempData["Message"] = " Your Work Experiences has been successfully deleted";
            }
            catch (Exception)
            {
                TempData["Message"] = " Encountered an error";
            }

            //return Json(TempData["Message"], JsonRequestBehavior.AllowGet);
            return Json(TempData["Message"], JsonRequestBehavior.AllowGet);
        }


        // Submit Files  PART ------------------------------------------------------------------------------
        public JsonResult UploadFileDelete(int Id)
        {
            try
            {
                var itemToRemove = db.Uploads.Where(x => x.Id == Id).FirstOrDefault();
                string fullPath = Server.MapPath($"~/UploadedFiles/{itemToRemove.AppId}/{itemToRemove.FileType + "_" + itemToRemove.FileName}");
                db.Uploads.Remove(itemToRemove);
                db.SaveChanges();
                //// File Delete part start

                if (System.IO.File.Exists(fullPath))
                    System.IO.File.Delete(fullPath);
                TempData["Message"] = " Your File has been successfully deleted";
            }
            catch (Exception)
            {
                TempData["Message"] = " Encountered an error";
            }


            return Json(TempData["Message"], JsonRequestBehavior.AllowGet);
        }
        public ActionResult Download(int Id)
        {
            var DocPath = db.Uploads.FirstOrDefault(x => x.Id == Id).FilePath;
            DocPath = Server.MapPath(DocPath);
            FileInfo dosya = new FileInfo(DocPath);

            Response.Clear(); // Her ihtimale karşı Buffer' da kalmış herhangibir veri var ise bunu silmek için yapıyoruz.
            if (dosya.Exists)
            {
                Response.AddHeader("Content-Disposition", "attachment; filename=" + dosya.Name); // Bu şekilde tarayıcı penceresinden hangi dosyanın indirileceği belirtilir. Eğer belirtilmesse bulunulan sayfanın kendisi indirilir. Okunaklı bir formattada olmaz.
                Response.AddHeader("Content-Length", dosya.Length.ToString()); // İndirilecek dosyanın uzunluğu bildirilir.
                Response.ContentType = "application/octet-stream"; // İçerik tipi belirtilir. Buna göre dosyalar binary formatta indirilirler.

                // octet stream için PDF türüne göre karar yapısı koyulacak. !!! !!!  !!!

                Response.WriteFile(DocPath); // Dosya indirme işlemi başlar.
                Response.Flush();
                Response.End(); // Süreç bu noktada sonlandırılır. Bir başka deyişle bu satırdan sonraki satırlar işletilmez hatta global.asax dosyasında eğer yazılmışsa Application_EndRequest metodu çağırılır.
            }

            return null;
        }
        public FileResult FullScreen(int Id) // File Showing part... Submit Documents
        {
            var req = db.Uploads.Where(w => w.Id == Id).FirstOrDefault();
            var DocPath = req.FilePath;
            DocPath = Server.MapPath(DocPath);
            FileInfo dosya = new FileInfo(DocPath);
            var mimetype = GetFileType(dosya.Extension);
            byte[] FileBytes = System.IO.File.ReadAllBytes(DocPath);
            if (mimetype == "Application/msword" || mimetype == "Application/pdf")
                return File(FileBytes, mimetype);
            else
                return File(FileBytes, DocPath);
        }
        [NonAction]
        public string GetFileType(string ext)  // for file extensions
        {
            //['jpg', 'gif', 'pdf', 'png', 'jpeg', 'bmp', 'doc', 'docx'];
            switch (ext)
            {
                case ".doc":
                case ".docx":
                    return "Application/msword";
                case ".pdf":
                    return "Application/pdf";
                case ".jpg":
                case ".jpeg":
                    return "image/jpeg";
                case ".gif":
                    return "image/gif";
                case ".png":
                    return "image/png";
                case ".bmp":
                    return "image/bmp";
                default:
                    return "";
            }
        }
        [HttpPost]
        public ActionResult FileUpload(FormCollection data) // File upload and streaming part...
        {
            int userID = (int)Session["userID"];
            var AppId = db.Applications.Where(x => x.UserId == userID).FirstOrDefault().ID;
            var FileType = data.GetValue("id").AttemptedValue;
            var UploadedFileInfo = new Dictionary<string, string>();
            string filename = "", dbfilepath = "";
            if (Request.Files["files"] != null)
            {
                HttpPostedFileBase file = Request.Files["files"];
                if (file != null)
                {
                    var fileinfo = new FileInfo(file.FileName);
                    if (file.ContentLength > 2097152) // More than 2MB check
                    {
                        UploadedFileInfo.Add("id", FileType);
                        UploadedFileInfo.Add("Message", "File size must be less than 2 MB");
                        return Json(UploadedFileInfo);
                    }
                    if (!Directory.Exists(Server.MapPath("~/UploadedFiles/" + AppId + "/")))
                        Directory.CreateDirectory(Server.MapPath("~/UploadedFiles/" + AppId + "/"));
                    filename = $"{AppId}/{FileType + "_" + file.FileName}";
                    UploadedFileInfo.Add("id", FileType);
                    UploadedFileInfo.Add("FileName", file.FileName);
                    file.SaveAs(Server.MapPath("~/UploadedFiles/" + filename));
                    dbfilepath = "~/UploadedFiles/" + filename;
                }
                var valueExist = db.Uploads.Where(x => x.AppId == (int)AppId && x.FileType == FileType).FirstOrDefault();
                if (valueExist != null)
                {
                    valueExist.FilePath = dbfilepath;
                    valueExist.FileName = file.FileName;
                }
                else
                {
                    db.Uploads.Add(new Upload
                    {
                        AppId = (int)AppId,
                        FilePath = dbfilepath,
                        FileType = FileType,
                        FileName = UploadedFileInfo["FileName"]
                    });
                }

                int ret = db.SaveChanges();
                // ViewData["UploadedList"] = GetFiles((int)AppId);
            }
            UploadFileReturn();
            return Json(UploadedFileInfo);
        }
        [HttpPost]
        public JsonResult CheckboxControl(AjaxData ajaxData)
        {
            try
            {
                if (ajaxData.ChkSeperator == 1) // Checkbox type is coming from Language Certificate
                {
                    var langcert = db.LanguageCerts.Where(W => W.AppId == ajaxData.appId).FirstOrDefault();
                    if (langcert == null) // Language certificate not inserted step
                    {
                        db.LanguageCerts.Add(new LanguageCert
                        {
                            AppId = ajaxData.appId,
                            LangCert = ajaxData.Checkbox
                        });
                    }
                    else
                        db.LanguageCerts.Where(W => W.AppId == ajaxData.appId).FirstOrDefault().LangCert = ajaxData.Checkbox;
                    db.SaveChanges();
                }

                if (ajaxData.ChkSeperator == 2) // Checkbox type is coming from Background education
                {
                    var Awarded = db.BgEducations.Where(W => W.AppID == ajaxData.appId).FirstOrDefault();
                    if (Awarded == null)
                    {
                        db.BgEducations.Add(new BgEducation
                        {
                            AppID = ajaxData.appId,
                            Awarded = ajaxData.Checkbox
                        });
                    }
                    else
                        db.BgEducations.Where(W => W.AppID == ajaxData.appId).FirstOrDefault().Awarded = ajaxData.Checkbox;
                    db.SaveChanges();
                }
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                Response.End();

            }
            return Json("", JsonRequestBehavior.AllowGet);
        }
        public PartialViewResult UploadFileReturn()    // Automatic File table refreshing with this Function
        {
            var userID = (int)Session["userID"];
            var AppID = db.Applications.Where(X => X.UserId == userID).FirstOrDefault().ID;
            var Upload1 = (from p in db.Uploads
                           where p.AppId == AppID
                           select new UploadedFiles
                           {
                               FilePath = p.FilePath,
                               FileType = p.FileType,
                               FileName = p.FileName,
                               FileNote = p.FileNote,
                               Id = db.Uploads.Where(x => x.AppId == AppID && x.FileType == p.FileType).FirstOrDefault().Id
                           }).ToList();
            return PartialView(@"~/Views/Application/UploadFileTable.cshtml", Upload1.AsEnumerable());
        }

        public JsonResult UploadFileNote(int id,string text) // File Comment's will insert to database
        {
            var userID = (int)Session["UserId"];
            int AppID = db.Applications.Where(x => x.UserId == userID).FirstOrDefault().ID;
            db.Uploads.Where(w => w.AppId == AppID && w.Id == id).FirstOrDefault().FileNote = text;
            db.SaveChanges();
            return Json("", JsonRequestBehavior.AllowGet);
        }

        // Redirect Pages -----------------------------------------------------------------------------------
        public ActionResult Submitted()
        {
            return View();
        }
        public ActionResult AboutMe()
        {
            var userID = Session["UserId"];
            var UserInfo = db.Users.Find((int)userID);
            return View("~/Views/AboutMe/AboutMe.cshtml", UserInfo);
        }
        public ActionResult ContactUs()
        {
            return View("~/Views/ContactUs/ContactUs.cshtml");
        }


        // Application Main Page Process ---------------------------------------------------------------------
        public ActionResult Index()
        {
            var userID = (int)Session["UserId"];
            var values = db.Users.FirstOrDefault(z => z.Id == userID);
            var ApplicationVrt = db.Applications.Where(x => x.UserId == values.Id).FirstOrDefault();

            ViewBag.Name = values.Name;
            ViewBag.Surname = values.Surname;
            if (ApplicationVrt != null)
            {
                Session["ApplicationID"] = ApplicationVrt.ID;
                var ApplicationID = Session["ApplicationID"];


                var Uploadeds = (from p in db.Uploads
                                 where p.AppId == (int)ApplicationID
                                 select new UploadedFiles
                                 {
                                     FilePath = p.FilePath,
                                     FileType = p.FileType,
                                     FileName = p.FileName
                                 }).ToList();

                for (int i = 0; i < Uploadeds.Count; i++)
                {
                    Server.MapPath("");
                    switch (Uploadeds[i].FileType)
                    {
                        case "passport":
                            ViewBag.passport = Uploadeds[i].FileName;
                            break;
                        case "NationalID":
                            ViewBag.NationalID = Uploadeds[i].FileName;
                            break;
                        case "Diploma":
                            ViewBag.Diploma = Uploadeds[i].FileName;
                            break;
                        case "Transcript":
                            ViewBag.Transcript = Uploadeds[i].FileName;
                            break;
                        case "CV":
                            ViewBag.CV = Uploadeds[i].FileName;
                            break;
                        case "IELTSorToeflCertificate":
                            ViewBag.IELTSorToeflCertificate = Uploadeds[i].FileName;
                            break;
                        case "Extradocument1":
                            ViewBag.Extradocument1 = Uploadeds[i].FileName;
                            break;
                        case "Extradocument2":
                            ViewBag.Extradocument2 = Uploadeds[i].FileName;
                            break;
                        case "Extradocument3":
                            ViewBag.Extradocument3 = Uploadeds[i].FileName;
                            break;
                        case "Extradocument4":
                            ViewBag.Extradocument4 = Uploadeds[i].FileName;
                            break;
                        default:
                            break;
                    }
                }

            }

            var newuser = new StudentApp.ObjectModels.Users
            {
                ActivationCode = values.ActivationCode,
                //ApplicationID = values,
                Email = values.Email,
                Id = values.Id,
                IsEmailVerified = values.IsEmailVerified,
                Name = values.Name,
                Surname = values.Surname,
                // Application1 = db.Applications.Find(ApplicationID)
            };
            return RedirectToAction("Index", "Dashboard");
        }
        public ActionResult ApplicationIndex()   // Application Index Redirect Part, and some controls
        {
            var userId = Session["UserId"];
            var AppRecordExist = db.Applications.Where(x => x.UserId == (int)userId).FirstOrDefault();
            var userTbl = db.Users.Find(userId);
            ViewBag.Name = userTbl.Name;
            ViewBag.Surname = userTbl.Surname;
            if (AppRecordExist == null)  // Application exist do 
            {
                db.Applications.Add(new Application
                {
                    Name = userTbl.Name,
                    Surname = userTbl.Surname,
                    Email = userTbl.Email,
                    UserId = userTbl.Id
                });
                db.SaveChanges();
                AppRecordExist = db.Applications.Where(x => x.UserId == (int)userId).FirstOrDefault();
                //db.BgEducations.Add(new BgEducation{ AppID = AppRecordExist.ID, Awarded = false });
                db.FtEducations.Add(new FtEducation { AppId = AppRecordExist.ID });
                //db.WorkExps.Add(new WorkExp { AppId = AppRecordExist.ID });
                //db.LanguageCerts.Add(new LanguageCert { AppId = AppRecordExist.ID, LangCert = false });
                db.SaveChanges();
                var appId = db.Applications.Where((x) => x.UserId == (int)userId).FirstOrDefault().ID;
                Session["ApplicationID"] = appId;
                var newApplication = new StudentApp.ObjectModels.Application
                {
                    Name = userTbl.Name,
                    Surname = userTbl.Surname,
                    userId = userTbl.Id,
                    Email = userTbl.Email,
                    ID = db.Applications.Where((x) => x.UserId == (int)userId).FirstOrDefault().ID,
                    BgEducation1 = db.BgEducations.Where(x => x.AppID == appId).ToList(),
                    FtEducation = db.FtEducations.Where(x => x.AppId == appId).FirstOrDefault(),
                    WorkExp = db.WorkExps.Where(x => x.AppId == appId).ToList(),
                    LanguageCert = db.LanguageCerts.Where(x => x.AppId == appId).FirstOrDefault()
                };


                if (newApplication.BgEducation1 == null || newApplication.BgEducation1.Count == 0) // if there is a Application but 
                {
                    //db.BgEducations.Add(new BgEducation { AppID = AppRecordExist.ID, Awarded = false });
                    //db.SaveChanges();
                    //newApplication.BgEducation1 = db.BgEducations.Where(x => x.AppID == AppRecordExist.ID).ToList();
                    BgEducation bg = new BgEducation();
                    newApplication.BgEducation1.Add(bg);
                    newApplication.BgEducation1[0].AppID = AppRecordExist.ID;
                    newApplication.BgEducation1[0].Faculty = "";
                    newApplication.BgEducation1[0].InsturactionLang = "";
                    newApplication.BgEducation1[0].EducationLevel = "";
                    newApplication.BgEducation1[0].AvarageGrade = 0.0;
                    newApplication.BgEducation1[0].Awarded = false;
                    newApplication.BgEducation1[0].EducationCompDate = DateTime.Now;
                    newApplication.BgEducation1[0].EducationStDate = DateTime.Now;
                    newApplication.BgEducation1[0].InsCountry = "";
                    newApplication.BgEducation1[0].InstitutionName = "";
                    newApplication.BgEducation1[0].StudyMode = "";
                }
                if (newApplication.FtEducation == null)
                {
                    //db.FtEducations.Add(new FtEducation { AppId = AppRecordExist.ID });
                    //db.SaveChanges();
                    //newApplication.FtEducation = db.FtEducations.Where(x => x.AppId == AppRecordExist.ID).FirstOrDefault();
                    newApplication.FtEducation.AppId = AppRecordExist.ID;
                    newApplication.FtEducation.Country1 = "";
                    newApplication.FtEducation.EducationLevel = "";
                    newApplication.FtEducation.AcademicYear = Convert.ToInt32(DateTime.Now.ToString("yyyy"));
                    newApplication.FtEducation.Country2 = "";
                    newApplication.FtEducation.FieldOfStudy = "";
                    newApplication.FtEducation.Intake = "";
                }
                if (newApplication.WorkExp == null || newApplication.WorkExp.Count == 0)
                {
                    //db.WorkExps.Add(new WorkExp { AppId = AppRecordExist.ID });
                    //db.SaveChanges();
                    //newApplication.WorkExp = db.WorkExps.Where(x => x.AppId == AppRecordExist.ID).ToList();
                    WorkExp wrExp = new WorkExp();
                    newApplication.WorkExp.Add(wrExp);
                    newApplication.WorkExp[0].AppId = AppRecordExist.ID;
                    newApplication.WorkExp[0].CompanyName = "";
                    newApplication.WorkExp[0].EmployeeAdress = "";
                    newApplication.WorkExp[0].EmployeeMail = "";
                    newApplication.WorkExp[0].EmployeePhone = "";
                    newApplication.WorkExp[0].JobDescription = "";
                    newApplication.WorkExp[0].JobType = "";
                    newApplication.WorkExp[0].ManagerName = "";
                    newApplication.WorkExp[0].Position = "";

                }
                if (newApplication.LanguageCert == null)
                {
                    newApplication.LanguageCert = new LanguageCert();
                    newApplication.LanguageCert.AppId = newApplication.ID;
                    newApplication.LanguageCert.OverallScoreTOEFL = "0";
                    newApplication.LanguageCert.OverallScoreIELTS = "0";
                    newApplication.LanguageCert.WritingTOEFL = 0;
                    newApplication.LanguageCert.WritingIELTS = 0;
                    newApplication.LanguageCert.ReadingIELTS = 0;
                    newApplication.LanguageCert.ReadingTOEFL = 0;
                    newApplication.LanguageCert.SpeakingTOEFL = 0;
                    newApplication.LanguageCert.SpeakingIELTS = 0;
                    newApplication.LanguageCert.ListeningIELTS = 0;
                    newApplication.LanguageCert.ListeningTOEFL = 0;
                    newApplication.LanguageCert.LangCert = false;
                    newApplication.LanguageCert.CertNoIELTS = " Doesn't Exist";
                    newApplication.LanguageCert.CertNoTOEFL = " Doesn't Exist";
                    newApplication.LanguageCert.CertType = "Other";
                    newApplication.LanguageCert.CertOtherIELTS = " Doesn't Exist";
                    newApplication.LanguageCert.CertOtherTOEFL = " Doesn't Exist";
                    newApplication.LanguageCert.TestDateIELTS = DateTime.Now;
                    newApplication.LanguageCert.TestDateTOEFL = DateTime.Now;
                }


                return View("Index", newApplication);
            }
            else    // Application doesn't exist 
            {
                var newApplication = new StudentApp.ObjectModels.Application // Application değerini tanımlı class'a atıyoruz.
                {
                    ID = AppRecordExist.ID,
                    Name = AppRecordExist.Name,
                    Surname = AppRecordExist.Surname,
                    userId = (int)AppRecordExist.UserId,
                    Gender = AppRecordExist.Gender,
                    maritalStatus = AppRecordExist.maritalStatus,
                    AddressLine1 = AppRecordExist.AddressLine1,
                    AddressLine2 = AppRecordExist.AddressLine2,
                    City = AppRecordExist.City,
                    State = AppRecordExist.State,
                    Zip = AppRecordExist.Zip,
                    Country = AppRecordExist.Country,
                    DateofBirth = AppRecordExist.DateofBirth,
                    CountryofBirth = AppRecordExist.CountryofBirth,
                    CitizenshipMain = AppRecordExist.CitizenshipMain,
                    PlaceofBirth = AppRecordExist.PlaceofBirth,
                    FatherName = AppRecordExist.FatherName,
                    FatherSurname = AppRecordExist.FatherSurname,
                    MotherName = AppRecordExist.MotherName,
                    MotherSurname = AppRecordExist.MotherSurname,
                    PhoneNumber = AppRecordExist.PhoneNumber,
                    PassaportNumber = AppRecordExist.PassaportNumber,
                    PassStartDate = AppRecordExist.PassStartDate,
                    PassExpireDate = AppRecordExist.PassExpireDate,
                    IssuingCountry = AppRecordExist.IssuingCountry,
                    IssuingAuthority = AppRecordExist.IssuingAuthority,
                    NationalId = AppRecordExist.NationalId,
                    IdStartDate = AppRecordExist.IdStartDate,
                    IdExpireDate = AppRecordExist.IdExpireDate,
                    CountryCitizenship = AppRecordExist.CountryCitizenship,
                    Email = AppRecordExist.Email,
                    BgEducation1 = db.BgEducations.Where(x => x.AppID == AppRecordExist.ID).ToList(),
                    FtEducation = db.FtEducations.Where(x => x.AppId == AppRecordExist.ID).FirstOrDefault(),
                    WorkExp = db.WorkExps.Where(x => x.AppId == AppRecordExist.ID).ToList(),
                    LanguageCert = db.LanguageCerts.Where(x => x.AppId == AppRecordExist.ID).FirstOrDefault()
                };
                if (newApplication.BgEducation1 == null || newApplication.BgEducation1.Count==0) // if there is a Application but 
                {
                    //db.BgEducations.Add(new BgEducation { AppID = AppRecordExist.ID, Awarded = false });
                    //db.SaveChanges();
                    //newApplication.BgEducation1 = db.BgEducations.Where(x => x.AppID == AppRecordExist.ID).ToList();
                    BgEducation bg = new BgEducation();
                    newApplication.BgEducation1.Add(bg);
                    newApplication.BgEducation1[0].AppID = AppRecordExist.ID;
                    newApplication.BgEducation1[0].Faculty = "";
                    newApplication.BgEducation1[0].InsturactionLang = "";
                    newApplication.BgEducation1[0].EducationLevel = "";
                    newApplication.BgEducation1[0].AvarageGrade = 0.0;
                    newApplication.BgEducation1[0].Awarded = false;
                    newApplication.BgEducation1[0].EducationCompDate = DateTime.Now;
                    newApplication.BgEducation1[0].EducationStDate = DateTime.Now;
                    newApplication.BgEducation1[0].InsCountry = "";
                    newApplication.BgEducation1[0].InstitutionName = "";
                    newApplication.BgEducation1[0].StudyMode = "";
                }
                if (newApplication.FtEducation == null)
                {
                    //db.FtEducations.Add(new FtEducation { AppId = AppRecordExist.ID });
                    //db.SaveChanges();
                    //newApplication.FtEducation = db.FtEducations.Where(x => x.AppId == AppRecordExist.ID).FirstOrDefault();
                    newApplication.FtEducation.AppId = AppRecordExist.ID;
                    newApplication.FtEducation.Country1 = "";
                    newApplication.FtEducation.EducationLevel = "";
                    newApplication.FtEducation.AcademicYear = Convert.ToInt32(DateTime.Now.ToString("yyyy"));
                    newApplication.FtEducation.Country2 = "";
                    newApplication.FtEducation.FieldOfStudy = "";
                    newApplication.FtEducation.Intake = "";
                }
                if (newApplication.WorkExp == null || newApplication.WorkExp.Count==0)
                {
                    //db.WorkExps.Add(new WorkExp { AppId = AppRecordExist.ID });
                    //db.SaveChanges();
                    //newApplication.WorkExp = db.WorkExps.Where(x => x.AppId == AppRecordExist.ID).ToList();
                    WorkExp wrExp = new WorkExp();
                    newApplication.WorkExp.Add(wrExp);
                    newApplication.WorkExp[0].AppId = AppRecordExist.ID;
                    newApplication.WorkExp[0].CompanyName = "";
                    newApplication.WorkExp[0].EmployeeAdress = "";
                    newApplication.WorkExp[0].EmployeeMail = "";
                    newApplication.WorkExp[0].EmployeePhone = "";
                    newApplication.WorkExp[0].JobDescription = "";
                    newApplication.WorkExp[0].JobType = "";
                    newApplication.WorkExp[0].ManagerName = "";
                    newApplication.WorkExp[0].Position = "";
                    
                } 
                if (newApplication.LanguageCert == null)
                {
                    newApplication.LanguageCert = new LanguageCert();
                    newApplication.LanguageCert.AppId = newApplication.ID;
                    newApplication.LanguageCert.OverallScoreTOEFL = "0";
                    newApplication.LanguageCert.OverallScoreIELTS = "0";
                    newApplication.LanguageCert.WritingTOEFL      = 0;
                    newApplication.LanguageCert.WritingIELTS      = 0;
                    newApplication.LanguageCert.ReadingIELTS      = 0;
                    newApplication.LanguageCert.ReadingTOEFL      = 0;
                    newApplication.LanguageCert.SpeakingTOEFL     = 0;
                    newApplication.LanguageCert.SpeakingIELTS     = 0;
                    newApplication.LanguageCert.ListeningIELTS    = 0;
                    newApplication.LanguageCert.ListeningTOEFL    = 0;
                    newApplication.LanguageCert.LangCert          = false;
                    newApplication.LanguageCert.CertNoIELTS       = " Doesn't Exist";
                    newApplication.LanguageCert.CertNoTOEFL       = " Doesn't Exist";
                    newApplication.LanguageCert.CertType          = "Other";
                    newApplication.LanguageCert.CertOtherIELTS    = " Doesn't Exist"; 
                    newApplication.LanguageCert.CertOtherTOEFL    = " Doesn't Exist";
                    newApplication.LanguageCert.TestDateIELTS     = DateTime.Now;
                    newApplication.LanguageCert.TestDateTOEFL     = DateTime.Now;
                }
                return View("ApplicationIndex", newApplication);
            }



        }
        public ActionResult DetailsButton(Application app)  // if application exist and detail button was triggered....
        {
            var application = db.Applications.Find(app.ID);
            var newApplication = new StudentApp.ObjectModels.Application
            {
                Name = application.Name,
                Surname = application.Surname,
                userId = (int)application.UserId,
                Gender = application.Gender,
                maritalStatus = application.maritalStatus,
                AddressLine1 = application.AddressLine1,
                AddressLine2 = application.AddressLine2,
                City = application.City,
                State = application.State,
                Zip = application.Zip,
                Country = application.Country,
                DateofBirth = application.DateofBirth,
                CountryofBirth = application.CountryofBirth,
                CitizenshipMain = application.CitizenshipMain,
                PlaceofBirth = application.PlaceofBirth,
                FatherName = application.FatherName,
                FatherSurname = application.FatherSurname,
                MotherName = application.MotherName,
                MotherSurname = application.MotherSurname,
                PhoneNumber = application.PhoneNumber,
                PassaportNumber = application.PassaportNumber,
                PassStartDate = application.PassStartDate,
                PassExpireDate = application.PassExpireDate,
                IssuingCountry = application.IssuingCountry,
                IssuingAuthority = application.IssuingAuthority,
                NationalId = application.NationalId,
                IdStartDate = application.IdStartDate,
                IdExpireDate = application.IdExpireDate,
                CountryCitizenship = application.CountryCitizenship,
                Email = application.Email,
                BgEducation1 = db.BgEducations.Where(x => x.AppID == application.ID).ToList(),
                FtEducation = db.FtEducations.Where(x => x.AppId == application.ID).FirstOrDefault(),
                WorkExp = db.WorkExps.Where(x => x.AppId == application.ID).ToList(),
                LanguageCert = db.LanguageCerts.Where(x => x.AppId == application.ID).FirstOrDefault()
            };
            return View("Index", newApplication);
        }
        [HttpPost]
        public ActionResult UpdateApplication(StudentApp.ObjectModels.Application user, Application application, HttpPostedFileBase YuklenecekDosya)
        {
            var userUpd = db.Users.Find(Session["UserId"]);
            var Application = db.Applications.Where(x => x.UserId == userUpd.Id).FirstOrDefault();
            // var appId = db.Applications.Find(application.ID);
            userUpd.Name = user.Name;
            /* 0 = Married 1= Divorced */
            userUpd.Surname = user.Surname;
            Application.Gender = user.Gender;
            Application.maritalStatus = user.maritalStatus;
            Application.AddressLine1 = user.AddressLine1;
            Application.AddressLine2 = user.AddressLine2;
            Application.City = user.City;
            Application.State = user.State;
            Application.Zip = user.Zip;
            Application.Country = user.Country;
            Application.DateofBirth = user.DateofBirth;
            Application.CountryofBirth = user.CountryofBirth;
            Application.CitizenshipMain = user.CitizenshipMain;
            Application.PlaceofBirth = user.PlaceofBirth;
            Application.FatherName = user.FatherName;
            Application.FatherSurname = user.FatherSurname;
            Application.MotherName = user.MotherName;
            Application.MotherSurname = user.MotherSurname;
            Application.PhoneNumber = user.PhoneNumber;
            Application.PassaportNumber = user.PassaportNumber;
            Application.PassStartDate = user.PassStartDate;
            Application.PassExpireDate = user.PassExpireDate;
            Application.IssuingCountry = user.IssuingCountry;
            Application.IssuingAuthority = user.IssuingAuthority;
            Application.NationalId = user.NationalId;
            Application.IdStartDate = user.IdStartDate;
            Application.IdExpireDate = user.IdExpireDate;
            Application.CountryCitizenship = user.CountryCitizenship;
            Application.Email = user.Email;


            //var bguser = db.BgEducations.Where(x => x.AppID == user.ID).FirstOrDefault();
            //db.BgEducations.Add(new BgEducation
            //{
            //    AppID = bguser.ID,
            //    InstitutionName = bguser.InstitutionName,
            //    Faculty = bguser.Faculty,
            //    InsturactionLang = bguser.InsturactionLang,
            //    InsCountry = bguser.InsCountry,
            //    StudyMode = bguser.StudyMode,
            //    EducationLevel = bguser.EducationLevel,
            //    EducationStDate = bguser.EducationStDate,
            //    EducationCompDate = bguser.EducationCompDate,
            //    AvarageGrade = bguser.AvarageGrade
            //});

            db.FtEducations.Add(new FtEducation
            {
                AppId = user.ID,
                Country1 = user.FtEducation.Country1,
                Country2 = user.FtEducation.Country2,
                FieldOfStudy = user.FtEducation.FieldOfStudy,
                EducationLevel = user.FtEducation.EducationLevel,
                Intake = user.FtEducation.Intake,
                AcademicYear = user.FtEducation.AcademicYear
            });

            //db.WorkExps.Add(new WorkExp
            //{
            //    AppId = user.ID,
            //    CompanyName = user.WorkExp.CompanyName,
            //    Position = user.WorkExp.Position,
            //    JobType = user.WorkExp.JobType,
            //    EmployeeAdress = user.WorkExp.EmployeeAdress,
            //    ManagerName = user.WorkExp.ManagerName,
            //    EmployeeMail = user.WorkExp.EmployeeMail,
            //    EmployeePhone = user.WorkExp.EmployeePhone,
            //    JobDescription = user.WorkExp.JobDescription
            //});


            // Check Record, if record already in database, update LangCertification 
            var CheckRecord = db.LanguageCerts.Where(x => x.AppId == user.ID).FirstOrDefault();
            if (CheckRecord == null)
            {
                db.LanguageCerts.Add(new LanguageCert
                {
                    AppId = user.ID,
                    ListeningIELTS = user.LanguageCert.ListeningIELTS,
                    ReadingIELTS = user.LanguageCert.ReadingIELTS,
                    WritingIELTS = user.LanguageCert.WritingIELTS,
                    SpeakingIELTS = user.LanguageCert.SpeakingIELTS,
                    OverallScoreIELTS = user.LanguageCert.OverallScoreIELTS,
                    TestDateIELTS = user.LanguageCert.TestDateIELTS,
                    CertNoIELTS = user.LanguageCert.CertNoIELTS,
                    CertOtherIELTS = user.LanguageCert.CertOtherIELTS,
                    ListeningTOEFL = user.LanguageCert.ListeningTOEFL,
                    ReadingTOEFL = user.LanguageCert.ReadingTOEFL,
                    WritingTOEFL = user.LanguageCert.WritingTOEFL,
                    SpeakingTOEFL = user.LanguageCert.SpeakingTOEFL,
                    OverallScoreTOEFL = user.LanguageCert.OverallScoreTOEFL,
                    TestDateTOEFL = user.LanguageCert.TestDateTOEFL,
                    CertNoTOEFL = user.LanguageCert.CertNoTOEFL,
                    CertOtherTOEFL = user.LanguageCert.CertOtherTOEFL,
                    CertType = user.LanguageCert.CertType
                });
            }
            else
            {
                CheckRecord.ListeningIELTS = user.LanguageCert.ListeningIELTS;
                CheckRecord.ReadingIELTS = user.LanguageCert.ReadingIELTS;
                CheckRecord.SpeakingIELTS = user.LanguageCert.SpeakingIELTS;
                CheckRecord.WritingIELTS = user.LanguageCert.WritingIELTS;
                CheckRecord.OverallScoreIELTS = user.LanguageCert.OverallScoreIELTS;
                CheckRecord.TestDateIELTS = user.LanguageCert.TestDateIELTS;
                CheckRecord.CertNoIELTS = user.LanguageCert.CertNoIELTS;
                CheckRecord.CertOtherIELTS = user.LanguageCert.CertOtherIELTS;
                CheckRecord.ListeningTOEFL = user.LanguageCert.ListeningTOEFL;
                CheckRecord.ReadingTOEFL = user.LanguageCert.ReadingTOEFL;
                CheckRecord.WritingTOEFL = user.LanguageCert.WritingTOEFL;
                CheckRecord.SpeakingTOEFL = user.LanguageCert.SpeakingTOEFL;
                CheckRecord.OverallScoreTOEFL = user.LanguageCert.OverallScoreTOEFL;
                CheckRecord.TestDateTOEFL = user.LanguageCert.TestDateTOEFL;
                CheckRecord.CertNoTOEFL = user.LanguageCert.CertNoTOEFL;
                CheckRecord.CertOtherTOEFL = user.LanguageCert.CertOtherTOEFL;
                CheckRecord.CertType = user.LanguageCert.CertType;
            }


            if (YuklenecekDosya != null)
            {
                string dosyaYolu = Path.GetFileName(YuklenecekDosya.FileName);
                var yuklemeYeri = Path.Combine(Server.MapPath("~/UploadedFiles"), dosyaYolu);
                YuklenecekDosya.SaveAs(yuklemeYeri);
            }

            db.SaveChanges();
            TempData["Message"] = "Your Application has been successfully Submitted.";

            return RedirectToAction("Submitted");
        }



    }
}
