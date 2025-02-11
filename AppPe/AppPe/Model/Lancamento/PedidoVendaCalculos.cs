using System;
using System.Linq;
using System.Threading.Tasks;
using Hlp.PedidoEletronico.Domain.Business.Calculos;
using Xamarin.HLP.Mobile.AppPE.View.Pedido;
using Xamarin.HLP.Mobile.AppPE.Core.PedidoVenda.Interfaces;
using Xamarin.HLP.Mobile.AppPE.Core.PedidoVenda.Implementacoes;
using Xamarin.HLP.Mobile.AppPE.Model.Repository;

namespace Xamarin.HLP.Mobile.AppPE.Model.Lancamento
{
    public static class PedidoVendaCalculos
    {
        private static readonly Pedido calc = new Pedido();



        /// <summary>
        /// Calcula o valor de venda com impostos
        /// </summary>
        /// <param name="vUnitarioVenda"></param>
        /// <param name="pSt"></param>
        /// <param name="pIpi"></param>
        /// <returns></returns>
        public static double CalculoValorUnitarioComImpostos(double vUnitarioVenda, double pSt, double pIpi)
        {
            return (double)calc.CalculoValorUnitarioComImpostos((decimal)vUnitarioVenda, (decimal)pSt, (decimal)pIpi);
        }

