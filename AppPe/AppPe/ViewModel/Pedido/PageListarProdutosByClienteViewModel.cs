using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Core.PedidoVenda;
using Xamarin.HLP.Mobile.AppPE.Model.Lancamento;
using Xamarin.HLP.Mobile.AppPE.Model.Repository;
using Xamarin.HLP.Mobile.AppPE.View.Pedido;

namespace Xamarin.HLP.Mobile.AppPE.ViewModel.Pedido
{
    public class PageListarProdutosByClienteViewModel : SearchCommom
    {
        BuscaPreco _buscaPreco;

        private ICommand _GerarPedido;

        public ICommand GerarPedido
        {
            get { return _GerarPedido; }
            set { _GerarPedido = value; NotifyPropertyChanged(); }
        }


        public PagePedidoNew pagePedidoNew { get; set; }

        private ObservableCollection<PedidoVendaItensModel> _Produtos = new ObservableCollection<PedidoVendaItensModel>();
        public ObservableCollection<PedidoVendaItensModel> Produtos
        {
            get { return _Produtos; }
            set { _Produtos = value; NotifyPropertyChanged(); }
        }

         

        private Dictionary<int, string> _lProdutosValoresDivergentes = new Dictionary<int, string>();
        public Dictionary<int,string> lProdutosValoresDivergentes
        {
            get { return _lProdutosValoresDivergentes; }
            set { _lProdutosValoresDivergentes = value; NotifyPropertyChanged(); }
        }

        public int idClientesOffLine { get; set; }

        public int idClientes { get; set; }

        public bool bUltimosProdutosAdquiridos { get; set; }


        private int _iCount;

        public int iCount
        {
            get { return _iCount; }
            set { _iCount = value; NotifyPropertyChanged(); }
        }

        private string _xNameCliente;

        public string xNameCliente
        {
            get { return _xNameCliente; }
            set { _xNameCliente = value; NotifyPropertyChanged(); }
        }

        private async void LoadItensUltimosProdutosCliente()
        {
            //Device.BeginInvokeOnMainThread(() =>
            //{
            //    IsBusy = true;
            //});

            //await Task.Run(() =>
            //{
            //    var lItens = ProdutoRepository.Get(
            //        Produtos.Count,
            //        500,
            //        "",
            //        new ConfiguracaoPesquisaProdutoModel { bUltimasCompras = true },
            //        idClientesOffLine,
            //        idClientes, 0);

            //    //this._buscaPreco = new BuscaPreco();
            //    //this._buscaPreco.Buscar(itens: lItens,
            //    //        idClienteOff: idClientesOffLine, idCliente: idClientes,
            //    //        idRepresentante: App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa_aspnetUsers ?? 0,
            //    //        idEmpresa: App.EnvironmentPE.idEmpresaLogada
            //    //        );

            //    Device.BeginInvokeOnMainThread(() =>
            //    {
            //        foreach (var item in lItens)
            //        {
            //            item.vUnitarioVenda = item.vVenda;
            //            item.vUnitarioVendaComImpostos = item.vVenda;

            //            Produtos.Add(item);
            //        }
            //        iCount = Produtos.Count;
            //    });
            //});

            //Device.BeginInvokeOnMainThread(() =>
            //{
            //    IsBusy = false;
            //});


            await Task.Run(() =>
            {
                var lItens = ProdutoRepository.Get(
                    Produtos.Count,
                    500,
                    "",
                    new ConfiguracaoPesquisaProdutoModel { bUltimasCompras = true },
                    idClientesOffLine,
                    idClientes, 0);

                this._buscaPreco = new BuscaPreco();
                this._buscaPreco.Buscar(itens: lItens,
                        idClienteOff: idClientesOffLine, idCliente: idClientes,
                        idRepresentante: App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa_aspnetUsers ?? 0,
                        idEmpresa: App.EnvironmentPE.idEmpresaLogada
                        );

                foreach (var item in lItens)
                {
                    item.vUnitarioVenda = item.vUltimaVenda;
                    item.vUnitarioVendaComImpostos = item.vUltimaVenda;
                    item.vUnitarioVendaComImpostosOriginal = item.vUltimaVenda;
                    item.vUltimaVenda = item.vUltimaVenda;
                    item.vQtdUltimaVenda = item.vQtdUltimaVenda;
                    item.vVenda = item.vUnitarioVendaSemImposto;
                    PedidoVendaCalculos.BuscaValorDesconto(item);

                    //utilizado para checar produtos com valores diferentes da tabela de preço atual e se o desc máx foi ultrapassado
                    if (item.vUltimaVenda != item.vUnitarioVendaSemImposto && item.currentTabelaPreco.vVenda > 0)
                    {
                        item.bProdutoComValorDiferente = true;
                        item.bDescMaximoPermitido = PedidoVendaCalculos.DescontoValidoVlrUnitarioComImpostos(item);

                        if (!item.bDescMaximoPermitido)
                        {
                            lProdutosValoresDivergentes.Add(item.idProduto.GetValueOrDefault(),item.xDescricao);
                        }
                    }


             

                    Produtos.Add(item);
                }
                iCount = Produtos.Count;
            });

        }


