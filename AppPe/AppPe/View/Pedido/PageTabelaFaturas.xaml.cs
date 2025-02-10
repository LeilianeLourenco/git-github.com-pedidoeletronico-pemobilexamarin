using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Controls.xaml;
using Xamarin.HLP.Mobile.AppPE.Model;
using Xamarin.HLP.Mobile.AppPE.Model.Financeiro;
using Xamarin.HLP.Mobile.AppPE.ViewModel.Pedido;

namespace Xamarin.HLP.Mobile.AppPE.View.Pedido
{
    public partial class PageTabelaFaturas : ContentPage
    {
        PedidoNewViewModel _page;

        public PageTabelaFaturas(PedidoNewViewModel page)
        {
            InitializeComponent();

            page.currentModel.dtInicial = page.currentModel.dtInicial == null ? page.currentModel.dEmissao : page.currentModel.dtInicial;
            page.currentModel.dtFinal = page.currentModel.dtFinal == null ? page.currentModel.dEmissao.AddDays(30) : page.currentModel.dtFinal;

            _page = page;
            BindingContext = _page;

            FaturasGeradas();
        }

        private void btnGerar(object sender, EventArgs e)
        {
            int parcelas;
            if (int.TryParse(EntryParcelas.Text, out parcelas))
            {
                int totalDias = (_page.currentModel.dtFinal.Value.Date - _page.currentModel.dtInicial.Value.Date).Days;

                if (parcelas < 50 && _page.vSubTotal > 0 && totalDias > 0)
                {
                    var dias = totalDias / parcelas;

                    StackPageFaturas.Children.Clear();
                    _page.lRecebimentoTitulosManualModel.Clear();
                    DateTime? dtVencimento = null;

                    double juros = Convert.ToDouble(_page.dAcrescimoMensal) / 100;
                    juros = juros / 30;

                    double valorPedido = _page.vSubTotalOriginal;
                    valorPedido = valorPedido * Math.Pow((1 + juros), totalDias);

                    double valorParcela = valorPedido / parcelas;

                    for (int i = 1; i <= parcelas; i++)
                    {
                        dtVencimento = dtVencimento != null ? dtVencimento.Value.AddDays(dias) : _page.currentModel.dtInicial.Value.AddDays(dias - 1);
                        var parcelaBase = Convert.ToDecimal(valorParcela);

                        RecebimentoTitulosPostModel recebimento = new RecebimentoTitulosPostModel
                        {
                            idPedidoVenda = _page.currentModel.idPedidoVenda ?? _page.currentModel.idPedidoVendaOffLine ?? 0,
                            idEmpresa = _page.currentModel.idEmpresa,
                            nParcela = i,
                            nSequencia = i,
                            vRecebido = 0,
                            vTitulo = parcelaBase,
                            xTitulo = parcelaBase.ToString("F2"),
                            vBaseComissao = Convert.ToDecimal(_page.vTotalComissao / parcelas),
                            pComissao = 0,
                            dtBaseComissao = dtVencimento ?? DateTime.UtcNow.AddHours(-3),
                            dtEmissao = DateTime.UtcNow.AddHours(-3),
                            dtVencimento = dtVencimento ?? DateTime.UtcNow.AddHours(-3),
                            xDtsVencimento = dtVencimento.ToString(),
                        };

                        StackPageFaturas.Children.Add(new PageFaturas()
                        {
                            BindingContext = recebimento
                        });

                        _page.lRecebimentoTitulosManualModel.Add(recebimento);
                    }

                    _page.ItemCondicaoPgto.Id = 0;
                    _page.currentModel.idCondicaoPagamento = 0;
                    _page.vSubTotal = Math.Round(valorPedido, 2, MidpointRounding.AwayFromZero);
                    _page.vJuros = _page.vSubTotal - _page.vSubTotalOriginal;
                    _page.ItemCondicaoPgto.Display = "Configurado manualmente";
                }

                RateioItens();
            }
        }

        public void RateioItens()
        {
            foreach (var item in _page.currentModel.lItens)
            {
                var representa = item.vUnitarioVendaComImpostos / _page.vSubTotalOriginal;
                item.vUnitarioVendaComImpostos = representa * _page.vSubTotal;
            }
        }

        private void FaturasGeradas()
        {
            foreach (var lin in _page.lRecebimentoTitulosManualModel)
            {
                StackPageFaturas.Children.Add(new PageFaturas()
                {
                    BindingContext = lin
                });
            }
        }
    }
}
