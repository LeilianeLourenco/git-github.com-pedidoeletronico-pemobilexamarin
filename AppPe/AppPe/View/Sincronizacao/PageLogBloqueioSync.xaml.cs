using System;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Model.Repository;
using Xamarin.HLP.Mobile.AppPE.View.Empresa;

namespace Xamarin.HLP.Mobile.AppPE.View.Sincronizacao
{
    public partial class PageLogBloqueioSync : ContentPage
    {
        public PageLogBloqueioSync()
        {
            InitializeComponent();
        }

        private async void BtnEmpresas_Clicked(object sender, EventArgs e)
        {
            try
            {
                await Application.Current.MainPage.Navigation.PushAsync(new PageEmpresa());
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Erro", ex.ToString(), "OK");
            }
        }

        private void BtnSair_Clicked(object sender, EventArgs e)
        {
            if (App.CurrentAspnetUserModel?.objEmpresaAspnetUsersModel != null)
            {
                App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.stAtivo = false;
                LoginRepository.UpdateUser();
            }

            //LoginRepository.DesbloquearUser();
            UtilNavidate.EfetivarLogoff();
        }
    }
}
