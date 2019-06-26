using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Cotacao.Apresentacao.WebApi.Models
{
    [DataContract]
    public class MoedaConvertidaModel
    {
        [DataMember(Name ="cotacaoUtlizada")]
        public decimal CotacaoUtlizada { get; set; }

        [DataMember(Name ="valorConvertido")]
        public decimal ValorConvertido { get; set; }
    }
}