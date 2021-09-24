using Rg.Plugins.Popup.Pages;
using Xamarin.HLP.Mobile.AppPE.View.Home;
using Xamarin.HLP.Mobile.AppPE.ViewModel.Sincronizacao;

namespace Xamarin.HLP.Mobile.AppPE.View.Sincronizacao
{
    public partial class PageSyncNew : PopupPage
    {
        public PageSyncNew()
        {
            InitializeComponent();
            CloseWhenBackgroundIsClicked = false;
            App.ParamBackButtonPressed?.SetParameter(false);
        }

        public SincronizacaoNewViewModel ViewModel => BindingContext as SincronizacaoNewViewModel;


        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (!ViewModel.IsBusy)
                ViewModel.InitSyncComplete();

        }

        protected override bool OnBackButtonPressed()
        {
            if (!ViewModel.IsBusy)
            {
                return base.OnBackButtonPressed();
            }
            App.Messages.ShowAsync("Aguarde a sincronização estar completa.");
            return true;
        }


        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            PageHomeNew.ViewModelStatic.ExecuttingAnyCommand = false;
        }
    }
}
