using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Controls.xaml;
using Xamarin.HLP.Mobile.AppPE.Core.PedidoVenda;
using Xamarin.HLP.Mobile.AppPE.Core.PedidoVenda.Implementacoes;
using Xamarin.HLP.Mobile.AppPE.Core.PedidoVenda.Interfaces;
using Xamarin.HLP.Mobile.AppPE.Model;
using Xamarin.HLP.Mobile.AppPE.Model.Lancamento;
using Xamarin.HLP.Mobile.AppPE.Model.Repository;
using Xamarin.HLP.Mobile.AppPE.View.Pedido;

namespace Xamarin.HLP.Mobile.AppPE.ViewModel.Pedido
{
    public class ListarProdutosNewViewModel : SearchCommom
    {
        BuscaPreco _buscaPreco;
        public SearchPE controlSearchPE { get; set; }

        public ConfiguracaoPesquisaProdutoModel Config { get; set; }
        public ICommand HabiliteToSearchCommand { get; set; }

        public ICommand ConfiguracaoCommand { get; set; }
        public ICommand AplicaFiltroItensCommand { get; set; }
        public ICommand AplicaFiltroRecorrenciaCommand { get; set; }
        public ICommand AplicaFiltroCampanhaCommand { get; set; }
        public ICommand AplicaFiltroDestaquesCommand { get; set; }


        private PedidoNewViewModel _currentPedidoViewModel;

        public PedidoNewViewModel currentPedidoViewModel
        {
            get { return _currentPedidoViewModel; }
            set
            {
                _currentPedidoViewModel = value;
                NotifyPropertyChanged();
            }
        }

        private ObservableCollection<PedidoVendaItensModel> _produtos;

        public ObservableCollection<PedidoVendaItensModel> Produtos
        {
            get { return _produtos; }
            set
            {
                _produtos = value;
                NotifyPropertyChanged();
            }
        }

        private PedidoVendaItensModel _itemSelected;

        public PedidoVendaItensModel itemSelected
        {
            get { return _itemSelected; }
            set
            {
                _itemSelected = value;
                NotifyPropertyChanged();
            }
        }

        private bool _bEscalonada = false;

        public bool bConfGeralEscalonada
        {
            get { return _bEscalonada; }
            set { _bEscalonada = value; NotifyPropertyChanged(); }
        }

        public bool IsUsingSearch { get; set; } = false;

        public bool isLoading { get; set; } = false;

        private bool _HasGradeSelected = false;

        public bool HasGradeSelected
        {
            get { return _HasGradeSelected; }
            set
            {
                _HasGradeSelected = value;
                NotifyPropertyChanged();
            }
        }


        private bool _bFiltroBotaoItens = false;

        public bool bFiltroBotaoItens
        {
            get { return _bFiltroBotaoItens; }
            set
            {
                _bFiltroBotaoItens = value;
                NotifyPropertyChanged();
            }
        }


        private bool _bFiltroBotaoRecorrencia = false;

        public bool bFiltroBotaoRecorrencia
        {
            get { return _bFiltroBotaoRecorrencia; }
            set
            {
                _bFiltroBotaoRecorrencia = value;
                NotifyPropertyChanged();
            }
        }

        private bool _bFiltroBotaoCampanhas = false;

        public bool bFiltroBotaoCampanhas
        {
            get { return _bFiltroBotaoCampanhas; }
            set
            {
                _bFiltroBotaoCampanhas = value;
                NotifyPropertyChanged();
            }
        }

        private bool _bBotaoFiltroDestaques = false;

        public bool bBotaoFiltroDestaques
        {
            get { return _bBotaoFiltroDestaques; }
            set
            {
                _bBotaoFiltroDestaques = value;
                NotifyPropertyChanged();
            }
        }


        private bool _bListaItensHabilitada = true;

        public bool bListaItensHabilitada
        {
            get { return _bListaItensHabilitada; }
            set
            {
                _bListaItensHabilitada = value;
                NotifyPropertyChanged();
            }
        }

        private bool _bUsaLocaisEstoque = false;

        public bool bUsaLocaisEstoque
        {
            get { return _bUsaLocaisEstoque; }
            set
            {
                _bUsaLocaisEstoque = value;
                NotifyPropertyChanged();
            }
        }

        private bool _bMostraProdutosVariacoes = false;

        public bool bMostraProdutosVariacoes
        {
            get { return _bMostraProdutosVariacoes; }
            set
            {
                _bMostraProdutosVariacoes = value;
                NotifyPropertyChanged();
            }
        }

