using System;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.ViewModel.Pedido;

namespace Xamarin.HLP.Mobile.AppPE.View.Pedido
{
    public partial class PageFinanceiroCliente : ContentPage
    {
          
        public PageFinanceiroClienteViewModel ViewModel => BindingContext as PageFinanceiroClienteViewModel;

        public PageFinanceiroCliente(int idClienteOffLine)
        {
            try
            { 
                InitializeComponent();
                NavigationPage.SetHasBackButton(this, true);
                ViewModel.idClienteOffline = idClienteOffLine; 
            }
            catch (Exception ex)
            {
                ex.TrakException();
            }
        }

        private void LoadMoreInfo(object sender, EventArgs e)
        {
            if (!ViewModel.bPararBusca)
            {
                ViewModel.LoadItens(ViewModel.nUltimaPaginaBuscada + 1); 
            }
        }

        protected override void OnAppearing()
        {
            try
            {
                base.OnAppearing();
                ViewModel.canExecuteInicial = true;
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
    }
}