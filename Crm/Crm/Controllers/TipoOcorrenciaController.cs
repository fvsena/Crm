using Crm.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Crm.Controllers
{
    public class TipoOcorrenciaController : Controller
    {
        TipoOcorrencia TipoOcorrencia = new TipoOcorrencia();

        public ActionResult Index()
        {
            return RedirectToAction("CadastroTipoOcorrencia");
        }

        public ActionResult CadastroTipoOcorrencia()
        {
            TipoOcorrencia.CarregaOcorrencias();
            return View("CadastroTipoOcorrencia", TipoOcorrencia);
        }

        public ActionResult GravarGrupo(TipoOcorrencia tipoOcorrencia)
        {
            tipoOcorrencia.GravarGrupoOcorrencia();
            return RedirectToAction("CadastroTipoOcorrencia");
        }

        public ActionResult GravarSubGrupo(string Grupos, string SubGrupo)
        {
            this.TipoOcorrencia.CodigoGrupo = Convert.ToInt32(Grupos);
            this.TipoOcorrencia.SubGrupo = SubGrupo; 
            this.TipoOcorrencia.GravarSubGrupoOcorrencia();
            return RedirectToAction("CadastroTipoOcorrencia");
        }
    }
}