        private async void LoadItens()
        {
            //Device.BeginInvokeOnMainThread(() =>
            //{
            //    IsBusy = true;
            //});

            //await Task.Run(() =>
            //{
            //    var lItens = ProdutoRepository.Get(
            //        Produtos.Count,
            //        500,
            //        "",
            //        new ConfiguracaoPesquisaProdutoModel { bUltimasCompras = true },
            //        idClientesOffLine,
            //        idClientes, 0);

            //    this._buscaPreco = new BuscaPreco();
            //this._buscaPreco.Buscar(itens: lItens,
            //        idClienteOff: idClientesOffLine, idCliente: idClientes,
            //        idRepresentante: App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa_aspnetUsers ?? 0,
            //        idEmpresa: App.EnvironmentPE.idEmpresaLogada
            //            );

            //    Device.BeginInvokeOnMainThread(() =>
            //    {
            //        foreach (var item in lItens)
            //        {
            //            Produtos.Add(item);
            //        }
            //        iCount = Produtos.Count;
            //    });
            //});

            //Device.BeginInvokeOnMainThread(() =>
            //{
            //    IsBusy = false;
            //});

            await Task.Run(() =>
            {
                var lItens = ProdutoRepository.Get(
                    Produtos.Count,
                    500,
                    "",
                    new ConfiguracaoPesquisaProdutoModel { bUltimasCompras = true },
                    idClientesOffLine,
                    idClientes, 0);

                this._buscaPreco = new BuscaPreco();
                this._buscaPreco.Buscar(itens: lItens,
                        idClienteOff: idClientesOffLine, idCliente: idClientes,
                        idRepresentante: App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa_aspnetUsers ?? 0,
                        idEmpresa: App.EnvironmentPE.idEmpresaLogada
                            );

                foreach (var item in lItens)
                {
                    Produtos.Add(item);
                }
                iCount = Produtos.Count;
            });
        }


