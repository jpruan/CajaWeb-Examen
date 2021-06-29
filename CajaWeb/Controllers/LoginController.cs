using CajaWeb.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CajaWeb.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult login()
        {
            return View();
        }


        public ActionResult makeLogin(String username, String password)
        {
            UserModel user = new UserModel();

            Boolean isLoginCorrect = user.mValidateLogin(username,password,out user);

            DataTable oResponseDynamic = new DataTable("Response");
            oResponseDynamic.Columns.Add("loginSuccess", typeof(Boolean));
            oResponseDynamic.Columns.Add("userInfo", typeof(UserModel));
          
            oResponseDynamic.Rows.Add(isLoginCorrect,user);

            string json = JsonConvert.SerializeObject(oResponseDynamic, Formatting.Indented);
            this.Session["_SessionCompany"] = json;
            return Content(json);
        }


        public ActionResult isSessionActive()
        {
            var session = this.Session["_SessionCompany"];


            if (session != null)
             return Content(session.ToString());

            return Content("");

        }

        public ActionResult logout()
        {
            this.Session["_SessionCompany"] = null;


            
                return RedirectToAction("login", "Login");



        }




    }
}