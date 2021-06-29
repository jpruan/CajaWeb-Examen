using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;





namespace CajaWeb.Models
{
    public class cCnx
    {

        #region Atributos
        public struct estParametres
        {
          
            public string vName;
        
            public string vValue;
        }
        public List<SqlParameter> lstParameters = new List<SqlParameter>();
        private string vServer;
        private string vDataBase;
        private string vUser;
        private string vPswd;
        /*private string vQuery;
        private string vStoreProcedure;*/
        private SqlConnection oCnx = new SqlConnection();
      
        public estParametres[] arrParametres = new estParametres[20];
        #endregion

       
        public cCnx(string Server, string Database, string User, string Password)
        {
            vServer = Server;
            vDataBase = Database;
            vUser = User;
            vPswd = Password;
        }
      

        
        private SqlConnection mGetConnectionString()
        {
            string vCnxStr = "Data source = " + vServer + "; initial catalog = " + vDataBase +
                "; user id=" + vUser + "; Password=" + vPswd + ";";
            SqlConnection vStrCon = new SqlConnection(vCnxStr);
            return vStrCon;
        }
       
        public DataTable mConsultar(string StoreProcedureName, bool isProcedure, out string pError)
        {
            string vCnxStr = mGetConnectionString().ConnectionString;
            DataTable oDt = new DataTable();
            SqlConnection oCnx = new SqlConnection(vCnxStr);
            SqlCommand oSqlCmd = new SqlCommand();
            if (isProcedure)
                oSqlCmd.CommandType = CommandType.StoredProcedure;
            oSqlCmd.CommandText = StoreProcedureName;
            oSqlCmd.CommandTimeout = 500;
            oSqlCmd.Connection = oCnx;
            foreach (SqlParameter oParameter in lstParameters)
                oSqlCmd.Parameters.Add(oParameter);
            SqlDataAdapter oDa = new SqlDataAdapter(oSqlCmd);
            try
            {
                oCnx.Open();
                oDa.Fill(oDt);
                pError = "";
                return oDt;
            }
            catch (SqlException e)
            {
                if (e.Number != 50000)
                {
                    pError = "[mConsultar]" +
                        "\r\nQuery Error->" + e.Message +
                        "\r\n-->Server  :" + vServer +
                        "\r\n-->Base    :" + vDataBase +
                        "\r\n-->User    :" + vUser +
                        "\r\n-->SP      :" + StoreProcedureName;
                }
                else
                    pError = e.Message;
                return null;
            }
            finally
            {
                oCnx.Close();
            }
        }
      
       
     
