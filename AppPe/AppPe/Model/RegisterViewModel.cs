using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Annotations;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Model.Repository;
using Xamarin.HLP.Mobile.AppPE.View;
using Xamarin.HLP.Mobile.AppPE.View.MainPage;

namespace Xamarin.HLP.Mobile.AppPE.Model
{
    public class RegisterViewModel : INotifyPropertyChanged
    {
        private string _email = "";
        private string _password = "";
        private string _confirmPassword = "";


        public RegisterViewModel()
        {
            RegistrarCommand = new Command(Registrar);
            VoltarCommand = new Command(() =>
            {
                Application.Current.MainPage = new PageLogin();
            });
        }

        public async void Registrar()
        {
            try
            {
                bValidPassword = (_password ?? "").Equals((_confirmPassword ?? ""));

                if (!bValidPassword)
                {
                    await App.Messages.ShowAsync("As senhas não conferem");
                    return;
                }

                if (!Email.IsValidEmailAddress())
                {
                    await App.Messages.ShowAsync("Email inválido");
                    return;
                }


                if (await App.IsConected() == false)
                {
                    await App.Messages.ShowAsync("Sem conexão com internet");
                    return;
                }
                this.isBusy = true;
                try
                {
                    var objreturn = await UtilHttp.PostRegister(this);

                    if (objreturn != null)
                    {
                        if (objreturn.status == SignInStatus.Success)
                        {
                            var currentAspnetUserModel =
                                        LoginRepository.SaveAspnetUsers(model: objreturn.objModel);
                            App.CurrentAspnetUserModel = currentAspnetUserModel;
                            isBusy = false;
                            Application.Current.MainPage = new RootPage();
                        }
                        else
                        {
                            isBusy = false;
                            await App.Messages.ShowAsync(objreturn.xMessage);

                        }
                    }
                }
                catch (Exception ex)
                {
                    isBusy = false;
                    ex.TrakException();
                }

            }
            catch (Exception ex)
            {
                xMensagem = ex.Message;
                ex.TrakException();
            }
        }


        public string Email
        {
            get { return _email; }
            set { _email = value; NotifyPropertyChanged(); }
        }

        public string Password
        {
            get { return _password; }
            set { _password = value; NotifyPropertyChanged(); }
        }

        public string ConfirmPassword
        {
            get { return _confirmPassword; }
            set
            {
                _confirmPassword = value; NotifyPropertyChanged();
                bValidPassword = _password.Equals(_confirmPassword);
            }
        }






        #region IgnoreDataMember

        private bool _bValidPassword = true;
        [IgnoreDataMember]
        public bool bValidPassword
        {
            get { return _bValidPassword; }
            set { _bValidPassword = value; NotifyPropertyChanged(); }
        }

        private string _xMensagem;
        [IgnoreDataMember]
        public string xMensagem
        {
            get { return _xMensagem; }
            set { _xMensagem = value; NotifyPropertyChanged(); }
        }

        [IgnoreDataMember]
        public ICommand RegistrarCommand { get; set; }
        [IgnoreDataMember]
        public ICommand VoltarCommand { get; set; }

        private bool _isBusy;
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

        #endregion

    }
}
