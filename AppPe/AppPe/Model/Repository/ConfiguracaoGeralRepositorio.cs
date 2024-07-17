using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Model.Cadastros;
using Xamarin.HLP.Mobile.AppPE.Model.Lancamento;

namespace Xamarin.HLP.Mobile.AppPE.Model.Repository
{
    public class ConfiguracaoGeralRepositorio
    {


        public static ConfiguracaoGeralModel GetConfiguracaoEmpresa()
        {
            ConfiguracaoGeralModel _configsGerais = new ConfiguracaoGeralModel();

            var xQuery = string.Empty;
            const string xFields =
                "idConfiguracaoGeral, " + 
                "bUtilizaLimiteMinimoVendas, " +
                "stCadastroLimiteVendasEmpresa,  " +
                "bBloquearVisualizacaoEstoqueVendedor,  " +
                "stCalculoLimiteVendasEmpresa, " +
                "dValorLimiteMinimo, " +
                "xInformacaoContrato, " +
                "bBloqueiaValorProdutoApp," +           
                "idEmpresa, " +
                "bMostraFaixaTabelaEscalonada";

            if (App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.stAdministrador)
            {
                xQuery =
                    $"select {xFields} from {TableMobile.TB_CONFIGURACOES_GERAIS} where idEmpresa = {App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa}";
            }
            else
            {
                xQuery =
                    $"select {xFields}, bForcarMinimoVendas from {TableMobile.TB_CONFIGURACOES_GERAIS} where idEmpresa = {App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa}";
            }

            var result = App.Data.Connection.Query<ConfiguracaoGeralModel>(xQuery).FirstOrDefault();

            if(result != null)
            {
                _configsGerais.idConfiguracaoGeral = result.idConfiguracaoGeral;
                _configsGerais.bForcarMinimoVendas = result.bForcarMinimoVendas;
                _configsGerais.bUtilizaLimiteMinimoVendas = result.bUtilizaLimiteMinimoVendas;
                _configsGerais.dValorLimiteMinimo = result.dValorLimiteMinimo;
                _configsGerais.stCadastroLimiteVendasEmpresa = result.stCadastroLimiteVendasEmpresa;
                _configsGerais.stCalculoLimiteVendasEmpresa = result.stCalculoLimiteVendasEmpresa;
                _configsGerais.bBloquearVisualizacaoEstoqueVendedor = result.bBloquearVisualizacaoEstoqueVendedor;
                _configsGerais.bMostraFaixaTabelaEscalonada = result.bMostraFaixaTabelaEscalonada;
                _configsGerais.idEmpresa = result.idEmpresa;
                _configsGerais.xInformacaoContrato = result.xInformacaoContrato;
                _configsGerais.bBloqueiaValorProdutoApp = result.bBloqueiaValorProdutoApp;
            }
             

            return _configsGerais;
        }


        #region Métodos Auxiliares

        public static bool GetExibeNF(int idEmpresa)
        {
            return App.Data.Connection.Table<ConfiguracaoGeralModel>()
                    .Where(c => c.idEmpresa == idEmpresa)
                    .Select(c => c.bEnviarInfoAdicionaisParaNf).FirstOrDefault();
        }

        public static double ObterValorLimiteVendasProduto(int? idProduto)
        {
            var xQuery = $"select vLimiteMinimoVenda from {TableMobile.TB_PRODUTO} where idProduto = {idProduto} and idEmpresa = {App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa}";
            var result = App.Data.Connection.Query<ProdutoModel>(xQuery).FirstOrDefault();
            var _limiteMinimo = result.vLimiteMinimoVenda;


            return _limiteMinimo.GetValueOrDefault();
        }

        public static string InformacoesBasicasProduto(int? idProduto)
        {
            var xQuery = $"select xNome from {TableMobile.TB_PRODUTO} where idProduto = {idProduto} and idEmpresa = {App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa}";
            var result = App.Data.Connection.Query<ProdutoModel>(xQuery).FirstOrDefault();
            var xNomeProduto = result.xNome;


            return xNomeProduto;
        }

        public static bool GetMelhoriaEspecificaCondicoesPagamento(int idEmpresa)
        {
            return App.Data.Connection.Table<ConfiguracaoEspecificaModel>()
                    .Where(c => c.idEmpresa == idEmpresa)
                    .Select(c => c.bAplicaMelhoriaRosaMaria).FirstOrDefault();
        }

        public static bool? GetMelhoriaEspecificaTl(int idEmpresa)
        {
            return App.Data.Connection.Table<ConfiguracaoEspecificaModel>()
                    .Where(c => c.idEmpresa == idEmpresa)
                    .Select(c => c.bAplicaMelhoriaTl).FirstOrDefault();
        }

