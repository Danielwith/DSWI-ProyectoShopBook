using ShopBook.Class_DTO;
using ShopBook.Entity;
using ShopBook.Filters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopBook.Controllers
{
    public class HomeController : Controller
    {
        private shopbookEntities db = new shopbookEntities();

        #region VistaDeInicio
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

        // VISTA COMUN USER
        [AutorizarUsuario(idOperacion: 4)]
        public ActionResult Inicio()
        {
            //var categorias = db.tb_categorias.ToList().OrderBy(x => x.nombreCate);
            var libros = (from e in db.tb_editoriales
                          join l in db.tb_libros on e.idEdito equals l.idEdito
                          join csl in db.tb_cate_subcate_libros on l.idLibro equals csl.idLibro
                          join sc in db.tb_sub_categorias on csl.idSubCate equals sc.idsubCate
                          join c in db.tb_categorias on csl.idCate equals c.idcate
                          where l.estado != 0
                          select new LibroDTO { titulo = l.tituLibro, autor = l.nomAutor, precio = l.precUni.ToString(), idCategoria = csl.idCate })
                          .AsEnumerable<LibroDTO>();

            var neecat = (from c in db.tb_categorias
                          select new CategoriaLibroDTO
                          {
                              idcategoria = c.idcate,
                              categoria = c.nombreCate,
                              libros = libros.Where(x => x.idCategoria == c.idcate)
                          }).ToList().OrderBy(x => x.categoria);
            return View(neecat);
        }

        #endregion VistaDeInicio

        #region CRUDProveedorDePrueba
        [AutorizarUsuario(idOperacion: 2)]
        public ActionResult Mantenimiento(string notification)
        {
            ViewBag.notification = notification;
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

        #endregion CRUDProveedorDePrueba

        // OPERACIONES
        [AutorizarUsuario(idOperacion: 1)]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

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