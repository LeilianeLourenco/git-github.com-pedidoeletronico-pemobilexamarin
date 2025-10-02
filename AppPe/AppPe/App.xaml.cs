using Hlp.PedidoEletronico.Domain.Business.Bo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Model;
using Xamarin.HLP.Mobile.AppPE.Model.Home;
using Xamarin.HLP.Mobile.AppPE.Model.Repository;
using Xamarin.HLP.Mobile.AppPE.Services;
using Xamarin.HLP.Mobile.AppPE.View.Home;
using Xamarin.HLP.Mobile.AppPE.View.Login;
using Xamarin.HLP.Mobile.AppPE.View.MainPage;
using Xamarin.HLP.Mobile.AppPE.View.Sincronizacao;

namespace Xamarin.HLP.Mobile.AppPE
{
    public partial class App : Application
    {
        public static TipoUser tipouser { get; set; } = TipoUser.NORMAL;

        public static Planos planoAtual { get; set; }
        public static string xErrorDataBase { get; set; } = "";

        public static bool ForcarAtualizacao
            => CurrentAspnetUserModel.objEmpresaAspnetUsersModel.UltimaSyncDateTime.Year < 2000;

        public static Assembly SvgAssembly => typeof(App).GetTypeInfo().Assembly;
        public static AspNetUsersModel CurrentAspnetUserModel { get; set; }
        public static string xPathUserMobile => $@"{UrlWebApi}Content/Templates/img/Avatar/Avatar_1.png";
        public static string xNameAvatar => "Avatar_Mobile";


        public static ImagesAppModel ImgApp { get; set; }
        public static DataAccess Data;
        public static IPicture Picture;
        public static IVersion Versao { get; set; }
        public static IBluetoothLE BluetoothLe { get; set; }
        public static IEncoded Encoded { get; set; }
        public static AppPE.ViewModel.Service.IMessageService Messages;

        public static IbackButtonPressed ParamBackButtonPressed { get; set; }


