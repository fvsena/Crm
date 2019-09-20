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

        public ActionResult SelecionaCliente(string Codigo, string Nome)
        {
            Session["CodigoCliente"] = Codigo;
            Session["NomeCliente"] = Nome;
            return RedirectToAction("CadastrarCliente");
        }

        public ActionResult Limpar()
        {
            Session["CodigoCliente"] = null;
            Session["NomeCliente"] = null;
            return RedirectToAction("CadastrarCliente");
        }

        public ActionResult CadastrarCliente()
        {
            if (Session["CodigoCliente"] != null)
            {
                this.cliente = new Cliente(Convert.ToInt32(Session["CodigoCliente"].ToString()));
                this.cliente.ObterTelefones();
                return View("CadastrarCliente", this.cliente);
            }
            return View("CadastrarCliente", new Cliente());
        }

        [HttpPost]
        public ActionResult GravarCliente(Cliente clienteView)
        {
            cliente = clienteView.GravarCliente();
            Session["CodigoCliente"] = cliente.Codigo;
            Session["NomeCliente"] = cliente.Nome;
            Response.Write("<script>alert(\"Cliente gravado com sucesso!\");</script>");
            return View("CadastrarCliente");
        }

        public ActionResult GravarEndereco(string Cep, string Logradouro, string Numero, string Bairro, string Complemento, string Cidade, string Uf)
        {
            if (Session["CodigoCliente"] == null)
            {
                Response.Write("<script>alert(\"Cliente não selecionado!\");</script>");
                return View("CadastrarCliente");
            }
            cliente.Codigo = Convert.ToInt32(Session["CodigoCliente"].ToString());
            cliente.Endereco = new Endereco()
            {
                CEP = Cep,
                Logradouro = Logradouro,
                Numero = Numero,
                Bairro = Bairro,
                Complemento = Complemento,
                Cidade = Cidade,
                UF = Uf
            };
            cliente.GravarEndereco();
            
            Response.Write("<script>alert(\"Endereço gravado com sucesso!\");</script>");
            return View("CadastrarCliente");
        }

    }
}