using System;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Model;
using Xamarin.HLP.Mobile.AppPE.Model.Repository;
using Xamarin.HLP.Mobile.AppPE.View.Home;
using Xamarin.HLP.Mobile.AppPE.View.Login;
using Xamarin.HLP.Mobile.AppPE.View.MainPage;
using Xamarin.HLP.Mobile.AppPE.View.Sincronizacao;

namespace Xamarin.HLP.Mobile.AppPE.Common
{
    public class UtilNavidate
    {

        public static async void PushModalAsync2(Page page)
        {
            try
            {
                var main = Application.Current.MainPage as NavigationPage;
                if (main != null)
                {
                    await main.Navigation.PushModalAsync(page);
                }
            }
            catch (Exception ex)
            {
                GoogleInsightsReportingConstants.TrakException("PushModalAsync", ex.Message, true);
            }
        }

        public static async void PushModalAsync(Page page)
        {
            try
            {
                var main = Application.Current.MainPage as RootPage;
                if (main != null)
                {
                    await main.Detail.Navigation.PushModalAsync(page);
                }
            }
            catch (Exception ex)
            {
                GoogleInsightsReportingConstants.TrakException("PushModalAsync", ex.Message, true);
            }
        }


        public static async void PopModalAsync()
        {
            await Task.Yield();
            var main = Application.Current.MainPage as MasterDetailPage;
            if (main != null)
                await main.Detail.Navigation.PopModalAsync();
        }

        public static async void PopModalAsync2()
        {
            await Task.Yield();
            var main = Application.Current.MainPage as NavigationPage;
            if (main != null)
                await main.Navigation.PopModalAsync();
        }


        public static async void PushAsync(Page page)
        {
            try
            {
                var main = Application.Current.MainPage as RootPage;
                if (main != null)
                {
                    Task.Yield();
                    await main.Detail.Navigation.PushAsync(page);
                }
            }
            catch (Exception ex)
            {
                GoogleInsightsReportingConstants.TrakException("PushAsync", ex.Message, true);
            }
        }

        public static async void PopAsync()
        {
            await Task.Yield();
            var main = Application.Current.MainPage as MasterDetailPage;
            if (main != null)
                await main.Detail.Navigation.PopAsync();
        }


        public static async void GoToHome()
        {

            Device.BeginInvokeOnMainThread(async () =>
            {
                await Task.Yield();
                StaticModel.SetNewPageHome();
                var main = Application.Current.MainPage as MasterDetailPage;
                if (main != null)
                    main.Detail = new NavigationPage(StaticModel.PageHome);
            });
        }


        public static Type GetTypeCurrentPage()
        {
            var main = Application.Current.MainPage as MasterDetailPage;
            if (main != null)
            {
                var navigationPage = main.Detail as NavigationPage;
                if (navigationPage != null) return navigationPage.CurrentPage.GetType();
            }
            return null;
        }


        public static async void Sincronizar(PageSyncNew page)
        {
            var main = Application.Current.MainPage as RootPage;
            if (main != null)
            {
                main.Detail.Opacity = 0.4;
            }

            //var page = new PageSyncNew();
            //page.ViewModel.bForcarSyncInit = bSincronizarTudo;

            await App.Navigation.PushPopupAsync(page, animate: true);
        }


        public static async void ShowPopupNew(PopupPage page)
        {
            await App.Navigation.PushPopupAsync(page, animate: true);
        }

        public static async void PopPopupNew()
        {
            await App.Navigation.PopPopupAsync();
        }



        public static async void Logoff()
        {
            try
            {
                if (!await App.Messages.ShowConfirmAsync("Deseja realmente se desconectar ?")) return;
                EfetivarLogoff();
            }
            catch (Exception ex)
            {
                ex.TrakException();
            }
        }

        public static void EfetivarLogoff()
        {
            try
            {
                LoginRepository.Loggout();
                //PageHome.HomeViewModel = null;
                PageHomeNew.ViewModelStatic = null;
                Application.Current.MainPage = new NavigationPage(new PageBeforeLogin());
            }
            catch (Exception ex)
            {
                ex.TrakException();
            }
        }


    }
}
