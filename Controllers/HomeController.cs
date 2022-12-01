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
            Session["categorias"] = (from c in db.tb_categorias select new CategoriaDTO{ idcate = c.idcate, nombreCate = c.nombreCate, descripcion = c.descripcion}).ToList();

            Session["subcate"] = (from sc in db.tb_sub_categorias
                                  join c in db.tb_categorias
                                  on sc.idCate equals c.idcate
                                  select new SubCategoriaDTO { id = sc.idsubCate, SubCate = sc.nombreSubCate, idcate = sc.idCate }).ToList();
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