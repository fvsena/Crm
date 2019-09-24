using Crm.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Crm.Models
{
    public class Usuario : Sql
    {
        public string Nome { get; set; }
        public string Login { get; set; }
        public string Senha { get; set; }
        public int Codigo { get; set; }

        public bool ValidarLogin()
        {
            bool valido = false;
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(new SqlParameter("@Login", this.Login));
            parametros.Add(new SqlParameter("@Senha", this.Senha));

            DataSet ds = ExecuteDataset(csCrm, "sp_ValidarLogin", parametros);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                try
                {
                    this.Codigo = Convert.ToInt32(ds.Tables[0].Rows[0]["IdAgent"].ToString());
                    valido = true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return valido;
        }
    }
}