        public static bool? GetMelhoriaEspecificaRepresentacaoPdf(int idEmpresa)
        {
            return App.Data.Connection.Table<ConfiguracaoEspecificaModel>()
                    .Where(c => c.idEmpresa == idEmpresa)
                    .Select(c => c.bAplicaMelhoriaEscolherRepresentacaoPdf).FirstOrDefault();
        }

        public static bool? GetMelhoriaEspecificaEmailsTelefoneTl(int idEmpresa)
        {
            return App.Data.Connection.Table<ConfiguracaoEspecificaModel>()
                    .Where(c => c.idEmpresa == idEmpresa)
                    .Select(c => c.bAplicaMelhoriaEmailsTelefoneTl).FirstOrDefault();
        }


        public static bool GetMelhoriaEspecificaReceitaBloqueio(int idEmpresa)
        {
            return App.Data.Connection.Table<ConfiguracaoEspecificaModel>()
                    .Where(c => c.idEmpresa == idEmpresa)
                    .Select(c => c.bAplicaMelhoriaBloqueiaEnderecoReceita).FirstOrDefault();
        }      

        public static double GetValorMinimoVendasTabelaPreco(int? idTabelaPreco)
        {
            var xQuery = $"select vLimiteMinimoVenda from {TableMobile.TB_TABELAPRECO} where idTabelaPreco = {idTabelaPreco} and idEmpresa = {App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa}";
            var result = App.Data.Connection.Query<TabelaPrecoModel>(xQuery).FirstOrDefault();
            var vLimite = result.vLimiteMinimoVenda;


            return vLimite;
        }

        public static string ObterInformacoesTabelaPreco(int? idTabelaPreco)
        {
            var xQuery = $"select xNome from {TableMobile.TB_TABELAPRECO} where idTabelaPreco = {idTabelaPreco} and idEmpresa = {App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa}";
            var result = App.Data.Connection.Query<TabelaPrecoModel>(xQuery).FirstOrDefault();
            var xNomeTabelaPreco = result.xNome;


            return xNomeTabelaPreco;
        }


        public static CondicaoPagamentoModel ObterCondicaoPagamento(int? idCondicaoPagamento)
        {
            var xQuery = $"select * from {TableMobile.TB_CONDICAOPAGAMENTO} where idCondicaoPagamento = {idCondicaoPagamento} and idEmpresa = {App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa}";
            var result = App.Data.Connection.Query<CondicaoPagamentoModel>(xQuery).FirstOrDefault();

            return result;
        }


        public static int ObterIdStatusAberto()
        {
            var xQuery = $"select idStatus from {TableMobile.TB_STATUS} where idEmpresa = {App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa} and stVenda = 0";            
            var _retorno = App.Data.Connection.Query<StatusModel>(xQuery).FirstOrDefault();
            

            var result = _retorno.idStatus;

            return result;
        }

        #endregion


