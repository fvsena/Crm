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

        public ActionResult Index()
        {
            return View();
        }

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

        public ActionResult NovaOcorrencia()
        {
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

        public ActionResult NovaAtualizacao(int Codigo)
        {
            return View();
        }
    }
}