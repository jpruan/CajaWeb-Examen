using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace CajaWeb.Models
{
    public class AbonosModel
    {

        public void mInsertAbono(String monto, int idCobro, out String vMsg)
        {
            cCnx oCnx = new cCnx(mGetSettings("dbAD", "Server"), mGetSettings("dbAD", "Database"), mGetSettings("dbAD", "User"), mGetSettings("dbAD", "Password"));


            DataTable odtAbono = oCnx.mConsultar(@"select * from tbl_cobros where pk_cobro=" + idCobro, out vMsg);
            String montoRestante = "";

            if (odtAbono.Rows.Count > 0)
            {
                foreach (DataRow row in odtAbono.Rows)
                {
                    montoRestante = row["monto_restante"].ToString();
                
                }
            }

            if (float.Parse(monto) < float.Parse(montoRestante))
            {
                oCnx.mConsultar(@"insert into tbl_abonos (fk_cobro,monto_abono) values (" + idCobro + "," + monto + ")", out vMsg);
                oCnx.mConsultar(@"update tbl_cobros set monto_restante=monto_restante - " + monto + " where pk_cobro=" + idCobro, out vMsg);

            }
            else {

                oCnx.mConsultar(@"insert into tbl_abonos (fk_cobro,monto_abono) values (" + idCobro + "," + montoRestante + ")", out vMsg);
                oCnx.mConsultar(@"update tbl_cobros set monto_restante=monto_restante - " + montoRestante + ", status_pagado ='1' where pk_cobro=" + idCobro, out vMsg);
            }



           


        }
        public void mDeleteAbono(int idAbono,out String vMsg)
        {
            cCnx oCnx = new cCnx(mGetSettings("dbAD", "Server"), mGetSettings("dbAD", "Database"), mGetSettings("dbAD", "User"), mGetSettings("dbAD", "Password"));

            DataTable odtAbono = oCnx.mConsultar(@"select * from tbl_abonos where pk_abono=" + idAbono, out vMsg);
            String monto="";
            String idCobro="";

            if (odtAbono.Rows.Count > 0) {
                foreach (DataRow row in odtAbono.Rows)
                {
                   monto= row["monto_abono"].ToString();
                    idCobro = row["fk_cobro"].ToString();
                }
            }

            oCnx.mConsultar(@"delete from tbl_abonos where pk_abono= " +idAbono, out vMsg);
            oCnx.mConsultar(@"update tbl_cobros set monto_restante=monto_restante + " + monto + ",status_pagado='0' where pk_cobro=" + idCobro, out vMsg);


        }


        public DataTable getAbonosByIdUser( int idUser, out String vMsg)
        {
            cCnx oCnx = new cCnx(mGetSettings("dbAD", "Server"), mGetSettings("dbAD", "Database"), mGetSettings("dbAD", "User"), mGetSettings("dbAD", "Password"));

            DataTable odtAbonos=oCnx.mConsultar(@"select pk_abono,fecha_abono, monto_abono, concepto, fk_cobro from tbl_abonos A inner join tbl_cobros C on C.pk_cobro=A.fk_cobro where fk_user= " + idUser+" order by fecha_abono asc", out vMsg);


            return odtAbonos;
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