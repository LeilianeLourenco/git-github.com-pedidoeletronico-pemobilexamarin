using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Annotations;
using Xamarin.HLP.Mobile.AppPE.Common;

namespace Xamarin.HLP.Mobile.AppPE.Model
{
    public class ExcluirContaViewModel : INotifyPropertyChanged
    {
        public ExcluirContaViewModel()
        {
            Email = App.CurrentAspnetUserModel?.Email;

            VoltarCommand = new Command(() =>
            {
                UtilNavidate.PopAsync();
            });

            ExcluirContaCommand = new Command(ExcluirConta);
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

        [IgnoreDataMember]
        public bool isBusy
        {
            get { return _isBusy; }
            set { _isBusy = value; NotifyPropertyChanged(); }
        }

        [IgnoreDataMember]
        public ICommand ExcluirContaCommand { get; set; }
        [IgnoreDataMember]
        public ICommand VoltarCommand { get; set; }

        private string _email;
        private string _password;
        private bool _isBusy;

        public async void ExcluirConta()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Password))
                {
                    await App.Messages.ShowAsync("Informe sua senha para confirmar a exclusão da conta.");
                    return;
                }

                var bConfirmou = await App.Messages.ShowConfirmAsync(
                    message: "Esta ação é IRREVERSÍVEL. Sua conta será desativada, e todos os usuários da sua empresa perderão o acesso ao sistema. Seus pedidos e histórico financeiro serão preservados, apenas desassociados da sua conta. Deseja continuar?",
                    accept: "EXCLUIR CONTA",
                    cancel: "Cancelar",
                    title: "Excluir conta");

                if (!bConfirmou) return;

                isBusy = true;
                var resultado = await UtilHttp.PostExcluirConta(this);
                isBusy = false;

                if (resultado != null && resultado.success)
                {
                    await App.Messages.ShowAsync("Sua conta foi excluída com sucesso.");
                    UtilNavidate.EfetivarLogoff();
                }
                else
                {
                    await App.Messages.ShowAsync(resultado?.xMessage ?? "Não foi possível excluir sua conta. Tente novamente.");
                }
            }
            catch (Exception ex)
            {
                isBusy = false;
                ex.TrakException("ExcluirConta", false);
                await App.Messages.ShowAsync("Não foi possível excluir sua conta. Tente novamente.");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
