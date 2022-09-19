using StudentApp.Models.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StudentApp.ObjectModels
{
    public class Upload
    {
        public int Id { get; set; }
        public string FileType { get; set; }
        public string FilePath { get; set; }
        public string FileNote { get; set; }
        public int AppId { get; set; }
        public string FileName { get; set; }
    }
}