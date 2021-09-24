using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Controls.xaml;
using Xamarin.HLP.Mobile.AppPE.Model.Agenda;
using Xamarin.HLP.Mobile.AppPE.Model.Cadastros;
using Xamarin.HLP.Mobile.AppPE.Model.Repository.Agenda;
using Xamarin.HLP.Mobile.AppPE.View.Agenda;
using Xamarin.HLP.Mobile.AppPE.View.Cliente;
using Xamarin.HLP.Mobile.AppPE.View.Sincronizacao;

namespace Xamarin.HLP.Mobile.AppPE.ViewModel.Agenda
{
    public class ListarEventosViewModel : SearchCommom
    {

        public ICommand HabiliteToSearchCommand { get; set; }
        public ICommand ConfiguracaoCommand { get; set; }
        public ICommand NovoCommand { get; set; }
        public ICommand SincronizarCommand { get; set; }

        public ConfiguracaoPesquisaListagemEventos Config { get; set; }

        private ObservableCollection<AgendaListarModel> _atividades = null;
        public ObservableCollection<AgendaListarModel> atividades
        {
            get { return _atividades; }
            set { _atividades = value; NotifyPropertyChanged(); }
        }

        private AgendaListarModel _currentModel;
        public AgendaListarModel currentModel
        {
            get { return _currentModel; }
            set { _currentModel = value; NotifyPropertyChanged(); }
        }


        public bool bUsaClienteEspecifico { get; set; }

        private string _xFooter1 = "Pesquisando..."; 
        public string xFooter1
        {
            get { return _xFooter1; }
            set { _xFooter1 = value; NotifyPropertyChanged(); }
        }

        private string _xFooter2 = "Registros: (0)"; 
        public string xFooter2
        {
            get { return _xFooter2; }
            set { _xFooter2 = value; NotifyPropertyChanged(); }
        }


        private double _dOpacityLista = Device.RuntimePlatform == Device.iOS ? 0.1 : 1; 
        public double dOpacityLista
        {
            get { return _dOpacityLista; }
            set { _dOpacityLista = value; NotifyPropertyChanged(); }
        }

        public bool IsUsingSearch { get; set; } = false;

        public SearchPE controlSearchPE { get; set; }

        public bool Initialize()
        {
            if (canExecuteInicial)
            {
                canExecuteInicial = false;
                PesquisaInicial();
            }

            return canExecuteInicial;
        }

        public void TratamentoErroToiOS()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                IsBusy = true;
                dOpacityLista = Device.RuntimePlatform == Device.iOS ? 0.1 : 1;
                if (atividades == null)
                    atividades = new ObservableCollection<AgendaListarModel>();
                else
                    atividades.Clear(); 
                for (int i = 0; i < 20; i++)
                {
                    atividades.Add(new AgendaListarModel
                    {
                        idAtividade = 0,
                        xDescricaoAtividade = "carregando..."                    
                    });
                }
            });
        }

        private void PesquisaInicial()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                IsBusy = true;
                atividades = new ObservableCollection<AgendaListarModel>();
                LoadItens();
            });
        }

        public ListarEventosViewModel()
        { 
            IsBusy = true;
            Config = new ConfiguracaoPesquisaListagemEventos();
            LoadItensCommand = new Command(LoadItens);
            SearchCommand = new Command(Search);


            HabiliteToSearchCommand = new Command(() =>
            {
                bFind = !bFind;
                if (bFind)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        controlSearchPE.GetEntry().Focus();
                    });
                }
                else
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        controlSearchPE.GetEntry().Unfocus();
                    });
                }
            });

            SincronizarCommand = new Command(() =>
            {
                var pageSync = new PageSyncNew();
                pageSync.ViewModel.AcaoAfterSyncCommand = new Command(PesquisaInicial);
                UtilNavidate.Sincronizar(pageSync);
            });

            ConfiguracaoCommand = new Command(() =>
            {
                UtilNavidate.PushAsync(new PageConfigurarPesquisaListagemEventos(Config));
            });

            NovoCommand = new Command(() =>
            {
                UtilNavidate.PushAsync(new PageEventoNew(new AtividadeAgendaModel()));
            });
        }

        public async void Search()
        {
            await Task.Run(() =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    IsUsingSearch = true;
                    atividades.Clear();
                    LoadItens();
                });
            });
        }

        private async void LoadItens()
        {
            IsBusy = true;

            await Task.Run(() =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    xFooter1 = "Pesquisando...";
                    int? idCliente = null;
                    if (bUsaClienteEspecifico)
                        idCliente = PageApresentacaoClienteNew.ViewModelStatic.idClientesOffLine;

                    var _dateNow = DateTime.Now.ToLocalTime();
                    var registros = AgendaRepository.GetInfinit(atividades.Count, 20,(IsUsingSearch ? xFiltro : ""), App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa, Config.bTrazerRealizados, null, idCliente, Config.bTrazerCancelados, Config.bTrazerTodosRepresentantes, Config.bOrdernarCrescente);

                    foreach (var registro in registros)
                    {
                        bool bAtrasado = false;
                        if (_dateNow > registro.dtInicioEvento.GetValueOrDefault())
                            bAtrasado = true;

                        if (registro.bEventoCancelado)
                        {
                            registro.ColorPendencia = Color.FromHex("ff6b68");
                            registro.xAtraso = $"Cancelado";
                        }
                        else if (bAtrasado && !registro.bRealizado)
                        {
                            registro.ColorPendencia = Color.FromHex("ff6b68");
                            registro.xAtraso = $"Atrasado";
                        }
                        else if (!registro.bRealizado)
                        {
                            registro.ColorPendencia = Color.FromHex("2196F3");
                            registro.xAtraso = $"Para fazer";
                        }
                        else
                        {
                            registro.ColorPendencia = Color.FromHex("32c787");
                            registro.xAtraso = $"Realizado";
                        } 

                        if (registro.dtInicioEvento != null)
                            if ((registro.dtInicioEvento ?? DateTime.Now).Kind != DateTimeKind.Local)
                                registro.dtInicioEvento = (registro.dtInicioEvento ?? DateTime.Now).ToLocalTime();


                        if (registro.dtFimEvento != null)
                            if ((registro.dtFimEvento ?? DateTime.Now).Kind != DateTimeKind.Local)
                                registro.dtFimEvento = (registro.dtFimEvento ?? DateTime.Now).ToLocalTime();


                        if(registro.xVendedorVinculado.Split(';').Count() > 1)
                        {
                            registro.xListaVendedores = registro.xVendedorVinculado;
                            registro.xVendedorVinculado = "VÁRIOS VENDEDORES";
                        }


                        atividades.Add(registro);
                    }
                    xFooter1 = string.Empty;
                    xFooter2 = $"Registros ({atividades.Count})";
                    dOpacityLista = 1;
                });
            });

            Device.BeginInvokeOnMainThread(() =>
            {
                Device.StartTimer(UtilMethods.GetStartTime, CloseIsBusy);
            });
        }
    }
}
