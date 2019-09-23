using Crm.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Crm.Controllers
{
    public class ContatoController : Controller
    {
        Contato contato = new Contato();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Listar()
        {
            if (Session["CodigoCliente"] == null)
            {
                Response.Write("<script>alert(\"É necessário selecionar um cliente!\");</script>");
                return RedirectToAction("Index", "Cliente");
            }

            return View(contato.ObterContatos(Convert.ToInt32(Session["CodigoCliente"])));
        }

        public ActionResult NovoContato()
        {
            return View();
        }

        public ActionResult GravarContato(Contato contato)
        {
            return View("Listar", contato.ObterContatos(Convert.ToInt32(Session["CodigoCliente"])));
        }
    }
}