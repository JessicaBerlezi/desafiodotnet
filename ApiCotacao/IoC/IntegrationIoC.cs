using Cotacao.Infraestrutura.ServicoExterno.Integracao.CotacaoMoeda.Contract;
using Cotacao.Infraestrutura.ServicoExterno.Integracao.CotacaoMoeda.Service;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cotacao.Apresentacao.WebApi.IoC
{
    public class IntegrationIoC
    {
        public static void RegisterServices(IKernel kernel)
        {
            //Mapeamento da camada de Integracao
            kernel.Bind<IIntegrationCotacaoMoeda>().To<IntegrationCotacaoMoeda>();
        }
    }
}