using Cotacao.Infraestrutura.ServicoExterno.Integracao.CotacaoMoeda.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conversor.Infraestrutura.Utilidades
{    
    public class ConversorMoeda
    {
        private readonly IIntegrationCotacaoMoeda _integrationCotacaoMoeda;

        public ConversorMoeda(IIntegrationCotacaoMoeda integrationCotacaoMoeda)
        {
            _integrationCotacaoMoeda = integrationCotacaoMoeda;
        }

        /// <summary>
        /// Método usado para converter determinada quantidade de moeda em outra
        /// </summary>
        /// <param name="siglaMoedaOrigem">Sigla da Moeda de Origem (ex.: USD, EUR)</param>
        /// <param name="siglaMoedaDestino">Sigla da Moeda de Destino (ex.: USD, EUR)</param>
        /// <param name="quantidade">Quantidade da Moeda a ser convertida</param>
        /// <param name="cotacaoUtilizada">Cotação utilizada na conversa</param>
        /// <param name="mensagemRetorno">Mensagem de retorno. Vazia em caso de sucesso</param>
        /// <returns>Valor convertido</returns>
        public decimal Converter(string siglaMoedaOrigem, string siglaMoedaDestino, int quantidade, out decimal cotacaoUtilizada, out string mensagemRetorno)
        {
            decimal valorConvertido = 0;
            mensagemRetorno = String.Empty;
            cotacaoUtilizada = 0;

            #region Validações            

            if (String.IsNullOrEmpty(siglaMoedaOrigem) || String.IsNullOrEmpty(siglaMoedaDestino))
            {
                mensagemRetorno = "Moeda origem e/ou destino não informada.";
                return valorConvertido;
            }

            siglaMoedaDestino = siglaMoedaDestino.ToUpper();
            siglaMoedaOrigem = siglaMoedaOrigem.ToUpper();

            if(siglaMoedaOrigem == siglaMoedaDestino)
            {
                mensagemRetorno = "A moeda origem e destino não podem ser iguais.";
                return valorConvertido;
            }

            if((siglaMoedaOrigem != "BRL" && siglaMoedaOrigem != "USD" && siglaMoedaOrigem != "EUR") ||
               (siglaMoedaDestino != "BRL" && siglaMoedaDestino != "USD" && siglaMoedaDestino != "EUR"))
            {
                mensagemRetorno = "Apenas as seguintes moedas são permitidas: REAL (BRL), DÓLAR (USD), EURO (EUR).";
            }

            if(quantidade <= 0)
            {
                mensagemRetorno = "A quantidade da moeda a ser convertida deve ser maior que 0.";
                return valorConvertido;
            }

            #endregion

            //Converter Euro para Dólar e vice-e-versa
            if ((siglaMoedaOrigem == "EUR" && siglaMoedaDestino == "USD") || (siglaMoedaOrigem == "USD" && siglaMoedaDestino == "EUR"))
            {
                decimal cotacaoDolar = 0;
                decimal cotacaoEuro = 0;

                cotacaoDolar = _integrationCotacaoMoeda.ObterCotacaoTempoReal("USD", out mensagemRetorno);
                cotacaoEuro = _integrationCotacaoMoeda.ObterCotacaoTempoReal("EUR", out mensagemRetorno);

                //Se a origem for dólar
                if (siglaMoedaOrigem == "USD")
                {
                    cotacaoUtilizada = cotacaoDolar;
                    valorConvertido = (cotacaoDolar / cotacaoEuro) * quantidade;
                }

                //Se for euro
                else
                {
                    cotacaoUtilizada = cotacaoEuro;
                    valorConvertido = (cotacaoEuro / cotacaoDolar) * quantidade;
                }
            }

            else
            {
                //Se a moeda destino for REAL, uso a moeda origem para descobrir a cotação
                decimal cotacaoMoedaOrigem = _integrationCotacaoMoeda.ObterCotacaoTempoReal(siglaMoedaDestino == "BRL" ? siglaMoedaOrigem : siglaMoedaDestino, out mensagemRetorno);

                if (!String.IsNullOrEmpty(mensagemRetorno))
                {
                    return valorConvertido;
                }

                cotacaoUtilizada = cotacaoMoedaOrigem;

                //Se a moeda destino for REAL
                if (siglaMoedaDestino == "BRL")
                {
                    valorConvertido = quantidade * cotacaoMoedaOrigem;
                }

                //Caso não seja
                else
                {
                    valorConvertido = quantidade / cotacaoMoedaOrigem;
                }
            }

            return Math.Round(valorConvertido, 2);
        }
    }
}