        /// <summary>
        /// Calcula valor unitario sem impostos - REFEITO
        /// </summary>
        /// <param name="vUnitarioVendaComImpostos"></param>
        /// <param name="pSt"></param>
        /// <param name="pIpi"></param>
        /// <returns></returns>
        public static double CalculoValorUnitario(double vUnitarioVendaComImpostos, double pSt, double pIpi)
        {
            return (double)calc.CalculoValorUnitario((decimal)vUnitarioVendaComImpostos, (decimal)pSt, (decimal)pIpi);


        }
        /// <summary>
        /// CALCULA VALOR SUBTOTAL DO PEDIDO - REFEITO
        /// </summary>
        /// <param name="item"></param>
        public static void CalculoValorSubTotal(PedidoVendaItensModel item)
        {
            double _vVendaComImposto = 0;
            if (item.ItensGrade != null && item.ItensGrade.Any())
            {  
                foreach (var i in item.ItensGrade)
                {
                    _vVendaComImposto = i.vUnitarioVendaComImpostos;

                    //double _desconto = 0;

                    //if (i.vDesconto > 0)
                    //{
                    //    if (i.currentTabelaPreco.vVendaSemArredondamento > 0)
                    //        _desconto = (i.currentTabelaPreco.vVendaSemArredondamento * (item.pDesconto / 100));
                    //    else
                    //        _desconto = (i.currentTabelaPreco.vUnitario * (item.pDesconto / 100)); 
                    //} 


                    //i.vSubTotal = i.vQtdItem * (_vVendaComImposto - _desconto);
                    i.vSubTotal = i.vQtdItem * _vVendaComImposto;
                }
            }
            else
            {  
                item.vSubTotal = item.vQtdItem * item.vUnitarioVendaComImpostos;
            }

            SumTotalizadoresPageEditarItem();
        }
        public static void CalculoValorComissao(PedidoVendaItensModel item)
        {
            IComissao _objComissao = new Comissao();

            if (item.ItensGrade != null && item.ItensGrade.Any())
            {
                foreach (var i in item.ItensGrade)
                {
                    i.vComissao = (double)(_objComissao.CalcularValorComissao(vUnitario: (decimal)i.vUnitarioVendaComImpostos,
                        pComissao: (decimal)i.pComissao, pIpi: (decimal)(i.pIpiVenda ?? 0), pSt: (decimal)(i.pStVenda ?? 0),
                        bDescIpi: i.stDescontaIpiComissao, bDescSt: i.stDescontaStComissao)) * i.vQtdItem;

                    //i.vComissao = (double)calc.CalculoValorComissao(
                    //   vUnitario: i.vUnitarioVenda.ToDecimalPtBr(),
                    //   qtd: i.vQtdItem.ToDecimalPtBr(),
                    //   pIpiVendaAcresc: i.pIpiVenda.ToDecimalPtBr(),
                    //   bDescontaIpiVenda: i.stDescontaIpiComissao,
                    //   bDescontaStVenda: i.stDescontaStComissao,
                    //   pStVendaAcresc: i.pStVenda.ToDecimalPtBr(),
                    //   pComis: i.pComissao.ToDecimalPtBr(),
                    //   pDesc: i.pDesconto.ToDecimalPtBr());

                }
                if (StaticModel.StaticEditarItemViewModel != null)
                    StaticModel.StaticEditarItemViewModel.vComissao = item.ItensGrade.Sum(c => c.vComissao);
            }
            else
            {
                item.vComissao = (double)(_objComissao.CalcularValorComissao(vUnitario: (decimal)item.vUnitarioVendaComImpostos,
                        pComissao: (decimal)item.pComissao, pIpi: (decimal)(item.pIpiVenda ?? 0), pSt: (decimal)(item.pStVenda ?? 0),
                        bDescIpi: item.stDescontaIpiComissao, bDescSt: item.stDescontaStComissao)) * item.vQtdItem;
                //item.vComissao = (double)calc.CalculoValorComissao(
                //         vUnitario: item.vUnitarioVenda.ToDecimalPtBr(),
                //         qtd: item.vQtdItem.ToDecimalPtBr(),
                //         pIpiVendaAcresc: item.pIpiVenda.ToDecimalPtBr(),
                //         bDescontaIpiVenda: item.stDescontaIpiComissao,
                //         bDescontaStVenda: item.stDescontaStComissao,
                //         pStVendaAcresc: item.pStVenda.ToDecimalPtBr(),
                //         pComis: item.pComissao.ToDecimalPtBr(),
                //         pDesc: item.pDesconto.ToDecimalPtBr());
            }
            SumTotalizadoresPageEditarItem();

        }
        public static void CalculoPorcComissao(PedidoVendaItensModel item, double vComissao)
        {
            if (item.ItensGrade != null && item.ItensGrade.Any())
            {
                foreach (var i in item.ItensGrade)
                {
                    i.pComissao = (double)calc.CalculoValorPorcComissao(
                       vUnitario: i.vUnitarioVenda.ToDecimalPtBr(),
                       qtd: i.vQtdItem.ToDecimalPtBr(),
                       pIpiVendaAcresc: i.pIpiVenda.ToDecimalPtBr(),
                       bDescontaIpiVenda: i.stDescontaIpiComissao,
                       bDescontaStVenda: i.stDescontaStComissao,
                       pStVendaAcresc: i.pStVenda.ToDecimalPtBr(),
                       pDesc: i.pDesconto.ToDecimalPtBr(),
                       vComis: vComissao.ToDecimalPtBr());
                }
            }
            else
            {
                item.pComissao = (double)calc.CalculoValorPorcComissao(
                       vUnitario: item.vUnitarioVenda.ToDecimalPtBr(),
                       qtd: item.vQtdItem.ToDecimalPtBr(),
                       pIpiVendaAcresc: item.pIpiVenda.ToDecimalPtBr(),
                       bDescontaIpiVenda: item.stDescontaIpiComissao,
                       bDescontaStVenda: item.stDescontaStComissao,
                       pStVendaAcresc: item.pStVenda.ToDecimalPtBr(),
                       pDesc: item.pDesconto.ToDecimalPtBr(),
                       vComis: vComissao.ToDecimalPtBr());
            }
            SumTotalizadoresPageEditarItem();
        }
        /// <summary>
        /// CALCULA VALOR TOTAL DO ITEM, SEM OS IMOSTOS - REFEITO
        /// </summary>
        /// <param name="item"></param>
        public static void CalculoValorSubTotalSemImpostos(PedidoVendaItensModel item)
        {
            ICalculoTotais _objCalc = new CalculoTotais();
            if (item.ItensGrade != null && item.ItensGrade.Any())
            {
                foreach (var i in item.ItensGrade)
                {
                    //i.vSubTotalSemImpostos = _objCalc.CalcularTotalSemImpostos(vUnitario: i.vUnitarioVendaSemImposto,
                    //    vQtd: i.vQtdItem, pIpiVenda: (i.pIpiVenda ?? 0), pStVenda: (i.pStVenda ?? 0)) - i.vDesconto;
                    if (i.pIpiVenda.GetValueOrDefault() > 0 || i.pStVenda.GetValueOrDefault() > 0)
                    {
                        var _pDesconto = i.pDesconto;

                        if (i.vDescontoDaCondicao > 0 && _pDesconto <= 0)
                            _pDesconto = i.vDescontoDaCondicao.GetValueOrDefault() * -1;



                        i.vSubTotalSemImpostos = i.vUnitarioVendaSemImposto - (i.vUnitarioVendaSemImposto * (_pDesconto / 100));                        
                    }
                    else
                    {
                        var _pDesconto = i.pDesconto;
                        var _desconto = i.vDesconto;
                        if (i.vDescontoDaCondicao > 0 && i.pDesconto <= 0)
                        { 
                            _pDesconto = i.vDescontoDaCondicao.GetValueOrDefault() * -1;
                            _desconto = i.vUnitarioVendaSemImposto * (_pDesconto / 100);
                        }


                        i.vSubTotalSemImpostos = i.vUnitarioVendaSemImposto - _desconto;
                    }
                }
            }
            else
            {
                //item.vSubTotalSemImpostos = _objCalc.CalcularTotalSemImpostos(vUnitario: item.vUnitarioVendaSemImposto,
                //        vQtd: item.vQtdItem, pIpiVenda: (item.pIpiVenda ?? 0), pStVenda: (item.pStVenda ?? 0)) - item.vDesconto;
                if (item.pIpiVenda.GetValueOrDefault() > 0 || item.pStVenda.GetValueOrDefault() > 0)
                {
                    var _pDesconto = item.pDesconto;

                    if (item.vDescontoDaCondicao > 0 && _pDesconto <= 0)
                        _pDesconto = item.vDescontoDaCondicao.GetValueOrDefault() * -1;


                    item.vSubTotalSemImpostos = item.vUnitarioVendaSemImposto - (item.vUnitarioVendaSemImposto * (_pDesconto / 100)); 
                }
                else
                {
                    var _pDesconto = item.pDesconto;
                    var _desconto = item.vDesconto;
                    if (item.vDescontoDaCondicao > 0 && item.pDesconto <= 0)
                    {
                        _pDesconto = item.vDescontoDaCondicao.GetValueOrDefault() * -1;
                        _desconto = item.vUnitarioVendaSemImposto * (_pDesconto / 100);
                    }


                    item.vSubTotalSemImpostos = item.vUnitarioVendaSemImposto - _desconto;
                }
               

            }
            SumTotalizadoresPageEditarItem();

        }
        /// <summary>
        /// CALCULAR O VALOR DO DESCONTO PELA PORCENTAGEM - REFEITO
        /// </summary>
        /// <param name="item"></param>
        /// <param name="pDesconto"></param>
        public static void CalculoDescontoPorPorcent(PedidoVendaItensModel item, double pDesconto)
        {
            if (item.ItensGrade != null && item.ItensGrade.Any())
            {
                foreach (var i in item.ItensGrade)
                {
                    //i.vDesconto = (double)calc.GetValorDesconto(
                    //                pDesconto: pDesconto.ToDecimalPtBr(),
                    //                vUnitarioVendaComImpostos: i.vUnitarioVendaComImpostos.ToDecimalPtBr(),
                    //                qtd: i.vQtdItem.ToDecimalPtBr());


                    if (pDesconto > 0)
                    {
                        // OS 35294 - Jessica Barbieri // os do desconto
                        if (i.vUnitarioVenda == 0)
                        {
                            i.vDesconto = i.vUnitarioVendaComImpostos * (pDesconto / 100);
                        }
                        else
                        {
                            i.vDesconto = i.vUnitarioVenda * (pDesconto / 100);
                        }
                    }

                    else
                    {
                        i.vDesconto = 0;
                    }

                }
            }
            else
            {
                //item.vDesconto = (double)calc.GetValorDesconto(
                //                    pDesconto: pDesconto.ToDecimalPtBr(),
                //                    vUnitarioVendaComImpostos: item.vUnitarioVendaComImpostos.ToDecimalPtBr(),
                //                    qtd: item.vQtdItem.ToDecimalPtBr());
                if (pDesconto > 0)
                {
                    item.vDesconto = item.vUnitarioVenda * (pDesconto / 100);
                }
                else
                {
                    item.vDesconto = 0;
                }

            }
            SumTotalizadoresPageEditarItem();
        }

