using Crm.Filters;
using Crm.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Crm.Controllers
{
    public class OcorrenciaController : Controller
    {
        Ocorrencia Ocorrencia = new Ocorrencia();

        [AuthFilter]
        public ActionResult Index()
        {
            return View();
        }

        [AuthFilter]
        public ActionResult Listar()
        {
            return View("Listar", Ocorrencia.ObterOcorrencias());
        }

        public ActionResult AcessarOcorrencia(int Codigo, int IdCliente, string NomeCliente)
        {
            Session["CodigoCliente"] = IdCliente;
            Session["NomeCliente"] = NomeCliente;
            Ocorrencia = Ocorrencia.BuscarOcorrencia(Codigo);
            return View("AcessarOcorrencia", Ocorrencia);
        }

        [AuthFilter]
        public ActionResult NovaOcorrencia()
        {
            if (Session["CodigoCliente"] == null)
            {
                return RedirectToAction("Index", "Cliente");
            }
            Ocorrencia.CarregaGrupos();
            return View("NovaOcorrencia", Ocorrencia);
        }

        [AuthFilter]
        [HttpGet]
        public string ObterSubGrupos(string IdGrupo)
        {
            Ocorrencia.CodigoGrupo = Convert.ToInt32(IdGrupo);
            Ocorrencia.CarregaSubGrupos();
            return JsonConvert.SerializeObject(Ocorrencia.SubGrupos);
        }

        [AuthFilter]
        [HttpGet]
        public string ObterDetalhes(string IdGrupo, string IdSubGrupo)
        {
            Ocorrencia.CodigoGrupo = Convert.ToInt32(IdGrupo);
            Ocorrencia.CodigoSubGrupo = Convert.ToInt32(IdSubGrupo);
            Ocorrencia.CarregaDetalhes();
            return JsonConvert.SerializeObject(Ocorrencia.Detalhes);
        }

        [AuthFilter]
        public ActionResult GravarOcorrencia(string Grupos, string SubGrupos, string Detalhes, string Descricao)
        {
            Ocorrencia.IdAgente = Convert.ToInt32(Session["CodigoUsuario"].ToString());
            Ocorrencia.IdCliente = Convert.ToInt32(Session["CodigoCliente"].ToString());
            Ocorrencia.CodigoGrupo = Convert.ToInt32(Grupos);
            Ocorrencia.CodigoSubGrupo = Convert.ToInt32(SubGrupos);
            Ocorrencia.CodigoDetalhe = Convert.ToInt32(Detalhes);
            Ocorrencia.Descricao = Descricao;
            Ocorrencia.GravarOcorrencia();
            return RedirectToAction("Listar");
        }

        [AuthFilter]
        public ActionResult NovaAtualizacao(int Codigo)
        {
            this.Ocorrencia.Codigo = Codigo;
            return View("NovaAtualizacao", this.Ocorrencia);
        }

        [AuthFilter]
        public ActionResult GravarAtualizacao(string TipoAtualizacao, string Mensagem, string Codigo = "")
        {
            if (string.IsNullOrWhiteSpace(Codigo))
            {
                return RedirectToAction("Listar", "Ocorrencia");
            }
            var IdCliente = Session["CodigoCliente"];
            var NomeCliente = Session["NomeCliente"];
            Ocorrencia.Codigo = Convert.ToInt32(Codigo);
            Ocorrencia.IdAgente = Convert.ToInt32(Session["CodigoUsuario"].ToString());
            Ocorrencia.IdCliente = Convert.ToInt32(Session["CodigoCliente"].ToString());
            Ocorrencia.GravarAtualizacao(Convert.ToInt32(TipoAtualizacao), Mensagem);
            return RedirectToAction("Listar", "Ocorrencia");
        }
    }
}