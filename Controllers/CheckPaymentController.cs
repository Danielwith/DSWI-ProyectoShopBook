using iText.Kernel.Pdf;
using ShopBook.Class_DTO;
using ShopBook.Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace ShopBook.Controllers
{
    public class CheckPaymentController : Controller
    {
        private shopbookEntities db = new shopbookEntities();

        #region DatosDeCliente
        [HttpGet]
        public ActionResult Checkout()
        {
            var usuario = (tb_usuario)Session["email"];
            
            return View(usuario);
        }

        [HttpPost]
        public ActionResult Checkout(string userEmail, string userName, string userApe, string tipoDoc, string userDocNum, string fecNac, string userPhone, string userAddress)
        {
            List<ShoppingCart> compras = (List<ShoppingCart>)Session["carrito"];

            ClientData client = new ClientData()
            {
                email = userEmail,
                name = userName.Trim(),
                apellido = userApe.Trim(),
                tipDoc = tipoDoc,
                docNum = userDocNum,
                fecNac = fecNac,
                phone = userPhone,
                direccion = userAddress
            };

            Session["dataCliente"] = client;

            string ruta = Server.MapPath("~/");
            string PathPDF = Path.Combine(ruta + "Assets\\Comprobante.pdf");

            var comprobante = new PdfGenerator();
            comprobante.generate(compras,PathPDF,client);
            
            return RedirectToAction("Payment");
        }
        #endregion DatosDeCliente

        #region MetodoDePago
        [HttpGet]
        public ActionResult Payment()
        {
            
            return View();
        }

        [HttpPost]
        public ActionResult Payment(string numTarjeta, int mes, int anio, int cvc, string titular, string codPostal)
        {
            var getTypeCredit = (from c in db.tb_creditcard
                                 where c.num_CC == numTarjeta
                                 select c.tip_CC).FirstOrDefault();

            var validate = (from c in db.tb_creditcard
                            where c.num_CC == numTarjeta && 
                                  c.mm_CC == mes && 
                                  c.aa_CC == anio && 
                                  c.cvc_CC == cvc && 
                                  c.nom_CC == titular
                            select c).FirstOrDefault();

            if (validate == null)
            {
                ViewBag.Notification = "Esta tarjeta no existe";
                return View();
            }
            else
            {
                string ruta = Server.MapPath("~/");
                string PathPDF = Path.Combine(ruta + "Assets\\Comprobante.pdf");

                var em = new EmailSenderDotNet();
                var user = (ClientData)Session["dataCliente"];

                var postResult = em.sendEmail(user, PathPDF);

                if (postResult == true)
                {
                    /*FinalizarCompra();*/
                    Session["dataCliente"] = null;
                    return RedirectToAction("Finish");
                }
                else
                {
                    ViewBag.Notification = "A ocurrido un error.";
                    return View();
                }
            }
        }
        #endregion MetodoDePago

        #region FinalizarCompra
        // Vista
        public ActionResult Finish()
        {
            return View();
        }

        // Metodo para registrar la compra en la database
        public void FinalizarCompra()
        {
            var usuario = (tb_usuario)Session["email"];
            List<ShoppingCart> compras = (List<ShoppingCart>)Session["carrito"];
            if (compras != null && compras.Count > 0)
            {
                tb_boletas nuevaVenta = new tb_boletas();
                nuevaVenta.nroBoleta = db.usp_maxBoleta().FirstOrDefault();
                nuevaVenta.fechGene = DateTime.Now;
                nuevaVenta.idUser = usuario.idUser;
                nuevaVenta.total = decimal.Parse(Session["total"].ToString());
                db.tb_boletas.Add(nuevaVenta);
                db.SaveChanges();

                foreach (var item in compras)
                {
                    tb_detalle_boletas nuevoDetalleBoleta = new tb_detalle_boletas();
                    nuevoDetalleBoleta.nroBoleta = nuevaVenta.nroBoleta;
                    nuevoDetalleBoleta.idLibro = item.Libros.idLibro;
                    nuevoDetalleBoleta.cantidad = item.Cantidad;
                    nuevoDetalleBoleta.precio = item.Libros.precUni;
                    nuevoDetalleBoleta.importe = item.Libros.precUni * item.Cantidad;
                    db.tb_detalle_boletas.Add(nuevoDetalleBoleta);
                    db.SaveChanges();
                }

                Session["carrito"] = new List<ShoppingCart>();
            }
        }

        #endregion FinalizarCompra
    }
}