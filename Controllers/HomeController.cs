using ShopBook.Entity;
using ShopBook.Filters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopBook.Controllers
{
    public class HomeController : Controller
    {
        private shopbookEntities db = new shopbookEntities();
        
        // PAGINA PRINCIPAL
        public ActionResult Index()
        {
            Session["subcate"] = null;
            var categorias = db.tb_categorias.ToList().OrderBy(x => x.nombreCate);
            return View(categorias);
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
        public ActionResult listarProveedor(int edit)
        {
            var proveedores= (from m in db.tb_usuario
                              join p in db.proveedor_data_temp on m.idUser equals p.idUser
                              where p.activo != 0
                              select new { p.idData, m.idUser, m.nombre, p.numVentas }).ToList(); 
            if (edit == 1)
            {
                proveedores = (from m in db.tb_usuario
                                   join p in db.proveedor_data_temp on m.idUser equals p.idUser
                                   select new { p.idData, m.idUser, m.nombre, p.numVentas }).ToList();
            }
            
            return Json(new { data = proveedores}, JsonRequestBehavior.AllowGet);
        }

        [AutorizarUsuario(idOperacion: 2)]
        public ActionResult GuardarProveedor(int idUser, int numVentas)
        {
            var data = new proveedor_data_temp() { idUser=idUser,numVentas=numVentas, activo=1};
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
            data.activo = 0;
            db.SaveChanges();
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        //Mantenimiento Libro

        [AutorizarUsuario(idOperacion: 1)]
        public ActionResult MantenimientoLibro(string notification)
        {
            ViewBag.editorial = new SelectList(db.tb_editoriales, "idEdito","nomEdito");
            ViewBag.notification = notification;
            return View();
        }

        //Funciones del mantenimiento Libros
        [AutorizarUsuario(idOperacion: 1)]
        public ActionResult listarLibros(int edit)
        {
            var libros = (from l in db.tb_libros
                          join e in db.tb_editoriales on l.idEdito equals e.idEdito
                          where l.estado == 1
                          select new { l.idLibro, l.tituLibro, l.nomAutor, l.precUni, l.fechPub, l.sinopsis, e.nomEdito ,e.idEdito}).ToList();
            var parseo = (from p in libros
                          select new
                          {
                              idLibro = p.idLibro,
                              tituLibro = p.tituLibro,
                              nomAutor = p.nomAutor,
                              precUni = p.precUni,
                              fechPub = p.fechPub.Value.ToString("yyyy-MM-dd"),
                              sinopsis=p.sinopsis,
                              nomEdito=p.nomEdito,
                              idEdito= p.idEdito
                          }).ToList();
            if (edit == 1)
            {
                libros = (from l in db.tb_libros
                              join e in db.tb_editoriales on l.idEdito equals e.idEdito
                              select new { l.idLibro, l.tituLibro, l.nomAutor, l.precUni, l.fechPub, l.sinopsis, e.nomEdito ,e.idEdito}).ToList();
                parseo = (from p in libros
                              select new
                              {
                                  idLibro = p.idLibro,
                                  tituLibro = p.tituLibro,
                                  nomAutor = p.nomAutor,
                                  precUni = p.precUni,
                                  fechPub = p.fechPub.Value.ToString("yyyy-MM-dd"),
                                  sinopsis = p.sinopsis,
                                  nomEdito = p.nomEdito,
                                  idEdito = p.idEdito
                              }).ToList();
            }
            return Json(new { data = parseo }, JsonRequestBehavior.AllowGet);
        }


        [AutorizarUsuario(idOperacion: 1)]
        public ActionResult GuardarLibros(string tituLibro,string nomAutor, decimal precUni, DateTime fechPub, string sinopsis,  int idEdito)
        { 
            var data = new tb_libros() { tituLibro = tituLibro, nomAutor = nomAutor, precUni = precUni,fechPub = fechPub ,sinopsis = sinopsis, idEdito = idEdito, estado = 1 };
            db.tb_libros.Add(data);
            db.SaveChanges();
            return Json(true, JsonRequestBehavior.AllowGet);
        }


        [AutorizarUsuario(idOperacion: 1)]
        public ActionResult EditarLibro(int idLibro, string tituLibro, string nomAutor, decimal precUni, int idEdito, DateTime fechPub, string sinopsis, int estado)
        {
            var data = db.tb_libros.Where(u => u.idLibro == idLibro).FirstOrDefault();
            data.tituLibro = tituLibro;
            data.sinopsis = sinopsis;
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



        // VISTA COMUN USER
        [AutorizarUsuario(idOperacion: 4)]
        public ActionResult Inicio()
        {
            var categorias = db.tb_categorias.ToList().OrderBy(x => x.nombreCate);
            return View(categorias);
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




        //Mantenimiento Editorial
        [AutorizarUsuario(idOperacion: 2)]
        public ActionResult Editorial()
        {
            return View();
        }

        [AutorizarUsuario(idOperacion: 2)]
        public ActionResult MantenimientoEditorial(string notification)
        {
            ViewBag.notification = notification;
            return View();
        }

        //Funciones
        [AutorizarUsuario(idOperacion: 2)]
        public ActionResult listarEditorial(int edit)
        {
            var Editorial = (from m in db.tb_editoriales

                             where m.estado == "Activo"
                             select new { m.idEdito, m.nomEdito, m.direccion, m.telefono, m.fechaRegistro }).ToList();

            var parseo = (from p in Editorial
                          select new
                          {
                              idEdito = p.idEdito,
                              nomEdito = p.nomEdito,
                              direccion = p.direccion,
                              telefono = p.telefono,
                              
                              fechaRegistro = p.fechaRegistro.Value.ToString("yyyy-MM-dd"),
                              
                          }).ToList();

            if (edit == 1)
            {
                Editorial = (from m in db.tb_editoriales
                             select new { m.idEdito, m.nomEdito, m.direccion, m.telefono, m.fechaRegistro }).ToList();
            }

            return Json(new { data = parseo }, JsonRequestBehavior.AllowGet);
        }
        




        [AutorizarUsuario(idOperacion: 2)]
        public ActionResult GuardarEditorial(string nomEdito, string direccion, string telefono,  DateTime fechaRegistro)
        {   
            var data = new tb_editoriales { nomEdito = nomEdito, direccion = direccion, telefono = telefono, estado = "Activo", fechaRegistro = fechaRegistro };
            db.tb_editoriales.Add(data);
            db.SaveChanges();
            return Json(true, JsonRequestBehavior.AllowGet);
        }
       
        [AutorizarUsuario(idOperacion: 2)]
         public ActionResult EditarEditorial(int idEdito, string nomEdito, string direccion, string telefono, DateTime fechaRegistro)
         {
             var data = db.tb_editoriales.Where(u => u.idEdito == idEdito).FirstOrDefault();
            data.nomEdito = nomEdito;
            data.direccion = direccion;
            data.telefono = telefono;
            data.fechaRegistro = fechaRegistro;
             db.SaveChanges();
             return Json(true, JsonRequestBehavior.AllowGet);
         }
        
        [AutorizarUsuario(idOperacion: 2)]
        public ActionResult EliminarEditorial(int idEdito)
        {
            var data = db.tb_editoriales.Find(idEdito);
            data.estado = "Inactivo";
            db.SaveChanges();
            return Json(true, JsonRequestBehavior.AllowGet);
        }

         

    }

}