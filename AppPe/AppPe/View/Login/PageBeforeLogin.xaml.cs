using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Model;

namespace Xamarin.HLP.Mobile.AppPE.View.Login
{
    public partial class PageBeforeLogin : ContentPage
    {
        public PageBeforeLogin()
        {
            InitializeComponent();
            GoogleInsightsReportingConstants.TrakPage(GoogleInsightsReportingConstants.InPage.PAGE_INICIO);

            ButtonEntrar.Command = new Command(async () =>
            {
                var main = Application.Current.MainPage as NavigationPage;
                if (main != null)
                {
                    await Task.Yield();
                    await main.Navigation.PushAsync(new PageLogin());
                }
            });

            ButtonCriarConta.Command = new Command(async () =>
            {
                //var main = Application.Current.MainPage as NavigationPage;
                //if (main != null)
                //{
                //    await Task.Yield();
                //    await main.Navigation.PushAsync(new PageRegister());
                //}
                await Browser.OpenAsync("https://www.pedidoeletronico.com/Account/Register", new BrowserLaunchOptions
                {
                    LaunchMode = BrowserLaunchMode.SystemPreferred,
                    TitleMode = BrowserTitleMode.Show,
                    PreferredToolbarColor = Color.AliceBlue,
                    PreferredControlColor = Color.Violet
                });
            });
        }
    }
}
