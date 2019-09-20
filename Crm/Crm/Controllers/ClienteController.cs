using Crm.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Crm.Controllers
{
    public class ClienteController : Controller
    {
        public Cliente cliente = new Cliente();

        public ActionResult Index()
        {
            List<Cliente> clientes = cliente.ObterClientes();
            return View(clientes);
        }

        public ActionResult CadastrarCliente()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GravarCliente(Cliente clienteView)
        {
            clienteView.GravarCliente();
            return View("CadastrarCliente");
        }
    }
}