        public static async Task<ValidacaoConfiguracaoMinimoVendas> ValidaConfiguracoesGerais(PedidoVendaModel modelPedido, ListItemModel ItemCondicaoPagamento)
        {
            string _xmsgRetorno = string.Empty;
            bool validaConfig = true;
            var _vLimiteMinimo = modelPedido.vLimiteMinimoVenda;
            var _pedidoItens = modelPedido.lItens;

            switch (modelPedido.stCadastroMinimoVenda)
            {
                case 1:
                    if (modelPedido.stCalculoMinimoVenda == 1)
                    {
                        validaConfig = _pedidoItens.Sum(p => p.vQtdItem) >= _vLimiteMinimo;
                        _xmsgRetorno = $"Este pedido não possui a quantidade mínima de {Extensions.ToCurrencyStringSimplesPtBr(_vLimiteMinimo)} itens";
                    }
                    else
                    {
                        validaConfig = _pedidoItens.Sum(p => p.vSubTotal) >= _vLimiteMinimo;
                        _xmsgRetorno = $"Este pedido não possui o valor total mínimo de {Extensions.ToCurrencyStringPtBr(_vLimiteMinimo)}";
                    }

                    break;
                case 2:
                    foreach (var item in _pedidoItens.GroupBy(p => p.idProduto))
                    {
                        var _vLimiteMinimoProdutoAux = ObterValorLimiteVendasProduto(idProduto: item.Select(i => i.idProduto).FirstOrDefault());

                        if(_vLimiteMinimoProdutoAux > 0)
                        {
                            var infoProduto = InformacoesBasicasProduto(idProduto: item.Select(p => p.idProduto).FirstOrDefault());
                            if (modelPedido.stCalculoMinimoVenda == 1)
                            {
                                double _qtd = 0;
                                var _itensGrade = item.Select(t => t.ItensGrade);

                                if(_itensGrade?.Where(t => t != null).Count() > 0)
                                {
                                    foreach (var grades in _itensGrade)
                                    {
                                        _qtd += grades.Sum(t => t.vQtdItem);
                                    }
                                }
                                else
                                {
                                    _qtd += item.Sum(t => t.vQtdItem);
                                }
                              

                                validaConfig = _qtd >= _vLimiteMinimoProdutoAux;
                                _xmsgRetorno = $"O item {infoProduto} não possui a quantidade mínima de {Extensions.ToCurrencyStringSimplesPtBr(_vLimiteMinimoProdutoAux)} itens"; 
                            }
                            else
                            {
                                double _vsubtotal = 0;
                                var _itensGrade = item.Select(t => t.ItensGrade);
                                foreach (var grades in _itensGrade)
                                {
                                    _vsubtotal += grades.Sum(t => t.vSubTotal);
                                }

                                validaConfig = _vsubtotal >= _vLimiteMinimoProdutoAux;
                                _xmsgRetorno = $"O item {infoProduto} não possui o valor total mínimo de {Extensions.ToCurrencyStringPtBr(_vLimiteMinimoProdutoAux)}";
                            }
                        }
                    }
                    break;
                case 3:                                       
                    if (modelPedido.stCalculoMinimoVenda == 1)
                    {
                        validaConfig = _pedidoItens.Sum(p => p.vQtdItem) >= modelPedido.vLimiteMinimoVendaCliente;
                        _xmsgRetorno = $"Este cliente não possui a quantidade mínima de {Extensions.ToCurrencyStringSimplesPtBr(modelPedido.vLimiteMinimoVendaCliente)} itens";
                    }
                    else
                    {
                        validaConfig = _pedidoItens.Sum(p => p.vSubTotal) >= modelPedido.vLimiteMinimoVendaCliente;
                        _xmsgRetorno = $"Este cliente não possui um valor total mínimo de {Extensions.ToCurrencyStringPtBr(modelPedido.vLimiteMinimoVendaCliente)}";
                    }
                    break;
                case 4:                    
                    var _itensPorTabelaPreco = _pedidoItens.GroupBy(p => p.idTabelaPreco);


                    foreach (var item in _itensPorTabelaPreco)
                    {
                        var _idTabelaPrecoAux = item.Select(p => p.idTabelaPreco).FirstOrDefault();
                        var _valorLimiteTabela = GetValorMinimoVendasTabelaPreco(_idTabelaPrecoAux);
                        var _xTabelaNome = ObterInformacoesTabelaPreco(_idTabelaPrecoAux);

                        if (modelPedido.stCalculoMinimoVenda == 1)
                        {
                            double qtdadeItensComGrade = 0;

                            var itensComGrade = item
                                .Where(p => p.ItensGrade?.Count() > 0 && p.HasGrade == true || p.HasGradeCor == true)
                                .Select(i => i.ItensGrade);

                            if(itensComGrade?.Count() > 0)
                            {
                                foreach(var gradeItem in itensComGrade)
                                {
                                    qtdadeItensComGrade += gradeItem.Sum(p => p.vQtdItem);
                                }
                            }
                            
                            validaConfig =  (item.Sum(p => p.vQtdItem) + qtdadeItensComGrade) >= _valorLimiteTabela;
                            _xmsgRetorno = $"A Tabela de preço {_xTabelaNome} utilizada não possui uma quantidade mínima de {Extensions.ToCurrencyStringSimplesPtBr(_valorLimiteTabela)} itens";
                        }
                        else
                        {
                            validaConfig = item.Sum(p => p.vSubTotal) >= _valorLimiteTabela;
                            _xmsgRetorno = $"A Tabela de preço {_xTabelaNome} utilizada não possui um valor total mínimo de {Extensions.ToCurrencyStringPtBr(_valorLimiteTabela)}";
                        }

                        if (!validaConfig) break;
                    }
                    break;
                case 5:

                    var _retornoCondicao = ObterCondicaoPagamento(idCondicaoPagamento: ItemCondicaoPagamento.IdOnline);

                    int nParcelas;
                    if (_retornoCondicao.nParcelas > 0)
                    {
                        nParcelas = _retornoCondicao.nParcelas.GetValueOrDefault();
                    }
                    else
                    {
                        nParcelas = _retornoCondicao.xFormula.Split('/').Count();
                    }

                    validaConfig = (_pedidoItens.Sum(p => p.vSubTotal) / nParcelas) >= modelPedido.vLimiteMinimoVenda;
                    _xmsgRetorno = $"Cada duplicata deste pedido deve possuir um valor total mínimo de " +
                        $"{Extensions.ToCurrencyStringPtBr(_vLimiteMinimo)}";
                    break;
            }


            return new ValidacaoConfiguracaoMinimoVendas
            {
                bValidado = validaConfig,
                xMensagemValidacao = _xmsgRetorno
            };
        }

    }
}
