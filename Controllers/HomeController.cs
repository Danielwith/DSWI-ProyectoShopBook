using ShopBook.Entity;
using ShopBook.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopBook.Controllers
{
    public class HomeController : Controller
    {
        private LoginEntities db = new LoginEntities();
        
        // PAGINA PRINCIPAL
        public ActionResult Index()
        {
            return View();
        }

        // DEPENDE DEL ROL
        [AutorizarUsuario(idOperacion: 1)]
        public ActionResult Admin()
        {
            return View();
        }

        // ROL: OPERADOR
        [AutorizarUsuario(idOperacion: 2)]
        public ActionResult Proveedor()
        {
            return View();
        }

        [AutorizarUsuario(idOperacion: 2)]
        public ActionResult Mantenimiento(string notification)
        {
            ViewBag.notification=notification;
            return View();
        }

        //Funciones
        [AutorizarUsuario(idOperacion: 2)]
        public ActionResult listarProveedor()
        {
            var proveedores = (from m in db.tb_usuario
                               join p in db.proveedor_data_temp on m.idUser equals p.idUser
                               select new { p.idData,m.idUser, m.nombre, p.numVentas }).ToList();
            return Json(new { data = proveedores}, JsonRequestBehavior.AllowGet);
        }

        [AutorizarUsuario(idOperacion: 2)]
        public ActionResult GuardarProveedor(int idUser, int numVentas)
        {
            var data = new proveedor_data_temp() { idUser=idUser,numVentas=numVentas};
            db.proveedor_data_temp.Add(data);
            db.SaveChanges();
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        
        [AutorizarUsuario(idOperacion: 2)]
        public ActionResult EditarProveedor(int idData, int numVentas)
        {
            var data = db.proveedor_data_temp.Where(u => u.idData == idData).FirstOrDefault();
            data.numVentas = numVentas;
            db.SaveChanges();
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [AutorizarUsuario(idOperacion: 2)]
        public ActionResult EliminarProveedor(int idData)
        {
            var data = db.proveedor_data_temp.Find(idData);
            db.proveedor_data_temp.Remove(data);
            db.SaveChanges();
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [AutorizarUsuario(idOperacion: 1)]
        public ActionResult MantenimientoLibro(string notification)
        {
            ViewBag.notification = notification;
            return View();
        }

        //Funciones del mantenimiento Libros
        [AutorizarUsuario(idOperacion: 1)]
        public ActionResult listarLibros()
        {
            var libros = (from l in db.tb_libros
                          join e in db.tb_editoriales on l.idEdito equals e.idEdito
                          where l.estado == 1
                          select new { l.idLibro, l.tituLibro, l.nomAutor, l.precUni, l.fechPub, e.nomEdito }).ToList();
            return Json(new { data = libros }, JsonRequestBehavior.AllowGet);
        }
        /*
        [AutorizarUsuario(idOperacion: 1)]
        public ActionResult GuardarLibros(string tituLibro, string sinopsis, string nomAutor, decimal precUni, int idEdito, DateTime fechPub)
        {
            var data = new tb_libros() { tituLibro = tituLibro, sinopsis = sinopsis, nomAutor = nomAutor, precUni = precUni, idEdito = idEdito, fechPub = fechPub, estado = 1 };
            db.tb_libros.Add(data);
            db.SaveChanges();
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [AutorizarUsuario(idOperacion: 1)]
        public ActionResult EditarLibro(int idLibro, string tituLibro, string sinopsis, int anio, string nomAutor, decimal precUni, int idEdito, DateTime fechPub, int estado)
        {
            var data = db.tb_libros.Where(u => u.idLibro == idLibro).FirstOrDefault();
            data.tituLibro = tituLibro;
            data.sinopsis = sinopsis;
            data.anio = anio;
            data.nomAutor = nomAutor;
            data.precUni = precUni;
            data.idEdito = idEdito;
            data.fechPub = fechPub;
            data.estado = estado;
            db.SaveChanges();
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [AutorizarUsuario(idOperacion: 1)]
        public ActionResult EliminarLibro(int idLibro)
        {
            var data = db.tb_libros.Find(idLibro);
            data.estado = 0;
            db.SaveChanges();
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        */


        // VISTA COMUN USER
        [AutorizarUsuario(idOperacion: 4)]
        public ActionResult Inicio()
        {
            return View();
        }

        // OPERACIONES
        [AutorizarUsuario(idOperacion: 1)]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        // Test Alert Form ! ! !
        // No funciona, falta implementar alert.js
        [HttpPost]
        [AutorizarUsuario(idOperacion:1)]
        public ActionResult About(string name, string descrip)
        {
            TempData["Notification"] = $"Notification('Esta es una prueba')";
            return View();
        }

        [AutorizarUsuario(idOperacion: 1)]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            
            return View();
        }
    }

}