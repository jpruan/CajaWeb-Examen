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
    public class CobrosController : Controller
    {
        public ActionResult addCobro(int idUser, String monto, String concepto)
        {
            CobrosModel cobro = new CobrosModel();

            String vMsgError = "" ;
            cobro.mInsertCobro(idUser,monto, concepto, out vMsgError);

            if(vMsgError!="")
                return Content("ERROR:"+ vMsgError);

            return Content("Cobro Insertado");


        }

        public ActionResult deleteCobro(int idCobro)
        {
            CobrosModel cobro = new CobrosModel();

            String vMsgError = "";
            cobro.mDeleteCobro(idCobro, out vMsgError);

            if (vMsgError != "")
                return Content("ERROR:" + vMsgError);

            return Content("Cobro Eliminado");


        }



        public ActionResult obtenerCobrosActivos(int idUser)
        {
            CobrosModel cobros = new CobrosModel();

            String vMsgError = "";
            DataTable odtCobros= cobros.mGetCobrosActivos(idUser, out vMsgError);

            if (vMsgError != "")
                return Content("ERROR:" + vMsgError);

            DataTable oResponseDynamic = new DataTable("Response");
            oResponseDynamic.Columns.Add("pkCobro", typeof(Int32));
            oResponseDynamic.Columns.Add("concepto", typeof(String));
            oResponseDynamic.Columns.Add("montoRestante", typeof(String));


            foreach (DataRow row in odtCobros.Rows)
            {
                oResponseDynamic.Rows.Add(row["pk_cobro"].ToString(), row["concepto"].ToString(), row["monto_restante"].ToString());
            }

            string json = JsonConvert.SerializeObject(oResponseDynamic, Formatting.Indented);
            return Content(json);



        }

        public ActionResult obtenerCobrosPorUsuario(int idUser)
        {
            CobrosModel cobros = new CobrosModel();

            String vMsgError = "";
            DataTable odtCobros = cobros.obtenerCobrosPorUsuario(idUser, out vMsgError);

            if (vMsgError != "")
                return Content("ERROR:" + vMsgError);

            DataTable oResponseDynamic = new DataTable("Response");
            oResponseDynamic.Columns.Add("pkCobro", typeof(Int32));
            oResponseDynamic.Columns.Add("fechaCobro", typeof(String));
            oResponseDynamic.Columns.Add("concepto", typeof(String));
            oResponseDynamic.Columns.Add("montoTotal", typeof(String));
            oResponseDynamic.Columns.Add("montoRestante", typeof(String));
            oResponseDynamic.Columns.Add("statusPagado", typeof(Boolean));

            foreach (DataRow row in odtCobros.Rows)
            {
                oResponseDynamic.Rows.Add(row["pk_cobro"].ToString(), row["fecha_cobro"].ToString(),row["concepto"].ToString(), row["monto_total"].ToString()
                    ,row["monto_restante"].ToString(), Boolean.Parse(row["status_pagado"].ToString())
                    );
            }

            string json = JsonConvert.SerializeObject(oResponseDynamic, Formatting.Indented);
            return Content(json);



        }
    }
}