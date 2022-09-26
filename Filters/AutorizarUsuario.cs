using ShopBook.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ShopBook.Filters
{
    [AttributeUsage(AttributeTargets.Method,AllowMultiple =true)]
    public class AutorizarUsuario : AuthorizeAttribute
    {
        private tb_usuario oEmail;
        private SistemaBibliotecaPerfilesEntities db = new SistemaBibliotecaPerfilesEntities();
        private int idOperacion;
        private int idModulo;


        public AutorizarUsuario(int idOperacion = 0, int idModulo = 0)
        {
            this.idOperacion = idOperacion;
            this.idModulo = idModulo;
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            String nombreOperacion = String.Empty;
            String nombreModulo = String.Empty;

            try
            {
                oEmail = (tb_usuario)HttpContext.Current.Session["email"];
                var lstMisOperaciones = from m in db.tb_rol_operacion
                                        where m.idRol == oEmail.idRol && m.idOperacion == idOperacion
                                        select m;

                /* CONDICIONAL EN CASO DE NO PASAR NINGUN DATO, CUENTA LA LISTA DE OPERACIONES A PODER REALIZAR */
                if (lstMisOperaciones.ToList().Count() < 1)
                {
                    var oOperacion = db.tb_operaciones.Find(idOperacion);
                    var oModulo = db.tb_modulo.Find(idModulo);
                    nombreOperacion = getNombreDeOperacion(idOperacion);
                    nombreModulo = getNombreDelModulo(idModulo);
                    var redirectError = new RouteValueDictionary(new
                    {
                        action = "OperacionNoAutorizada",
                        controller = "Error",
                        operacion = nombreOperacion,
                        modulo = nombreModulo,
                        msgErrorExcepcion = "."
                    });
                    filterContext.Result = new RedirectToRouteResult(redirectError);
                }
            }
            catch(Exception ex)
            {
                var redirectError = new RouteValueDictionary(new
                {
                    action = "OperacionNoAutorizada",
                    controller = "Error",
                    operacion = nombreOperacion,
                    modulo = nombreModulo,
                    msgErrorExcepcion = ex
                });
                filterContext.Result = new RedirectToRouteResult(redirectError);
            }
            
        }

        public string getNombreDeOperacion(int idOperacion)
        {
            var ope = from op in db.tb_operaciones
                      where op.idOperacion == idOperacion
                      select op.acceso;
            String nombreOperacion;
            try
            {
                return nombreOperacion = ope.First();
            }
            catch(Exception)
            {
                return nombreOperacion = String.Empty;
            }
        }

        public string getNombreDelModulo(int idModulo)
        {
            var mod = from m in db.tb_modulo
                      where m.idModulo == idModulo
                      select m.descrip;
            String nombreModulo;
            try
            {
                return nombreModulo = mod.First();
            }
            catch (Exception)
            {
                return nombreModulo = String.Empty;
            }
        }
    }
}