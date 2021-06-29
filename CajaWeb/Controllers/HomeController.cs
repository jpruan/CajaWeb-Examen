using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CajaWeb.Models;
using static CajaWeb.Models.UserModel;

namespace CajaWeb.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult index()
        {
           


            return View();
        }


        public ActionResult caja()
        {

           
            return View();
        }
        public ActionResult estado()
        {

          
            return View();
        }
        public ActionResult usuarios()
        {


            return View();
        }
        /* public ActionResult users()
         {

           //  userInfo user = new userInfo { user_name = "juan_ruan", name = "Ruan", age = 23 };



            // return View(user);
             // return Content("Hola Mundo");
             // return new EmptyResult();
             //return HttpNotFound();
            // return RedirectToAction("Index", "Home/Index", new { page = 1, sort = "nameo" });
         }
         */
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

      
    }
}