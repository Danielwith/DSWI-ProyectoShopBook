using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using ShopBook.Entity;
using ShopBook.Class_DTO;

namespace ShopBook.Controllers
{
    public class CatalogoController : Controller
    {
        private shopbookEntities db = new shopbookEntities();
        // GET: Catalogo
        public ActionResult listarLibrosPorCategoria(int idcate)
        {
           
            Session["nomcate"] = db.tb_categorias.Find(idcate).nombreCate;

            Session["subcate"] = (from sc in db.tb_sub_categorias
                                         join c in db.tb_categorias
                                         on sc.idCate equals c.idcate
                                         where sc.idCate == idcate
                                         select new SubCategoriaDTO { id = sc.idsubCate.ToString(), SubCate = sc.nombreSubCate }).ToList();
                
                 IEnumerable<LibroDTO>  libros = (from e in db.tb_editoriales
                                                    join l in db.tb_libros on e.idEdito equals l.idEdito
                                                    join csl in db.tb_cate_subcate_libros on l.idLibro equals csl.idLibro 
                                                    join sc in db.tb_sub_categorias on csl.idSubCate equals sc.idsubCate
                                                    join c in db.tb_categorias on csl.idCate equals c.idcate
                                                    where c.idcate == idcate && l.estado != 0
                                                    select new LibroDTO
                                                    {
                                                        idLibro = l.idLibro.ToString(),
                                                        titulo = l.tituLibro,
                                                        autor = l.nomAutor,
                                                        precio = l.precUni.ToString(),
                                                        img = l.img

                                                    }).ToList();

            return View(libros.OrderBy(x => x.titulo));
        }

        public ActionResult listarLibrosXSubCate(int idSubCate)
        {
            ViewBag.subcategoria = db.tb_sub_categorias.Find(idSubCate).nombreSubCate;

            IEnumerable<LibroDTO> libros = (from e in db.tb_editoriales
                                            join l in db.tb_libros on e.idEdito equals l.idEdito
                                            join csl in db.tb_cate_subcate_libros on l.idLibro equals csl.idLibro
                                            join sc in db.tb_sub_categorias on csl.idSubCate equals sc.idsubCate
                                            join c in db.tb_categorias on csl.idCate equals c.idcate
                                            where sc.idsubCate == idSubCate && l.estado != 0
                                            select new LibroDTO
                                            {
                                                idLibro = l.idLibro.ToString(),
                                                titulo = l.tituLibro,
                                                autor = l.nomAutor,
                                                precio = l.precUni.ToString(),
                                                img = l.img
                                            }).ToList();

            return View(libros.OrderBy(x => x.titulo));
        }
        
        public ActionResult listLibros(int idLib, string notification)
        {
            LibroDTO libro = (from e in db.tb_editoriales
                                            join l in db.tb_libros on e.idEdito equals l.idEdito
                                            join csl in db.tb_cate_subcate_libros on l.idLibro equals csl.idLibro
                                            join sc in db.tb_sub_categorias on csl.idSubCate equals sc.idsubCate
                                            join c in db.tb_categorias on csl.idCate equals c.idcate
                                            where csl.idLibro == idLib
                                            select new LibroDTO
                                            {
                                                idLibro = l.idLibro.ToString(),
                                                titulo = l.tituLibro,
                                                autor = l.nomAutor,
                                                sinopsis = l.sinopsis,
                                                precio = l.precUni.ToString(),
                                                img = l.img
                                            }).FirstOrDefault();
            ViewBag.notification = notification;
            return View(libro);
        }
    }
}