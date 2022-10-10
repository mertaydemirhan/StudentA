using StudentApp.Models.Entity;
using StudentApplication.Repositories;
using System;
using System.Linq;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Net.Mail;
using SendGrid.Helpers.Mail;
using System.Net;
using System.Threading;
using System.Web.Security;
using StudentApp.Models;

namespace StudentApplication.Controllers
{
    public class UserController : Controller
    {
        readonly StudentAppEntities db = new StudentAppEntities();
        readonly RegistrationRepository registrationRepository = new RegistrationRepository();


        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }


        #region Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register([Bind(Exclude = "IsEmailVerified,ActivationCode")] StudentApp.Models.Entity.User user)
        {
            bool Status = false;
            string Message;
            try { 
            //Model validation
            if (ModelState.IsValid)
            {
                //Email already exist
                var isExist = IsEmailExist(user.Email);
                if (isExist)
                {
                    ModelState.AddModelError("EmailExist", "Email already exists");
                    return View(user);
                }


                if (user.Password != user.ConfirmPassword)
                {
                    ModelState.AddModelError("PasswordsDoNotMatch", "Passwords do not match!");
                    return View(user);
                }

                //Generate Activation Code
                user.ActivationCode = Guid.NewGuid();

                //Password Hashing
                user.Password = Crypto.Hash(user.Password);
                user.ConfirmPassword = Crypto.Hash(user.ConfirmPassword);
                user.IsEmailVerified = false;
                //Save data to database
                // registrationRepository.StudentAdd(db.Users.);
                try
                {
                    db.Users.Add(user);
                    db.SaveChanges();
                }catch(Exception)
                {
                    throw;
                }
               

                SendVerificationEmail(user.Email, user.ActivationCode.ToString());
                Message = "Registration successfully done. Account activation link " +
                         "has been sent to your Email: " + user.Email;
                Status = true;

                //ModelState.Clear();
            }
            else
            {
                Message = "Something went wrong, please try again.";
            }
            }
            catch
            {
                Message = "Something went wrong, please try again.";

            }
            ViewBag.Status = Status;
            ViewBag.Message = Message;

            return View();

        }
        #endregion

        #region Verify Account
        [HttpGet]
        public ActionResult VerifyAccount(string id)
        {
            bool Status = false;
            db.Configuration.ValidateOnSaveEnabled = false;
            var v = db.Users.Where(a => a.ActivationCode == new Guid(id)).FirstOrDefault();
            if (v != null)
            {
                v.IsEmailVerified = true;
                db.SaveChanges();
                Status = true;
            }
            else
            {
                ViewBag.Message = "Invalid Request";
            }
            ViewBag.Status = Status;
            return View();
        }
        #endregion

        #region IsEmailExist
        [NonAction]
        public bool IsEmailExist(string emailID)
        {
            try
            {
                using (StudentAppEntities db = new StudentAppEntities())
                {
                    var v = db.Users.Where(a=>a.Email==emailID).FirstOrDefault();
                    return v != null;
                }
            }
            catch (Exception)
            {

                throw;
            }

        }
        #endregion

        #region SendVerificationEmail
        [NonAction]
        public void SendVerificationEmail(string emailID, string activationCode)
        {
            var verifyUrl = "/User/VerifyAccount/" + activationCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

            var fromEmail = new MailAddress("admin@57colleges.com", "57Colleges | no-reply");
            var toEmail = new MailAddress(emailID);
            var fromEmailPassword = "kv5vsBiMSBRmc";
            string subject = "";
            string body = "<br/><br/> We are excited to tell you that your account is " +
                           "successfully created. Please click on the link below to verify your account " +
                           "<br/><br/><a href='" + link + "'>" + link + "</a>";


            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })

                smtp.Send(message);
        }
        #endregion




        #region Login

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(StudentApp.Models.Entity.User user)
        {
            string message = "";
        
            var v = db.Users.Where(a => a.Email == user.Email).FirstOrDefault();
            if (v != null)
            {
                if (string.Compare(Crypto.Hash(user.Password), v.Password) == 0)
                {
                    FormsAuthentication.SetAuthCookie(v.Email, false);
                    Session["Mail"] = v.Email.ToString();
                    Session["UserId"] = v.Id;
                    ViewBag.Name  = v.Name;
                    ViewBag.Surname = v.Surname;
                    return RedirectToAction("Welcome", "Welcome"    );
                }

                else
                {
                    message = "Invalid credentials provided";
                }
            }

            else
            {
                message = "Something wrong happened, please try again";
            }

            ViewBag.MessageLogin = message;
            return View();
        }
        #endregion

        #region LogOut

        public ActionResult Logout()
        {
            Session["UserInfo"] = null;
            Session.Abandon();
            return RedirectToAction("Login", "User");
        }
        #endregion
    }
}
