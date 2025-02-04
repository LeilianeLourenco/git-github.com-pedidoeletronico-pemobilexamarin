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
                    _page.lRecebimentoTitulosModel.Clear();
                    DateTime? dtUltimaParcela = null;

                    double juros = 2.2 / 100;
                    juros = juros / 30;
                    juros = Math.Truncate(juros * 100000000) / 100000000;

                    double valorPedido = _page.vSubTotal;
                    valorPedido = Math.Floor(valorPedido * 100000) / 100000;

                    valorPedido = valorPedido * Math.Pow((1 + juros), totalDias);
                    valorPedido = Math.Floor(valorPedido * 100000) / 100000;

                    double valorParcela = valorPedido / parcelas;
                    valorParcela = Math.Floor(valorParcela * 100000) / 100000;
                    valorParcela = Math.Round(valorParcela, 2);

                    for (int i = 1; i <= parcelas; i++)
                    {
                        dtUltimaParcela = dtUltimaParcela != null ? dtUltimaParcela.Value.AddDays(dias) : _page.currentModel.dtInicial.Value.AddDays(dias - 1);
                        var parcelaBase = Convert.ToDecimal(valorParcela);

                        //if (i == parcelas)
                        //    parcelaBase = total - (parcelaBase * (parcelas - 1));

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
                            dtBaseComissao = dtUltimaParcela ?? DateTime.UtcNow.AddHours(-3),
                            dtEmissao = DateTime.UtcNow.AddHours(-3),
                            dtVencimento = dtUltimaParcela ?? DateTime.UtcNow.AddHours(-3),
                            xDtsVencimento = dtUltimaParcela.Value.ToString("dd/MM/yyyy"),
                        };

                        StackPageFaturas.Children.Add(new PageFaturas()
                        {
                            BindingContext = recebimento
                        });

                        _page.lRecebimentoTitulosModel.Add(recebimento);
                    }

                    _page.vSubTotal = valorPedido;
                    _page.ItemCondicaoPgto.Display = $"Configurado manualmente - {parcelas}x";
                }
            }
        }

        private void FaturasGeradas()
        {
            foreach (var lin in _page.lRecebimentoTitulosModel)
            {
                StackPageFaturas.Children.Add(new PageFaturas()
                {
                    BindingContext = lin
                });
            }
        }
    }
}
