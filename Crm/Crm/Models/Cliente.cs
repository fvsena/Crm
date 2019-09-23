using Crm.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Crm.Models
{
    public class Cliente : Sql
    {
        #region Propriedades públicas
        public int Codigo { get; set; }

        [Required]
        public string Nome { get; set; }

        [Required]
        public string Documento { get; set; }

        [Required]
        public DateTime DataNascimento { get; set; }
        public string Genero { get; set; }

        [Required]
        public ICollection<string> Telefones { get; set; } = new List<string>();

        public Endereco Endereco { get; set; }
        #endregion

        #region Construtores
        public Cliente()
        {
            this.Endereco = new Endereco();
            this.Telefones = new List<string>();
        }

        public Cliente(int codigo)
        {
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(new SqlParameter("@IdCliente", codigo));
            DataSet ds = ExecuteDataset(csCrm, "sp_ObterCliente", parametros);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    this.Codigo = Convert.ToInt32(dr["Codigo"].ToString());
                    this.Nome = dr["Nome"].ToString();
                    this.Genero = dr["Genero"].ToString();
                    this.Documento = dr["Documento"].ToString();
                    this.DataNascimento = Convert.ToDateTime(dr["DataNascimento"].ToString());
                }
            }
        } 
        #endregion

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

        public void ObterTelefones()
        {
            try
            {
                this.Telefones.ToList().Clear();
                List<SqlParameter> parametros = new List<SqlParameter>();
                parametros.Add(new SqlParameter("@IdCliente", this.Codigo));
                DataSet ds = ExecuteDataset(csCrm, "sp_ObterTelefones", parametros);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        this.Telefones.Add(dr["Telefone"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ObterEndereco()
        {
            try
            {
                this.Endereco = new Endereco();
                List<SqlParameter> parametros = new List<SqlParameter>();
                parametros.Add(new SqlParameter("@IdCliente", this.Codigo));
                DataSet ds = ExecuteDataset(csCrm, "sp_ObterEndereco", parametros);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        this.Endereco.CEP = dr["PostalCode"].ToString();
                        this.Endereco.Logradouro = dr["PublicPlace"].ToString();
                        this.Endereco.Numero = dr["Number"].ToString();
                        this.Endereco.Bairro = dr["Neighborhood"].ToString();
                        this.Endereco.Complemento = dr["Complement"].ToString();
                        this.Endereco.Cidade = dr["City"].ToString();
                        this.Endereco.UF = dr["FS"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Endereco GravarEndereco()
        {
            try
            {
                List<SqlParameter> parametros = new List<SqlParameter>();
                parametros.Add(new SqlParameter("@IdCliente", this.Codigo));
                parametros.Add(new SqlParameter("@Cep", this.Endereco.CEP));
                parametros.Add(new SqlParameter("@Logradouro", this.Endereco.Logradouro));
                parametros.Add(new SqlParameter("@Numero", this.Endereco.Numero));
                parametros.Add(new SqlParameter("@Bairro", this.Endereco.Bairro));
                parametros.Add(new SqlParameter("@Complemento", this.Endereco.Complemento));
                parametros.Add(new SqlParameter("@Cidade", this.Endereco.Cidade));
                parametros.Add(new SqlParameter("@Uf", this.Endereco.UF));
                DataSet ds = ExecuteDataset(csCrm, "sp_GravarEndereco", parametros);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        string Erro = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                        throw new Exception(Erro);
                    }
                }
                return this.Endereco;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Cliente GravarCliente(int CodigoCliente = 0)
        {
            try
            {
                List<SqlParameter> parametros = new List<SqlParameter>();
                parametros.Add(new SqlParameter("@Nome", this.Nome));
                parametros.Add(new SqlParameter("@Genero", this.Genero));
                parametros.Add(new SqlParameter("@Documento", this.Documento));
                parametros.Add(new SqlParameter("@DataNascimento", this.DataNascimento));
                if (CodigoCliente > 0)
                {
                    parametros.Add(new SqlParameter("@IdCliente", CodigoCliente));
                }
                DataSet ds = ExecuteDataset(csCrm, "sp_GravarCliente", parametros);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    try
                    {
                        this.Codigo = Convert.ToInt32(ds.Tables[0].Rows[0]["IdCustomer"].ToString());
                        foreach (string telefone in this.Telefones)
                        {
                            GravarTelefone(this.Codigo, telefone);
                        }
                    }
                    catch
                    {
                        string Erro = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                        throw new Exception(Erro);
                    }
                }
                return this;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void GravarTelefone(int codigoCliente, string telefone)
        {
            try
            {
                List<SqlParameter> parametros = new List<SqlParameter>();
                parametros.Add(new SqlParameter("@IdCliente", codigoCliente));
                parametros.Add(new SqlParameter("@Telefone", telefone));
                ExecuteCommand(csCrm, "sp_GravarTelefone", parametros);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}