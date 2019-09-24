using Crm.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Crm.Controllers
{
    public class LoginController : Controller
    {
        public ActionResult Index()
        {
            return View("Login");
        }

        public ActionResult Logon(Usuario usuario)
        {
            if (usuario.ValidarLogin())
            {
                Session["login"] = usuario.Login;
                Session["CodigoUsuario"] = usuario.Codigo;
                return RedirectToAction("Index", "Cliente");
            }
            return View("Login");
        }
    }
}