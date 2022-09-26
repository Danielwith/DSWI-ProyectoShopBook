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
        [AutorizarUsuario(idOperacion:1,idModulo:1)]
        public ActionResult Index()
        {
            ViewBag.idOperacion = 1;
            ViewBag.idModulo = 1;
            return View();
        }

        [AutorizarUsuario(idOperacion: 1,idModulo:1)]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [AutorizarUsuario(idOperacion: 1, idModulo: 1)]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}