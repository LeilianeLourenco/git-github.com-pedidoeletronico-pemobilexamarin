using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hlp.PedidoEletronico.Domain.Business.Enums;
using Hlp.PedidoEletronico.Domain.Business.Helpers;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Model.Cadastros;
using Xamarin.HLP.Mobile.AppPE.Model.Lancamento;
using Xamarin.HLP.Mobile.AppPE.Model.TabelaPreco;
using Xamarin.HLP.Mobile.AppPE.Model.Repository.Precos.Interfaces;
using Xamarin.HLP.Mobile.AppPE.Model.Repository.Precos.Implementacoes;
using Xamarin.HLP.Mobile.AppPE.Model.Estoque;
using System.Collections.ObjectModel;
using System.Globalization;
using Hlp.PedidoEletronico.Domain.Business.Calculos;

namespace Xamarin.HLP.Mobile.AppPE.Model.Repository
{
    public class ProdutoRepository
    {

        public static string GetAnotacaoProduto(int idProdutoOffLine)
        {
            if (idProdutoOffLine == 0) return "";
            var xQuery = $@"Select idProdutoOffLine, bExibirAnotacaoNoPedido, xAnotacao from {TableMobile.TB_PRODUTO} 
                                            where idProdutoOffLine = {idProdutoOffLine}";


            var resultado = App.Data.Connection.Query<ProdutoModel>(xQuery);

            var registro = resultado.FirstOrDefault();

            if ((registro.bExibirAnotacaoNoPedido ?? false))
            {
                return registro.xAnotacao ?? "";
            }

            return "";
        }

        public static List<DisplayListaModel> GetAllListasPrecoByProduto(int idProduto, List<BasicPickerModel> listas)
        {
            try
            {
                var lRetorno = new List<DisplayListaModel>();
                //var listas = TabelaPrecoRepository.GetListToBasicPickerModel(true);
                foreach (var item in listas)
                {
                    var produtos = GetAllProdutosByListaPreco(item.Id, true, idProduto);
                    lRetorno.AddRange(produtos);
                }
                return lRetorno.OrderBy(c => c.vVenda).ToList();
            }
            catch (Exception ex)
            {
                ex.TrakException();
                return new List<DisplayListaModel>();
            }
        }

        public static List<DisplayListaModel> GetAllProdutosByListaPreco(int idTabelaPreco, bool addNameTabela = false,
            int? idProduto = null)
        {
            try
            {
                ITabelaPrecoManItensRepos _tblManItensRepos = new PrecosPorTabelaPrecoRepos(addNameTabela: addNameTabela, idProduto: idProduto);
                return _tblManItensRepos.ObterProdutosTabela(idTabelaPreco: idTabelaPreco);

                //var objTabelaPrecoModel = TabelaPrecoRepository.GetRetistro(idTabelaPreco);


                //if(objTabelaPrecoModel.stTabelaPreco != 2)
                //{

                //}
                //var xWhere =
                //    $"stAtivo = 1 and idEmpresa = {App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa}";
                //if (!App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.stAdministrador)
                //{
                //    var lRepresentadasAspnetUsers =
                //        App.Data.Connection.Table<RepresentadaAspnetUsersModel>()
                //            .Where(
                //                c =>
                //                    c.idEmpresa_aspnetUsers ==
                //                    App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa_aspnetUsers)
                //            .ToList();
                //    var inIdRepresentada =
                //        lRepresentadasAspnetUsers.Select(c => c.idRepresentada.ToString())
                //            .ToList()
                //            .Aggregate("", (current, item) => current + ((current == "" ? "" : " , ") + item));
                //    xWhere += $" and idRepresentada in ({inIdRepresentada}) ";
                //}

                //if (idProduto != null && idProduto > 0)
                //{
                //    xWhere += $" and idProduto = {idProduto}";
                //}
                //var xQuery =
                //    $@"SELECT 
                //            idProduto, 
                //            idProdutoOffLine, pIpiVenda, pStVenda, 
                //            (coalesce(tb_produto.cAlternativo,'') || ' - ' || xNome) xDisplay,
                //            vVenda from {TableMobile.TB_PRODUTO} where {xWhere} order by xNome";

                //var lreturn = App.Data.Connection.Query<DisplayListaModel>(xQuery);
                //foreach (var item in lreturn)
                //{
                //    item.vVenda = TabelaPrecoRepository.GetValorProdutoTabelaPreco(tbl: objTabelaPrecoModel,
                //        idProduto: item.idProduto ?? 0,
                //        vVenda: item.vVenda,
                //        pIpiVenda: item.pIpiVenda ?? 0,
                //        pStVenda: item.pStVenda ?? 0, embuteImpostos: true);

                //    if (addNameTabela)
                //    {
                //        item.xDisplay =
                //            $"{(objTabelaPrecoModel.stTabelaPreco == 0 ? "(T) " : "(C) ")} {objTabelaPrecoModel.xNome} - {item.vVenda.ToCurrencyStringPtBr()}";
                //    }
                //    else
                //    {
                //        item.xDisplay = $"{item.vVenda.ToCurrencyStringPtBr()} - {item.xDisplay}";
                //    }
                //}
                //return lreturn;

            }
            catch (Exception ex)
            {
                ex.TrakException();
                return new List<DisplayListaModel>();
            }
        }

