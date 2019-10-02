using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Crm.Models
{
    public class AtualizacaoOcorrencia
    {
        public int Codigo { get; set; }
        public DateTime DataAtualizacao { get; set; }
        public string TipoAtualizacao { get; set; }
        public string Mensagem { get; set; }
        public int CodigoAgente { get; set; }
        public string NomeAgente { get; set; }
    }
}