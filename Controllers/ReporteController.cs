using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopBook.Class_DTO;
using ShopBook.Entity;
using ShopBook.Filters;
using ShopBook.ViewModels;

namespace ShopBook.Controllers
{
    public class ReporteController : Controller
    {
        private shopbookEntities db = new shopbookEntities();

        // GET: Reporte
        [AutorizarUsuario(idOperacion: 1)]
        public ActionResult Reportes(string dateini, string dateend, string username)
        {
            IEnumerable<BoletaUsuario> data = null;
            if (!String.IsNullOrEmpty(dateini) && !String.IsNullOrEmpty(dateend) && !String.IsNullOrEmpty(username))
            {
                var name = username.Trim();
                var ini = Convert.ToDateTime(dateini);
                var end = Convert.ToDateTime(dateend);
                data = (from b in db.tb_boletas
                        join u in db.tb_usuario on b.idUser equals u.idUser
                        where  u.nombre.Contains(name) && (b.fechGene >= ini && b.fechGene <= end)
                        select new BoletaUsuario
                        {
                            nroBoleta = b.nroBoleta,
                            fechGene = b.fechGene,
                            nombre = u.nombre,
                            email = u.email
                        }).ToList();
            }
            else if (!String.IsNullOrEmpty(dateini) && !String.IsNullOrEmpty(dateend))
            {
                var ini = Convert.ToDateTime(dateini);
                var end = Convert.ToDateTime(dateend);
                data = (from b in db.tb_boletas
                        join u in db.tb_usuario on b.idUser equals u.idUser
                        where b.fechGene >= ini && b.fechGene <= end
                        select new BoletaUsuario
                        {
                            nroBoleta = b.nroBoleta,
                            fechGene = b.fechGene,
                            nombre = u.nombre,
                            email = u.email
                        }).ToList();
            }
            else if (!String.IsNullOrEmpty(username))
            {
                var name = username.Trim();
                data = (from b in db.tb_boletas
                        join u in db.tb_usuario on b.idUser equals u.idUser
                        where u.nombre.Contains(name)
                        select new BoletaUsuario
                        {
                            nroBoleta = b.nroBoleta,
                            fechGene = b.fechGene,
                            nombre = u.nombre,
                            email = u.email
                        }).ToList();
            }
            else
            {
                data = (from b in db.tb_boletas
                        join u in db.tb_usuario on b.idUser equals u.idUser
                        select new BoletaUsuario
                        {
                            nroBoleta = b.nroBoleta,
                            fechGene = b.fechGene,
                            nombre = u.nombre,
                            email = u.email
                        }).ToList();
            }
            return View(data);
        }

        [AutorizarUsuario(idOperacion: 1)]
        public FileResult GeneratePDF(string nroBoleta)
        {
            string ruta = Server.MapPath("~/");
            string ReportURL = Path.Combine(ruta + "Assets\\Reporte.pdf");
            var data = (from b in db.tb_boletas
                        join det in db.tb_detalle_boletas on b.nroBoleta equals det.nroBoleta
                        select new { b.nroBoleta, b.fechGene, b.idUser}).ToList();
            var reporte = new ReporteGenerator();
            reporte.generate(ReportURL);
            byte[] FileBytes = System.IO.File.ReadAllBytes(ReportURL);
            return File(FileBytes, "application/pdf");
        }
    }
}