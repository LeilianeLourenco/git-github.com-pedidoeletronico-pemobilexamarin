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
using Xamarin.HLP.Mobile.AppPE.Model;
using Xamarin.HLP.Mobile.AppPE.Model.Lancamento;
using Xamarin.HLP.Mobile.AppPE.Model.Repository;
using Xamarin.HLP.Mobile.AppPE.View.Cliente;
using Xamarin.HLP.Mobile.AppPE.View.Pedido;
using Xamarin.HLP.Mobile.AppPE.View.Pesquisas;
using Xamarin.HLP.Mobile.AppPE.View.Sincronizacao;
using Xamarin.HLP.Mobile.AppPE.ViewModel.Pesquisa;

namespace Xamarin.HLP.Mobile.AppPE.ViewModel.Pedido
{
    public class ListarPedidoViewModelNew : SearchCommom
    {


        public ICommand HabiliteToSearchCommand { get; set; }
        public ICommand NovoCommand { get; set; }
        public ICommand SincronizarCommand { get; set; }

        public ICommand GoToRepresentantesCommand { get; set; }


        public bool bUsaClienteEspecifico { get; set; }
        public SearchPE controlSearchPE { get; set; }

        private PedidoVendaListarModel _currentModel;
        public PedidoVendaListarModel currentModel
        {
            get { return _currentModel; }
            set { _currentModel = value; NotifyPropertyChanged(); }
        }

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


        private ListItemModel _ItemRepresentante = new ListItemModel { Display = "clique aqui para pesquisar" };

        public ListItemModel ItemRepresentante
        {
            get { return _ItemRepresentante; }
            set
            {
                _ItemRepresentante = value;
                NotifyPropertyChanged();
            }
        }

        private double _dOpacityLista = Device.RuntimePlatform == Device.iOS ? 0.1 : 1;

        public double dOpacityLista
        {
            get { return _dOpacityLista; }
            set { _dOpacityLista = value; NotifyPropertyChanged(); }
        }


        private ObservableCollection<PedidoVendaListarModel> _pedidos = null;
        public ObservableCollection<PedidoVendaListarModel> pedidos
        {
            get { return _pedidos; }
            set { _pedidos = value; NotifyPropertyChanged(); }
        }
        public bool IsUsingSearch { get; set; } = false;


        public ListarPedidoViewModelNew()
        {
            SincronizarCommand = new Command(() =>
            {
                var pageSync = new PageSyncNew();
                pageSync.ViewModel.AcaoAfterSyncCommand = new Command(PesquisaInicial);
                UtilNavidate.Sincronizar(pageSync);
            });

            ItemRepresentante = new ListItemModel
            {
                Id = App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa_aspnetUsers ?? 0,
                Display = App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.xNome,
                Detail = App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.xEmail
            };

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


            GoToRepresentantesCommand = new Command(() =>
            {
                if (!ExecuttingAnyCommand)
                {
                    ExecuttingAnyCommand = true;
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        if (ItemRepresentante == null)
                            ItemRepresentante = new ListItemModel();
                        var pesquisa = new PagePesquisaPadrao(ItemRepresentante,
                            PesquisaPadraoViewModel.Tabela.TB_REPRESENTANTE_MAIS_TODOS)
                        {
                            Title = "Representante",
                        };
                        UtilNavidate.PushAsync(pesquisa);
                    });
                }

            });

            NovoCommand = new Command(() =>
            {
                if(!PedidoRepository.bPermiteJornada(App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa, App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa_aspnetUsers.GetValueOrDefault()))
                {
                    App.Messages.ShowAsync("Horário fora do expediente, operação não permitida!");
                    return;
                }

                UtilNavidate.PushAsync(new PagePedidoNew(new PedidoVendaModel()));
            });

        }


        public void TratamentoErroToiOS()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                IsBusy = true;
                dOpacityLista = Device.RuntimePlatform == Device.iOS ? 0.1 : 1;
                if (pedidos == null)
                    pedidos = new ObservableCollection<PedidoVendaListarModel>();
                else
                    pedidos.Clear();
                //var registros = PedidoRepository.GetInfinit(pedidos.Count, 20, (IsUsingSearch ? xFiltro : ""), ItemRepresentante.Id.ToString(), null);
                //foreach (var registro in registros)
                //{
                //    pedidos.Add(registro);
                //}
                for (int i = 0; i < 20; i++)
                {
                    pedidos.Add(new PedidoVendaListarModel
                    {
                        idPedidoVenda = 0,
                        stLancamento = 0,
                        DisplayCliente = "caregando..",
                        DisplayPrazo = "prazos..",
                        DisplayForma = "forma..",
                        idStatus = 0,
                        DisplayEmail = "...",
                        VTotal = 0,
                        XdEmissao = DateTime.Now.ToString("dd/MM/yyyy"),
                        idPedidoDisplay = 0,
                        xSigla = "--"

                    });
                }
            });
        }
        private void PesquisaInicial()
        { 
            if (!IsBusy)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    pedidos = new ObservableCollection<PedidoVendaListarModel>();
                    LoadItens();
                });
            }
             
        }

        public bool Initialize()
        {
            if (canExecuteInicial && !IsBusy)
            {
                canExecuteInicial = false;
                PesquisaInicial();
            }

            return canExecuteInicial;
        }

        private async void LoadItens()
        {
            if (IsBusy)
                return;
             
            await Task.Run(() =>
            {
                Device.BeginInvokeOnMainThread(() =>
                { 
                    IsBusy = true;

                    xFooter1 = "Pesquisando...";

                    int? idCliente = null;
                    if (bUsaClienteEspecifico)
                        idCliente = PageApresentacaoClienteNew.ViewModelStatic.idClientesOffLine;

                    var registros = PedidoRepository.GetInfinit(pedidos.Count, 20, (IsUsingSearch ? xFiltro : ""), ItemRepresentante.Id.ToString(), idCliente);

                    foreach (var registro in registros)
                    {
                        pedidos.Add(registro);
                    }
                    xFooter1 = string.Empty;
                    xFooter2 = $"Registros ({pedidos.Count})";
                    dOpacityLista = 1;

                    IsBusy = false;
                });
            });

            Device.BeginInvokeOnMainThread(() =>
            {
                Device.StartTimer(UtilMethods.GetStartTime, CloseIsBusy);
            });
        }

        public async void Search()
        {
            await Task.Run(() =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    IsUsingSearch = true;
                    pedidos.Clear();
                    LoadItens();
                });
            });
        }


    }
}
