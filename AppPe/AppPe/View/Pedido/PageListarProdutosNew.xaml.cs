using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Controls.custom;
using Xamarin.HLP.Mobile.AppPE.Controls.xaml.ListagemProdutoPedido;
using Xamarin.HLP.Mobile.AppPE.Model.Lancamento;
using Xamarin.HLP.Mobile.AppPE.Model.Lancamento.Behaviors;
using Xamarin.HLP.Mobile.AppPE.Model.Repository;
using Xamarin.HLP.Mobile.AppPE.ViewModel.Pedido;
using ZXing.Net.Mobile.Forms;

namespace Xamarin.HLP.Mobile.AppPE.View.Pedido
{
    public partial class PageListarProdutosNew : ContentPage
    {


        public static ListarProdutosNewViewModel currentViewModel { get; set; }

        public PageListarProdutosNew(PedidoNewViewModel pedido)
        {
            try
            {
                InitializeComponent();
                if (Device.RuntimePlatform == Device.UWP || Device.RuntimePlatform == Device.WPF)
                {
                    GridPesquisaBarCode.IsVisible = false;
                }

                NavigationPage.SetHasBackButton(this, false);
                currentViewModel = ViewModel;
                ViewModel.currentPedidoViewModel = pedido;
                ViewModel.controlSearchPE = SearchBarPesquisa;

                btnExecutePesquisaBarCode.Command = new Command(() =>
                {
                    ReadBarCode();
                });

                SetTemplateLista();

                ToolbarItemSave.Command = new Command(async () =>
                {
                    await IsValidPage(true);
                    ViewModel.SaveItens();
                });

                if (!ViewModel.currentPedidoViewModel.currentModel.bMostraFaixaEscalonada)
                    ToolbarItems.Remove(toolbarEscalonada);
            }
            catch (Exception ex)
            {
                ex.TrakException();
            }

        }