        public bool Initialize()
        {
            if (canExecuteInicial && !IsBusy)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    IsBusy = true;
                    canExecuteInicial = false;

                    xNameCliente = ClienteRepository.GetDisplayByIdOffLine(idClientesOffLine);
                    Produtos = new ObservableCollection<PedidoVendaItensModel>();

                    if (bUltimosProdutosAdquiridos)
                        LoadItensUltimosProdutosCliente();
                    else
                        LoadItens();

                    IsBusy = false;
                });
            }
            return canExecuteInicial;
        }


        public PageListarProdutosByClienteViewModel()
        {
            pagePedidoNew = new PagePedidoNew(new PedidoVendaModel
            {
                idClientesOffLine = idClientesOffLine,
                idClientes = idClientes
            }, true);

            GerarPedido = new Command(async () =>
            {
                pagePedidoNew.ViewModel.currentModel.idClientesOffLine = idClientesOffLine;
                pagePedidoNew.ViewModel.currentModel.idClientes = idClientes;
                if (Produtos.Count(c => c.vQtdItem > 0) > 0)
                {

                    //processo para verificar sobre os valores da lista de pedido
                    if (Produtos.Where(c => c.vQtdItem > 0 && c.bProdutoComValorDiferente == true).Count() > 0)
                    {


                        if (!await App.Messages.ShowConfirmAsync($"Existem produtos com valores diferentes da tabela de preço, deseja atualizar os valores?",
                             "Sim", "Não"))
                        {
                            List<int> idsProdutosDivergentes =  Produtos.Where(c => c.vQtdItem > 0 && c.bProdutoComValorDiferente == true).Select(p => p.idProduto ?? 0).ToList();
                            if (lProdutosValoresDivergentes.Where(p => idsProdutosDivergentes.Contains(p.Key))?.Count() > 0)
                            {
                                string xProdutosMsg = string.Join(",", lProdutosValoresDivergentes.Where(p => idsProdutosDivergentes.Contains(p.Key)).Select(p => p.Value));

                                if (!await App.Messages.ShowConfirmAsync($"Os produtos: {xProdutosMsg} possuem um valor acima do desconto máximo permitido. Caso queira continuar os valores serão atualizados, continuar?",
                                     "Sim", "Não"))
                                {
                                    return;
                                }
                                else
                                {
                                    foreach (var item in Produtos.Where(c => c.vQtdItem > 0))
                                    {
                                        item.vSubTotal = item.currentTabelaPreco.vVenda * item.vQtdItem;
                                        item.vSubTotalSemImpostos = item.currentTabelaPreco.vUnitario * item.vQtdItem;
                                        item.vUnitarioVenda = item.currentTabelaPreco.vVenda;
                                        item.vUnitarioVendaComImpostos = item.currentTabelaPreco.vVenda;
                                        item.vUnitarioVendaComImpostosOriginal = item.currentTabelaPreco.vVenda;
                                        item.vUltimaVenda = 0;
                                        item.pDesconto = 0;
                                        item.vDesconto = 0;
                                        item.vComissao = (item.pComissao / 100) * item.vSubTotal;
                                        item.SetDetalheItem();
                                        //PedidoVendaCalculos.BuscaValorDesconto(item);
                                    }
                                } 
                            }
                        }
                        else
                        {
                            foreach (var item in Produtos.Where(c => c.vQtdItem > 0))
                            {
                                item.vSubTotal = item.currentTabelaPreco.vVenda * item.vQtdItem;
                                item.vUnitarioVenda = item.currentTabelaPreco.vVenda;
                                item.vUnitarioVendaComImpostos = item.currentTabelaPreco.vVenda; 
                                item.vUnitarioVendaComImpostosOriginal = item.currentTabelaPreco.vVenda; 
                                item.vUltimaVenda = 0;
                                item.pDesconto = 0;
                                item.vDesconto = 0;
                                item.SetDetalheItem();
                                item.vComissao = (item.pComissao / 100) * item.vSubTotal;
                                //PedidoVendaCalculos.BuscaValorDesconto(item);
                            }
                        }
                    }


                    PagePedidoNew.CurrentViewModel.currentModel.lItens = new ObservableCollection<PedidoVendaItensModel>(Produtos.Where(c => c.vQtdItem > 0));
                }

                UtilNavidate.PopAsync();
                Device.StartTimer(new TimeSpan(0, 0, 0, 0, 200), GoToPedido);
            });
        }


        public bool GoToPedido()
        {
            if (!IsBusy)
                UtilNavidate.PushAsync(pagePedidoNew);

            return false;
        }

    }
}
