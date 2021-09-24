using System;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Model.Repository;
using Xamarin.HLP.Mobile.AppPE.View.MainPage;
using Xamarin.HLP.Mobile.AppPE.View.Testes;

namespace Xamarin.HLP.Mobile.AppPE.View
{
    public partial class PageTeste : ContentPage
    {
        public PageTeste()
        {
            InitializeComponent();
        }

        private void Button_OnClicked(object sender, EventArgs e)
        {
            try
            {
                App.Data = new DataAccess();
                App.Data.PrimeiraAnalise();
            }
            catch (Exception ex)
            {
                App.Messages.ShowAsync(ex.Message);
            }
        }

        private void Button2_OnClicked(object sender, EventArgs e)
        {
            //UtilNavidate.PushAsync(new Page2());
            //Application.Current.MainPage = new Page2();

            try
            {
                if (LoginRepository.HasLogin())
                    App.CurrentAspnetUserModel = LoginRepository.GetAspnetUsers();
                else
                    Application.Current.MainPage = new RootPage();
            }
            catch (Exception ex)
            {

                App.Messages.ShowAsync(ex.Message);
            }
        }
        private void Button3_OnClicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new NavigationPage(new PageLogin());

        }
        private void Button4_OnClicked(object sender, EventArgs e)
        {
            try
            {
                App.Data = new DataAccess();
            }
            catch (Exception ex)
            {
                App.Messages.ShowAsync(ex.Message);
            }
        }
        private void Button5_OnClicked(object sender, EventArgs e)
        {
            //UtilNavidate.PushAsync(new PageRegister());
            Application.Current.MainPage = new Page5();
        }
        private void Button6_OnClicked(object sender, EventArgs e)
        {
            try
            {
                App.Data.PrimeiraAnalise();
            }
            catch (Exception ex)
            {
                App.Messages.ShowAsync(ex.Message);
            }
        }

        private void Button7_OnClicked(object sender, EventArgs e)
        {
            //UtilNavidate.PushAsync(new PageForgotPassword());
            Application.Current.MainPage = new PageLogin();
        }

        private void Button8_OnClicked(object sender, EventArgs e)
        {
            //UtilNavidate.PushAsync(new PageForgotPassword());
            Application.Current.MainPage = new PageTeste();
        }
    }
}
