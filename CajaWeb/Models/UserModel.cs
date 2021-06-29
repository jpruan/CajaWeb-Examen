using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace CajaWeb.Models
{
    public class UserModel
    {
     
            public int pkUser { get; set; }
            public string fullName { get; set; }
            public string username { get; set; }
            public int fkTypeUser { get; set; }
     
        public String[] menus { get; set; }








        public Boolean mValidateLogin(String user, String pass, out UserModel userData)
        {

            UserModel data = new UserModel();
            
            cCnx oCnx = new cCnx(mGetSettings("dbAD", "Server"), mGetSettings("dbAD", "Database"), mGetSettings("dbAD", "User"), mGetSettings("dbAD", "Password"));
            String vMsg;
            DataTable dtUser = oCnx.mConsultar(@"select * from tbl_users where username ='"+user+"' and PWDCOMPARE('"+pass+"',pass)= 1", out vMsg);

            if (vMsg != "")
                throw new Exception(vMsg);
            if (dtUser.Rows.Count > 0)
            {
                foreach (DataRow row in dtUser.Rows)
                {
                    data.pkUser = Int32.Parse(row["pk_user"].ToString());
                    data.fullName = row["fullname"].ToString();
                    data.username = row["username"].ToString();
                    data.fkTypeUser = Int32.Parse(row["fk_type_user"].ToString());

                    switch (Int32.Parse(row["fk_type_user"].ToString()))
                    {
                        case 1:
                            data.menus = new String[] { "caja", "estado", "usuarios" };
                            break;
                        case 2:
                            data.menus = new String[] { "caja", "estado" };
                            break;
                        case 3:
                            data.menus = new String[] { "estado" };
                            break;
                        default:
                            data.menus = new String[] { };
                            break;


                    }

                    userData = data;

                    return true;
                }
            }


            userData = data;
            return false;
        }

        public DataTable mGetAllUsers() {
            cCnx oCnx = new cCnx(mGetSettings("dbAD", "Server"), mGetSettings("dbAD", "Database"), mGetSettings("dbAD", "User"), mGetSettings("dbAD", "Password"));
            String vMsg;
            DataTable dtAllUsers = oCnx.mConsultar(@"select pk_user,fullname,username,type_user from tbl_users U inner join tbl_type_user T on T.pk_type=U.fk_type_user", out vMsg);

            if (vMsg != "")
                throw new Exception(vMsg);
            return dtAllUsers;
        }

        public DataTable mGetSearchUsers(String username, String fullname, int id)
        {
            cCnx oCnx = new cCnx(mGetSettings("dbAD", "Server"), mGetSettings("dbAD", "Database"), mGetSettings("dbAD", "User"), mGetSettings("dbAD", "Password"));
            String vMsg;
            String query = "select pk_user, fullname, username from tbl_users where ";
            if (username != "")
                query += "username like '%" + username + "%' and ";

            if (fullname != "")
                query += "fullname like '%" + fullname + "%' and ";

            if (id>0)
                query += "pk_user = " + id + " and ";

            query += "fk_type_user=3"; 

            DataTable dtUsers = oCnx.mConsultar(query, out vMsg);

            if (vMsg != "")
                throw new Exception(vMsg);
            return dtUsers;
        }


        public Boolean mExisteUsername(String username, Boolean isEdit, int id)
        {
            cCnx oCnx = new cCnx(mGetSettings("dbAD", "Server"), mGetSettings("dbAD", "Database"), mGetSettings("dbAD", "User"), mGetSettings("dbAD", "Password"));
            String vMsg;
            DataTable dtUser = oCnx.mConsultar(@"select * from tbl_users where username='" + username + "'", out vMsg);

            if (vMsg != "")
                throw new Exception(vMsg);

            if (dtUser.Rows.Count > 0)
            {
                if (isEdit)
                {
                    foreach (DataRow row in dtUser.Rows)
                    {
                        if (Int32.Parse(row["pk_user"].ToString()) != id)
                            return true;

                    }

                }
                else
                {
                    return true;
                }

            }
            return false;

        }
        public DataTable mGetUserById(String id)
        {
            cCnx oCnx = new cCnx(mGetSettings("dbAD", "Server"), mGetSettings("dbAD", "Database"), mGetSettings("dbAD", "User"), mGetSettings("dbAD", "Password"));
            String vMsg;
            DataTable dtUser = oCnx.mConsultar(@"select pk_user,fullname,username,fk_type_user from tbl_users where pk_user="+id, out vMsg);

            if (vMsg != "")
                throw new Exception(vMsg);
            return dtUser;
        }


        
        public Boolean mExisteFullname(String fullname, Boolean isEdit, int id)
        {
            cCnx oCnx = new cCnx(mGetSettings("dbAD", "Server"), mGetSettings("dbAD", "Database"), mGetSettings("dbAD", "User"), mGetSettings("dbAD", "Password"));
            String vMsg;
            DataTable dtUser = oCnx.mConsultar(@"select * from tbl_users where fullname='" + fullname + "'", out vMsg);


            if (vMsg != "")
                throw new Exception(vMsg);

            if (dtUser.Rows.Count > 0) {
                if (isEdit)
                {
                    foreach (DataRow row in dtUser.Rows)
                    {
                        if (Int32.Parse(row["pk_user"].ToString()) != id)
                            return true;

                    }
                    
                }
                else {
                    return true;
                }


            }
           

            return false;
        }


     

        public void mInsert(String fullname, String username, String password, String typeUser)
        {
            cCnx oCnx = new cCnx(mGetSettings("dbAD", "Server"), mGetSettings("dbAD", "Database"), mGetSettings("dbAD", "User"), mGetSettings("dbAD", "Password"));
            String vMsg;
            DataTable dtUser = oCnx.mConsultar(@"insert into tbl_users (fullname,username,pass,fk_type_user) values ('"+fullname+"','"+username+"',PWDENCRYPT('"+password+"'),"+typeUser+")", out vMsg);


            if (vMsg != "")
                throw new Exception(vMsg);

        
        }

        public void mEdit(String fullname, String username, String password, String typeUser, Boolean isEditPass,int id)
        {
            cCnx oCnx = new cCnx(mGetSettings("dbAD", "Server"), mGetSettings("dbAD", "Database"), mGetSettings("dbAD", "User"), mGetSettings("dbAD", "Password"));
            String vMsg;

            String query = (isEditPass) ? 
                "update tbl_users set fullname='"+fullname+"' , username='"+username+ "',pass= PWDENCRYPT('"+password+"'),fk_type_user=" + typeUser+" where pk_user="+id 
                : 
                "update tbl_users set fullname='" + fullname + "' , username='" + username + "',fk_type_user=" + typeUser +" where pk_user=" + id;




            DataTable dtUser = oCnx.mConsultar(query, out vMsg);


            if (vMsg != "")
                throw new Exception(vMsg);


        }

        public void mDelete(int id)
        {
            cCnx oCnx = new cCnx(mGetSettings("dbAD", "Server"), mGetSettings("dbAD", "Database"), mGetSettings("dbAD", "User"), mGetSettings("dbAD", "Password"));
            String vMsg;

            CobrosModel cobros = new CobrosModel();

            cobros.mDeleteAllCobrosByIdUser(id, out vMsg);
            if (vMsg != "")
                throw new Exception(vMsg);



            oCnx.mConsultar("delete from tbl_users where pk_user="+ id, out vMsg);

            if (vMsg != "")
                throw new Exception(vMsg);


        }




        private string mGetSettings(string pSection, string pField)
        {
            string vReturn;
            string oConfig = ConfigurationManager.AppSettings.Get(pSection + "." + pField).ToString();
            vReturn = oConfig;
            try
            {
                if (vReturn == null)
                    throw new Exception("No existe el campo " + pField + ". Verifica el Web.config");
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return vReturn;
        }

    }
}