        public static List<PedidoVendaItensModel> Get(int skip, int take, string xFiltro,
            ConfiguracaoPesquisaProdutoModel config, int idClientesOffLine, int? idClientes, int _idRepresentante,
            List<int> idProdutos = null, bool? bUsaLocaisEstoque = null, bool? bMostraVariacoes = null, bool? bBotaoFiltroUltimasCompras = false, bool? bBotaoFiltroDestques = false)
        {
            try
            {
                var idEmpresa = App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa;
                var xWhere =
                    $" tb_produto.idEmpresa = {idEmpresa} and tb_produto.stAtivo = '1' ";

                if (!string.IsNullOrEmpty(xFiltro))
                {
                    xWhere += $@" and (UPPER(tb_produto.xNome) like('%{xFiltro.ToUpper()}%') 
                                  or UPPER(tb_produto.cAlternativo) like('%{xFiltro.ToUpper()}%') 
                                  or UPPER(tb_produto.cEan) like('%{xFiltro.ToUpper()}%')
                                  or UPPER(tb_produto.cEanEmb) like('%{xFiltro.ToUpper()}%')) ";
                }


                //se for mostrar eu não coloco o where
                if (!bMostraVariacoes.GetValueOrDefault())
                    xWhere += $@" and tb_produto.idProdutoPai is null ";

                if (config.paramCategoria != null && config.paramCategoria.Id > 0)
                {
                    xWhere += $" and tb_produto.idCategoria = {config.paramCategoria.Id} ";
                }

                if (config.paramRepresentacao != null && config.paramRepresentacao.Id > 0)
                {
                    xWhere += $" and tb_produto.idRepresentada = {config.paramRepresentacao.Id} ";
                }
                else
                {
                    var _usu = EmpresaAspnetUsersRepository.GetEmpresaAspnetUsers(idEmpresa_aspnetUsers: _idRepresentante);

                    if (_usu != null && !_usu.stAdministrador)
                    {
                        var xQueryRepresentacoes = $@"SELECT * FROM {TableMobile.TB_REPRESENTADA_ASPNETUSERS} WHERE
                                                        idEmpresa_aspnetUsers = {_usu.idEmpresa_aspnetUsers}";

                        var lRepresentadasAspnetUsers =
                            App.Data.Connection.Query<RepresentadaAspnetUsersModel>(xQueryRepresentacoes);

                        var inIdRepresentada =
                            lRepresentadasAspnetUsers.Select(c => c.idRepresentada.ToString())
                                .ToList()
                                .Aggregate("", (current, item) => current + ((current == "" ? "" : " , ") + item));
                        xWhere += $" and tb_produto.idRepresentada in ({inIdRepresentada}) ";
                    }
                }


                if (config.bUltimasCompras || bBotaoFiltroUltimasCompras.GetValueOrDefault())
                {
                    xWhere += $" and tb_pedidovenda.idClientesOffLine = {idClientesOffLine} ";
                }

                //fitlro de destques
                if (bBotaoFiltroDestques.GetValueOrDefault())
                    xWhere += $" and tb_produto.bDestaqueCatalogo = '1' ";


                if (idProdutos != null && idProdutos?.Count() > 0)
                {
                    var _inProdutos = idProdutos.Select(pr => pr)
                        .ToList().Aggregate("", (current, item) => current + (current == "" ? "" : " , ") + item);

                    xWhere += $" and tb_produto.idProduto in ({_inProdutos})";
                }

                var xFrom = "";
                var xFields = "";
                if (config.bUltimasCompras || bBotaoFiltroUltimasCompras.GetValueOrDefault())
                {
                    List<int?> idsPedidosCliente = App.Data.Connection.Table<PedidoVendaModel>().Where(p => p.idClientesOffLine == idClientesOffLine).Select(p => p.idPedidoVendaOffLine).ToList();
                    IEnumerable<IGrouping<int, PedidoVendaItensModel>> itens = App.Data.Connection.Table<PedidoVendaItensModel>().Where(p => idsPedidosCliente.Contains(p.idPedidoVendaOffLine)).GroupBy(p => p.idProdutoOffLine);
                    List<int?> lIdsItensPedidoVenda = new List<int?>();

                    foreach (IGrouping<int, PedidoVendaItensModel> item in itens)
                    {
                        lIdsItensPedidoVenda.Add(item.OrderByDescending(p => p.idPedidoVendaItemOffLine).Select(p => p.idPedidoVendaItemOffLine).FirstOrDefault());
                    }

                    if (lIdsItensPedidoVenda?.Count() > 0)
                    {
                        xWhere += $" and tb_pedidovendaitens.idPedidoVendaItemOffLine in ({string.Join(",", lIdsItensPedidoVenda)})";
                    }

                    xFrom = $@" from tb_pedidovenda  
		                            inner join tb_pedidovendaitens 
		                            on tb_pedidovenda.idPedidoVendaOffLine = tb_pedidovendaitens.idPedidoVendaOffLine
                                    inner join tb_produto 
                                    on tb_produto.idProdutoOffLine = tb_pedidovendaitens.idProdutoOffLine ";
                    xFields = $@"tb_pedidovendaitens.vDesconto,
                                 tb_pedidovendaitens.vQtdItem as vQtdUltimaVenda,
                                 tb_produto.vVenda,
                                 tb_pedidovendaitens.vUnitarioVendaComImpostos as vUltimaVenda,";

                }
                else
                {
                    xFrom = $@" from tb_produto ";
                    xFields = $@"tb_produto.vCusto vCusto,
                                 tb_produto.vVenda,";
                }

                var xNomeDisplay = "";
                if (config.Ordenacao == 0)
                {
                    //xNomeDisplay = " tb_produto.xNome xDescricao, ";
                    xNomeDisplay = "(tb_produto.xNome || ' - ' || tb_produto.cAlternativo) xDescricao, ";
                }
                else
                {
                    xNomeDisplay = " (tb_produto.cAlternativo || ' - ' || tb_produto.xNome) xDescricao, ";
                }

                if (config.bUtilizaMinimoVendasProduto)
                {
                    xFields += $"tb_produto.vLimiteMinimoVenda,";
                }


                //mexer em retorno de campo QtdeGrade na query
                var xQuery =
                    $@"select distinct 
                                                                tb_produto.idProduto,
                                                                tb_produto.idProdutoOffLine,                                                                
                                                                tb_produto.idEmpresa, 
                                                                tb_produto.cAlternativo,                                                                
                                                                {xNomeDisplay}         
                                                                {xFields}
                                                                coalesce(tb_produto.pIpiVenda,0)pIpiVenda,
                                                                coalesce(tb_produto.pStVenda,0)pStVenda,
                                                                tb_produto.idRepresentada idRepresentada,
                                                                TB_REPRESENTADA.stComissaoIPI stDescontaIpiComissao,
                                                                TB_REPRESENTADA.stComissaoST stDescontaStComissao,                                                                
                                                                coalesce(tb_unidademedida.nCasasDecimais,0) nCasasDecimais,
                                                                coalesce(TB_UNIDADEMEDIDA.xSigla,'UN') xSigla,    
                                                                tb_produto.xFileImagePrincipal ,                                                                                                     
                                                                (Count(tb_gradetamanho.idGradeTamanho) + Count(tb_gradecor.idGradeCor)) as QtdeGrade
		                                                {xFrom}                                                                         
                                                                        left join tb_gradetamanho 
                                                                                        on tb_produto.idProduto = tb_gradetamanho.idProduto
						                                                left join tb_gradecor 
								                                                        on tb_produto.idProduto = tb_gradecor.idProduto									
                                                                        left join tb_unidademedida
                                                                                        on tb_produto.idUnidadeMedida = tb_unidademedida.idUnidadeMedida
                                                                        left join TB_REPRESENTADA
                                                                                        on tb_produto.idRepresentada = TB_REPRESENTADA.idRepresentada
                                                        where {xWhere}                                                                             
                                                                                group by 
                                                                                        tb_produto.idProduto, 
                                                                                        tb_produto.idProdutoOffLine,
                                                                                        tb_produto.idEmpresa, 
                                                                                        tb_produto.idUnidadeMedida, 
                                                                                        tb_produto.xNome, 
                                                                                        tb_produto.idRepresentada,
                                                                                        tb_produto.xFileImagePrincipal,
																						tb_unidademedida.nCasasDecimais";



                if (config.Ordenacao == 0)
                {
                    xQuery += $" order by tb_produto.xNome  LIMIT {take} OFFSET {skip}";
                }
                else
                {
                    xQuery += $" order by tb_produto.cAlternativo  LIMIT {take} OFFSET {skip}";
                }



                var objReturn = App.Data.Connection.Query<PedidoVendaItensModel>(xQuery);
                if (objReturn == null) return null;


                Dictionary<int, string> _dicLocais = new Dictionary<int, string>();
                List<LocalEstoqueSimplificado> lLocaisSimplificado = new List<LocalEstoqueSimplificado>();
                if (bUsaLocaisEstoque.GetValueOrDefault())
                {
                    _dicLocais = PedidoRepository.BuscarLocaisEstoqueParaListas(idClientes, _idRepresentante, idEmpresa);

                    foreach (var local in _dicLocais)
                    {
                        lLocaisSimplificado.Add(new LocalEstoqueSimplificado
                        {
                            idEmpresa = idEmpresa,
                            idLocalEstoque = local.Key,
                            xNomeLocal = local.Value
                        });
                    }
                }

                //Foreach criado para ajustar a brecha do campo QtdeGrade que ficava populado mesmo a grade estando inativa, ocasionando erro na listagem dos produtos
                foreach (var lproduto in objReturn)
                {
                    int? idLocalEstoque = _dicLocais.OrderBy(t => t.Value).Select(t => t.Key).FirstOrDefault();
                    lproduto.idLocalEstoque = idLocalEstoque.GetValueOrDefault();
                    lproduto.lLocaisEstoque = lLocaisSimplificado;
                    if (idLocalEstoque == 0)
                        idLocalEstoque = null;


                    var _bGradeAtiva = GradeAtiva(idEmpresa: lproduto.idEmpresa, idProduto: lproduto.idProduto ?? lproduto.idProdutoOffLine);

                    if (!_bGradeAtiva)
                        lproduto.QtdeGrade = 0;


                    var _controlaEstoque = ControlaEstoque(idEmpresa: lproduto.idEmpresa, idRepresentada: lproduto.idRepresentada);
                    if (_controlaEstoque && !_bGradeAtiva)
                    {
                        lproduto.vQtdEstoque = ObterEstoqueProduto(idEmpresa: lproduto.idEmpresa, idProduto: lproduto.idProduto ?? 0, idLocalEstoque: idLocalEstoque);
                    }



                    if (lproduto.vLimiteMinimoVenda > 0)
                    {
                        switch (config.stCalculoVendas)
                        {
                            case 1:
                                lproduto.xMinimoVendas = $"Quantidade mínima para venda de {lproduto.vLimiteMinimoVenda.ToCurrencyStringSimplesPtBr()}";
                                break;
                            case 2:
                                lproduto.xMinimoVendas = $"Total mínimo para venda de {lproduto.vLimiteMinimoVenda.ToCurrencyStringPtBr()}";
                                break;
                            default:
                                break;
                        }

                        lproduto.bUsaMinimoVendas = true;
                    }


                    var _lImagens = ImagemRepository.GetAllImages(lproduto.idProduto.GetValueOrDefault());
                    lproduto.ListaImagens = new List<ImageSource>();
                    if (_lImagens?.Count() > 0)
                    {

                        foreach (var item in _lImagens)
                        {
                            var xNameImage = item.xFilePath.PathToNameImage();
                            var _image = UtilMethods.GetLocalProdutoImageSource(xNameImage);

                            lproduto.ListaImagens.Add(_image);
                        }
                    }
                    else
                    {
                        //se não tiver nenhuma imagem vai ser preenchido com o default
                        var _image = UtilMethods.GetLocalProdutoImageSource("");

                        lproduto.ListaImagens.Add(_image);
                    }
                }

                return objReturn;
            }
            catch (Exception ex)
            {
                ex.TrakException();
                return new List<PedidoVendaItensModel>();
            }
        }