        /// <summary>
        /// CALCULA ALIQ DESCONTO BASEADO NO VALOR DE DESCONTO - REFEITO
        /// </summary>
        /// <param name="item"></param>
        /// <param name="vDesconto"></param>
        public static void CalculoDescontoPorValor(PedidoVendaItensModel item, double vDesconto)
        {
            var _vTabela = item.vUnitarioVenda
                                + (item.vUnitarioVenda * ((item.pIpiVenda ?? 0) / 100))
                                + (item.vUnitarioVenda * ((item.pStVenda ?? 0) / 100));

            if (item.ItensGrade != null && item.ItensGrade.Any())
            {
                foreach (var i in item.ItensGrade)
                {
                    i.vDesconto = vDesconto;
                    if (_vTabela > 0)
                    {
                        i.pDesconto = (vDesconto * 100) / _vTabela;
                    }
                    else
                    {
                        i.pDesconto = 0;
                        i.vDesconto = 0;
                    }

                    //i.vDesconto = vDesconto;
                    //i.pDesconto = (double)calc.GetPorcentDesconto(
                    //                           vDesconto: vDesconto.ToDecimalPtBr(),
                    //                           vUnitarioVendaComImpostos: i.vUnitarioVendaComImpostos.ToDecimalPtBr(),
                    //                           qtd: i.vQtdItem.ToDecimalPtBr());
                }
            }
            else
            {
                item.vDesconto = vDesconto;
                if (_vTabela > 0)
                {
                    item.pDesconto = (vDesconto * 100) / _vTabela;
                }
                else
                {
                    item.pDesconto = 0;
                    item.vDesconto = 0;
                }

                //item.vDesconto = vDesconto;
                //item.pDesconto = (double)calc.GetPorcentDesconto(
                //                               vDesconto: item.vDesconto.ToDecimalPtBr(),
                //                               vUnitarioVendaComImpostos: item.vUnitarioVendaComImpostos.ToDecimalPtBr(),
                //                               qtd: item.vQtdItem.ToDecimalPtBr());
            }
            SumTotalizadoresPageEditarItem();

        }



