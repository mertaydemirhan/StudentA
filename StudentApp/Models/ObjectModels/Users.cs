using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StudentApp.ObjectModels
{
    public class Users
    {
        public Guid ActivationCode { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Confirm Password is required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match!")]
        public string ConfirmPassword { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Name is required")]

        public string Name { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Surname is required")]

        public string Surname { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Email Address is required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Min 6 characters required")]
        public string Password { get; set; }
        public bool IsEmailVerified { get; set; }
        public int Id { get; set; }
        public int ApplicationID { get; set; }
        public StudentApp.Models.Entity.Application Application1 { get; set; }
        public StudentApp.Models.Entity.Upload Upload1 { get; set; }
    }
}