using Marmara.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Marmara.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        //MarmaraEntities db = new MarmaraEntities(); 
        
        // GET: Admin/Dashboard
        public ActionResult Index()
        {
            return View();
        }
    }
}