using System.Runtime.Serialization;

namespace Cotacao.Infraestrutura.ServicoExterno.Integracao.CotacaoMoeda.Model
{
    [DataContract]
    public class ValorCotacaoMoedaModel
    {
        [DataMember(Name ="cotacaoCompra")]
        public string CotacaoCompra { get; set; }
    }
}
