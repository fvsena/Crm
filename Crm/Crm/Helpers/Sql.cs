using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Crm.Helpers
{
    public abstract class Sql
    {
        protected readonly string csCrm = ConfigurationManager.ConnectionStrings["CRM"].ToString();

        #region Métodos protegidos
        /// <summary>
        /// Executa uma stored procedure sem parâmetros
        /// </summary>
        /// <param name="connectionString">String de conexão</param>
        /// <param name="sql">Comando em SQL</param>
        /// <returns>Quantidade de linhas afetadas</returns>
        protected int ExecuteCommand(string connectionString, string sql)
        {
            try
            {
                int linesAffecteds;
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.CommandTimeout = 360000;
                        cmd.CommandType = CommandType.StoredProcedure;
                        linesAffecteds = cmd.ExecuteNonQuery();
                    }
                    conn.Close();
                    conn.Dispose();
                }
                return linesAffecteds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Executa uma stored procedure com parâmetros
        /// </summary>
        /// <param name="connectionString">String de conexão</param>
        /// <param name="sql">Comando em SQL</param>
        /// <param name="parameters">Coleção de parâmetros da procedure</param>
        /// <returns>Quantidade de linhas afetadas</returns>
        protected int ExecuteCommand(string connectionString, string sql, List<SqlParameter> parameters)
        {
            try
            {
                int linesAffecteds;
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.CommandTimeout = 360000;
                        cmd.CommandType = CommandType.StoredProcedure;

                        foreach (SqlParameter parameter in parameters)
                        {
                            cmd.Parameters.Add(parameter);
                        }

                        linesAffecteds = cmd.ExecuteNonQuery();
                    }
                    conn.Close();
                    conn.Dispose();
                }
                return linesAffecteds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Executa uma stored procedure sem parâmetros
        /// </summary>
        /// <param name="connectionString">String de conexão</param>
        /// <param name="sql">Comando em SQL</param>
        /// <returns>Dataset com resultado da consulta</returns>
        protected DataSet ExecuteDataset(string connectionString, string sql)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.CommandTimeout = 360000;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(ds);
                            da.Dispose();
                        }
                    }
                    conn.Close();
                    conn.Dispose();
                }
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Executa uma stored procedure com parâmetros
        /// </summary>
        /// <param name="connectionString">String de conexão</param>
        /// <param name="sql">Comando em SQL</param>
        /// <param name="parameters">Coleção de parâmetros da procedure</param>
        /// <returns>Dataset com o resultado da consulta</returns>
        protected DataSet ExecuteDataset(string connectionString, string sql, List<SqlParameter> parameters)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.CommandTimeout = 360000;
                        cmd.CommandType = CommandType.StoredProcedure;

                        foreach (SqlParameter parameter in parameters)
                        {
                            cmd.Parameters.Add(parameter);
                        }

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(ds);
                            da.Dispose();
                        }
                    }
                    conn.Close();
                    conn.Dispose();
                }
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}