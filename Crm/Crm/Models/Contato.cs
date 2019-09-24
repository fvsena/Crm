using Crm.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Crm.Models
{
    public class Contato : Sql
    {
        public int CodigoContato { get; set; }
        public int CodigoCliente { get; set; }
        public int CodigoAgente { get; set; }
        public string NomeAgente { get; set; }
        public DateTime DataContato { get; set; }
        public string Detalhe { get; set; }

        public List<Contato> ObterContatos(int codigoCliente)
        {
            try
            {
                Contato c = null;
                List<Contato> contatos = new List<Contato>();
                List<SqlParameter> parametros = new List<SqlParameter>();
                parametros.Add(new SqlParameter("@IdCliente", codigoCliente));
                DataSet ds = ExecuteDataset(csCrm, "sp_ObterContatos", parametros);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        c = new Contato();
                        c.CodigoCliente = Convert.ToInt32(dr["IdCustomer"].ToString());
                        c.CodigoAgente = Convert.ToInt32(dr["IdAgent"].ToString());
                        c.NomeAgente = dr["Name"].ToString();
                        c.CodigoContato = Convert.ToInt32(dr["IdContact"].ToString());
                        c.DataContato = Convert.ToDateTime(dr["ContactDate"].ToString());
                        c.Detalhe = dr["Detail"].ToString();
                        contatos.Add(c);
                    }
                }
                return contatos;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void GravarContato()
        {
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(new SqlParameter("@IdCliente", this.CodigoCliente));
            parametros.Add(new SqlParameter("@IdAgente", this.CodigoAgente));
            parametros.Add(new SqlParameter("@Detalhe", this.Detalhe));

            DataSet ds = ExecuteDataset(csCrm, "sp_GravarContato", parametros);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                try
                {
                    string Erro = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    throw new Exception(Erro);
                }
                catch(Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}