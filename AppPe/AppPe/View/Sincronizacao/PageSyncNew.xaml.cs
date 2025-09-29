using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Controls.xaml;
using Xamarin.HLP.Mobile.AppPE.Model.Sincronizacao;
using Xamarin.HLP.Mobile.AppPE.View.Home;
using Xamarin.HLP.Mobile.AppPE.ViewModel.Sincronizacao;

namespace Xamarin.HLP.Mobile.AppPE.View.Sincronizacao
{
    public partial class PageSyncNew : PopupPage
    {
        private string _typeSync;
        public PageSyncNew(string typeSync)
        {
            _typeSync = typeSync;

            InitializeComponent();
            CloseWhenBackgroundIsClicked = false;
            App.ParamBackButtonPressed?.SetParameter(false);
        }

        public SincronizacaoNewViewModel ViewModel => BindingContext as SincronizacaoNewViewModel;


        protected override void OnAppearing()
        {
            base.OnAppearing();

            MessagingCenter.Subscribe<SincronizacaoNewModel>(this, "SyncAtt", (sync) =>
            {
                if (ViewModel?.currentModel != null)
                {
                    ViewModel.currentModel.xDetail = sync.xDetail;
                    ViewModel.currentModel.iCount = sync.iCount;
                }
            });

            MessagingCenter.Subscribe<object>(this, "SyncFinalizada", async (sender) =>
            {
                var masterDetail = Application.Current.MainPage as MasterDetailPage;
                if (masterDetail != null)
                {
                    Page currentPage = null;

                    if (masterDetail.Detail is NavigationPage navPage)
                        currentPage = navPage.CurrentPage;
                    else
                        currentPage = masterDetail.Detail;

                    if (currentPage != null)
                    {
                        var novaPagina = (Page)Activator.CreateInstance(currentPage.GetType());
                        masterDetail.Detail = new NavigationPage(novaPagina);

                        masterDetail.IsPresented = false;
                    }

                    if (PopupNavigation.Instance.PopupStack.Any())
                        await App.Navigation.PopPopupAsync();
                }
            });

            if (!ViewModel.IsBusy)
            {
                if (_typeSync == "Assinatura")
                    ViewModel.SyncAssnaturaPedido();
                else
                {
                    var syncService = DependencyService.Get<IBackgroundSyncService>();
                    syncService?.StartSync();
                }
            }
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