        private string _xtitleButtonEditar = "Editar";

        public string xtitleButtonEditar
        {
            get { return _xtitleButtonEditar; }
            set
            {
                _xtitleButtonEditar = value;
                NotifyPropertyChanged();
            }
        }

        private EditarItemViewModel _editarItemViewModel = null;

        public EditarItemViewModel editarItemViewModel
        {
            get { return _editarItemViewModel; }
            set
            {
                _editarItemViewModel = value;
                NotifyPropertyChanged();
            }
        }

        private IEnumerable<BasicPickerModel> _listaTabelaPreco = new List<BasicPickerModel>
        {
            new BasicPickerModel {Id = 0, Display = "Tabela de preço"}
        };

        public IEnumerable<BasicPickerModel> ListaTabelaPreco
        {
            get { return _listaTabelaPreco; }
            set
            {
                _listaTabelaPreco = value;
                NotifyPropertyChanged();
            }
        }

        private BasicPickerModel _currentTabelaPreco;

        public BasicPickerModel CurrentTabelaPreco
        {
            get { return _currentTabelaPreco; }
            set
            {
                _currentTabelaPreco = value;
                if (value != null && editarItemViewModel != null)
                {
                    editarItemViewModel.CurrentTabelaPreco = value;
                }
                NotifyPropertyChanged();
            }
        }

        private IEnumerable<BasicPickerModel> _listaLocalEstoque = new List<BasicPickerModel>
        {
            new BasicPickerModel {Id = 0, Display = "Local Padrão"}
        };
        public IEnumerable<BasicPickerModel> ListaLocalEstoque
        {
            get { return _listaLocalEstoque; }
            set
            {
                _listaLocalEstoque = value;
                NotifyPropertyChanged();
            }
        }

        private BasicPickerModel _currentLocalEstoque;

        public BasicPickerModel CurrentLocalEstoque
        {
            get { return _currentLocalEstoque; }
            set
            {
                _currentLocalEstoque = value;
                if (value != null && editarItemViewModel != null)
                {
                    editarItemViewModel.CurrentLocalEstoque = value;
                }

                NotifyPropertyChanged();
            }
        }

        public bool bAdmin
        {
            get
            {
                try
                {
                    return App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.stAdministrador;
                }
                catch (Exception)
                {
                    return false;
                }

            }
        }

        public ListarProdutosNewViewModel()
        {
            CurrentTabelaPreco = ListaTabelaPreco.FirstOrDefault();
            CurrentLocalEstoque = ListaLocalEstoque.FirstOrDefault();
            bFiltroBotaoItens = true; // coloco isso pra que quando abra a tela o botão de itens seja o marcado inicial.
            Config = new ConfiguracaoPesquisaProdutoModel();
            Produtos = new ObservableCollection<PedidoVendaItensModel>();
            LoadItensCommand = new Command(LoadItens);
            SearchCommand = new Command(Search);
            AplicaFiltroItensCommand = new Command(AplicaFiltroItens);
            AplicaFiltroRecorrenciaCommand = new Command(AplicaFiltroRecorrencia);
            AplicaFiltroCampanhaCommand = new Command(AplicaFiltroCampanhas);
            AplicaFiltroDestaquesCommand = new Command(AplicaFiltroDestaques);

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

            ConfiguracaoCommand = new Command(() =>
            {
                UtilNavidate.PushAsync(new PageConfigurarPesquisaProduto(Config));
            });


            this._buscaPreco = new BuscaPreco();
        }



        public bool Initialize()
        {
            if (canExecuteInicial && !IsBusy)
            {
                canExecuteInicial = false;
                Produtos = new ObservableCollection<PedidoVendaItensModel>();
                //Device.BeginInvokeOnMainThread(() =>
                //{
                //    IsBusy = true;
                //});

                VerificaLocaisEstoque();
                VerificaMostraVariacoes();
                PreparaInformacoesConfigs();
                LoadItens();
            }
            return canExecuteInicial;
        }