        public static async Task<bool> IsConected()
        {
            try
            {
                //if (AmbienteApp == Ambiente.Homologacao)
                //    return true;
                //if (Device.OS == TargetPlatform.Windows)
                //    return await CrossConnectivity.Current.IsReachable(host: "pedidoeletronico.azurewebsites.net");

                //return CrossConnectivity.Current.IsConnected;

                var current = Connectivity.NetworkAccess;

                if (current == NetworkAccess.Internet)
                    return true;

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static CurrentUserLoginModel EnvironmentPE { get; set; }

        public static Ambiente AmbienteApp = Ambiente.Producao;

        public static bool LoginHlp { get; set; } = false;

        public enum Ambiente
        {
            Producao,
            Homologacao,
            Local,
            HlpHom,
            HomologacaoProducao
        }

        public static string UrlWebApi
        {
            get
            {
                switch (AmbienteApp)
                {
                    case Ambiente.Homologacao:
                        return "http://homologacaope.azurewebsites.net/";
                    case Ambiente.Producao:
                        return "https://pedidoeletronico.com/";
                    case Ambiente.Local:
                        return "http://hom-pedidoeletronico.azurewebsites.net/";
                    case Ambiente.HlpHom:
                        return "http://hlpsistemas.no-ip.org:8088/";
                    case Ambiente.HomologacaoProducao:
                        return "http://homologacao.pedidoeletronico.com/";
                    default:
                        return "https://pedidoeletronico.azurewebsites.net/";
                }

            }
        }

        public static string UrlWebApiMobile
        {
            get
            {
                switch (AmbienteApp)
                {
                    case Ambiente.Homologacao:
                        return "http://homologacaoapimobile.azurewebsites.net/";
                    case Ambiente.Producao:
                        return "http://apimobile.pedidoeletronico.com/";
                    case Ambiente.HomologacaoProducao:
                        return "http://prodhomapimobile.azurewebsites.net/";
                    default:
                        return "http://apimobile.pedidoeletronico.com/";
                }

            }
        }

        public static string UrlApiImage
        {
            get
            {
                switch (AmbienteApp)
                {
                    case Ambiente.Homologacao:
                        return "http://apidataimage.pedidoeletronico.com/";
                    case Ambiente.Producao:
                        return "http://apidataimage.pedidoeletronico.com/";
                    case Ambiente.HomologacaoProducao:
                        return "http://apidataimage.pedidoeletronico.com/";
                    default:
                        return "http://apidataimage.pedidoeletronico.com/";
                }

            }
        }

        public static string UrlReport
        {
            get
            {
                switch (AmbienteApp)
                {
                    case Ambiente.Homologacao:
                        //return "https://191.235.81.52:8080/";
                        //return "http://hlpsistemas.sytes.net:8094/";
                        return "http://pereporthom.pedidoeletronico.com/";
                    case Ambiente.Producao:
                        //return "https://191.235.81.52/"; 
                        return "http://pe-reports.sytes.net/";
                    case Ambiente.Local:
                        return "http://hlpsistemas.sytes.net:8094/";
                    case Ambiente.HomologacaoProducao:
                        return "http://pe-reports.sytes.net/";
                    default:
                        return "http://pe-reports.sytes.net/";
                }
            }
        }

        public static string UrlPortal
        {
            get
            {
                switch (AmbienteApp)
                {
                    case Ambiente.Homologacao:
                        return "http://hom-portalpedidoeletronico.azurewebsites.net/";
                    case Ambiente.Producao:
                        return "http://portalpagamentos.pedidoeletronico.com/";
                    //return "http://portalpedidoeletronico.azurewebsites.net/";


                    case Ambiente.Local:
                        return "http://hom-portalpedidoeletronico.azurewebsites.net/";
                    case Ambiente.HomologacaoProducao:
                        return "http://portalpagamentos.pedidoeletronico.com/";
                    default:
                        return "http://hom-portalpedidoeletronico.azurewebsites.net/";
                }

            }
        }


        public App()
        {
            try
            {
                InitializeComponent();
                Iniciar();
            }
            catch (Exception ex)
            {
                App.Messages.ShowAsync(ex.Message);
            }

        }

        private void Iniciar()
        {
            try
            {
                ImgApp = new ImagesAppModel();
                DependencyService.Register<ViewModel.Service.IMessageService, View.Service.MessageService>();
                Messages = DependencyService.Get<AppPE.ViewModel.Service.IMessageService>();
                Picture = DependencyService.Get<IPicture>();
                Encoded = DependencyService.Get<IEncoded>();
                Versao = DependencyService.Get<IVersion>();
                BluetoothLe = DependencyService.Get<IBluetoothLE>();
                ParamBackButtonPressed = DependencyService.Get<IbackButtonPressed>();

                Data = new DataAccess();
                Data.PrimeiraAnalise();

                if (LoginRepository.StatusBloqueio())
                {
                    CurrentAspnetUserModel = LoginRepository.GetAspnetUsers();

                    if (App.EnvironmentPE == null)
                        App.EnvironmentPE = LoginRepository.GetUserLoginModel();

                    MainPage = new NavigationPage(new PageLogBloqueioSync());
                    return;
                }

                if (LoginRepository.HasLogin())
                    CurrentAspnetUserModel = LoginRepository.GetAspnetUsers();

                if (!CurrentAspnetUserModel?.objEmpresaAspnetUsersModel?.stAtivo ?? false)
                {
                    var idEmpresa = CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa;

                    if (App.EnvironmentPE == null)
                        App.EnvironmentPE = LoginRepository.GetUserLoginModel();

                    var empresaAtiva = App.CurrentAspnetUserModel?.lEpresaAspnetUsersModel?
                        .FirstOrDefault(c => c.stAtivo);

                    if (empresaAtiva != null)
                        App.EnvironmentPE.idEmpresaLogada = empresaAtiva.idEmpresa;
                    else
                        App.EnvironmentPE.idEmpresaLogada = idEmpresa; 

                    LoginRepository.DesbloquearUser();
                    LoginRepository.UpdateUser();

                    CurrentAspnetUserModel = LoginRepository.GetAspnetUsers();

                    MainPage = new RootPage();
                    return;
                }

                if (CurrentAspnetUserModel == null)
                    MainPage = new NavigationPage(new PageBeforeLogin());
                else
                    MainPage = new RootPage();

            }
            catch (Exception ex) // catch all other errors
            {
                App.Messages.ShowAsync($"Inicializacao - {ex.Message}");
                GoogleInsightsReportingConstants.TrakException("Inicializacao App", ex.Message, true);
            }
        }

        public static INavigation Navigation;

        public enum TipoUser { OMIE, BLING, TINY, NORMAL }
    }
}
