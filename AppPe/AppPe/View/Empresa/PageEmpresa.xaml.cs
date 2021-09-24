using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Model;
using Xamarin.HLP.Mobile.AppPE.ViewModel.Empresa;

namespace Xamarin.HLP.Mobile.AppPE.View.Empresa
{
    public partial class PageEmpresa : ContentPage
    {
        public PageEmpresa()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();


            Device.StartTimer(UtilMethods.GetStartTime, ViewModel.Initialize);

            GoogleInsightsReportingConstants.TrakPage(GoogleInsightsReportingConstants.InPage.PAGE_TROCAR_EMPRESA);
        }

        public EmpresaViewModel ViewModel => BindingContext as EmpresaViewModel;
    }
}
