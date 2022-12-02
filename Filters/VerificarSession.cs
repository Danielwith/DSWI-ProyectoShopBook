using ShopBook.Controllers;
using ShopBook.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ShopBook.Filters
{
    public class VerificarSession : ActionFilterAttribute
    {
        private tb_usuario oEmail;
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            try
            {
                base.OnActionExecuted(filterContext);
                oEmail = (tb_usuario)HttpContext.Current.Session["email"];
                if (oEmail == null)
                {
                    //Enviar mensaje de error LOGEO, no enviar el 404
                    /*
                    if (filterContext.Controller is AccesoController == false)
                    {
                        filterContext.HttpContext.Response.Redirect("/Acceso/Login");
                    }
                    */
                }
            }
            catch(Exception)
            {
                filterContext.HttpContext.Response.Redirect("/Error/OperacionNoAutorizada");
            }
            
        }
    }
}