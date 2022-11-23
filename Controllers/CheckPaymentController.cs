using ShopBook.Class_DTO;
using ShopBook.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace ShopBook.Controllers
{
    public class CheckPaymentController : Controller
    {
        private shopbookEntities db = new shopbookEntities();

        // GET: CheckPayment
        [HttpGet]
        public ActionResult Checkout()
        {
            var usuario = (tb_usuario)Session["email"];
            
            return View(usuario);
        }

        [HttpPost]
        public ActionResult Checkout(string userEmail, string userName, string userApe)
        {
            Session["comprobante"] = new EmailSend()
            {
                email = userEmail,
                name = userName.Trim() + " " + userApe.Trim(),
                message = "Se a realizado con exito su compra.",
                html = "<h1>Gracias por su compra!</h1>"
            };
            
            return RedirectToAction("Payment");
        }

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
                using (var client = new HttpClient())
                {
                    var email = (EmailSend)Session["comprobante"];
                    client.BaseAddress = new Uri("https://shopbook.ml/api/contact");

                    var postTask = client.PostAsJsonAsync<EmailSend>("contact", email);
                    postTask.Wait();

                    var postResult = postTask.Result;
                    if (postResult.IsSuccessStatusCode)
                    {
                        List<ShoppingCart> compras = (List<ShoppingCart>)Session["carrito"];
                        var comprobante = new PdfGenerator();
                        comprobante.generate(compras);
                        /*FinalizarCompra();*/
                        return RedirectToAction("Finish");
                    }
                    else
                    {
                        ViewBag.Notification = "A ocurrido un error.";
                        return View();
                    }
                }
                
            }
        }

        public ActionResult Finish()
        {
            return View();
        }

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
    }
}