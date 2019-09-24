using Crm.Filters;
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

        [AuthFilter]
        public ActionResult Index()
        {
            return View();
        }

        [AuthFilter]
        public ActionResult Listar()
        {
            if (Session["CodigoCliente"] == null)
            {
                Response.Write("<script>alert(\"É necessário selecionar um cliente!\");</script>");
                return RedirectToAction("Index", "Cliente");
            }

            return View(contato.ObterContatos(Convert.ToInt32(Session["CodigoCliente"])));
        }

        [AuthFilter]
        public ActionResult NovoContato()
        {
            return View();
        }

        [AuthFilter]
        public ActionResult GravarContato(Contato contato)
        {
            contato.CodigoAgente = Convert.ToInt32(Session["CodigoUsuario"].ToString());
            contato.CodigoCliente = Convert.ToInt32(Session["CodigoCliente"].ToString());
            contato.GravarContato();
            Response.Write("<script>alert(\"Contato gravado com sucesso!\");</script>");
            return View("Listar", contato.ObterContatos(Convert.ToInt32(Session["CodigoCliente"])));
        }
    }
}