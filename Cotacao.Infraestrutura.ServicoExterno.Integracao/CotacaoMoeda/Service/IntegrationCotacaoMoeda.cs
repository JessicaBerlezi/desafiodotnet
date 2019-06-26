using Cotacao.Infraestrutura.ServicoExterno.Integracao.CotacaoMoeda.Contract;
using Cotacao.Infraestrutura.ServicoExterno.Integracao.CotacaoMoeda.Model;
using System;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Cotacao.Infraestrutura.ServicoExterno.Integracao.CotacaoMoeda.Service
{
    public class IntegrationCotacaoMoeda : IIntegrationCotacaoMoeda
    {
        #region Variáveis e Objetos

        private readonly string caminhoWebApiCotacao = ConfigurationSettings.AppSettings["UrlApiCotacaoMoeda"];
        private HttpClient client;
        private HttpResponseMessage response;

        #endregion

        public IntegrationCotacaoMoeda()
        {
            client = new HttpClient();            
            client.BaseAddress = new Uri(caminhoWebApiCotacao);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));            
        }

        /// <summary>
        /// Método que obtém a cotação de uma determinada moeda em tempo real
        /// </summary>
        /// <param name="siglaMoeda">Sigla da moeda a ser obtida a cotação (ex.: USD (Dólar), EUR (Euro))</param>
        /// <param name="mensagemRetorno">Mensagem de retorno. Vazia em caso de sucesso e preenchida em caso de exceção</param>
        /// <returns>Cotação da moeda</returns>
        public decimal ObterCotacaoTempoReal(string siglaMoeda, out string mensagemRetorno)
        {
            CotacaoMoedaModel cotacaoMoedaModel = null;
            decimal cotacaoMoeda = 0;
            mensagemRetorno = String.Empty;

            try
            {
                //Verifico se o caominho da api existe
                if (String.IsNullOrEmpty(caminhoWebApiCotacao))
                {
                    mensagemRetorno = "Caminho da API de Cotação de Moeda não encontrado.";
                }

                else
                {
                    //Obtenho a resposta da cotação
                    response = client.GetAsync(String.Format("CotacaoMoedaDia(moeda=@moeda,dataCotacao=@dataCotacao)?@moeda='{0}'&@dataCotacao='{1}'&$top=1&$format=json&$select=cotacaoCompra", siglaMoeda, DateTime.Now.ToString("MM-dd-yyyy")))
                                     .GetAwaiter().GetResult();

                    //Sucesso
                    if (response.IsSuccessStatusCode)
                    {
                        cotacaoMoedaModel = response.Content.ReadAsAsync<CotacaoMoedaModel>().GetAwaiter().GetResult();
                        cotacaoMoeda = Convert.ToDecimal(cotacaoMoedaModel.Value.FirstOrDefault().CotacaoCompra.Replace(".", ","));
                    }

                    //Erro no consumo da api
                    else
                    {
                        mensagemRetorno = "Erro ao consultar a API de Conversão de Moeda.";
                    }
                }
            }

            catch(Exception ex)
            {
                mensagemRetorno = ex.ToString();
            }

            return cotacaoMoeda;
        }
    }
}
