using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.ViewModel.Pedido;

namespace Xamarin.HLP.Mobile.AppPE.View.Pedido
{
    public partial class PageViewToPrint : PopupPage
    {
        public PageViewToPrint(int idPedidoVendaOffLine)
        {
            InitializeComponent();
            
            
            ViewModel.idPedidoVendaOffLine = idPedidoVendaOffLine;

            if(Device.RuntimePlatform == Device.iOS)
            {
                OSAppTheme currentTheme = Application.Current.RequestedTheme;
                if (currentTheme == OSAppTheme.Dark)
                {
                    FrameImpressao.BackgroundColor = Color.Black;
                }
            }           
        }


        public PedidoToPrintViewModel ViewModel => BindingContext as PedidoToPrintViewModel;


        protected override void OnAppearing()
        {
            base.OnAppearing();

            Device.StartTimer(UtilMethods.GetStartTime, ViewModel.Initialize);

        }

        private void BtnCompartilhar_Clicked(object sender, System.EventArgs e)
        {          
            var vm = (PedidoToPrintViewModel)BindingContext;
            vm.CompartilharPdfCommand.Execute(MeuStackLayout);
        }
    }
}
