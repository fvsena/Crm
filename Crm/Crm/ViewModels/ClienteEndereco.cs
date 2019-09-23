using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Crm.ViewModels
{
    public class ClienteEndereco
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
        public ICollection<string> Telefones { get; set; } = new List<string>();
        public string CEP { get; set; }
        public string Logradouro { get; set; }
        public string Numero { get; set; }
        public string Bairro { get; set; }
        public string Complemento { get; set; }
        public string Cidade { get; set; }
        public string UF { get; set; }
    }
}