        private async void LoadItens()
        {
            try
            {
                if (IsBusy)
                    return;


                await Task.Run(() =>
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        IsBusy = true;
                        this.bListaItensHabilitada = false;

                        var idClientesOffLine = currentPedidoViewModel.ItemCliente.Id;
                        var idClientes = ClienteRepository.GetIdClienteNuvem(idClientesOffLine);
                        var idRepresentante = (currentPedidoViewModel.currentModel.idRepresentantePedido ?? App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa_aspnetUsers) ?? 0;


                        var iCountToFind = 30;

                        if (App.EnvironmentPE.TipoPageProdutos == 0)
                        {
                            iCountToFind = 15;
                        }
                        else if (App.EnvironmentPE.TipoPageProdutos == 1)
                        {
                            iCountToFind = 15;
                        }

                        IBuscaProdutosParaVenda _buscaProdutos = new BuscaProdutosExistentesEmTabelaPreco(
                            idCliente: idClientes, idClienteOffLine: idClientesOffLine, idRepresentante: idRepresentante, idRepresentacao: 0
                            );


                        List<int> _idProdutos = new List<int>();


                        if (bFiltroBotaoCampanhas)
                        {
                            _idProdutos = _buscaProdutos.BuscarProdutosCampanha(idEmpresa: App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa, idCondicaoParaTabelaPreco:
                    currentPedidoViewModel.ItemCondicaoPgto.Id);

                            if (_idProdutos?.Count() == 0)
                                _idProdutos.Add(0);
                        }
                        else
                            _idProdutos = _buscaProdutos.BuscarProdutos(idEmpresa: App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa, idCondicaoParaTabelaPreco:
                         currentPedidoViewModel.ItemCondicaoPgto.Id);

                        var lItens = ProdutoRepository.Get(
                            Produtos.Count,
                            iCountToFind,
                            (IsUsingSearch ? xFiltro : ""),
                            Config,
                            idClientesOffLine,
                            idClientes,
                            idRepresentante,
                            _idProdutos,
                            bUsaLocaisEstoque,
                            false,
                            bFiltroBotaoRecorrencia,
                            bBotaoFiltroDestaques
                            );

                        this._buscaPreco.Buscar(itens: lItens,
                            idClienteOff: idClientesOffLine, idCliente: idClientes,
                            idRepresentante: idRepresentante,
                            idEmpresa: App.EnvironmentPE.idEmpresaLogada,
                            idTabelaPrecoCondicao: currentPedidoViewModel.idTabelaPrecoCondicao
                            );

                        AplicaRegrasComerciaisProduto(lItens);

                        bool bAplicaBloquearVisualizacaoEstoque = false;
                        if (currentPedidoViewModel.currentModel.bBloquearVisualizacaoEstoqueVendedor && App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.stAdministrador)
                        {
                            bAplicaBloquearVisualizacaoEstoque = true;
                        }

                        foreach (var item in lItens)
                        {
                            item.bBloquearVisualizacaoEstoqueVendedor = bAplicaBloquearVisualizacaoEstoque;
                            item.bExibirValorPorPeso = currentPedidoViewModel.bExibirValorPorPeso;
                            if (ItensSelecionados != null)
                            {
                                Produtos.Add(ItensSelecionados.Any(c => c.idProduto == item.idProduto)
                                    ? ItensSelecionados.FirstOrDefault(c => c.idProduto == item.idProduto)
                                    : item);
                            }
                        }

                        if (Produtos.Any() && currentPedidoViewModel.currentModel.lItens.Any())
                        {
                            foreach (var itemIncluso in currentPedidoViewModel.currentModel.lItens)
                            {
                                if (Produtos.Any(c => c.idProduto == itemIncluso.idProduto))
                                {
                                    var item = Produtos.FirstOrDefault(c => c.idProduto == itemIncluso.idProduto);
                                    item.ItemJaIncluso = true;
                                }
                            }
                        }

                        IsBusy = false;
                        this.bListaItensHabilitada = true;
                    });
                    //isLoading = false;
                });