        public async void ReadBarCode()
        {
            try
            {
                if (await UtilMethods.PermissionCamera())
                {
                    ZXingScannerPage scanPage = new ZXingScannerPage();
                    scanPage.AutoFocus();
                    scanPage.OnScanResult += (result) =>
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            SearchBarPesquisa.GetEntry().Text = ViewModel.xFiltro = result.Text;
                            ViewModel.SearchCommand?.Execute(null);
                            ListViewDados.Focus();
                            UtilNavidate.PopAsync();
                        });
                    UtilNavidate.PushAsync(scanPage);
                }
            }
            catch (Exception ex)
            {
                ex.TrakException();
            }


        }

        protected override void OnAppearing()
        {
            try
            {
                base.OnAppearing();
                if (ViewModel.Config.bNeedRefresh)
                {
                    ViewModel.Config.bNeedRefresh = false;
                    ViewModel.canExecuteInicial = true;
                }
                if (ViewModel.itemSelected != null)
                {
                    ViewModel.itemSelected.editting = false;
                    var item = ViewModel.itemSelected;
                    ListViewDados.SelectedItem = null;
                    ListViewDados.SelectedItem = item;
                }

                Device.BeginInvokeOnMainThread(() =>
                {
                    Device.StartTimer(UtilMethods.GetStartTime, ViewModel.Initialize);
                });
            }
            catch (Exception ex)
            {
                ex.TrakException();
            }
        }


        public ListarProdutosNewViewModel ViewModel => BindingContext as ListarProdutosNewViewModel;


        private async void ListViewDados_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as PedidoVendaItensModel;


            if (item != null)
            {
                if (ViewModel.editarItemViewModel != null)
                    await IsValidPage(true);

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

                            await Task.Run(() =>
                            {
                                PedidoRepository.SetLocalEstoque(ViewModel.currentPedidoViewModel.currentModel.CurrentItemModel,
                                        ClienteRepository.GetIdClienteNuvem(ViewModel.currentPedidoViewModel.currentModel.idClientesOffLine),
                                        ViewModel.currentPedidoViewModel.currentModel.idRepresentantePedido ?? 0);

                            });

                            Device.BeginInvokeOnMainThread(() =>
                            {
                                IsBusy = false;
                            });

                        }



                        ViewModel.editarItemViewModel = new EditarItemViewModel();
                        PedidoVendaCalculos.SumTotalizadoresPageEditarItem();
                        CarregarListaPreco();
                        CarregarListaLocais();
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            ViewModel.bListaItensHabilitada = true;
                        });
                    }
                }
            }
        }


        private async Task<bool> IsValidPage(bool zerarvalores = false)
        {
            if (ViewModel.CurrentTabelaPreco == null)
            {
                return false;
            }

            if (App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.stAdministrador)
                return true;

            var valorUnitario = (EntryValorUnitario.Behaviors[0] as ValorUnitarioComImpostosBehaviors);
            var pdesconto = (EntryDesconto.Behaviors[0] as DescontoItemBehaviors);
            var vdesconto = (EntryValorDesconto.Behaviors[0] as DescontoItemBehaviors);        
            if (ViewModel.editarItemViewModel == null)
            {
                return false;
            }
            return await ViewModel.editarItemViewModel.ValidateCamposTask(zerarvalores, valorUnitario, pdesconto, vdesconto);
        }

        private void CarregarListaPreco()
        {
            if (ViewModel.editarItemViewModel != null)
                Device.BeginInvokeOnMainThread(() =>
            {
                ViewModel.ListaTabelaPreco = ViewModel.editarItemViewModel.ListaTabelaPreco;

                if (Device.OS == TargetPlatform.WinPhone || Device.OS == TargetPlatform.Windows)
                {
                    StackLayoutListaPreco.Children.Clear();
                    var bindable = new BindablePickerV2 { DisplayMemberPath = "Display" };
                    bindable.SetBinding(BindablePickerV2.SelectedItemProperty, new Binding("CurrentTabelaPreco"));
                    bindable.SetBinding(BindablePickerV2.ItemsSourceProperty, new Binding("ListaTabelaPreco"));
                    bindable.BindingContext = ViewModel;
                    StackLayoutListaPreco.Children.Add(bindable);
                }
                Device.StartTimer(new TimeSpan(0, 0, 0, 0, 100), SelectListaPreco);
            });
        }

        private void CarregarListaLocais()
        {
            if (ViewModel.editarItemViewModel != null)
                Device.BeginInvokeOnMainThread(() =>
                {
                    ViewModel.ListaLocalEstoque = ViewModel.editarItemViewModel.ListaLocalEstoque;

                    if (Device.OS == TargetPlatform.WinPhone || Device.OS == TargetPlatform.Windows)
                    {
                        StackLayoutLocalEstoque.Children.Clear();
                        var bindable = new BindablePickerV2 { DisplayMemberPath = "Display" };
                        bindable.SetBinding(BindablePickerV2.SelectedItemProperty, new Binding("CurrentLocalEstoque"));
                        bindable.SetBinding(BindablePickerV2.ItemsSourceProperty, new Binding("ListaLocalEstoque"));
                        bindable.BindingContext = ViewModel;
                        StackLayoutLocalEstoque.Children.Add(bindable);
                    }
                    Device.StartTimer(new TimeSpan(0, 0, 0, 0, 100), SelectLocalEstoque);
                });
        }

        private bool SelectListaPreco()
        {
            ViewModel.CurrentTabelaPreco = ViewModel.editarItemViewModel.CurrentTabelaPreco;
            return false;
        }

        private bool SelectLocalEstoque()
        {
            ViewModel.CurrentLocalEstoque = ViewModel.editarItemViewModel.CurrentLocalEstoque;
            return false;
        }

        private void EntryQtde_OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            ViewModel.SaveItem();
        }

        private void EntryQtde_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            ViewModel.VerificaQtdEstoque(e.OldTextValue);
        }

        private async void MenuItem_OnClicked(object sender, EventArgs e)
        {
            try
            {
                var tipo = await App.Messages.TipoListagem();

                if (tipo != null)
                {
                    App.EnvironmentPE.TipoPageProdutos = Convert.ToByte(tipo);
                    SetTemplateLista();
                    LoginRepository.UpdateUser();
                    UtilNavidate.PopAsync();

                    ViewModel.currentPedidoViewModel.ExecuttingAnyCommand = true;
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        UtilNavidate.PushAsync(new PageListarProdutosNew(ViewModel.currentPedidoViewModel));
                    });

                }
            }
            catch (Exception ex)
            {
                ex.TrakException();
            }
        }

        private void ButtonFiltros_OnClicked(object sender, EventArgs e)
        {
            if (ViewModel.IsBusy)
                return;

            var _buttonClicked = sender as Button;
            foreach (var buttons in ScrollButtonsFiltros.Children)
            {
                var _btn = buttons as Button;

                if (_buttonClicked.Text == _btn.Text)
                {

                    if (Device.RuntimePlatform != Device.iOS)
                    {
                        _btn.TextColor = Color.FromHex("#FFFFFF");
                        _btn.BackgroundColor = Color.FromHex("#555555");
                    }
                    else
                        _btn.TextColor = Color.FromHex("#555555");
                }
                else
                {
                    _btn.TextColor = Color.FromHex("");
                    _btn.BackgroundColor = Color.FromHex("");
                }
            }

        }



        private void SetTemplateLista()
        {
            try
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    IsBusy = true;
                    if (App.EnvironmentPE.TipoPageProdutos == 0)
                    {
                        ListViewDados.ItemTemplate = new DataTemplateFull();
                        GridImpostos.IsVisible = false;
                    }
                    else if (App.EnvironmentPE.TipoPageProdutos == 1)
                    {
                        ListViewDados.ItemTemplate = new DataTemplateImageFull();
                        GridImpostos.IsVisible = false;
                    }
                    else if (App.EnvironmentPE.TipoPageProdutos == 2)
                    {
                        ListViewDados.ItemTemplate = new DataTemplateBasic();
                        GridImpostos.IsVisible = true;
                    }
                    IsBusy = false;
                });

            }
            catch (Exception ex)
            {
                ex.TrakException();
            }

        }

        private bool _canClose = true;
        protected override bool OnBackButtonPressed()
        {
            if (_canClose)
            {
                QuestionToBack();
            }
            return _canClose;
        }
        private async void QuestionToBack()
        {
            var answer = await App.Messages.ShowConfirmAsync("Deseja realmente sair da pesquisa sem salvar ?");
            if (answer)
            {
                _canClose = false;
                UtilNavidate.PopAsync();
            }
        }
    }




}


