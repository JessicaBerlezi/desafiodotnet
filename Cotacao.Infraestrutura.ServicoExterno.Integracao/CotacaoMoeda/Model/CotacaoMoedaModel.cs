using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Cotacao.Infraestrutura.ServicoExterno.Integracao.CotacaoMoeda.Model
{
    [DataContract]
    public class CotacaoMoedaModel
    {
        [DataMember(Name ="value")]
        public ICollection<ValorCotacaoMoedaModel> Value { get; set; }
    }
}
