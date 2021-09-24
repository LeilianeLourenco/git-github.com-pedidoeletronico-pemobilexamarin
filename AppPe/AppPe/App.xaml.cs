using System;
using Xamarin.Forms;
using Hlp.PedidoEletronico.Domain.Business.Bo;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Model;
using Xamarin.HLP.Mobile.AppPE.Model.Home;
using Xamarin.HLP.Mobile.AppPE.Model.Repository;
using Xamarin.HLP.Mobile.AppPE.Services;
using Xamarin.HLP.Mobile.AppPE.View.Login;
using Xamarin.HLP.Mobile.AppPE.View.MainPage;
using System.Reflection;
using System.Threading.Tasks;
using Plugin.Connectivity;
using Xamarin.Essentials;

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
            HlpHom
        }

        public static string UrlWebApi
        {
            get
            {
                switch (AmbienteApp)
                {
                    case Ambiente.Homologacao:
                        return "http://hlpsistemas.sytes.net:8089/";
                    case Ambiente.Producao:
                        return "https://pedidoeletronico.com/";
                    case Ambiente.Local:
                        return "http://hom-pedidoeletronico.azurewebsites.net/";
                    case Ambiente.HlpHom:
                        return "http://hlpsistemas.no-ip.org:8088/";
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
                        return "http://hlpsistemas.sytes.net:8085/";
                    case Ambiente.Producao:
                        return "http://apimobile.pedidoeletronico.com/";
                    default:
                        return "http://apimobile.pedidoeletronico.com/";
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
                        return "http://hlpsistemas.sytes.net:8094/";
                    case Ambiente.Producao:
                        //return "https://191.235.81.52/"; 
                        return "http://pe-reports.sytes.net/";

                    //return "https://prod-pereport.sytes.net/";  link interno hlp
                    case Ambiente.Local:
                        return "http://hlpsistemas.sytes.net:8094/";
                    default:
                        return "http://hlpsistemas.no-ip.org:8087/";
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




                if (LoginRepository.HasLogin())
                    CurrentAspnetUserModel = LoginRepository.GetAspnetUsers();

                if (CurrentAspnetUserModel == null)
                    MainPage = new NavigationPage(new PageBeforeLogin());
                else
                    MainPage = new RootPage();

                //MainPage = new NavigationPage(new PageListarPedidos());

            }
            catch (Exception ex) // catch all other errors
            {
                App.Messages.ShowAsync($"Inicializacao - {ex.Message}");
                GoogleInsightsReportingConstants.TrakException("Inicializacao App", ex.Message, true);
            }
        }

        public static INavigation Navigation;

        public enum TipoUser { OMIE, BLING, NORMAL }
    }
}
