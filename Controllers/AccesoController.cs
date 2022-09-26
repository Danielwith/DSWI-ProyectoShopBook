using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopBook.Controllers
{
    public class AccesoController : Controller
    {
        // GET: Acceso
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string email, string pass)
        {
            try
            {
                using (Entity.SistemaBibliotecaPerfilesEntities db = new Entity.SistemaBibliotecaPerfilesEntities())
                {
                    var oEmail = (from d in db.tb_usuario
                                  where d.email == email.Trim() && d.password == pass.Trim()
                                  select d).FirstOrDefault();
                    if(oEmail== null)
                    {
                        ViewBag.Error = "Usuario o Contraseña invalida";
                        return View();
                    }

                    Session["email"] = oEmail;

                }
                /* LOGIN REALIZADO CON EXITO */
                return RedirectToAction("Index", "Home");
            }
            catch(Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }
        }
    }
}
