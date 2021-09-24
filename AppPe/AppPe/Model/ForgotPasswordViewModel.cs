using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Annotations;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.View;

namespace Xamarin.HLP.Mobile.AppPE.Model
{
    public class ForgotPasswordViewModel : INotifyPropertyChanged
    {

        public ForgotPasswordViewModel()
        {

            VoltarCommand = new Command(() =>
            {
                Application.Current.MainPage = new PageLogin();
            });

            imageLogin = $"Login3".ToImagemPNG();

            LembarSenhaCommand = new Command(LembrarSenha);

            xUrl = App.UrlWebApi;
        }

        public string Email
        {
            get { return _email; }
            set { _email = value; NotifyPropertyChanged(); }
        }


        public string xUrl { get; set; }


        public async void LembrarSenha()
        {
            try
            {
                if (!Email.IsValidEmailAddress()) return;
                this.isBusy = true;

                var objRetorno = await UtilHttp.PostForgot(this);

                isBusy = false;

                if (objRetorno.success)
                {
                    await
                        App.Messages.ShowAsync(
                            "E-mail enviado para a sua conta de e-mail com instruções a serem seguidas para a alteração da sua senha");
                    Application.Current.MainPage = new PageLogin();
                }
                else
                {
                    await
                       App.Messages.ShowAsync(
                           "Conta de e-mail inexistente.");
                }

            }
            catch (Exception ex)
            {
                await App.Messages.ShowAsync(ex.Message);
            }
        }

        private ImageSource _imageLogin;
        [IgnoreDataMember]
        public ImageSource imageLogin
        {
            get { return _imageLogin; }
            set { _imageLogin = value; NotifyPropertyChanged(); }
        }



        [IgnoreDataMember]
        public ICommand VoltarCommand { get; set; }
        [IgnoreDataMember]
        public ICommand LembarSenhaCommand { get; set; }

        private bool _isBusy;
        private string _email;

        [IgnoreDataMember]
        public bool isBusy
        {
            get { return _isBusy; }
            set { _isBusy = value; NotifyPropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
