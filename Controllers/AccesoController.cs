using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopBook.Controllers
{
    public class AccesoController : Controller
    {
        // GET: Login
        // Este comentario es de Dwight
        // Este comentario es de Carlos chinguel quinto
        // Este comentario es de Luis
        // Escribi Apellido 
        //Comentario clemente Romero Sting

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string email, string pass)
        {
            var descripRol=String.Empty;
            try
            {
                using (Entity.shopbookEntities db = new Entity.shopbookEntities())
                {
                    var oEmail = (from d in db.tb_usuario
                                  where d.email == email.Trim() && d.password == pass.Trim()
                                  select d).FirstOrDefault();
                    if(oEmail== null)
                    {
                        ViewBag.Error = "Usuario o Contraseña invalida";
                        return View();
                    }
                    // LA DESCRIPCION DEL ROL, ES EL ActionResult
                    var rol = from d in db.tb_rol
                          where d.idRol == oEmail.idRol
                          select d.descrip;

                    descripRol = rol.FirstOrDefault().ToString();

                    Session["DescriRol"] = descripRol;
                    Session["email"] = oEmail;

                }
                /* LOGIN REALIZADO CON EXITO */
                return RedirectToAction(descripRol, "Home");
            }
            catch(Exception ex)
            {
                ViewBag.Error = ex.Message;
                return RedirectToAction("OperacionNoAutorizada", "Error");
            }
        }

        public ActionResult Desconectar()
        {
            Session["email"] = null;
            Session["carrito"] = null;
            return RedirectToAction("Login", "Acceso");
        }

        // GET: Registrar
        public ActionResult Registrar()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Registrar(string nombre,string email, string pass, string repPass)
        {
            if (pass != repPass)
            {
                return RedirectToAction("Registrar","Acceso");
            }
            try
            {
                using(var db = new Entity.shopbookEntities())
                {
                    var checkEmail = (from e in db.tb_usuario
                                     where email == e.email
                                     select e).FirstOrDefault();

                    if (checkEmail == null) {
                        db.usp_regUser(nombre, email, pass);
                    }
                    else
                    {
                        return RedirectToAction("Registrar", "Acceso");
                    }
                };

                return RedirectToAction("Login","Acceso");
            }
            catch(Exception)
            {
                return RedirectToAction("OperacionNoAutorizada", "Error");
            }
        }
    }
}
