namespace Cotacao.Infraestrutura.ServicoExterno.Integracao.CotacaoMoeda.Contract
{
    /// <summary>
    /// Interface Cotacao Moeda
    /// </summary>
    public interface IIntegrationCotacaoMoeda
    {
        /// <summary>
        /// Método que obtém a cotação de uma determinada moeda em tempo real
        /// </summary>
        /// <param name="siglaMoeda">Sigla da moeda a ser obtida a cotação (ex.: USD (Dólar), EUR (Euro))</param>
        /// <param name="mensagemRetorno">Mensagem de retorno. Vazia em caso de sucesso e preenchida em caso de exceção</param>
        /// <returns>Cotação da moeda</returns>
        decimal ObterCotacaoTempoReal(string siglaMoeda, out string mensagemRetorno);
    }
}
