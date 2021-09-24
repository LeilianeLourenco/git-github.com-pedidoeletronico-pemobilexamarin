using System;
using FFImageLoading.Forms.Args;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Model;
using Xamarin.HLP.Mobile.AppPE.Model.Repository;
using Xamarin.HLP.Mobile.AppPE.ViewModel.Cadastro;

namespace Xamarin.HLP.Mobile.AppPE.View.Cliente
{
    public partial class PageApresentacaoClienteNew : ContentPage
    {
        public static ClienteApresentacaoNewViewModel ViewModelStatic { get; set; }
        public PageApresentacaoClienteNew(int _idClientesOffLine)
        {
            try
            {
                InitializeComponent();
                GoogleInsightsReportingConstants.TrakPage(GoogleInsightsReportingConstants.InPage.PAGE_APRESENTACAO_CLIENTE);
                ViewModel.idClientesOffLine = _idClientesOffLine;
                ViewModelStatic = ViewModel;
            }
            catch (Exception ex)
            {
                App.Messages.ShowAsync(ex.Message);
            }

        }



        private ClienteApresentacaoNewViewModel ViewModel => BindingContext as ClienteApresentacaoNewViewModel;


        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (ViewModel.pageCliente != null)
            {
                if (ViewModel.pageCliente.ViewModel.needRestore)
                {
                    ViewModel.currentModel = ClienteRepository.GetClienteModel(ViewModel.idClientesOffLine);
                }
            }

            Device.StartTimer(UtilMethods.GetStartTime, ViewModel.Initialize);
        }

      

    }
}
