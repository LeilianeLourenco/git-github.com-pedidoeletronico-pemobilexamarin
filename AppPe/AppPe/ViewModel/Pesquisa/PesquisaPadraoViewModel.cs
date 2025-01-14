using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Controls.xaml;
using Xamarin.HLP.Mobile.AppPE.Model;
using Xamarin.HLP.Mobile.AppPE.Model.Cadastros;
using Xamarin.HLP.Mobile.AppPE.Model.Repository;
using Xamarin.HLP.Mobile.AppPE.Model.Repository.Agenda;
using Xamarin.HLP.Mobile.AppPE.View.Pedido;
using static Xamarin.HLP.Mobile.AppPE.Model.Cadastros.ClientesModel;

namespace Xamarin.HLP.Mobile.AppPE.ViewModel.Pesquisa
{
    public class PesquisaPadraoViewModel : SearchCommom
    {
        public enum Tabela
        {
            TB_TIPOATIVIDADESCRM,
            TB_CLIENTE,
            TB_ENDERECO,
            TB_TABELA_PRECO,
            TB_TRANSPORTADORA,
            TB_CONDICAO_PAGAMENTO,
            TB_REPRESENTANTE,
            TB_REPRESENTANTE_MAIS_TODOS,
            RAMO_ATIVIDADE,
            STATUS_PEDIDO,
            STATUS_PEDIDO_APRESENTACAO,
            TB_REPRESENTADA,
            TB_FORMA_PAGAMENTO
        }

        public Tabela tabela { get; set; }

        public SearchPE controlSearchPE { get; set; }
        private ListItemModel _itemSelected;
        public Command HabiliteToSearchCommand { get; set; }


        public bool IsUsingSearch { get; set; } = false;

        public int? idClienteOffline { get; set; }
        public int? idCondicaoPagamento { get; set; }
        public object objFiltro { get; set; }


        private async void LoadItens()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                IsBusy = true;
            });

            await Task.Run(() =>
            {
                var litens = new List<ListItemModel>();

                if (tabela == Tabela.TB_CLIENTE)
                {
                    litens = ClienteRepository.Get(LItens.Count, 50, (IsUsingSearch ? xFiltro : ""), TipoTela.pedido);
                }
                if (tabela == Tabela.TB_ENDERECO)
                {
                    litens = EnderecoRepository.Get(LItens.Count, 50, (IsUsingSearch ? xFiltro : ""), idClienteOffline);
                }
                if (tabela == Tabela.RAMO_ATIVIDADE)
                {
                    litens = RamoAtividadeRepository.Get(LItens.Count, 50, (IsUsingSearch ? xFiltro : ""));
                }
                if (tabela == Tabela.TB_TABELA_PRECO)
                {
                    ClientesModel clientesFiltro = objFiltro as ClientesModel;
                    litens = TabelaPrecoRepository.Get(LItens.Count, 50, (IsUsingSearch ? xFiltro : ""), clientesFiltro: clientesFiltro);
                }
                if (tabela == Tabela.TB_CONDICAO_PAGAMENTO)
                {
                    litens = CondicaoPagamentoRepository.Get(LItens.Count, 50, (IsUsingSearch ? xFiltro : ""), idClienteOffline);
                }
                else if (tabela == Tabela.TB_FORMA_PAGAMENTO)
                {
                    litens = CondicaoPagamentoRepository.BuscaFormasPagamentoPorCondicao((IsUsingSearch ? xFiltro : ""), idCondicaoPagamento);
                }
                else if (tabela == Tabela.TB_TRANSPORTADORA)
                {
                    litens = TransportadoraRepository.Get(LItens.Count, 50, (IsUsingSearch ? xFiltro : ""));
                }
                else if (tabela == Tabela.TB_TIPOATIVIDADESCRM)
                {
                    litens = AgendaRepository.Get(LItens.Count, 50, (IsUsingSearch ? xFiltro : ""));
                }
                else if (tabela == Tabela.TB_REPRESENTANTE)
                {
                    litens = EmpresaAspnetUsersRepository.Get(LItens.Count, 50, (IsUsingSearch ? xFiltro : ""));
                }
                else if (tabela == Tabela.TB_REPRESENTADA)
                {
                    litens = RepresentadaRepository.GetListItemModel();
                }
                else if (tabela == Tabela.TB_REPRESENTANTE_MAIS_TODOS)
                {
                    litens = EmpresaAspnetUsersRepository.Get(LItens.Count, 100, (IsUsingSearch ? xFiltro : ""));

                    if (LItens.Any(c => c.Id == 0) == false)
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            LItens.Add(new ListItemModel { Detail = "todos os vendedores", Display = "Todos", Id = 0 });
                        });
                    }
                }
                else if (tabela == Tabela.STATUS_PEDIDO)
                {
                    litens = StatusRepository.Get(LItens.Count, 50, App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa_aspnetUsers ?? 0, (IsUsingSearch ? xFiltro : ""), PagePedidoNew.CurrentViewModel.currentModel.stLancamento);
                }
                else if (tabela == Tabela.STATUS_PEDIDO_APRESENTACAO)
                {
                    litens = StatusRepository.Get(LItens.Count, 50, App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa_aspnetUsers ?? 0, (IsUsingSearch ? xFiltro : ""), PageDetalhesPedido.viewmodelStatic.currentModel.stLancamento);
                }

                foreach (var item in litens)
                {
                    if (tabela == Tabela.STATUS_PEDIDO_APRESENTACAO)
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            if (LItens.Any(c => c.XId == item.XId) == false)
                                LItens.Add(item);
                        });
                    }
                    else if (tabela == Tabela.TB_FORMA_PAGAMENTO)
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            if (!LItens.Any(c => c.Display == item.Display))
                                LItens.Add(item);
                        });
                    }
                    else
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            if (LItens.Any(c => c.XId == item.XId) == false)
                                LItens.Add(item);
                        });
                    }

                }
            });

            Device.BeginInvokeOnMainThread(() =>
            {
                IsBusy = false;
            });
        }



        public async void Search()
        {
            await Task.Run(() =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    IsUsingSearch = true;
                    LItens = new ObservableCollection<ListItemModel>();


                    LoadItens();
                });
            });
        }

        public ListItemModel ItemSelected
        {
            get { return _itemSelected; }
            set
            {
                _itemSelected = value;
                if (itemCadastro != null)
                {
                    itemCadastro.Id = value.Id;
                    itemCadastro.XId = value.XId;
                    itemCadastro.Display = value.Display;
                    itemCadastro.Detail = value.Detail;
                    itemCadastro.IdOnline = value.IdOnline;
                }
                NotifyPropertyChanged();
            }
        }

        private ListItemModel _itemCadastro;

        public ListItemModel itemCadastro
        {
            get { return _itemCadastro; }
            set { _itemCadastro = value; NotifyPropertyChanged(); }
        }


        private ObservableCollection<ListItemModel> _lItens;

        public ObservableCollection<ListItemModel> LItens
        {
            get { return _lItens; }
            set
            {
                _lItens = value;
                NotifyPropertyChanged();
            }
        }




        public PesquisaPadraoViewModel()
        {
            LItens = new ObservableCollection<ListItemModel>();
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
        }



        public bool Initialize()
        {
            if (canExecuteInicial)
            {
                canExecuteInicial = false;
                Device.BeginInvokeOnMainThread(() =>
                {
                    IsBusy = true;
                });
                LoadItens();
            }
            return canExecuteInicial;
        }
    }
}
