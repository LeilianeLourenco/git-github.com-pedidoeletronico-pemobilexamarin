using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Model;
using Xamarin.HLP.Mobile.AppPE.Model.Repository;
using Xamarin.HLP.Mobile.AppPE.View;
using Xamarin.HLP.Mobile.AppPE.View.MainPage;
using Xamarin.HLP.Mobile.AppPE.View.Sincronizacao;
using Xamarin.HLP.Mobile.AppPE.ViewModel.Sincronizacao;

namespace Xamarin.HLP.Mobile.AppPE.ViewModel
{
    public class InicioViewModel : ViewModelComum<LoginViewModel>
    {


        public ICommand EsqueciSenhaCommand { get; set; }


        public InicioViewModel()
        {
            this.currentModel = new LoginViewModel
            {
                LoginCommand = new Command(Login),

            };
            EsqueciSenhaCommand = new Command(EsqueciSenha);
        }

        public bool Initialize()
        {
            if (canExecuteInicial)
            {
                canExecuteInicial = false;

                currentModel.Email = LoginRepository.GetUltimoEmailLogin();
            }

            return canExecuteInicial;
        }


        private async void EsqueciSenha()
        {
            if (currentModel.BProcessando == false)
            {
                //var main = Application.Current.MainPage as NavigationPage;
                //if (main != null)
                //{
                //    await Task.Yield();
                //    await main.Navigation.PushAsync(new PageForgotPassword());
                //}
                await Browser.OpenAsync("https://pedidoeletronico.com/Account/ForgotPassword", new BrowserLaunchOptions
                {
                    LaunchMode = BrowserLaunchMode.SystemPreferred,
                    TitleMode = BrowserTitleMode.Show,
                    PreferredToolbarColor = Color.AliceBlue,
                    PreferredControlColor = Color.Violet
                });
            }
        }

        private async void Login()
        {
            if (App.LoginHlp)
            {
                currentModel.Email = "roberto@hlp.com.br";
                currentModel.Password = "1Qazxsw+";

                //currentModel.Email = "testeestoque@hlp.com.br";
                //currentModel.Password = "1Qazxsw+";

                //currentModel.Email = "paulo@pedidoeletronico.com";
                //currentModel.Password = "290513";

                //currentModel.Email = "andre00correa@gmail.com";
                //currentModel.Password = "virginia12345";
            }

            if (this.CanLogin())
            {
                if (await App.IsConected() == false)
                {
                    await App.Messages.ShowAsync("É necessário uma conexão com internet.");
                    return;
                }
                if (this.currentModel.Email.IsValidEmailAddress())
                {
                    if (!string.IsNullOrEmpty(App.xErrorDataBase))
                    {
                        await App.Messages.ShowAsync(App.xErrorDataBase);
                        this.currentModel.BProcessando = false;
                        return;
                    }

                    this.currentModel.BProcessando = true;
                    try
                    {
                        var objreturn = await UtilHttp.PostLogin(this.currentModel);

                        if (SincronizacaoNewViewModel.bFalhaConexao)
                        {
                            await App.Messages.ShowAsync("Falha na conexão, tente realizar o login novamente");
                            this.currentModel.BProcessando = false;
                            return;
                        }
                        if (objreturn != null && objreturn.objModel != null)
                        {
                            switch (objreturn.status)
                            {
                                case SignInStatus.Success:
                                    {
                                        LoginRepository.DesbloquearUser();

                                        var currentAspnetUserModel =
                                            LoginRepository.SaveAspnetUsers(model: objreturn.objModel);

                                        App.CurrentAspnetUserModel = currentAspnetUserModel;
                                        this.currentModel.BProcessando = false;

                                        if (currentAspnetUserModel?.objEmpresaAspnetUsersModel?.stAtivo ?? false)
                                            Application.Current.MainPage = new RootPage();
                                        else
                                        {
                                            LoginRepository.BloquearUser();
                                            Application.Current.MainPage = new NavigationPage(new PageLogBloqueioSync());
                                        }
                                    }
                                    break;
                                case SignInStatus.LockedOut:
                                    {
                                        this.currentModel.BProcessando = false;
                                        await App.Messages.ShowAsync("Usuário não habilitado");
                                    }
                                    break;
                                case SignInStatus.RequiresVerification:
                                    {
                                        this.currentModel.BProcessando = false;
                                        await App.Messages.ShowAsync("Usuário não habilitado");
                                    }
                                    break;
                                case SignInStatus.Failure:
                                    {
                                        this.currentModel.BProcessando = false;
                                        await App.Messages.ShowAsync("Usuário ou senha invalidos");
                                    }
                                    break;
                                default:
                                    {
                                        currentModel.BProcessando = false;
                                        await App.Messages.ShowAsync("Usuário ou senha invalidos");
                                    }
                                    break;
                            }
                        }
                        else
                        {
                            this.currentModel.BProcessando = false;
                            await App.Messages.ShowAsync("Usuário ou senha invalidos");
                        }

                    }
                    catch (Exception ex)
                    {
                        this.currentModel.BProcessando = false;
                        await App.Messages.ShowAsync(ex.Message);
                    }
                }
                else
                {
                    this.currentModel.BProcessando = false;
                    await App.Messages.ShowAsync("Email inválido");
                }
            }
        }

        private bool CanLogin()
        {
            if ((!string.IsNullOrEmpty(this.currentModel.Email)) && (!string.IsNullOrEmpty(this.currentModel.Password)) && (this.currentModel.BProcessando == false))
                return true;
            else
                return false;
        }


    }
}