        /// <summary>
        /// CALCULA ALIQ DESCONTO BASEADO NO VALOR DE DESCONTO - REFEITO
        /// </summary>
        /// <param name="item"></param>
        /// <param name="vDesconto"></param>
        public static void BuscaValorDesconto(PedidoVendaItensModel item)
        {
            var _vTabela = item.currentTabelaPreco.vUnitario
                                + (item.currentTabelaPreco.vUnitario * ((item.pIpiVenda ?? 0) / 100))
                                + (item.currentTabelaPreco.vUnitario * ((item.pStVenda ?? 0) / 100));

            if (item.ItensGrade != null && item.ItensGrade.Any())
            {
                foreach (var i in item.ItensGrade)
                {
                    i.vDesconto = item.vUnitarioVendaComImpostos - _vTabela ;

                    if (i.vDesconto < 0)
                        i.vDesconto = i.vDesconto * (-1);

                    if (_vTabela > 0)
                    {
                        i.pDesconto = (i.vDesconto * 100) / _vTabela;
                    }
                    else
                    {
                        i.pDesconto = 0;
                        i.vDesconto = 0;
                    }

                    //i.vDesconto = vDesconto;
                    //i.pDesconto = (double)calc.GetPorcentDesconto(
                    //                           vDesconto: vDesconto.ToDecimalPtBr(),
                    //                           vUnitarioVendaComImpostos: i.vUnitarioVendaComImpostos.ToDecimalPtBr(),
                    //                           qtd: i.vQtdItem.ToDecimalPtBr());
                }
            }
            else
            {
                item.vDesconto = item.vUnitarioVendaComImpostos - _vTabela;

                if (item.vDesconto < 0)
                    item.vDesconto = item.vDesconto * (-1);

                if (_vTabela > 0)
                {
                    item.pDesconto = (item.vDesconto * 100) / _vTabela;
                }
                else
                {
                    item.pDesconto = 0;
                    item.vDesconto = 0;
                }

                //item.vDesconto = vDesconto;
                //item.pDesconto = (double)calc.GetPorcentDesconto(
                //                               vDesconto: item.vDesconto.ToDecimalPtBr(),
                //                               vUnitarioVendaComImpostos: item.vUnitarioVendaComImpostos.ToDecimalPtBr(),
                //                               qtd: item.vQtdItem.ToDecimalPtBr());
            }
            SumTotalizadoresPageEditarItem();

        }
        public static async Task CalculoByStepper()
        {
            //await Task.Run(() =>
            //{
            if (PagePedidoNew.CurrentViewModel.currentModel.CurrentItemModel != null)
            {
                if (PagePedidoNew.CurrentViewModel.currentModel.CurrentItemModel.ItensGrade != null &&
                    PagePedidoNew.CurrentViewModel.currentModel.CurrentItemModel.ItensGrade.Any())
                {
                    var _itemComQtdade = PagePedidoNew.CurrentViewModel.currentModel.CurrentItemModel.ItensGrade.Where(i => i.vQtdItem > 0).FirstOrDefault();

                    foreach (var item in PagePedidoNew.CurrentViewModel.currentModel.CurrentItemModel.ItensGrade)
                    {      
                        if(item.vQtdItem == 0 && _itemComQtdade != null)
                        {
                            item.vUnitarioVenda = _itemComQtdade.vUnitarioVenda;
                            item.vUnitarioVendaComImpostos = _itemComQtdade.vUnitarioVendaComImpostos;
                            item.vUnitarioVendaComImpostosOriginal = _itemComQtdade.vUnitarioVendaComImpostos;
                            item.vUnitarioVendaSemImposto = _itemComQtdade.vUnitarioVendaSemImposto;
                            item.vDesconto = _itemComQtdade.vDesconto;
                            item.pDesconto = _itemComQtdade.pDesconto;
                        }

                        AtualizaValores(item);
                    }
                    SumTotalizadoresPageEditarItem();
                    PagePedidoNew.CurrentViewModel.currentModel.CurrentItemModel.NotifyTotalizadores();
                }
                else
                    AtualizaValores(PagePedidoNew.CurrentViewModel.currentModel.CurrentItemModel);

                try
                {
                    if (StaticModel.StaticEditarItemViewModel != null && PagePedidoNew.CurrentViewModel.currentModel.CurrentItemModel.ItensGrade != null)
                    {
                        StaticModel.StaticEditarItemViewModel.vDesconto =
                            PagePedidoNew.CurrentViewModel.currentModel.CurrentItemModel.ItensGrade.Where(c =>  c.vDesconto > 0)
                            .Select(c => c.vDesconto).FirstOrDefault();
                        StaticModel.StaticEditarItemViewModel.vComissao =
                            PagePedidoNew.CurrentViewModel.currentModel.CurrentItemModel.ItensGrade.Where(c => c.vQtdItem > 0).Sum(
                                c => c.vComissao);
                        StaticModel.StaticEditarItemViewModel.pComissao =
                            PagePedidoNew.CurrentViewModel.currentModel.CurrentItemModel.ItensGrade.FirstOrDefault()
                                .pComissao;
                        StaticModel.StaticEditarItemViewModel.pDesconto =
                            PagePedidoNew.CurrentViewModel.currentModel.CurrentItemModel.ItensGrade.Where(c => c.pDesconto > 0)
                            .Select(c => c.pDesconto).FirstOrDefault();
                        StaticModel.StaticEditarItemViewModel.pIpiVenda =
                            PagePedidoNew.CurrentViewModel.currentModel.CurrentItemModel.ItensGrade.FirstOrDefault()
                                .pIpiVenda;                        

                        StaticModel.StaticEditarItemViewModel.pStVenda =
                            PagePedidoNew.CurrentViewModel.currentModel.CurrentItemModel.ItensGrade.FirstOrDefault()
                                .pStVenda;



                    }
                }
                catch (Exception ex)
                {
                    GoogleInsightsReportingConstants.TrakException("Erro ao passar parametros de impostos", ex.Message, true);
                }
            }
            if (PagePedidoNew.CurrentViewModel != null)
                PagePedidoNew.CurrentViewModel.AtualizaTotalizadoresPedido();

            //});
        }

