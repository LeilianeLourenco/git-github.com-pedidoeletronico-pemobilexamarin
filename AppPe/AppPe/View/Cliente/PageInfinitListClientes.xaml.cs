using System;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Model;
using Xamarin.HLP.Mobile.AppPE.ViewModel.Listagem;

namespace Xamarin.HLP.Mobile.AppPE.View.Cliente
{
    public partial class PageInfinitListClientes : ContentPage
    {
        public PageInfinitListClientes()
        {
            InitializeComponent();
            GoogleInsightsReportingConstants.TrakPage(GoogleInsightsReportingConstants.InPage.PAGE_LISTAR_CLIENTES);
            ViewModel.controlSearchPE = SearchBarPesquisa;
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
               UtilNavidate.PushAsync(new PageApresentacaoClienteNew(listItemModel.Id));
            }
            ListViewDados.SelectedItem = null;
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();

            ViewModel.canExecuteInicial = true;
            Device.StartTimer(UtilMethods.GetStartTime, ViewModel.Initialize);
        }

        public ClienteInfinitListViewModel ViewModel => BindingContext as ClienteInfinitListViewModel;

        private async void MenuItem_OnClicked(object sender, EventArgs e)
        {
            var menuItem = sender as MenuItem;
            var item = menuItem?.BindingContext as ListItemModel;
            if (item != null)
               await ViewModel.Remover(item.Id);
        }
    }
}
