using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Model.Cadastros;
using Xamarin.HLP.Mobile.AppPE.Model.RegrasComerciais;

namespace Xamarin.HLP.Mobile.AppPE.Model.Repository
{
    public class RegrasComerciaisRepository
    {
        public static List<RegrasComerciaisModel> GetRegrasComerciais(int idEmpresa)
        {
            var regras = App.Data.Connection.Table<RegrasComerciaisModel>()
                        .Where(c => c.idEmpresa == idEmpresa && !c.bDeletado).ToList();

            foreach (var regra in regras)
            {
                regra.lFaixas = App.Data.Connection.Table<RcFaixasModel>()
                                        .Where(f => f.idRegraComercial == regra.idRegraComercial).ToList();

                foreach (var faixa in regra.lFaixas)
                {
                    faixa.lCriterios = App.Data.Connection.Table<RegrasComerciaisCriteriosModel>()
                                                .Where(c => c.idRegraFaixa == faixa.idRegraFaixa).ToList();

                    foreach (var criterio in faixa.lCriterios)
                    {
                        criterio.lClientes = App.Data.Connection.Table<RccClientesModel>()
                                                    .Where(x => x.idCriterio == criterio.idCriterio).ToList();

                        //criterio.lCategoriasProduto = App.Data.Connection.Table<CriterioCategoriaProdutoModel>()
                        //                                .Where(x => x.idCriterio == criterio.id).ToList();

                        //criterio.lCondicoesPagamento = App.Data.Connection.Table<CriterioCondicaoPagamentoModel>()
                        //                                  .Where(x => x.idCriterio == criterio.id).ToList();

                        //criterio.lProdutos = App.Data.Connection.Table<CriterioProdutoModel>()
                        //                        .Where(x => x.idCriterio == criterio.id).ToList();

                        //criterio.lRamosAtividade = App.Data.Connection.Table<CriterioRamoAtividadeModel>()
                        //                             .Where(x => x.idCriterio == criterio.id).ToList();

                        //criterio.lRepresentadas = App.Data.Connection.Table<CriterioRepresentadaModel>()
                        //                             .Where(x => x.idCriterio == criterio.id).ToList();

                        //criterio.lTabelasPreco = App.Data.Connection.Table<CriterioTabelaPrecoModel>()
                        //                           .Where(x => x.idCriterio == criterio.id).ToList();

                        //criterio.lUfs = App.Data.Connection.Table<CriterioUfModel>()
                        //                   .Where(x => x.idCriterio == criterio.id).ToList();
                    }
                }
            }

            return regras;
        }
    }
}
