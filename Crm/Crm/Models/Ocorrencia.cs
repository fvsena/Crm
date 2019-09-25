using Crm.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Crm.Models
{
    public class Ocorrencia : Sql
    {
        public int Codigo { get; set; }
        public int IdAgente { get; set; }
        public string NomeAgente { get; set; }
        public int IdCliente { get; set; }
        public string NomeCliente { get; set; }
        public DateTime DataAbertura { get; set; }
        public int CodigoGrupo { get; set; }
        public string Grupo { get; set; }
        public int CodigoSubGrupo { get; set; }
        public string SubGrupo { get; set; }
        public int CodigoDetalhe { get; set; }
        public string Detalhe { get; set; }
        public int CodigoStatus { get; set; }
        public string Status { get; set; }
        public string Descricao { get; set; }

        public List<GrupoOcorrencia> Grupos { get; set; } = new List<GrupoOcorrencia>();
        public List<SubGrupo> SubGrupos { get; set; } = new List<SubGrupo>();
        public List<DetalheOcorrencia> Detalhes { get; set; } = new List<DetalheOcorrencia>();

        public void CarregaGrupos()
        {
            Grupos.Clear();
            DataSet ds = ExecuteDataset(csCrm, "sp_ObterGrupoOcorrencia");
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Grupos.Add(new GrupoOcorrencia()
                    {
                        Id = Convert.ToInt32(dr["IdGrupo"].ToString()),
                        Nome = dr["Grupo"].ToString()
                    });
                }
            }
        }

        public void CarregaSubGrupos()
        {
            SubGrupos.Clear();
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(new SqlParameter("@IdGrupo", this.CodigoGrupo));
            DataSet ds = ExecuteDataset(csCrm, "sp_ObterSubGrupoOcorrencia", parametros);
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    SubGrupos.Add(new SubGrupo()
                    {
                        Id = Convert.ToInt32(dr["IdSubGrupo"].ToString()),
                        Nome = dr["SubGrupo"].ToString()
                    });
                }
            }
        }

        public void CarregaDetalhes()
        {
            Detalhes.Clear();
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(new SqlParameter("@IdGrupo", this.CodigoGrupo));
            parametros.Add(new SqlParameter("@IdSubGrupo", this.CodigoSubGrupo));
            DataSet ds = ExecuteDataset(csCrm, "sp_ObterDetalheOcorrencia", parametros);
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Detalhes.Add(new DetalheOcorrencia()
                    {
                        Id = Convert.ToInt32(dr["IdDetalhe"].ToString()),
                        Nome = dr["Detalhe"].ToString()
                    });
                }
            }
        }
    }
}