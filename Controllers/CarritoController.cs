using ShopBook.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopBook.Controllers
{
    public class CarritoController : Controller
    {
        private shopbookEntities db = new shopbookEntities();

        // Conseguir Libro
        private int getLibro(int id)
        {
            List<ShoppingCart> compras = (List<ShoppingCart>)Session["carrito"];
            for (int i = 0; i < compras.Count; i++)
            {
                if (compras[i].Libros.idLibro == id)
                {
                    return i;
                }
            }
            return -1;
        }

        public ActionResult listarCarrito()
        {
            var ar = (List<ShoppingCart>)System.Web.HttpContext.Current.Session["carrito"];
            if (ar != null)
            {
                decimal? sum = 0;
                var json = ar.Select(c =>
                   new {
                       idLibro = c.Libros.idLibro,
                       tituLibro = c.Libros.tituLibro,
                       nomAutor = c.Libros.nomAutor,
                       precUni = c.Libros.precUni,
                       cantidad = c.Cantidad,
                       subtotal = c.Libros.precUni * c.Cantidad,
                       img = "/Assets/images/pic01.jpg"
                   }).ToList();
                foreach (var i in ar)
                {
                    sum += i.Libros.precUni;
                }
                Session["total"] = sum;
                return Json(new { data = json }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { data = new { } }, JsonRequestBehavior.AllowGet);
            }
        }

        public void AgregarCarrito(int idLib, int cantidad)
        {
            var usuario = (tb_usuario)Session["email"];
            if (Session["carrito"] == null)
            {
                List<ShoppingCart> compras = new List<ShoppingCart>();
                compras.Add(new ShoppingCart(db.tb_libros.Find(idLib),cantidad,usuario));
                Session["carrito"] = compras;
            }
            else
            {
                List<ShoppingCart> compras = (List<ShoppingCart>)Session["carrito"];
                int LibroExistente = getLibro(idLib);
                if (LibroExistente == -1)
                    compras.Add(new ShoppingCart(db.tb_libros.Find(idLib), cantidad, usuario));
                else
                    compras[LibroExistente].Cantidad += cantidad;
                    Session["carrito"] = compras;
            }
        }

        public ActionResult Compras(string notification)
        {
            ViewBag.notification = notification;
            if (Session["total"] != null)
            {
                ViewBag.total = Session["total"].ToString();
            }
            return View();
        }

        //public ActionResult prueba()
        //{
        //    var oEmail = (tb_usuario)Session["email"];
        //        return Json(new { data = oEmail.idUser }, JsonRequestBehavior.AllowGet);
            
        //}

        public ActionResult Delete(int id)
        {
            List<ShoppingCart> compras = (List<ShoppingCart>)Session["carrito"];
            compras.RemoveAt(getLibro(id));
            return View("Compras");
        }
        //[Function(Name = "dbo.ufn_generarNroBoleta")]
        //[return: Parameter(DbType ="NChar(6)")]
        //public static string generarNroBoleta()
        //{
        //    IExecuteResult result = this.ExecuteMethodCell(this, )
        //    return ((string)(result.ReturnValue));
        //}

        
    }
}