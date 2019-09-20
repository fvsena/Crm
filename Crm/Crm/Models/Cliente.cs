using Crm.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Crm.Models
{
    public class Cliente : Sql
    {
        public int Codigo { get; set; }

        [Required]
        public string Nome { get; set; }

        [Required]
        public string Documento { get; set; }

        [Required]
        public DateTime DataNascimento { get; set; }
        public string Genero { get; set; }

        [Required]
        public string Telefone { get; set; }


        public Endereco Endereco { get; set; }

        public Cliente()
        {
             this.Endereco = new Endereco();
        }

        public static List<Cliente> ObterClientesTeste()
        {
            List<Cliente> clientes = new List<Cliente>();
            clientes.Add(new Cliente()
            {
                Codigo = 1,
                Nome = "Felipe",
                Documento = "123456789",
                DataNascimento = DateTime.Now.AddYears(-20),
                Genero = "M"
            });

            clientes.Add(new Cliente()
            {
                Codigo = 2,
                Nome = "Chirley",
                Documento = "987654321",
                DataNascimento = DateTime.Now.AddYears(-28),
                Genero = "F"
            });

            clientes.Add(new Cliente()
            {
                Codigo = 3,
                Nome = "Raphael",
                Documento = "123987456",
                DataNascimento = DateTime.Now.AddYears(-18),
                Genero = "M"
            });

            return clientes;
        }

        public List<Cliente> ObterClientes()
        {
            try
            {
                Cliente c = null;
                List<Cliente> clientes = new List<Cliente>();
                DataSet ds = ExecuteDataset(csCrm, "sp_ObterClientes");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        c = new Cliente();
                        c.Codigo = Convert.ToInt32(dr["Codigo"].ToString());
                        c.Nome = dr["Nome"].ToString();
                        c.Genero = dr["Genero"].ToString();
                        c.Documento = dr["Documento"].ToString();
                        c.DataNascimento = Convert.ToDateTime(dr["DataNascimento"].ToString());
                        clientes.Add(c);
                    }
                }
                return clientes;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int GravarCliente()
        {
            try
            {
                List<SqlParameter> parametros = new List<SqlParameter>();
                parametros.Add(new SqlParameter("@Nome", this.Nome));
                parametros.Add(new SqlParameter("@Genero", this.Genero));
                parametros.Add(new SqlParameter("@Documento", this.Documento));
                parametros.Add(new SqlParameter("@DataNascimento", this.DataNascimento));
                return ExecuteCommand(csCrm, "sp_GravarCliente", parametros);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}