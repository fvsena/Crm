using Crm.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Crm.Models
{
    public class TipoOcorrencia : Sql
    {
        public int CodigoGrupo { get; set; }
        public int CodigoSubGrupo { get; set; }
        public int CodigoDetalhe { get; set; }
        public string Grupo { get; set; }
        public string SubGrupo { get; set; }
        public string Detalhe { get; set; }

        public List<GrupoOcorrencia> Grupos { get; set; } = new List<GrupoOcorrencia>();
        public List<SubGrupo> SubGrupos { get; set; } = new List<SubGrupo>();
        public List<DetalheOcorrencia> DetalheOcorrencias { get; set; } = new List<DetalheOcorrencia>();

        public void CarregaOcorrencias()
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

        public void GravarGrupoOcorrencia()
        {
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(new SqlParameter("@Grupo", this.Grupo));
            ExecuteCommand(csCrm, "sp_GravarGrupoOcorrencia", parametros);
        }

        public void GravarSubGrupoOcorrencia()
        {
            if (this.CodigoGrupo.Equals(0))
            {
                return;
            }
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(new SqlParameter("@Grupo", this.CodigoGrupo));
            parametros.Add(new SqlParameter("@SubGrupo", this.SubGrupo));
            ExecuteCommand(csCrm, "sp_GravarSubGrupoOcorrencia", parametros);
        }

        public void GravarDetalheOcorrencia()
        {
            if (this.CodigoGrupo.Equals(0) || this.CodigoSubGrupo.Equals(0))
            {
                return;
            }
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(new SqlParameter("@Grupo", this.CodigoGrupo));
            parametros.Add(new SqlParameter("@SubGrupo", this.CodigoSubGrupo));
            parametros.Add(new SqlParameter("@Detalhe", this.Detalhe));
            ExecuteCommand(csCrm, "sp_GravarDetalheOcorrencia", parametros);
        }
    }
}