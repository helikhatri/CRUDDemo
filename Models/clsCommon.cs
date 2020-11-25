using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRUDdemo.Models
{
    public class clsCommon
    {
        public int intProjectId { get; set; }
        public string strProjectName { get; set; }
        public HttpPostedFileBase file { get; set; }
        public bool? isActive { get; set; }
        public string strFile { get; set; }
        public string strDate { get; set; }
        public string response { get; set; }
        public List<clsCommon> LSTList { get; set; }
        public DateTime? dtDevelopementStartDate { get; set; }
    }
}