        public DataTable mConsultar(string Query, out string pError)
        {
            string vCnxStr = mGetConnectionString().ConnectionString;
            DataTable oDt = new DataTable();
            SqlConnection oCnx = new SqlConnection(vCnxStr);
            SqlDataAdapter oDa = new SqlDataAdapter(Query, oCnx);
            try
            {
                oCnx.Open();
                oDa.Fill(oDt);
                pError = "";
                return oDt;
            }
            catch (Exception e)
            {
                pError = "[cCnx.mConsultar]" +
                    "<br />Query Error->" + e.Message +
                    "<br />-->Server  :" + vServer +
                    "<br />-->Base    :" + vDataBase +
                    "<br />-->User    :" + vUser +
                    "<br />-->Query   :" + Query;
                return null;
            }
            finally
            {
                oCnx.Close();
            }
        }


      
        public DataTable mConsultar(string StoreProcedureName, estParametres[] Parametres, out string pError)
        {
            string vCnxStr = mGetConnectionString().ConnectionString;
            DataTable oDt = new DataTable();
            SqlConnection oCnx = new SqlConnection(vCnxStr);
            SqlCommand oSqlCmd = new SqlCommand();
            oSqlCmd.CommandType = CommandType.StoredProcedure;
            oSqlCmd.CommandText = StoreProcedureName;
            oSqlCmd.CommandTimeout = 500;
            oSqlCmd.Connection = oCnx;
            estParametres[] arrParametres = Parametres;
            for (int i = 0; i < arrParametres.Length; i++)
            {
                if (arrParametres[i].vValue != null)
                {
                    oSqlCmd.Parameters.AddWithValue(arrParametres[i].vName, arrParametres[i].vValue);
                }

            }
            SqlDataAdapter oDa = new SqlDataAdapter(oSqlCmd);
            try
            {
                oCnx.Open();
                oDa.Fill(oDt);
                pError = "";
                return oDt;
            }
            catch (SqlException e)
            {
                pError = "[mConsultar]" +
                    "\r\nQuery Error->" + e.Message +
                    "\r\n-->Server  :" + vServer +
                    "\r\n-->Base    :" + vDataBase +
                    "\r\n-->User    :" + vUser +
                    "\r\n-->SP      :" + StoreProcedureName;
                return null;
            }
            finally
            {
                oCnx.Close();
            }
        }
        public bool mConsultar(string StoreProcedureName, estParametres[] Parametres, out int pID, out string pError)
        {
            string vCnxStr = mGetConnectionString().ConnectionString;
            SqlConnection oCnx = new SqlConnection(vCnxStr);
            SqlCommand oSqlCmd = new SqlCommand();
            oSqlCmd.CommandType = CommandType.StoredProcedure;
            oSqlCmd.CommandText = StoreProcedureName;
            oSqlCmd.Connection = oCnx;
            estParametres[] arrParametres = Parametres;
            for (int i = 0; i < arrParametres.Length; i++)
            {
                if (arrParametres[i].vValue != null)
                {
                    oSqlCmd.Parameters.AddWithValue(arrParametres[i].vName, arrParametres[i].vValue);
                }
            }
            oSqlCmd.Parameters.Add("@thisId", System.Data.SqlDbType.Int).Direction = ParameterDirection.Output;
            SqlDataAdapter oDa = new SqlDataAdapter(oSqlCmd);
            try
            {
                oCnx.Open();
                oSqlCmd.ExecuteNonQuery();
                pID = (int)oSqlCmd.Parameters["@thisId"].Value;
                pError = "";
                return true;
            }
            catch (SqlException e)
            {
                pError = "[mConsultar]" +
                    "\r\nQuery Error->" + e.Message +
                    "\r\n-->Server  :" + vServer +
                    "\r\n-->Base    :" + vDataBase +
                    "\r\n-->User    :" + vUser +
                    "\r\n-->SP      :" + StoreProcedureName;
                pID = 0;
                return false;
            }
            finally
            {
                oCnx.Close();
            }
        }
     
        public int mActualizar(string Query, out string pError)
        {
            string vCnxStr = mGetConnectionString().ConnectionString;
            SqlConnection oCnx = new SqlConnection(vCnxStr);
            SqlCommand oSqlCmd = new SqlCommand(Query, oCnx);
            try
            {
                oCnx.Open();
                int vUpdated = oSqlCmd.ExecuteNonQuery();
                pError = "";
                return vUpdated;
            }
            catch (Exception e)
            {
                pError = "[cCnx.mActualizar]" +
                    "\r\nQuery Error->" + e.Message +
                    "\r\n-->Server  :" + vServer +
                    "\r\n-->Base    :" + vDataBase +
                    "\r\n-->User    :" + vUser +
                    "\r\n-->Query   :" + Query;
                return -1;
            }
            finally
            {
                oCnx.Close();
            }
        }
      
        public int mConsultaEscalar(string Query, out string pError)
        {
            string vCnxStr = mGetConnectionString().ConnectionString;
            SqlConnection oCnx = new SqlConnection(vCnxStr);
            SqlCommand oSqlCmd = new SqlCommand(Query, oCnx);
            try
            {
                oCnx.Open();
                int vScalar = (int)oSqlCmd.ExecuteScalar();
                pError = "";
                return vScalar;
            }
            catch (Exception e)
            {
                pError = "[cCnx.mConsultaEscalar]" +
                    "<br />Query Error->" + e.Message +
                    "<br />-->Server  :" + vServer +
                    "<br />-->Base    :" + vDataBase +
                    "<br />-->User    :" + vUser +
                    "<br />-->Query   :" + Query;
                return 0;
            }
            finally
            {
                oCnx.Close();
            }

        }
       

    }
}