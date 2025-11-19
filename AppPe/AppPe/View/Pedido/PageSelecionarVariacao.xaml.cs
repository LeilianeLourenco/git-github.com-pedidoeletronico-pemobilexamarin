using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Controls.xaml.ListagemProdutoPedido;
using Xamarin.HLP.Mobile.AppPE.Model.Lancamento;
using Xamarin.HLP.Mobile.AppPE.Model.Lancamento.Behaviors;
using Xamarin.HLP.Mobile.AppPE.Model.Repository;
using Xamarin.HLP.Mobile.AppPE.ViewModel.Pedido;

namespace Xamarin.HLP.Mobile.AppPE.View.Pedido
{
    public partial class PageSelecionarVariacao : ContentPage
    {
        public int _idProduto;

        public PageSelecionarVariacao(ListarVariacoesPedidoViewModel viewModel,
            ListarProdutosNewViewModel produtosNewViewModel, int idProduto)
        {
            InitializeComponent();

            viewModel.currentPedidoViewModel = produtosNewViewModel.currentPedidoViewModel;
            BindingContext = viewModel;

            _idProduto = idProduto;
        }

        public ListarVariacoesPedidoViewModel ViewModel => BindingContext as ListarVariacoesPedidoViewModel;

        protected override void OnAppearing()
        {
            try
            {
                base.OnAppearing();
              
                if (ViewModel.itemSelected != null)
                {
                    ViewModel.itemSelected.editting = false;
                    var item = ViewModel.itemSelected;
                    ListViewDados.SelectedItem = null;
                    ListViewDados.SelectedItem = item;
                }

                Device.BeginInvokeOnMainThread(() =>
                {
                    Device.StartTimer(UtilMethods.GetStartTime, () => ViewModel.Initialize(_idProduto));
                });
            }
            catch (Exception ex)
            {
                ex.TrakException();
            }
        }

        private async void ListViewDados_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as PedidoVendaItensModel;

            try
            {
                if (item != null)
                {
                    item.editting = false;

                    if (ListViewDados.ItemTemplate.GetType() == typeof(DataTemplateBasic))
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            ViewModel.bListaItensHabilitada = false;
                        });

                        ViewModel.currentPedidoViewModel.currentModel.CurrentItemModel = item;
                        ViewModel.currentPedidoViewModel.currentModel.CurrentItemModel?.PublicNotifyPropertyChanged(
                            "pDesconto");
                        if (ViewModel.currentPedidoViewModel.currentModel.CurrentItemModel != null)
                        {
                            ViewModel.HasGradeSelected =
                                ViewModel.currentPedidoViewModel.currentModel.CurrentItemModel.HasGrade;
                            ViewModel.xtitleButtonEditar =
                                ViewModel.currentPedidoViewModel.currentModel.CurrentItemModel.HasGrade
                                    ? "Grade"
                                    : "Editar";

                            if (ViewModel.currentPedidoViewModel.currentModel.CurrentItemModel.bTabelasCarregadas == false)
                            {
                                Device.BeginInvokeOnMainThread(() =>
                                {
                                    IsBusy = true;
                                });

                                await Task.Run(() =>
                                {
                                    TabelaPrecoRepository.SetTabelaPrecoByProduto(ViewModel.currentPedidoViewModel.currentModel.CurrentItemModel,
                                    ViewModel.currentPedidoViewModel.currentModel.idClientesOffLine,
                                    ClienteRepository.GetIdClienteNuvem(ViewModel.currentPedidoViewModel.currentModel.idClientesOffLine),
                                    ViewModel.currentPedidoViewModel.currentModel.idRepresentantePedido ?? 0, ViewModel.currentPedidoViewModel.idTabelaPrecoCondicao);
                                    ProdutoRepository.SetComissao(item: ViewModel.currentPedidoViewModel.currentModel.CurrentItemModel);
                                });

                                Device.BeginInvokeOnMainThread(() =>
                                {
                                    IsBusy = false;
                                });

                            }


                            if (item.pComissao == 0)
                            {
                                Device.BeginInvokeOnMainThread(() =>
                                {
                                    IsBusy = true;
                                });

                                await Task.Run(() =>
                                {
                                    ProdutoRepository.SetComissao(item: ViewModel.currentPedidoViewModel.currentModel.CurrentItemModel);
                                });

                                Device.BeginInvokeOnMainThread(() =>
                                {
                                    IsBusy = false;
                                });
                            }

                            if (ViewModel.currentPedidoViewModel.currentModel.CurrentItemModel.bLocaisCarregados == false)
                            {
                                Device.BeginInvokeOnMainThread(() =>
                                {
                                    IsBusy = true;
                                });

                                try
                                {

                                    PedidoRepository.SetLocalEstoque(ViewModel.currentPedidoViewModel.currentModel.CurrentItemModel,
                                            ClienteRepository.GetIdClienteNuvem(ViewModel.currentPedidoViewModel.currentModel.idClientesOffLine),
                                            ViewModel.currentPedidoViewModel.currentModel.idRepresentantePedido ?? 0);


                                }
                                catch (Exception ex)
                                {

                                }

                                Device.BeginInvokeOnMainThread(() =>
                                {
                                    IsBusy = false;
                                });

                            }

                            ViewModel.editarItemViewModel = new EditarItemViewModel();
                            PedidoVendaCalculos.SumTotalizadoresPageEditarItem();
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                ViewModel.bListaItensHabilitada = true;
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        public async void StepperValor_OnValueChanged(object sender, ValueChangedEventArgs e)
        {
            try
            {
                if (Math.Abs(e.NewValue - e.OldValue) > 0)
                {
                    var item = sender as Stepper;

                    if (item.BindingContext != null)
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

                if (item != null && !PageListarProdutosNew.currentViewModel.IsBusy)
                {
                    PedidoVendaItensModel itemPedido = item.BindingContext as PedidoVendaItensModel;

                    if (itemPedido.pStVenda == 0)
                        itemPedido.pStVenda = itemPedido?.currentTabelaPreco?.pStVenda;

                    if (itemPedido.pIpiVenda == 0)
                        itemPedido.pIpiVenda = itemPedido?.currentTabelaPreco?.pIpiVenda;

                    bool stVendaSemEstoque = itemPedido?.stVendaSemEstoque ?? false;
                    bool stControlaEstoque = ProdutoRepository.ControlaEstoque(itemPedido.idEmpresa, itemPedido.idRepresentada);

                    if (item?.Value <= itemPedido?.vQtdEstoque || stVendaSemEstoque || !stControlaEstoque)
                    {
                        itemPedido.vQtdItem = item.Value;

                        var listViewModel = PageListarProdutosNew.currentViewModel;
                        var pedidoViewModel = PagePedidoNew.CurrentViewModel;

                        if (listViewModel != null)
                            listViewModel.itemSelected = itemPedido;

                        pedidoViewModel.currentModel.CurrentItemModel = itemPedido;
                        if (itemPedido != null)
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
