using System;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Model;
using Xamarin.HLP.Mobile.AppPE.ViewModel;

namespace Xamarin.HLP.Mobile.AppPE.View
{
    public partial class PageLogin : ContentPage
    {
        public PageLogin()
        {
            try
            {
                InitializeComponent();

                EntryUsuario.Completed += (sender, e) => { EntrySenha.Focus(); };
                EntrySenha.Completed += (sender, e) =>
                {
                    if (!ViewModel.currentModel.LoginCommand.CanExecute(null)) return;
                    ButtonEntrar.Focus();
                    ViewModel.currentModel.LoginCommand.Execute(null);
                };

                ViewModel.currentModel.ControleNavigationCommand = new Command(() =>
                {
                    if (ViewModel.currentModel.BProcessando)
                    {
                        NavigationPage.SetHasBackButton(this, false);
                    }
                    else
                    {
                        NavigationPage.SetHasBackButton(this, true);
                    }
                });

            }
            catch (Exception ex)
            {
                App.Messages.ShowAsync($"{ex.Message } - {ex.InnerException?.Message}");
            }


        }


       

        public InicioViewModel ViewModel => (BindingContext as InicioViewModel);

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Device.StartTimer(UtilMethods.GetStartTime, ViewModel.Initialize);
            GoogleInsightsReportingConstants.TrakPage(GoogleInsightsReportingConstants.InPage.PAGE_LOGIN);

        }

        protected override bool OnBackButtonPressed()
        {
            if (!ViewModel.currentModel.BProcessando)
            {
                return base.OnBackButtonPressed();
            }
            App.Messages.ShowAsync("Aguarde um pouco, estamos realizando seu login.");
            return true;
        }
    }
}
