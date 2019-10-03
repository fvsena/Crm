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

        public List<AtualizacaoOcorrencia> Atualizacoes { get; set; } = new List<AtualizacaoOcorrencia>();
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

        public List<Ocorrencia> ObterOcorrencias()
        {
            List<Ocorrencia> ocorrencias = new List<Ocorrencia>();
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(new SqlParameter("@Quantidade", 100));
            DataSet ds = ExecuteDataset(csCrm, "sp_ObterOcorrencias", parametros);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ocorrencias.Add(new Ocorrencia()
                {
                    Codigo = Convert.ToInt32(dr["IdOcorrencia"].ToString()),
                    IdCliente = Convert.ToInt32(dr["IdCliente"].ToString()),
                    DataAbertura = Convert.ToDateTime(dr["DataAbertura"].ToString()),
                    NomeCliente = dr["Cliente"].ToString(),
                    NomeAgente = dr["Agente"].ToString(),
                    Grupo = dr["Grupo"].ToString(),
                    SubGrupo = dr["SubGrupo"].ToString(),
                    Detalhe = dr["Detalhe"].ToString(),
                    Descricao = dr["UltimaAtualizacao"].ToString()
                });
            }
            return ocorrencias;
        }

        public void GravarOcorrencia()
        {
            if (this.CodigoGrupo.Equals(0) || this.CodigoSubGrupo.Equals(0) || this.CodigoDetalhe.Equals(0) || string.IsNullOrWhiteSpace(this.Descricao))
            {
                return;
            }
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(new SqlParameter("@IdCliente", this.IdCliente));
            parametros.Add(new SqlParameter("@IdAgente", this.IdAgente));
            parametros.Add(new SqlParameter("@CodigoGrupo", this.CodigoGrupo));
            parametros.Add(new SqlParameter("@CodigoSubGrupo", this.CodigoSubGrupo));
            parametros.Add(new SqlParameter("@CodigoDetalhe", this.CodigoDetalhe));
            parametros.Add(new SqlParameter("@Mensagem", this.Descricao));
            DataSet ds = ExecuteDataset(csCrm, "sp_GravarOcorrencia", parametros);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                string Erro = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                throw new Exception(Erro);
            }
        }

        public Ocorrencia BuscarOcorrencia(int codigo)
        {
            Ocorrencia ocorrencia = new Ocorrencia();
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(new SqlParameter("@Codigo", codigo));
            DataSet ds = ExecuteDataset(csCrm, "sp_BuscarOcorrencia", parametros);
            if (ds.Tables.Count > 1)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    ocorrencia.Codigo = Convert.ToInt32(dr["IdOcorrencia"]);
                    ocorrencia.IdCliente = Convert.ToInt32(dr["IdCliente"].ToString());
                    ocorrencia.DataAbertura = Convert.ToDateTime(dr["DataAbertura"].ToString());
                    ocorrencia.NomeCliente = dr["Cliente"].ToString();
                    ocorrencia.NomeAgente = dr["Agente"].ToString();
                    ocorrencia.Grupo = dr["Grupo"].ToString();
                    ocorrencia.SubGrupo = dr["SubGrupo"].ToString();
                    ocorrencia.Detalhe = dr["Detalhe"].ToString();
                }

                foreach (DataRow dr in ds.Tables[1].Rows)
                {
                    ocorrencia.Atualizacoes.Add(new AtualizacaoOcorrencia()
                    {
                        Codigo = Convert.ToInt32(dr["IdAtualizacao"].ToString()),
                        DataAtualizacao = Convert.ToDateTime(dr["DataAtualizacao"].ToString()),
                        CodigoAgente = Convert.ToInt32(dr["IdAgente"]),
                        NomeAgente = dr["Agente"].ToString(),
                        TipoAtualizacao = dr["TipoAtualizacao"].ToString(),
                        Mensagem = dr["Mensagem"].ToString()
                    });
                }
            }
            return ocorrencia;
        }

        public void GravarAtualizacao(int idTipoAtualizacao, string mensagem)
        {
            if (this.Codigo.Equals(0) || idTipoAtualizacao.Equals(0) || string.IsNullOrWhiteSpace(mensagem))
            {
                return;
            }
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(new SqlParameter("@IdOcorrencia", this.Codigo));
            parametros.Add(new SqlParameter("@IdAgente", this.IdAgente));
            parametros.Add(new SqlParameter("@IdTipoAtualizacao", idTipoAtualizacao));
            parametros.Add(new SqlParameter("@Mensagem", mensagem));
            DataSet ds = ExecuteDataset(csCrm, "sp_GravarAtualizacao", parametros);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                string Erro = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                throw new Exception(Erro);
            }
        }
    }
}