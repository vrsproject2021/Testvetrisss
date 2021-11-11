using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;

namespace VETRIS.API.Controllers
{
    public class FunctionalitiesController : Controller
    {
        //
        // GET: /Functionalities/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DicomRouterLatestVersion()
        {
            string strPublishURL = ConfigurationManager.AppSettings["PublishURL"];
            ViewBag.PublishURL = strPublishURL.Trim();
            return View();
        }
        public ActionResult DicomRouterInstitutionDetails()
        {
            string strPublishURL = ConfigurationManager.AppSettings["PublishURL"];
            ViewBag.PublishURL = strPublishURL.Trim();
            return View();
        }
        public ActionResult DicomRouterCheckSession()
        {
            string strPublishURL = ConfigurationManager.AppSettings["PublishURL"];
            ViewBag.PublishURL = strPublishURL.Trim();
            return View();
        }
        public ActionResult DicomRouterUpdateOnlineStatus()
        {
            string strPublishURL = ConfigurationManager.AppSettings["PublishURL"];
            ViewBag.PublishURL = strPublishURL.Trim();
            return View();
        }
        public ActionResult DicomRouterCreateUploadNotification()
        {
            string strPublishURL = ConfigurationManager.AppSettings["PublishURL"];
            ViewBag.PublishURL = strPublishURL.Trim();
            return View();
        }
        public ActionResult DicomRouterCreateDownloadNotification()
        {
            string strPublishURL = ConfigurationManager.AppSettings["PublishURL"];
            ViewBag.PublishURL = strPublishURL.Trim();
            return View();
        }
        public ActionResult DicomRouterCreateFileTransferNotification()
        {
            string strPublishURL = ConfigurationManager.AppSettings["PublishURL"];
            ViewBag.PublishURL = strPublishURL.Trim();
            return View();
        }
        public ActionResult DicomRouterCreateFileTransferOTNotification()
        {
            string strPublishURL = ConfigurationManager.AppSettings["PublishURL"];
            ViewBag.PublishURL = strPublishURL.Trim();
            return View();
        }
        public ActionResult ChatUserDetails()
        {
            string strPublishURL = ConfigurationManager.AppSettings["PublishURL"];
            ViewBag.PublishURL = strPublishURL.Trim();
            return View();
        }
    }
}
