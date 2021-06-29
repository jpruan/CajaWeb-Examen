using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using CajaWeb.Models;
using Newtonsoft.Json;

namespace CajaWeb.Controllers
{
    public class UsersController : Controller
    {
        // GET: Users
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult getAllUsers()
        {
            UserModel userObject = new UserModel();

           DataTable allUsers = userObject.mGetAllUsers();

            DataTable oResponseDynamic = new DataTable("Response");
            oResponseDynamic.Columns.Add("pkUser", typeof(Int32));
            oResponseDynamic.Columns.Add("fullname", typeof(String));
            oResponseDynamic.Columns.Add("username", typeof(String));
            oResponseDynamic.Columns.Add("typeUser", typeof(String));



            foreach (DataRow row in allUsers.Rows) {
                oResponseDynamic.Rows.Add(row["pk_user"].ToString(), row["fullname"].ToString(), row["username"].ToString(), row["type_user"].ToString());


            }

            string json = JsonConvert.SerializeObject(oResponseDynamic, Formatting.Indented);
            return Content(json);

        }

        public ActionResult searchUsers(String username, String fullname, int id)
        {
            UserModel userObject = new UserModel();

            DataTable allUsers = userObject.mGetSearchUsers(username,fullname,id);

            DataTable oResponseDynamic = new DataTable("Response");
            oResponseDynamic.Columns.Add("pkUser", typeof(Int32));
            oResponseDynamic.Columns.Add("fullname", typeof(String));
            oResponseDynamic.Columns.Add("username", typeof(String));


            foreach (DataRow row in allUsers.Rows)
            {
                oResponseDynamic.Rows.Add(row["pk_user"].ToString(), row["fullname"].ToString(),row["username"].ToString());


            }

            string json = JsonConvert.SerializeObject(oResponseDynamic, Formatting.Indented);
            return Content(json);

        }

        public ActionResult Add(String fullname,String username,String password, String typeuser)
        {
            UserModel userObject = new UserModel();


            if (userObject.mExisteFullname(fullname, false,0))
                return Content("Error: El usuario " + fullname + " ya existe");

            if (userObject.mExisteUsername(username, false,0))
                return Content("Error: El username " + username + " ya existe");

            userObject.mInsert(fullname,username,password,typeuser);

            return Content("Registro insertado!!!");

        }

        public ActionResult Edit(String fullname, String username, String password, String typeuser, Boolean iseditpass, int id)
        {
            UserModel userObject = new UserModel();

            if (userObject.mExisteFullname(fullname, true, id))
                return Content("Error: El usuario " + fullname + " ya existe");

            if (userObject.mExisteUsername(username, true, id))
                return Content("Error: El username " + username + " ya existe");


            userObject.mEdit(fullname, username, password, typeuser,iseditpass,id);

            return Content("Registro Actualizado!!!");

        }

        public ActionResult Delete(int id)
        {
            UserModel userObject = new UserModel();

            userObject.mDelete(id);

            return Content("Registro Actualizado!!!");

        }

        public ActionResult getUserById(String id)
        {
            UserModel userObject = new UserModel();


            DataTable dtUser = userObject.mGetUserById(id);

            DataTable oResponseDynamic = new DataTable("Response");
            oResponseDynamic.Columns.Add("pkUser", typeof(Int32));
            oResponseDynamic.Columns.Add("fullname", typeof(String));
            oResponseDynamic.Columns.Add("username", typeof(String));
            oResponseDynamic.Columns.Add("typeUser", typeof(String));


            foreach (DataRow row in dtUser.Rows)
            {
                oResponseDynamic.Rows.Add(row["pk_user"].ToString(), row["fullname"].ToString(), row["username"].ToString(), row["fk_type_user"].ToString());
            }

            string json = JsonConvert.SerializeObject(oResponseDynamic, Formatting.Indented);
            return Content(json);


        }


    }
}