using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.Connectivity;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Model;
using Xamarin.HLP.Mobile.AppPE.Model.Home;
using Xamarin.HLP.Mobile.AppPE.Model.Repository;
using Xamarin.HLP.Mobile.AppPE.View.Agenda;
using Xamarin.HLP.Mobile.AppPE.View.Cliente;
using Xamarin.HLP.Mobile.AppPE.View.DashBoard;
using Xamarin.HLP.Mobile.AppPE.View.Empresa;
using Xamarin.HLP.Mobile.AppPE.View.Home;
using Xamarin.HLP.Mobile.AppPE.View.ListaPreco;
using Xamarin.HLP.Mobile.AppPE.View.MainPage;
using Xamarin.HLP.Mobile.AppPE.View.Pedido;
using Xamarin.HLP.Mobile.AppPE.View.Produto;
using Xamarin.HLP.Mobile.AppPE.View.Sincronizacao;

namespace Xamarin.HLP.Mobile.AppPE.ViewModel.Home
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class HomeNewViewModel : NotifyCommon
    {


        #region Properties

        public PageHomeNew page { get; set; }

        public Command GoToListaPreCommand { get; set; }
        public Command GoToPedidosCommand { get; set; }
        public Command GoToAgendaCommand { get; set; }
        public Command GoToClientesCommand { get; set; }
        public Command GoToProdutosCommand { get; set; }
        public string ImageIconListarPreco => Device.OnPlatform("ApplicationBarListarTabelaPreco.png", "ApplicationBarListarTabelaPreco.png", "Assets/ApplicationBarListarTabelaPreco.png");
        public string ImageIconPedido => Device.OnPlatform("ApplicationBarListarPedidos.png", "ApplicationBarListarPedidos.png", "Assets/ApplicationBarListarPedido.png");
        public string ImageIconListarProdutos => Device.OnPlatform("ApplicationBarListarProdutos.png", "ApplicationBarListarProdutos.png", "Assets/ApplicationBarListarProdutos.png");

        public string ImageIconListarAgenda => Device.OnPlatform("ApplicationBarListarAgenda.png", "ApplicationBarListarAgenda.png", "Assets/ApplicationBarListarAgenda.png");
        public string ImageIconListarClientes => Device.OnPlatform("ApplicationBarListarClientes.png", "ApplicationBarListarClientes.png", "Assets/ApplicationBarListarClientes.png");

        private string _xDisplayConexao;

        public string xDisplayConexao
        {
            get { return _xDisplayConexao; }
            set { _xDisplayConexao = value; NotifyPropertyChanged(); }
        }


        private string _xDateSync;

        public string xDateSync
        {
            get { return _xDateSync; }
            set { _xDateSync = value; NotifyPropertyChanged(); }
        }

        private string _xVendasEmitidasMes;

        public string xVendasEmitidasMes
        {
            get { return _xVendasEmitidasMes; }
            set { _xVendasEmitidasMes = value; NotifyPropertyChanged(); }
        }

        private Color _corConexao = ColorStaticModel.AzulMedio;

        public Color corConexao
        {
            get { return _corConexao; }
            set { _corConexao = value; NotifyPropertyChanged(); }
        }


        private string _xEmailUser;
        public string xEmailUser
        {
            get { return _xEmailUser; }
            set { _xEmailUser = value; NotifyPropertyChanged(); }
        }
        private string _xEmpresaAtual;
        public string xEmpresaAtual
        {
            get { return (_xEmpresaAtual ?? "").ToUpper(); }
            set
            {
                _xEmpresaAtual = value;
                NotifyPropertyChanged();
            }
        }

        private ImageSource _ImgEmpresa;
        public ImageSource ImgEmpresa
        {
            get { return _ImgEmpresa; }
            set
            {
                _ImgEmpresa = value;
                NotifyPropertyChanged();
            }
        }

        private DashMetaMensalModel _dashmensal = new DashMetaMensalModel();

        public DashMetaMensalModel dashmensal
        {
            get { return _dashmensal; }
            set { _dashmensal = value; NotifyPropertyChanged(); }
        }





        #endregion



        public HomeNewViewModel()
        {
            CrossConnectivity.Current.ConnectivityChanged += (sender, args) =>
            {
                RefreshConexao(args.IsConnected);
            };

            GoToListaPreCommand = new Command(() =>
            {
                if (!ExecuttingAnyCommand)
                {
                    ExecuttingAnyCommand = true;
                    UtilNavidate.PushAsync(new PageListaPreco());
                }

            });

            GoToPedidosCommand = new Command(() =>
            {
                if (!ExecuttingAnyCommand)
                {
                    ExecuttingAnyCommand = true;
                    UtilNavidate.PushAsync(new TabbedPagePedidos());
                }

            });

            GoToProdutosCommand = new Command(() =>
            {
                if (!ExecuttingAnyCommand)
                {
                    ExecuttingAnyCommand = true;
                    UtilNavidate.PushAsync(new PageInfinitListProdutos());
                }

            });

            GoToAgendaCommand = new Command(() =>
            {
                if (!ExecuttingAnyCommand)
                {
                    ExecuttingAnyCommand = true;
                    UtilNavidate.PushAsync(new PageListagemEventos());
                }
            });


            GoToClientesCommand = new Command(() =>
            {
                if (!ExecuttingAnyCommand)
                {
                    ExecuttingAnyCommand = true;
                    UtilNavidate.PushAsync(new PageInfinitListClientes());
                }

            });
        }

        public void RefreshConexao(bool IsConnected)
        {
            xDisplayConexao = IsConnected ? "DISPONÍVEL" : "OFFLINE";
            corConexao = IsConnected ? ColorStaticModel.AzulMedio : ColorStaticModel.VermelhoPrincipal;
        }

        public bool Initialize()
        {
            if (canExecuteInicial)
            {
                canExecuteInicial = false;
                StartDados();
                if (App.EnvironmentPE == null)
                {
                    App.EnvironmentPE = LoginRepository.GetUserLoginModel();
                }
            }
            return canExecuteInicial;
        }

        private void StartDados()
        {
            if (!App.ForcarAtualizacao)
            {
                if (App.ImgApp.ImgEmpresa == null || App.ImgApp.ImgUser == null || App.ImgApp.ImgCapa == null)
                    AtualizaImagemApp();

                ImgEmpresa = App.ImgApp.ImgEmpresa;

                CarregarInformacoesHome();
                GoogleInsightsReportingConstants.TrakPage(GoogleInsightsReportingConstants.InPage.PAGE_HOME);
            }

            if (App.ForcarAtualizacao)
                Sincronizar();

            LoginRepository.RefreshTipoUsuario();
        }

        public void CarregarInformacoesHome()
        {
            ExecuttingAnyCommand = false;
            xEmailUser = App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.xEmail;
            xEmpresaAtual = App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.objEmpresaModel.xRazaoSocial;
            xDateSync =
                App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.UltimaSyncDateTime.ToLocalTime()
                    .ToString("dd/MM/yyyy HH:mm");

            if (PedidoRepository.GetDataRelatorio(App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.objEmpresaModel.idEmpresa.GetValueOrDefault()) == 0)
                dashmensal = PedidoRepository.GetDadosDashMensal(page.Width - 10);
            else
                dashmensal = PedidoRepository.GetDadosDashMensalPorDataFaturamento(page.Width - 10);


            xVendasEmitidasMes = PedidoRepository.GetTotalVendasMesAtual();

        }

        public void Sincronizar()
        {
            if (!ExecuttingAnyCommand)
            {
                ExecuttingAnyCommand = true;
                var pageSync = new PageSyncNew("Total");
                //pageSync.ViewModel.bForcarSyncInit = App.ForcarAtualizacao;
                pageSync.ViewModel.AcaoAfterSyncCommand = new Command(CarregarInformacoesHome);
                UtilNavidate.Sincronizar(pageSync);
            }
        }

        public void AtualizaImagemApp()
        {
            try
            {
                var usuario = EmpresaAspnetUsersRepository.GetUsuario();
                var empresa = EmpresaRepository.GetEmpresa();

                App.ImgApp.ImgEmpresa =
                     UtilMethods.GetLocalImage(
                         empresa?.imLogoMarca,
                         UtilMethods.TipoImagem.CapaEmpresa);

                App.ImgApp.ImgUser =
                    UtilMethods.GetLocalImage(usuario?.imUsuario,
                        UtilMethods.TipoImagem.Usuario);

                App.ImgApp.ImgCapa =
                  UtilMethods.GetLocalImage(empresa?.xCaminhoImgCapa,
                      UtilMethods.TipoImagem.CapaEmpresa);

                RootPage.currentMenuStatic.AtualizaMenu(App.ImgApp.ImgUser, App.ImgApp.ImgCapa);
                ImgEmpresa = App.ImgApp.ImgEmpresa;
            }
            catch (Exception ex)
            {
                ex.TrakException();
            }
        }

        public void TrocarEmpresa()
        {
            if (!ExecuttingAnyCommand)
            {
                ExecuttingAnyCommand = true;
                UtilNavidate.PushAsync(new PageEmpresa());
            }
        }

        public void IrParaDashBoard()
        {
            if (!ExecuttingAnyCommand)
            {
                ExecuttingAnyCommand = true;
                UtilNavidate.PushAsync(new PageDashBoard());
            }
        }

    }
}