        public static void AtualizarEstoqueProduto(int idEmpresa, int? idProduto, int? idLocalEstoque, double vQtdItem)
        {
            var xWhere = $"tb_movimentoestoque.idEmpresa = {idEmpresa} and tb_movimentoestoque.idProduto = {idProduto}";

            if (idLocalEstoque.GetValueOrDefault() > 0)
                xWhere += $" and tb_movimentoestoque.idLocalEstoque = {idLocalEstoque}";
            else
                xWhere += " and tb_movimentoestoque.idLocalEstoque is null";

            var vEstoque = ObterEstoqueProduto(idEmpresa: idEmpresa,
                      idProduto: idProduto ?? 0, idLocalEstoque: idLocalEstoque);

            App.Data.Connection.ExecuteScalar<double>(
                   $@"UPDATE tb_movimentoestoque
                      SET vEstoque = {vEstoque - vQtdItem} 
                      WHERE {xWhere}");

        }

        public static bool GradeAtiva(int idEmpresa, int idProduto)
        {
            var xWhere = $" tb_gradecor.idEmpresa = {idEmpresa} and tb_gradecor.idProduto = {idProduto} and tb_gradecor.stAtivo = 1";
            var icount = App.Data.Connection.ExecuteScalar<int>(
                $@"select count(tb_gradecor.idGradeCor) from tb_gradecor 
                          where {xWhere}");

            if (icount > 0)
                return true;


            xWhere = $" tb_gradetamanho.idEmpresa = {idEmpresa} and tb_gradetamanho.idProduto = {idProduto} and tb_gradetamanho.stAtivo = 1";
            icount = App.Data.Connection.ExecuteScalar<int>(
                $@"select count(tb_gradetamanho.idGradeTamanho) from tb_gradetamanho
                          where {xWhere}");

            return icount > 0;
        }

        public static bool ControlaEstoque(int idEmpresa, int idRepresentada)
        {
            var xWhere = $" tb_representada.idEmpresa = {idEmpresa} and tb_representada.idRepresentada = {idRepresentada}";
            var _controle = App.Data.Connection.ExecuteScalar<bool>(
                $@"select tb_representada.stControleEstoque from tb_representada 
                          where {xWhere}");

            return _controle;
        }

        //public static bool ControlaEstoque(int idEmpresa, int idProduto)
        //{
        //    
        //    var xWhere = $" tb_produto.idEmpresa = {idEmpresa} and tb_produto.idProduto = {idProduto}";
        //    var _controle = App.Data.Connection.ExecuteScalar<bool>(
        //        $@"select tb_produto.stVendaSemEstoque from tb_produto 
        //                  where {xWhere}");

        //    return !_controle;
        //}


        public static double ObterEstoqueProduto(int idEmpresa, int idProduto, int? idLocalEstoque)
        {
            var xWhere = $" tb_movimentoestoque.idEmpresa = {idEmpresa} and tb_movimentoestoque.idProduto = {idProduto} ";

            if (idLocalEstoque.GetValueOrDefault() > 0)
                xWhere += $" and tb_movimentoestoque.idLocalEstoque = {idLocalEstoque}";
            else
                xWhere += $" and tb_movimentoestoque.idLocalEstoque is null ";

            var vEstoque = App.Data.Connection.ExecuteScalar<double>(
                    $@"select tb_movimentoestoque.vEstoque from tb_movimentoestoque 
                          where {xWhere}");

            return vEstoque;
        }


        public static double ObterEstoqueGradeCorTamanhoProduto(int idEmpresa, int idProduto, int? idGradeCor, int? idGradeTamanho, int? idLocalEstoque)
        {
            string xWhere = string.Empty;
            if (idGradeTamanho != null && idGradeCor != null)
            {
                xWhere = $@"where tb_movimentoestoque.idEmpresa = {idEmpresa} and tb_movimentoestoque.idProduto = {idProduto} and tb_movimentoestoque.idGradeCor = {idGradeCor} and tb_movimentoestoque.idGradeTamanho = {idGradeTamanho} ";
            }
            else if (idGradeTamanho != null)
            {
                xWhere = $@"where tb_movimentoestoque.idEmpresa = {idEmpresa} and tb_movimentoestoque.idProduto = {idProduto} and tb_movimentoestoque.idGradeTamanho = {idGradeTamanho} ";
            }
            else if (idGradeCor != null)
            {
                xWhere = $@"where tb_movimentoestoque.idEmpresa = {idEmpresa} and tb_movimentoestoque.idProduto = {idProduto} and tb_movimentoestoque.idGradeCor = {idGradeCor} ";
            }


            if (idLocalEstoque.GetValueOrDefault() > 0)
            {
                xWhere += $@" and tb_movimentoestoque.idLocalEstoque = {idLocalEstoque} ";
            }
            else
            {
                xWhere += $@" and tb_movimentoestoque.idLocalEstoque is null  ";
            }

            var xQuery = $@"select tb_movimentoestoque.vEstoque from tb_movimentoestoque {xWhere}";

            var vEstoque = App.Data.Connection.ExecuteScalar<double>(xQuery);

            return vEstoque;
        }


        public static EstoqueModel ObterRegistroEstoqueProduto(int idEmpresa, int idProduto)
        {
            var xQuery = $"select * from tb_movimentoestoque where tb_movimentoestoque.idEmpresa = {idEmpresa} and tb_movimentoestoque.idProduto = {idProduto}";
            var objEstoque = App.Data.Connection.Query<EstoqueModel>(xQuery).FirstOrDefault();

            return objEstoque;
        }

        public static EstoqueModel ObterRegistroEstoqueComGradeProduto(int idEmpresa, int idProduto, int? idGradeCor, int? idGradeTamanho)
        {
            string xWhere = string.Empty;
            if (idGradeTamanho != null && idGradeCor != null)
            {
                xWhere = $@"where tb_movimentoestoque.idEmpresa = {idEmpresa} and tb_movimentoestoque.idProduto = {idProduto} and tb_movimentoestoque.idGradeCor = {idGradeCor} and tb_movimentoestoque.idGradeTamanho = {idGradeTamanho} ";
            }
            else if (idGradeTamanho != null)
            {
                xWhere = $@"where tb_movimentoestoque.idEmpresa = {idEmpresa} and tb_movimentoestoque.idProduto = {idProduto} and tb_movimentoestoque.idGradeTamanho = {idGradeTamanho} ";
            }
            else if (idGradeCor != null)
            {
                xWhere = $@"where tb_movimentoestoque.idEmpresa = {idEmpresa} and tb_movimentoestoque.idProduto = {idProduto} and tb_movimentoestoque.idGradeCor = {idGradeCor} ";
            }

            var xQuery = $@"select * from tb_movimentoestoque {xWhere}";

            var objEstoque = App.Data.Connection.Query<EstoqueModel>(xQuery).FirstOrDefault();

            return objEstoque;
        }

        public static PedidoVendaItensModel GetProdutoToDisplay(int idProdutoOffLine, int idClientesOffLine,
            int? idClientes, int _idRepresentante)
        {
            try
            {
                var xWhere =
                    $" tb_produto.idEmpresa = {App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa} and tb_produto.idProdutoOffLine = {idProdutoOffLine}";
                var xQuery =
                    $@"select distinct 
                                                                tb_produto.idProduto,
                                                                tb_produto.idProdutoOffLine,
                                                                tb_produto.idEmpresa, 
                                                                tb_produto.cAlternativo,
                                                                tb_produto.xNome xDescricao,                                                                 
                                                                tb_produto.vCusto vCusto,
                                                                tb_produto.vVenda ,
                                                                coalesce(tb_produto.pIpiVenda,0)pIpiVenda,
                                                                coalesce(tb_produto.pStVenda,0)pStVenda,
                                                                tb_produto.idRepresentada idRepresentada,
                                                                TB_REPRESENTADA.stComissaoIPI stDescontaIpiComissao,
                                                                TB_REPRESENTADA.stComissaoST stDescontaStComissao,                                                                
                                                                coalesce(tb_unidademedida.nCasasDecimais,0) nCasasDecimais,
                                                                coalesce(TB_UNIDADEMEDIDA.xSigla,'UN') xSigla,    
                                                                tb_produto.xFileImagePrincipal ,                                                                                                     
                                                                (Count(tb_gradetamanho.idGradeTamanho) + Count(tb_gradecor.idGradeCor)) as QtdeGrade
		                                                from tb_produto                                                                         
                                                                        left join tb_gradetamanho 
                                                                                        on tb_produto.idProduto = tb_gradetamanho.idProduto
						                                                left join tb_gradecor 
								                                                        on tb_produto.idProduto = tb_gradecor.idProduto									
                                                                        left join tb_unidademedida
                                                                                        on tb_produto.idUnidadeMedida = tb_unidademedida.idUnidadeMedida
                                                                        left join TB_REPRESENTADA
                                                                                        on tb_produto.idRepresentada = TB_REPRESENTADA.idRepresentada
                                                        where {xWhere} 
                                                                                group by 
                                                                                        tb_produto.idProduto, 
                                                                                        tb_produto.idProdutoOffLine,
                                                                                        tb_produto.idEmpresa, 
                                                                                        tb_produto.idUnidadeMedida, 
                                                                                        tb_produto.xNome, 
                                                                                        tb_produto.idRepresentada,
                                                                                        tb_produto.xFileImagePrincipal,
																						tb_unidademedida.nCasasDecimais";

                var objReturn = App.Data.Connection.Query<PedidoVendaItensModel>(xQuery).FirstOrDefault();
                if (objReturn == null) return null;

                //nova rotina
                TabelaPrecoRepository.SetTabelaPrecoByProduto(objReturn, idClientesOffLine, idClientes, _idRepresentante);
                //nova rotina
                PedidoRepository.SetLocalEstoque(objReturn, idClientes, _idRepresentante);

                SetComissao(objReturn);

                //objReturn.ImageProduto = UtilMethods.GetLocalProdutoImageSource(objReturn.xFileImagePrincipal);
                var _lImagens = ImagemRepository.GetAllImages(objReturn.idProduto.GetValueOrDefault());
                objReturn.ListaImagens = new List<ImageSource>();
                if (_lImagens?.Count() > 0)
                {

                    foreach (var item in _lImagens)
                    {
                        var xNameImage = item.xFilePath.PathToNameImage();
                        var _image = UtilMethods.GetLocalProdutoImageSource(xNameImage);

                        objReturn.ListaImagens.Add(_image);
                    }
                }
                else
                {
                    //se não tiver nenhuma imagem vai ser preenchido com o default
                    var _image = UtilMethods.GetLocalProdutoImageSource("");

                    objReturn.ListaImagens.Add(_image);
                }

                return objReturn;
            }
            catch (Exception ex)
            {
                ex.TrakException();
                //Insights.Report(ex, Insights.Severity.Error);
                return new PedidoVendaItensModel();
            }
        }

        public static List<PedidoVendaItensModel> GetGradeItem(PedidoVendaItensModel produto)
        {
            var lItens = new List<PedidoVendaItensModel>();
            try
            {
                var lGradeCor = produto.idProduto != null
                    ? GetGradeCorProduto(Convert.ToInt32(produto.idProduto))
                    : new List<GradeCorModel>();
                var lGradeTamanho = produto.idProduto != null
                    ? GetGradeTamahoProduto(Convert.ToInt32(produto.idProduto))
                    : new List<GradeTamanhoModel>();
                PedidoVendaItensModel item;
                if (lGradeCor.Count > 0)
                {
                    foreach (var gradeCor in lGradeCor)
                    {
                        if (lGradeTamanho.Count > 0)
                        {
                            foreach (var gradeTamanho in lGradeTamanho)
                            {
                                item = produto.CloneItem();
                                item.xDescricao = gradeTamanho.xNome.ToUpper();
                                item.idGradeTamanho = gradeTamanho.idGradeTamanho;
                                item.vQtdEstoque = ProdutoRepository.ObterEstoqueGradeCorTamanhoProduto(item.idEmpresa, item.idProduto ?? 0, gradeCor.idGradeCor, gradeTamanho.idGradeTamanho, item.idLocalEstoque);
                                item.xDescricao += (item.xDescricao != "" ? " - " : "") + gradeCor.xNome.ToUpper();
                                item.idGradeCor = gradeCor.idGradeCor;
                                item.xCor = Color.FromHex(gradeCor.xCor ?? "FFFFFF");

                                lItens.Add(item);
                            }
                        }
                        else
                        {
                            item = produto.CloneItem();
                            item.vQtdEstoque = ProdutoRepository.ObterEstoqueGradeCorTamanhoProduto(item.idEmpresa, item.idProduto ?? 0, gradeCor.idGradeCor, null, item.idLocalEstoque);
                            item.xDescricao = gradeCor.xNome.ToUpper();
                            item.xCor = Color.FromHex(gradeCor.xCor ?? "FFFFFF");
                            item.idGradeCor = gradeCor.idGradeCor;
                            lItens.Add(item);
                        }
                    }
                }
                else
                {
                    foreach (var gradeTamanho in lGradeTamanho)
                    {
                        item = produto.CloneItem();
                        item.vQtdEstoque = ProdutoRepository.ObterEstoqueGradeCorTamanhoProduto(item.idEmpresa, item.idProduto ?? 0, null, gradeTamanho.idGradeTamanho, item.idLocalEstoque);
                        item.xDescricao = gradeTamanho.xNome.ToUpper();
                        item.idGradeTamanho = gradeTamanho.idGradeTamanho;
                        lItens.Add(item);
                    }
                }
                foreach (var itemGrade in lItens)
                {
                    itemGrade.lTabelaPreco = produto.lTabelaPreco;
                    itemGrade.currentTabelaPreco = produto.currentTabelaPreco;
                }
            }
            catch (Exception ex)
            {
                ex.TrakException();
                //Insights.Report(ex, Insights.Severity.Error);
            }
            return lItens;
        }

        public static List<GradeCorModel> GetGradeCorProduto(int idProduto)
        {
            try
            {
                const string sQueryGradeCor = @"select idGradeCor, xNome, xCor from tb_gradecor
                                                        where idProduto = {0} and stAtivo = 1 and idEmpresa = {1}";
                return
                    App.Data.Connection.Query<GradeCorModel>(string.Format(sQueryGradeCor, idProduto,
                        App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<GradeTamanhoModel> GetGradeTamahoProduto(int idProduto)
        {
            try
            {
                const string sQueryGradeTamanho = @"select idGradeTamanho, xNome from tb_gradetamanho
                                                        where idProduto = {0} and stAtivo = 1 and idEmpresa = {1}";
                return
                    App.Data.Connection.Query<GradeTamanhoModel>(string.Format(sQueryGradeTamanho, idProduto,
                        App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método para buscar a comissão e setar no objeto do item do pedido
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static void SetComissao(PedidoVendaItensModel item)
        {
            try
            {
                if (item.currentTabelaPreco != null && !item.currentTabelaPreco.bEscalonada)
                {
                    var representada = (from c in App.Data.Connection.Table<RepresentadaModel>()
                                        where c.idRepresentada == item.idRepresentada
                                        select new
                                        {
                                            xOrdemComissao = c.xOrdemComissao,
                                            pComissao = c.pComissao
                                        }).FirstOrDefault();

                    if (string.IsNullOrEmpty(representada.xOrdemComissao)) return;
                    var xOrdemComissao = representada.xOrdemComissao.Split(';');
                    var bComissaoEncontrada = false;
                    foreach (var xComissao in xOrdemComissao)
                    {

                        var stComissao = HelperPedidoVenda.GetTipoComissao(xComissao: xComissao);
                        if (bComissaoEncontrada == false)
                        {
                            switch (stComissao)
                            {

                                case TipoComissao.produto:
                                    {
                                        var pComissao = (from c in App.Data.Connection.Table<ProdutoModel>()
                                                         where c.idProdutoOffLine == item.idProdutoOffLine
                                                         select c.pComissao).FirstOrDefault();
                                        item.pComissao = item.pComissaoOriginal = pComissao ?? 0;
                                        item.stComissao = xComissao;
                                        bComissaoEncontrada = item.pComissao > 0;
                                    }
                                    break;
                                case TipoComissao.representante:
                                    {
                                        var pComissao = (from c in App.Data.Connection.Table<RepresentadaAspnetUsersModel>()
                                                         where
                                                         c.idEmpresa_aspnetUsers ==
                                                         App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa_aspnetUsers
                                                         && c.idRepresentada == item.idRepresentada
                                                         select c.pComissao).FirstOrDefault();
                                        if (pComissao != null)
                                            item.pComissao = item.pComissaoOriginal = (double)pComissao;
                                        bComissaoEncontrada = item.pComissao > 0;
                                        item.stComissao = xComissao;
                                    }
                                    break;
                                case TipoComissao.representacao:
                                    {
                                        item.pComissao = item.pComissaoOriginal = representada.pComissao ?? 0;
                                        bComissaoEncontrada = item.pComissao > 0;
                                        item.stComissao = xComissao;
                                    }
                                    break;
                                case TipoComissao.tabelapreco:
                                default:
                                    {
                                        item.pComissao = item.pComissaoOriginal = item.currentTabelaPreco.pComissao;
                                        bComissaoEncontrada = item.pComissao > 0;
                                        item.stComissao = xComissao;
                                    }
                                    break;
                            }
                        }
                    }
                }
                else
                {
                    if (item.currentTabelaPreco != null)
                    {

                        if (item.currentTabelaPreco.lFaixaComissao?.Count() > 0)
                        {
                            item.pComissaoOriginal =
                                item.currentTabelaPreco.lFaixaComissao.Select(c => c.pComissao).Max();
                            item.pComissao = item.currentTabelaPreco.SelectComissaoEscalonada(item.pDesconto);
                        }
                        else
                        {
                            item.pComissaoOriginal = 0;
                            item.pComissao = 0;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                ex.TrakException();
                //Insights.Report(ex, Insights.Severity.Error);
            }
        }



        public static ProdutoModel GetProdutoParaCalculoDePreco(int idProdutoOffLine)
        {
            try
            {
                var xQuery = $"SELECT * FROM {TableMobile.TB_PRODUTO} WHERE idProdutoOffLine = {idProdutoOffLine}";
                var objProduto = App.Data.Connection.Query<ProdutoModel>(xQuery).FirstOrDefault();


                return objProduto;
            }
            catch (Exception ex)
            {
                ex.TrakException();
                return null;
            }
        }

        public static ProdutoModel GetProduto(int idProdutoOffLine)
        {
            try
            {
                var xQuery = $"SELECT * FROM {TableMobile.TB_PRODUTO} WHERE idProdutoOffLine = {idProdutoOffLine}";
                var objProduto = App.Data.Connection.Query<ProdutoModel>(xQuery).FirstOrDefault();
                //App.Data.Connection.Table<ProdutoModel>().FirstOrDefault(c => c.idProdutoOffLine == idProdutoOffLine);
                objProduto.bUtilizaEstoqueMinMax = !(objProduto.vEstoqueMax == null && objProduto.vEstoqueMin == null);

                //controle de estoque - o.s 34993
                objProduto.stControleEstoque = ControlaEstoque(objProduto.idEmpresa, objProduto.idRepresentada);
                if (objProduto.stControleEstoque)
                {
                    var _dtAtual = App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.UltimaSyncDateTime.AddHours(-3);
                    objProduto.dtUltimaSincronizacaoEstoque = _dtAtual.ToString("dd/MM/yyyy hh:mm tt");

                    xQuery = $"SELECT * FROM {TableMobile.TB_MOVIMENTOESTOQUE}  where idProduto = {objProduto.idProduto} and idEmpresa = {objProduto.idEmpresa}";
                    var _listaEstoqueProduto = App.Data.Connection.Query<EstoqueModel>(xQuery);
                    objProduto.lEstoqueProduto = new ObservableCollection<ListEstoqueProduto>();

                    if (_listaEstoqueProduto?.Count() > 0)
                    {


                        foreach (var e in _listaEstoqueProduto)
                        {
                            var _objModel = new ListEstoqueProduto
                            {
                                idMovimentoEstoqueMobile = e.idMovimentoEstoqueMobile ?? 0,
                                idProduto = e.idProduto,
                                vEstoque = e.vEstoque,
                                xLocalEstoque = e.xLocalEstoque
                            };


                            if (e.idGradeCor == null && e.idGradeTamanho == null)
                                _objModel.xNomeGrade = $"Estoque Atual: {_objModel.vEstoque} - {_objModel.xLocalEstoque}";
                            else
                                _objModel.xNomeGrade = $" {e.xGradeCor} {e.xGradeTamanho}: {_objModel.vEstoque} - {_objModel.xLocalEstoque}";

                            objProduto.lEstoqueProduto.Add(_objModel);
                        }
                    }
                    else
                    {
                        var _objModel = new ListEstoqueProduto();

                        _objModel.xNomeGrade = $"Estoque Atual: 0";

                        objProduto.lEstoqueProduto.Add(_objModel);
                    }
                }

                //objReturn.ImageProduto = UtilMethods.GetLocalProdutoImageSource(objReturn.xFileImagePrincipal);
                var _lImagens = ImagemRepository.GetAllImages(objProduto.idProduto.GetValueOrDefault());
                objProduto.ListaImagens = new List<ImageSource>();
                if (_lImagens?.Count() > 0)
                {

                    foreach (var item in _lImagens)
                    {
                        var xNameImage = item.xFilePath.PathToNameImage();
                        var _image = UtilMethods.GetLocalProdutoImageSource(xNameImage);

                        objProduto.ListaImagens.Add(_image);
                    }
                }
                else
                {
                    //se não tiver nenhuma imagem vai ser preenchido com o default
                    var _image = UtilMethods.GetLocalProdutoImageSource("");

                    objProduto.ListaImagens.Add(_image);
                }



                return objProduto;
            }
            catch (Exception ex)
            {
                ex.TrakException();
                return null;
            }
        }


        public static double? GetPorcIpi(int idProdutoOffLine)
        {
            return App.Data.Connection.ExecuteScalar<double?>(
                $@"select pIpiVenda from {TableMobile.TB_PRODUTO} 
                    where idProdutoOffLine = {idProdutoOffLine}");
        }
        public static double? GetPorcSt(int idProdutoOffLine)
        {
            return App.Data.Connection.ExecuteScalar<double?>(
                $@"select  pStVenda from {TableMobile.TB_PRODUTO} 
                    where idProdutoOffLine = {idProdutoOffLine}"
                );

        }

        public static ProdutoModel ObterItem(int idProdutoOffLine)
        {
            //var _item = App.Data.Connection.Table<TabelaPrecoItemModel>(
            //    ).FirstOrDefault(ti => ti.idTabelaPreco == idTabelaPreco && ti.idProduto == idProduto);

            var _item = App.Data.Connection.Query<ProdutoModel>(
                $@"select pIpiVenda, pStVenda, vVendaComImpostos, vVenda, idProduto, pDescontoMaximo, pComissao from {TableMobile.TB_PRODUTO} 
                    where and idProdutoOffLine = {idProdutoOffLine}"
                ).FirstOrDefault();

            return _item;
        }

        /// <summary>
        /// GET todos os registros de produto ( BasicPickerModel )
        /// </summary>
        /// <param name="bAtivo">True - traz somente registros ativos, False - todos os registros</param>
        /// <returns></returns>
        public static List<BasicPickerModel> GetAll(bool bAtivo = true)
        {
            try
            {
                const string xFields =
                    "TB_PRODUTO.idProdutoOffLine Id, TB_PRODUTO.xNome Display, TB_CATEGORIA.xCategoria Detail, TB_PRODUTO.idProduto IdOnline, bProblemaSincronizacao, 'False' bTrazerImagem ";

                var xWhere = bAtivo
                    ? $"TB_PRODUTO.stAtivo = 1 and TB_PRODUTO.idEmpresa = {App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa}"
                    : $"TB_PRODUTO.idEmpresa = {App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa}";

                if (!App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.stAdministrador)
                {
                    var lRepresentadasAspnetUsers =
                        App.Data.Connection.Table<RepresentadaAspnetUsersModel>()
                            .Where(
                                c =>
                                    c.idEmpresa_aspnetUsers ==
                                    App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa_aspnetUsers)
                            .ToList();
                    var inIdRepresentada =
                        lRepresentadasAspnetUsers.Select(c => c.idRepresentada.ToString())
                            .ToList()
                            .Aggregate("", (current, item) => current + ((current == "" ? "" : " , ") + item));
                    xWhere += $" and idRepresentada in ({inIdRepresentada}) ";
                }

                string xQuery = $@"select {xFields} from TB_PRODUTO inner join TB_CATEGORIA 
                                                            on TB_PRODUTO.idCategoria = TB_CATEGORIA.idCategoria 
                                                    where {xWhere}";
                var result = App.Data.Connection.Query<BasicPickerModel>(xQuery);
                return result;
            }
            catch (Exception ex)
            {
                ex.TrakException();
                //Insights.Report(ex, Insights.Severity.Error);
                return new List<BasicPickerModel>();
            }
        }


        public static List<ListItemModel> GetToPesquisa(int skip, int take, string xFiltro, bool bAtivo)
        {
            try
            {
                const string xFields =
                    "TB_PRODUTO.idProdutoOffLine Id, TB_PRODUTO.xNome Display, TB_PRODUTO.cAlternativo Detail ";

                var xWhere = bAtivo
                    ? $"stAtivo = 1 and idEmpresa = {App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa}"
                    : $"idEmpresa = {App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa}";

                if (!string.IsNullOrEmpty(xFiltro))
                {
                    xFiltro = xFiltro.RemoverAcentos().ToUpper();
                    xWhere += $@" and (UPPER(xNome) like('%{xFiltro}%') 
                                    or UPPER(coalesce(xDisplaySemCaracter,'')) like('%{xFiltro}%') 
                                    or UPPER(cAlternativo) like('%{xFiltro}%') 
                                    or UPPER(cEan) like('%{xFiltro}%')
                                    or UPPER(cEanEmb) like('%{xFiltro}%'))";
                }

                if (!App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.stAdministrador)
                {
                    var xQueryRepresentacoes = $@"SELECT * FROM {TableMobile.TB_REPRESENTADA_ASPNETUSERS} WHERE
                                                        idEmpresa_aspnetUsers = {App.CurrentAspnetUserModel
                        .objEmpresaAspnetUsersModel.idEmpresa_aspnetUsers}";

                    var lRepresentadasAspnetUsers =
                        App.Data.Connection.Query<RepresentadaAspnetUsersModel>(xQueryRepresentacoes);

                    //var lRepresentadasAspnetUsers = App.Data.Connection.Table<RepresentadaAspnetUsersModel>().Where(c => c.idEmpresa_aspnetUsers == App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa_aspnetUsers).ToList();
                    var inIdRepresentada =
                        lRepresentadasAspnetUsers.Select(c => c.idRepresentada.ToString())
                            .ToList()
                            .Aggregate("", (current, item) => current + ((current == "" ? "" : " , ") + item));
                    xWhere += $" and idRepresentada in ({inIdRepresentada}) ";
                }

                string xQuery = $@"select {xFields} from TB_PRODUTO  
                                                    where {xWhere} order by UPPER(xNome)
                                            LIMIT {take} OFFSET {skip}";
                var result = App.Data.Connection.Query<ListItemModel>(xQuery);
                return result;
            }
            catch (Exception ex)
            {
                ex.TrakException();
                //Insights.Report(ex, Insights.Severity.Error);
                return new List<ListItemModel>();
            }
        }

        public static List<ProdutoModel> GetAllToSync()
        {
            var lUpload =
                App.Data.Connection.Table<ProdutoModel>()
                    .Where(
                        c =>
                            c.dtUltimaAlteracao >
                            App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.UltimaSyncDateTime &&
                            c.idEmpresa == App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa);
            return lUpload.ToList();
        }

        public static ProdutoModel Save(ProdutoModel objProdutoModel)
        {
            try
            {
                if (!objProdutoModel.bUtilizaEstoqueMinMax)
                {
                    objProdutoModel.vEstoqueMax = objProdutoModel.vEstoqueMin = null;
                }

                objProdutoModel.idEmpresa = App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa;

                if (objProdutoModel.idProdutoOffLine == null)
                {
                    if (objProdutoModel.idAspNetUserInclusao == null)
                        objProdutoModel.idAspNetUserInclusao = App.CurrentAspnetUserModel.Id;

                    objProdutoModel.dtCadastro = DateTime.Now.ToUniversalTime();
                    objProdutoModel.dtUltimaAlteracao = DateTime.Now.ToUniversalTime();

                    App.Data.Connection.Insert(objProdutoModel);
                }
                else
                {
                    objProdutoModel.dtUltimaAlteracao = DateTime.Now.ToUniversalTime();
                    App.Data.Connection.Update(objProdutoModel);
                }

                return objProdutoModel;
            }
            catch (Exception ex)
            {
                ex.TrakException();
                //Insights.Report(ex, Insights.Severity.Error);
                return null;
            }
        }

        public static bool CodigoAlternativoExiste(ProdutoModel objProdutoModel)
        {
            var AddWhere = "";
            if (objProdutoModel.idProdutoOffLine != null)
                AddWhere = $" and idProdutoOffLine <> {objProdutoModel.idProdutoOffLine}";
            var icount = App.Data.Connection.ExecuteScalar<int>(
                $@"select count(*) from {TableMobile.TB_PRODUTO} 
                                            where idEmpresa = {App
                    .CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa} and cAlternativo = '{objProdutoModel
                    .cAlternativo}' {AddWhere}");
            return icount > 0;
        }

        public static bool NomeExiste(ProdutoModel objProdutoModel)
        {
            try
            {
                var AddWhere = "";

                if (objProdutoModel.idProdutoOffLine != null)
                    AddWhere = $" and idProdutoOffLine <> {objProdutoModel.idProdutoOffLine}";

                var idEmpresa = App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa;

                var xQuery = $@"select count(*) from {TableMobile.TB_PRODUTO} where idEmpresa = {idEmpresa} and xDisplaySemCaracter = '{objProdutoModel.xNome.RemoverAcentos()}' {AddWhere}";

                var icount = App.Data.Connection.ExecuteScalar<int>(xQuery);
                return icount > 0;
            }
            catch (Exception ex)
            {
                throw;
            }




        }

        public static void UpdateAfterUpload(ProdutoModel objProdutoModel)
        {
            try
            {
                var xQuery = @"UPDATE {0} SET idProduto = {1} 
                                        WHERE idProdutoOffLine = {2} AND idEmpresa = {3}";

                App.Data.Connection.Execute(string.Format(xQuery, TableMobile.TB_PRODUTO, objProdutoModel.idProduto, objProdutoModel.idProdutoOffLine, objProdutoModel.idEmpresa));

                App.Data.Connection.Execute(string.Format(xQuery, TableMobile.TB_PEDIDOVENDAITENS, objProdutoModel.idProduto, objProdutoModel.idProdutoOffLine, objProdutoModel.idEmpresa));
            }
            catch (Exception ex)
            {
                ex.TrakException();
                //Insights.Report(ex, Insights.Severity.Error);
            }
        }


        public static string GetNomeByIdOffLine(int idProdutoOffLine)
        {
            var prod = App.Data.Connection.Table<ProdutoModel>().FirstOrDefault(c => c.idProdutoOffLine == idProdutoOffLine);

            if (prod != null)
            {
                return prod.xNome + " - " + prod.cAlternativo;
            }
            else
            {
                return "";
            }
        }

        public static int GetIdOffLineByIdOnline(int idProduto)
        {

            var idOffLine = (from c in App.Data.Connection.Table<ProdutoModel>()
                             where
                                 c.idProduto == idProduto &&
                                 c.idEmpresa == App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa
                             select c.idProdutoOffLine ?? 0).FirstOrDefault();
            return idOffLine;
            //var prod = App.Data.Connection.Table<ProdutoModel>().FirstOrDefault(c => c.idProduto == idProduto);

            //if (prod != null)
            //{
            //    return prod.idProdutoOffLine ?? 0;
            //}
            //return 0;
        }

        public static string GetNomeByIdCliente(int idClienteOffLine, int idProduto)
        {
            var xretorno = "";
            var idCliente = ClienteRepository.GetIdClienteNuvem(idClienteOffLine);
            if (idCliente > 0)
            {
                var xQuery =
                    $@"SELECT xCodigoProdutoCliente from tb_produto_codigocliente where idProduto = {idProduto} and idClientes = {idCliente}";
                xretorno = App.Data.Connection.ExecuteScalar<string>(xQuery);
            }
            return xretorno ?? "";
        }

        public static bool CanRemove(ProdutoModel objProdutoModel)
        {
            return App.Data.Connection.Table<PedidoVendaItensModel>().Count(c => c.idProdutoOffLine == objProdutoModel.idProdutoOffLine) <= 0;
        }

        public static async Task<bool> Delete(ProdutoModel objProdutoModel)
        {
            try
            {
                var removido = false;
                if (objProdutoModel.idProduto == null)
                {
                    if (await UtilMessages.Exclusao())
                        if (ProdutoRepository.CanRemove(objProdutoModel))
                        {
                            App.Data.Connection.Delete(objProdutoModel);
                            removido = true;
                        }
                        else
                            await App.Messages.ShowAsync("Já existe pedido/orçamento utilizando esse produto, impossível remover.");
                }
                else
                    await App.Messages.ShowAsync("Não é possível excluir um registro já sincronizado pelo app, acesse o pedidoeletronico.com");
                return removido;
            }
            catch (Exception ex)
            {
                ex.TrakException();
                //Insights.Report(ex, Insights.Severity.Error);
                return false;
            }
        }







    }
}
