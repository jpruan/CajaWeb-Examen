using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace CajaWeb.Models
{
    public class CobrosModel
    {

        public void mInsertCobro(int id , String monto, String concepto, out String vMsg)
        {
            cCnx oCnx = new cCnx(mGetSettings("dbAD", "Server"), mGetSettings("dbAD", "Database"), mGetSettings("dbAD", "User"), mGetSettings("dbAD", "Password"));
      
            oCnx.mConsultar(@"insert into tbl_cobros (fk_user,monto_total,monto_restante,concepto) values ("+id+","+monto+","+monto+",'" +concepto + "')", out vMsg);
   
        }

        public DataTable mGetCobrosActivos(int id, out String vMsg)
        {
            cCnx oCnx = new cCnx(mGetSettings("dbAD", "Server"), mGetSettings("dbAD", "Database"), mGetSettings("dbAD", "User"), mGetSettings("dbAD", "Password"));

            DataTable dtCobros=oCnx.mConsultar(@"select * from tbl_cobros where status_pagado=0 and fk_user= "+ id, out vMsg);


            return dtCobros;

        }

        public DataTable obtenerCobrosPorUsuario(int id, out String vMsg)
        {
            cCnx oCnx = new cCnx(mGetSettings("dbAD", "Server"), mGetSettings("dbAD", "Database"), mGetSettings("dbAD", "User"), mGetSettings("dbAD", "Password"));

            DataTable dtCobros = oCnx.mConsultar(@"select * from tbl_cobros where fk_user= " + id, out vMsg);


            return dtCobros;

        }


        public void mDeleteCobro(int id, out String vMsg)
        {
            cCnx oCnx = new cCnx(mGetSettings("dbAD", "Server"), mGetSettings("dbAD", "Database"), mGetSettings("dbAD", "User"), mGetSettings("dbAD", "Password"));

            oCnx.mConsultar(@"delete from tbl_abonos where fk_cobro="+id, out vMsg);
            oCnx.mConsultar(@"delete from tbl_cobros where pk_cobro="+id, out vMsg);

        }


        public void mDeleteAllCobrosByIdUser(int id, out String vMsg)
        {
            cCnx oCnx = new cCnx(mGetSettings("dbAD", "Server"), mGetSettings("dbAD", "Database"), mGetSettings("dbAD", "User"), mGetSettings("dbAD", "Password"));

           DataTable odtCobros= oCnx.mConsultar(@"select * from tbl_cobros where fk_user=" + id, out vMsg);
            if (vMsg != "")
                throw new Exception(vMsg);

            foreach (DataRow row in odtCobros.Rows) {
                
                mDeleteCobro(Int32.Parse(row["pk_cobro"].ToString()),out vMsg);
            }

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