using StudentApp.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentApp.Models
{
    public class UploadedFiles
    {
        public int Id { get; set; }
        public string FileType { get; set; }
        public string FilePath { get; set; }
        public int File_AppID { get; set; }
        public string FileName { get; set; }
        public string FileNote { get; set; }
        public IEnumerable<Upload> Files { get; set; }
    }
}