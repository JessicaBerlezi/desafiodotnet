using Conversor.Infraestrutura.Utilidades;
using Cotacao.Apresentacao.WebApi.Models;
using Cotacao.Infraestrutura.ServicoExterno.Integracao.CotacaoMoeda.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Cotacao.Apresentacao.WebApi.Controllers
{

    [RoutePrefix("api/v1")]
    public class ConversaoMoedaController : ApiController
    {
        private readonly ConversorMoeda conversorMoeda;

        public ConversaoMoedaController(IIntegrationCotacaoMoeda integrationCotacaoMoeda)
        {
            conversorMoeda = new ConversorMoeda(integrationCotacaoMoeda);
        }

        /// <summary>
        /// Método usado para converter determinada quantidade de moeda em outra
        /// </summary>
        /// <param name="de">Sigla da Moeda de Origem (ex.: USD, EUR)</param>
        /// <param name="para">Sigla da Moeda de Destino (ex.: USD, EUR)</param>
        /// <param name="quantidade">Quantidade da Moeda a ser convertida</param>
        [HttpGet]
        [Route("converter")]
        public IHttpActionResult Get(string de, string para, int quantidade)
        {
            decimal cotacaoUtilizada = 0;
            string mensagemRetorno = String.Empty;

            decimal valorConvertido = conversorMoeda.Converter(de, para, quantidade, out cotacaoUtilizada, out mensagemRetorno);

            if (String.IsNullOrEmpty(mensagemRetorno))
            {
                MoedaConvertidaModel moedaConvertidaModel = new MoedaConvertidaModel()
                {
                    CotacaoUtlizada = cotacaoUtilizada,
                    ValorConvertido = valorConvertido
                };

                return Ok(moedaConvertidaModel);
            }

            else
            {
                return BadRequest(mensagemRetorno);
            }
        }        
    }
}