        /// <summary>
        ///  VALIDA DESCONTO PELO VALOR UNITARIO COM IMPOSTOS -  REFEITO
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static bool DescontoValidoVlrUnitarioComImpostos(PedidoVendaItensModel item)
        {
            var objToCalc = (item.ItensGrade != null && item.ItensGrade.Any()) ? item.ItensGrade.FirstOrDefault() : item;

            if (objToCalc.currentTabelaPreco == null)
            {
                return false;
            }

            var resultado = "";
            //if (objToCalc.vUnitarioVendaComImpostos > 0)
            resultado = calc.DescontoValido(
                                    vUnitarioVendaComImpostos: (objToCalc.vUnitarioVendaComImpostos).ToDecimalPtBr(),
                                    idTabelaPreco: 0,
                                    idProduto: 0,
                                    vlrTabelaPreco: objToCalc.currentTabelaPreco.vVenda.ToDecimalPtBr(),
                                    pDescMaximo: objToCalc.currentTabelaPreco.pDescontoMaximo.ToDecimalPtBr(),
                                    pDesconto: objToCalc.pDesconto.ToDecimalPtBr(),
                                    stChamada: Pedido.TipoChamadaCalculoDesconto.valorUnit);

            return resultado == "";

        }

        /// <summary>
        /// Analisa desconto valido por desconto - REFEITO
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static bool DescontoValidoPorcDesc(PedidoVendaItensModel item)
        {
            var objToCalc = (item.ItensGrade != null && item.ItensGrade.Any()) ? item.ItensGrade.FirstOrDefault() : item;

            var resultado = "";
            if (objToCalc.vUnitarioVendaComImpostos > 0)
                resultado = calc.DescontoValido(
                                        vUnitarioVendaComImpostos: objToCalc.vUnitarioVendaComImpostos.ToDecimalPtBr(),
                                        idTabelaPreco: 0,
                                        idProduto: 0,
                                        vlrTabelaPreco: objToCalc.currentTabelaPreco.vVenda.ToDecimalPtBr(),
                                        pDescMaximo: objToCalc.currentTabelaPreco.pDescontoMaximo.ToDecimalPtBr(),
                                        pDesconto: objToCalc.pDesconto.ToDecimalPtBr(), stChamada: Pedido.TipoChamadaCalculoDesconto.desconto);
            return resultado == "";
        }
        public static bool ComissaoValida(PedidoVendaItensModel item)
        {
            var objToCalc = (item.ItensGrade != null && item.ItensGrade.Any()) ? item.ItensGrade.FirstOrDefault() : item;
            if (App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.stAdministrador)
                if (objToCalc.pComissao <= 100)
                    return true;

            //se for escalonada, ela sobrescreve as comissões selecionadas
            if (objToCalc.currentTabelaPreco.bEscalonada)
            {
                if (objToCalc.currentTabelaPreco.lFaixaComissao?.Count() > 0)
                {
                    if (objToCalc.currentTabelaPreco.lFaixaComissao.OrderByDescending(f => f.pFimFaixa).FirstOrDefault().pFimFaixa > objToCalc.pComissao)
                        return true;
                } 
            }
             


            return !(objToCalc.pComissao > objToCalc.pComissaoOriginal);
        }

