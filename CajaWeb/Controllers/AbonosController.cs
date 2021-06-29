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
    public class AbonosController : Controller
    {
        public ActionResult addAbono(String monto, int idCobro)
        {
            AbonosModel abono = new AbonosModel();

            String vMsgError = "";
            abono.mInsertAbono( monto, idCobro, out vMsgError);

            if (vMsgError != "")
                return Content("ERROR:" + vMsgError);

            return Content("Abono Insertado");


        }


        public ActionResult deleteAbono(int idAbono)
        {
            AbonosModel abono = new AbonosModel();

            String vMsgError = "";
            abono.mDeleteAbono(idAbono, out vMsgError);

            if (vMsgError != "")
                return Content("ERROR:" + vMsgError);

            return Content("Abono Eliminado");


        }
        

        public ActionResult obtenerAbonosByIdUser( int idUser)
        {
            AbonosModel abono = new AbonosModel();

            String vMsgError = "";
            DataTable odtAbonos=abono.getAbonosByIdUser( idUser, out vMsgError);

            if (vMsgError != "")
                return Content("ERROR:" + vMsgError);


            DataTable oResponseDynamic = new DataTable("Response");
            oResponseDynamic.Columns.Add("pkAbono", typeof(Int32));
            oResponseDynamic.Columns.Add("concepto", typeof(String));
            oResponseDynamic.Columns.Add("fechaAbono", typeof(String));
            oResponseDynamic.Columns.Add("montoAbono", typeof(String));
            oResponseDynamic.Columns.Add("fkCobro", typeof(String));
            foreach (DataRow row in odtAbonos.Rows)
            {
                oResponseDynamic.Rows.Add(row["pk_abono"].ToString(), row["concepto"].ToString(), row["fecha_abono"].ToString(), row["monto_abono"].ToString(), row["fk_cobro"].ToString());
            }

            string json = JsonConvert.SerializeObject(oResponseDynamic, Formatting.Indented);
            return Content(json);



        }




    }
}