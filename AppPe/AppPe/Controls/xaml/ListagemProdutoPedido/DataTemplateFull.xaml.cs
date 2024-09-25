using System;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Model.Lancamento;
using Xamarin.HLP.Mobile.AppPE.Model.Repository;
using Xamarin.HLP.Mobile.AppPE.View.Pedido;

namespace Xamarin.HLP.Mobile.AppPE.Controls.xaml.ListagemProdutoPedido
{
    public partial class DataTemplateFull : DataTemplate
    {
        public DataTemplateFull()
        {
            InitializeComponent();

        }

        public async void StepperValor_OnValueChanged(object sender, ValueChangedEventArgs e)
        {
            try
            {
                if (Math.Abs(e.NewValue - e.OldValue) > 0)
                {
                    var item = sender as Stepper;
                    await StepperOnChanged(item);

                    PageListarProdutosNew.currentViewModel.itemSelected = null;
                }
            }
            catch (Exception ex)
            {
                ex.TrakException("StepperValor_OnValueChanged");
                //GoogleInsightsReportingConstants.TrakException("Erro no evendo de Stepper do Item do pedido ", ex.Message, true);
            }
        }

        public static async Task StepperOnChanged(Stepper item)
        {
            try
            {
                if (PageListarProdutosNew.currentViewModel == null)
                    PageListarProdutosNew.currentViewModel = new ViewModel.Pedido.ListarProdutosNewViewModel();

                if (item != null)
                {
                    PedidoVendaItensModel itemPedido = item.BindingContext as PedidoVendaItensModel;

                    if (itemPedido.pStVenda == 0)
                        itemPedido.pStVenda = itemPedido.currentTabelaPreco.pStVenda;

                    bool stVendaSemEstoque = itemPedido?.stVendaSemEstoque ?? false;
                    if (item?.Value <= itemPedido?.vQtdEstoque || stVendaSemEstoque)
                    {
                        itemPedido.vQtdItem = item.Value;
                        var listViewModel = PageListarProdutosNew.currentViewModel;
                        var pedidoViewModel = PagePedidoNew.CurrentViewModel;

                        if (listViewModel != null)
                            listViewModel.itemSelected = itemPedido;

                        pedidoViewModel.currentModel.CurrentItemModel = itemPedido;
                        if (itemPedido != null)
                        {

                            var page = UtilNavidate.GetTypeCurrentPage();
                            if (page == typeof(PageListarProdutosNew) || page == typeof(PageListarProdutosByCliente) || page == typeof(PageEditarItem))
                            {
                                if (itemPedido.vUnitarioVendaComImpostos > 0)
                                {
                                    await PedidoVendaCalculos.CalculoByStepper();
                                    if (PagePedidoNew.CurrentViewModel.currentModel.CurrentItemModel != null)
                                    {
                                        PagePedidoNew.CurrentViewModel.currentModel.CurrentItemModel.NotifyTotalizadores();
                                        ProdutoRepository.SetComissao(item: PagePedidoNew.CurrentViewModel.currentModel.CurrentItemModel);
                                        PagePedidoNew.CurrentViewModel.AtualizaTotalizadoresPedido();
                                        listViewModel?.SaveItem();
                                        itemPedido.SetDetalheItem();
                                    }
                                }
                                else
                                {
                                    if (page != typeof(PageListarProdutosNew))
                                        PagePedidoNew.CurrentViewModel.currentModel.CurrentItemModel.vQtdItem = 0;
                                }
                            }
                            else
                            {
                                itemPedido.vQtdItem = 0;
                            }
                        }
                    }
                    else
                    {
                        if (itemPedido != null)
                        {
                            if (itemPedido.vQtdItem > 0)
                                itemPedido.vQtdItem -= 1;
                            await App.Current.MainPage.DisplayAlert("Erro", "Estoque insuficiente", "Ok");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ex.TrakException();
            }
        }
    }
}
