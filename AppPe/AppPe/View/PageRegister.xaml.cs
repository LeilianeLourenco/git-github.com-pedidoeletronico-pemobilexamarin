using System;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Model;

namespace Xamarin.HLP.Mobile.AppPE.View
{
    public partial class PageRegister : ContentPage
    {
        public PageRegister()
        {
            InitializeComponent();

            EntryUsuario.Completed += (sender, e) => { EntryPassword.Focus(); };

            EntryPassword.Completed += (sender, e) => { EntryConfirmar.Focus(); };

            EntryConfirmar.Completed += (sender, e) =>
            {
                if (!ViewModel.RegistrarCommand.CanExecute(null)) return;
                ButtonEntrar.Focus();
                ViewModel.RegistrarCommand.Execute(null);
            };

            GridCondicoes.Command = new Command(async() =>
            {
                try
                {
                    var main = Application.Current.MainPage as NavigationPage;
                    if (main != null)
                    {
                      await  Task.Yield();
                        await main.Navigation.PushAsync(new PageOpenHtml("TERMOS", "Termos de uso"));
                    }
                }
                catch (Exception ex)
                {
                    GoogleInsightsReportingConstants.TrakException("PushAsync", ex.Message, true);
                }
            });
        }

        public RegisterViewModel ViewModel => (BindingContext as RegisterViewModel);

        protected override void OnAppearing()
        {
            base.OnAppearing();
            GoogleInsightsReportingConstants.TrakPage(GoogleInsightsReportingConstants.InPage.PAGE_REGISTER);
        }

       
    }
}
