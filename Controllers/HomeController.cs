using CRUDdemo.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;

namespace CRUDdemo.Controllers
{
    public class HomeController : Controller
    {
        CRUDdbDataContext db = new CRUDdbDataContext();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult ProjectList(clsCommon cls)
        {
            cls.LSTList = (from data in db.tblProjects
                           select new clsCommon
                           {
                               strProjectName = data.ProjectName,
                               dtDevelopementStartDate = data.DevelopementStartDate,
                               strFile = data.ProjectLogo,
                               intProjectId=data.ProjectId,
                               isActive=data.IsActive,
                           }
            ).OrderByDescending(l=>l.intProjectId).ToList();
            return View(cls);
        }
        [HttpPost]
        public ActionResult InsertData(clsCommon cls)
        {
            tblProject project = new tblProject();
            if (cls != null)
            {
                if (cls.intProjectId > 0)
                {
                    var projectdata = (from data in db.tblProjects where data.ProjectId == cls.intProjectId select data).FirstOrDefault();
                    if (projectdata != null)
                    {
                        var duplicate = (from data in db.tblProjects where data.ProjectId != cls.intProjectId && cls.strProjectName == data.ProjectName.Trim() select data).FirstOrDefault();
                        if (duplicate != null)
                        {
                            cls.response = "exist";
                        }
                        else
                        {
                            projectdata.ProjectName = cls.strProjectName;
                            if (cls.file != null)
                            {
                                var fileName = Path.GetFileName(cls.file.FileName);
                                var ext = Path.GetExtension(cls.file.FileName);
                                string myfile = fileName + "_" + DateTime.Now.ToString("dd/MM/yyyy") + ext;
                                var path = System.Web.HttpContext.Current.Server.MapPath("~/Image/" + myfile);
                                cls.file.SaveAs(path);
                                projectdata.ProjectLogo = myfile;
                            }
                            if (cls.strDate != null)
                            {
                                projectdata.DevelopementStartDate = DateTime.ParseExact(cls.strDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            }
                            db.SubmitChanges();
                            cls.response = "Update";
                        }
                    }
                }
                else
                {
                    var duplicate = (from data in db.tblProjects where cls.strProjectName == data.ProjectName.Trim() select data).FirstOrDefault();
                    if (duplicate != null)
                    {
                        cls.response = "exist";
                    }
                    else
                    {
                        project.ProjectName = cls.strProjectName;
                        if (cls.file != null)
                        {
                            var fileName = Path.GetFileName(cls.file.FileName);
                            var ext = Path.GetExtension(cls.file.FileName);
                            string myfile = fileName + "_" + DateTime.Now.ToString("dd/MM/yyyy") + ext;
                            var path = System.Web.HttpContext.Current.Server.MapPath("~/Image/" + myfile);
                            cls.file.SaveAs(path);
                            project.ProjectLogo = myfile;
                        }
                        if (cls.strDate != null)
                        {
                            project.DevelopementStartDate = DateTime.ParseExact(cls.strDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        }
                        project.IsActive = true;
                        db.tblProjects.InsertOnSubmit(project);
                        db.SubmitChanges();
                        cls.response = "Insert";
                    }
                }
            }
            return Json(cls.response, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getData(clsCommon cls)
        {
            var project = (from data in db.tblProjects where data.ProjectId == cls.intProjectId select data).FirstOrDefault();
            
            return Json(project, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteData(clsCommon cls)
        {
            var project = (from data in db.tblProjects where data.ProjectId == cls.intProjectId select data).FirstOrDefault();
            if (project != null)
            {
                db.tblProjects.DeleteOnSubmit(project);
                db.SubmitChanges();
                cls.response = "success";
            }
            return Json(cls.response, JsonRequestBehavior.AllowGet);
        }
        public ActionResult UpdateStatus(clsCommon cls)
        {
            var project = (from data in db.tblProjects where data.ProjectId == cls.intProjectId select data).FirstOrDefault();
            if (project != null)
            {
                project.IsActive = project.IsActive == true ? false : true;
                db.SubmitChanges();
                cls.response = "success";
            }
            return Json(cls.response, JsonRequestBehavior.AllowGet);
        }
    }
}