        public static void SumTotalizadoresPageEditarItem()
        {
            if (StaticModel.StaticEditarItemViewModel != null)
            {
                if (PagePedidoNew.CurrentViewModel?.currentModel?.CurrentItemModel != null)
                {
                    if (PagePedidoNew.CurrentViewModel?.currentModel?.CurrentItemModel?.ItensGrade != null)
                    {
                        StaticModel.StaticEditarItemViewModel.vSubTotal =
                            PagePedidoNew.CurrentViewModel.currentModel.CurrentItemModel.ItensGrade.Sum(c => c.vSubTotal);
                        StaticModel.StaticEditarItemViewModel.vSubTotalSemImpostos =
                            PagePedidoNew.CurrentViewModel.currentModel.CurrentItemModel.ItensGrade.Sum(
                                c => (c.vQtdItem * c.vSubTotalSemImpostos));
                        StaticModel.StaticEditarItemViewModel.vQtdeTotal =
                            PagePedidoNew.CurrentViewModel.currentModel.CurrentItemModel.ItensGrade.Sum(c => c.vQtdItem);
                    }
                }

            }
        }



        public static void AtualizaValores(PedidoVendaItensModel item)
        {
            try
            {
                if (!ComissaoValida(item) ||
                    !DescontoValidoPorcDesc(item) ||
                    !DescontoValidoVlrUnitarioComImpostos(item))
                {
                    //item.vQtdItem = 0;
                }

                CalculoValorSubTotal(item);
                //CalculoDescontoPorPorcent(item, item.pDesconto);
                CalculoValorSubTotalSemImpostos(item);
                CalculoValorComissao(item);
                item.SetDetalheItem();
            }
            catch (Exception ex)
            {
                GoogleInsightsReportingConstants.TrakException("Erro ao atualizar valores", ex.Message, true);
            }
        }
    }
}