                //Device.BeginInvokeOnMainThread(() =>
                //{
                //    IsBusy = false;
                //    this.bListaItensHabilitada = true;
                //});
            }
            catch (Exception ex)
            {
                ex.TrakException("desculpe por isso =/", true);
                Device.BeginInvokeOnMainThread(() =>
                {
                    IsBusy = false;
                });
            }
        }

        public async void Search()
        {

            try
            {
                await Task.Run(() =>
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        IsUsingSearch = true;
                        if (Device.RuntimePlatform == Device.Android || Device.RuntimePlatform == Device.iOS)
                            Produtos.Clear();
                        else
                            Produtos = new ObservableCollection<PedidoVendaItensModel>();
                        LoadItens();
                    });
                });
            }
            catch (Exception ex)
            {
                ex.TrakException();
            }
        }

        public void AplicaRegrasComerciaisProduto(List<PedidoVendaItensModel> lItens)
        {

            if (!(currentPedidoViewModel.currentModel.lRegrasComerciais.Count > 0))
                return;

            var regra = currentPedidoViewModel.currentModel.lRegrasComerciais?.FirstOrDefault();

            switch (regra.stTipoPercentual)
            {
                case 0:
                    break;
                case 1:

                    //foreach (var item in currentModel.lItens)
                    //{
                    //    item.vAcrescimo += item.vSubTotalSemImpostos - item.vSubTotal;
                    //}

                    break;
                case 2:

                    foreach (var item in lItens)
                    {
                        item.pDesconto += (double)(regra?.nPercentual ?? 0);
                        item.vDesconto += item.vUnitarioVendaComImpostos * (item.pDesconto / 100.0);

                        //item.vSubTotal -= item.vDesconto;
                        //item.vSubTotalSemImpostos -= item.vDesconto;

                        item.vUnitarioVendaComImpostos -= item.vDesconto;
                        item.vUnitarioVendaComImpostosOriginal -= item.vDesconto;
                        //item.vVenda = item.vDesconto;


                        // item.pDesconto += (double)(regra?.nPercentual ?? 0);
                        //item.vDesconto += item.vSubTotalSemImpostos * (item.pDesconto / 100.0);

                        //item.vSubTotal -= item.vDesconto;
                        //item.vSubTotalSemImpostos -= item.vDesconto;

                        //item.vUnitarioVenda -= item.vDesconto;
                        //item.vUnitarioVendaSemImposto -= item.vDesconto;
                        //item.vUnitarioVendaComImpostos -= item.vDesconto;
                        //item.vUnitarioVendaComImpostosOriginal -= item.vDesconto;
                        //item.vVenda = item.vDesconto;
                    }

                    currentPedidoViewModel.currentModel.idRegraComercial = (int)(regra.idRegraComercial);

                    break;
            }
        }

        public async void AplicaFiltroItens()
        {
            try
            {
                await Task.Run(() =>
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        bFiltroBotaoItens = true;
                        bFiltroBotaoCampanhas = false;
                        bBotaoFiltroDestaques = false;
                        bFiltroBotaoRecorrencia = false;

                        if (Device.RuntimePlatform == Device.Android || Device.RuntimePlatform == Device.iOS)
                            Produtos.Clear();
                        else
                            Produtos = new ObservableCollection<PedidoVendaItensModel>();


                        LoadItens();
                    });
                });
            }
            catch (Exception ex)
            {
                ex.TrakException();
            }
        }

        public async void AplicaFiltroCampanhas()
        {
            try
            {
                await Task.Run(() =>
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        bFiltroBotaoItens = false;
                        bFiltroBotaoCampanhas = true;
                        bBotaoFiltroDestaques = false;
                        bFiltroBotaoRecorrencia = false;

                        if (Device.RuntimePlatform == Device.Android || Device.RuntimePlatform == Device.iOS)
                            Produtos.Clear();
                        else
                            Produtos = new ObservableCollection<PedidoVendaItensModel>();


                        LoadItens();
                    });
                });

            }
            catch (Exception ex)
            {
                ex.TrakException();
            }
        }

        public async void AplicaFiltroRecorrencia()
        {
            try
            {
                await Task.Run(() =>
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        bFiltroBotaoItens = false;
                        bFiltroBotaoCampanhas = false;
                        bBotaoFiltroDestaques = false;
                        bFiltroBotaoRecorrencia = true;

                        if (Device.RuntimePlatform == Device.Android || Device.RuntimePlatform == Device.iOS)
                            Produtos.Clear();
                        else
                            Produtos = new ObservableCollection<PedidoVendaItensModel>();


                        LoadItens();
                    });
                });

            }
            catch (Exception ex)
            {
                ex.TrakException();
            }

        }

        public async void AplicaFiltroDestaques()
        {
            try
            {
                await Task.Run(() =>
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        bFiltroBotaoItens = false;
                        bFiltroBotaoCampanhas = false;
                        bBotaoFiltroDestaques = true;
                        bFiltroBotaoRecorrencia = false;

                        if (Device.RuntimePlatform == Device.Android || Device.RuntimePlatform == Device.iOS)
                            Produtos.Clear();
                        else
                            Produtos = new ObservableCollection<PedidoVendaItensModel>();


                        LoadItens();
                    });
                });

            }
            catch (Exception ex)
            {
                ex.TrakException();
            }
        }


        public async void VerificaLocaisEstoque()
        {
            try
            {
                await Task.Run(() =>
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        bUsaLocaisEstoque = PedidoRepository.EmpresaUtilizaLocaisEstoque(App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa);
                    });
                });
            }
            catch (Exception ex)
            {
                ex.TrakException();
            }
        }

        public async void VerificaMostraVariacoes()
        {
            try
            {
                await Task.Run(() =>
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        bMostraProdutosVariacoes = PedidoRepository.EmpresaUtilizaLocaisEstoque(App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa);
                    });
                });
            }
            catch (Exception ex)
            {
                ex.TrakException();
            }
        }

        public void PreparaInformacoesConfigs()
        {
            try
            {
                if (currentPedidoViewModel.currentModel.bUsaMinimoVendas && currentPedidoViewModel.currentModel.stCadastroMinimoVenda == 2)
                {
                    Config.bUtilizaMinimoVendasProduto = true;
                    Config.stCalculoVendas = currentPedidoViewModel.currentModel.stCalculoMinimoVenda;
                }
            }
            catch (Exception ex)
            {
                ex.TrakException();
            }
        }


        private ObservableCollection<PedidoVendaItensModel> _ItensSelecionados = new ObservableCollection<PedidoVendaItensModel>();

        public ObservableCollection<PedidoVendaItensModel> ItensSelecionados
        {
            get { return _ItensSelecionados; }
            set { _ItensSelecionados = value; NotifyPropertyChanged(); }
        }



        public async void SaveItem()
        {
            try
            {
                if (IsBusy)
                    return;

                if (currentPedidoViewModel?.currentModel?.CurrentItemModel != null)
                {
                    PedidoVendaCalculos.AtualizaValores(currentPedidoViewModel.currentModel.CurrentItemModel);

                    //currentPedidoViewModel.currentModel.CurrentItemModel.SetDetalheItem();
                    var vQtde = currentPedidoViewModel.currentModel.CurrentItemModel.vQtdItem;

                    if (currentPedidoViewModel.currentModel.CurrentItemModel.ItensGrade != null && currentPedidoViewModel.currentModel.CurrentItemModel.ItensGrade.Any())
                        vQtde = currentPedidoViewModel.currentModel.CurrentItemModel.ItensGrade.Sum(c => c.vQtdItem);

                    if (itemSelected != null)
                    {
                        if (vQtde > 0 && itemSelected.currentTabelaPreco != null)
                        {
                            if (ItensSelecionados.Any(c => c.idProduto == itemSelected.idProduto) == false)
                            {
                                if ((itemSelected.vQtdEstoque > 0 || itemSelected.vQtdEstoque == null) || itemSelected.stVendaSemEstoque)
                                    ItensSelecionados.Add(itemSelected);
                                else
                                {
                                    itemSelected.vQtdItem -= 1;
                                    await App.Current.MainPage.DisplayAlert("Erro", "Não é possível adicionar um item sem estoque", "Ok");
                                }
                            }
                        }
                        else if (ItensSelecionados.Any(c => c.idProduto == itemSelected.idProduto))
                            ItensSelecionados.Remove(itemSelected);
                    }

                    //var _item = currentPedidoViewModel.currentModel.CurrentItemModel;

                    //_item.vSubTotal = vQtde * _item.vUnitarioVendaComImpostos;
                    //_item.vSubTotalSemImpostos = vQtde * _item.vUnitarioVenda;
                }
            }
            catch (Exception ex)
            {
                ex.TrakException("SaveItem");

            }
        }

        private bool _verificouEstoque;

        public bool verificouEstoque
        {
            get { return _verificouEstoque; }
            set
            {
                _verificouEstoque = value; NotifyPropertyChanged();
            }
        }

        public async void VerificaQtdEstoque(string qtd)
        {
            try
            {
                var vQtde = currentPedidoViewModel.currentModel.CurrentItemModel.vQtdItem;

                if (vQtde > itemSelected.vQtdEstoque && !itemSelected.stVendaSemEstoque)
                {
                    if (!verificouEstoque)
                    {
                        itemSelected.vQtdItem = Convert.ToDouble(qtd.Replace(",", "."));
                        verificouEstoque = true;
                        await App.Current.MainPage.DisplayAlert("Erro", "Estoque insuficiente", "Ok");
                    }
                }
                else
                    verificouEstoque = false;

            }
            catch (Exception ex)
            {

            }
        }

        public void SaveItens()
        {
            foreach (var item in ItensSelecionados)
            {
                PagePedidoNew.CurrentViewModel.currentModel.lItens.Add(item);
                StaticModel.StaticEditarItemViewModel = null;
            }
            PagePedidoNew.CurrentViewModel.AtualizaTotalizadoresPedido();
            UtilNavidate.PopAsync();
        }

    }
}
