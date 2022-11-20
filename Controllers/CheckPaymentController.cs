using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopBook.Controllers
{
    public class CheckPaymentController : Controller
    {
        // GET: CheckPayment
        public ActionResult Checkout()
        {
            return View();
        }
    }
}