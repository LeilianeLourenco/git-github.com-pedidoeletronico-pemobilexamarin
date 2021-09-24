using System;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Model;
using Xamarin.HLP.Mobile.AppPE.ViewModel.Produto;
using ZXing.Net.Mobile.Forms;

namespace Xamarin.HLP.Mobile.AppPE.View.Produto
{
    public partial class PageInfinitListProdutos : ContentPage
    {
        public PageInfinitListProdutos()
        {
            InitializeComponent();
            ViewModel.controlSearchPE = SearchBarPesquisa;

            if (Device.RuntimePlatform == Device.UWP || Device.RuntimePlatform == Device.WPF)
            {
                GridPesquisaBarCode.IsVisible = false;
            }

            btnExecutePesquisaBarCode.Command = new Command(() =>
            {
                ReadBarCode();
            });

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



        private void ListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null) return;
            var listItemModel = e.SelectedItem as ListItemModel;
            if (listItemModel != null)
            {
                //StaticModel.StaticClientesModel =
                //    ClienteRepository.GetClienteModel(listItemModel.Id);
                //UtilNavidate.PushAsync(new PageApresentacaoCliente());
                UtilNavidate.PushAsync(new PageApresentacaoProduto(listItemModel.Id));
            }
            ListViewDados.SelectedItem = null;
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();
            ViewModel.canExecuteInicial = true;
            Device.StartTimer(UtilMethods.GetStartTime, ViewModel.Initialize);
        }

        public ProdutoInfinitListViewModel ViewModel => BindingContext as ProdutoInfinitListViewModel;

        private async void MenuItem_OnClicked(object sender, EventArgs e)
        {
            var menuItem = sender as MenuItem;
            var item = menuItem?.BindingContext as ListItemModel;
            if (item != null)
                await ViewModel.Remover(item.Id);
        }

    }
}
