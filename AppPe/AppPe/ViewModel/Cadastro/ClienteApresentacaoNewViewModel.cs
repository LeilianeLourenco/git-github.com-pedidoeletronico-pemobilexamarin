using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Model;
using Xamarin.HLP.Mobile.AppPE.Model.Cadastros;
using Xamarin.HLP.Mobile.AppPE.Model.Lancamento;
using Xamarin.HLP.Mobile.AppPE.Model.Repository;
using Xamarin.HLP.Mobile.AppPE.View.Agenda;
using Xamarin.HLP.Mobile.AppPE.View.Cliente;
using Xamarin.HLP.Mobile.AppPE.View.Pedido;

// ReSharper disable All

namespace Xamarin.HLP.Mobile.AppPE.ViewModel.Cadastro
{
    public class ClienteApresentacaoNewViewModel : NotifyCommon
    {


        #region Propriedades
        private ClientesModel _currentModel = new ClientesModel();
        public ClientesModel currentModel
        {
            get { return _currentModel; }
            set { _currentModel = value; NotifyPropertyChanged(); }
        }

        public int idClientesOffLine { get; set; }

        private string _xProspeccao;

        public string xProspeccao
        {
            get { return _xProspeccao; }
            set { _xProspeccao = value; NotifyPropertyChanged(); }
        }

        private string _xTrasportador;

        public string xTransportadora
        {
            get { return _xTrasportador; }
            set { _xTrasportador = value; NotifyPropertyChanged(); }
        }

        private bool _bAcessaAgenda;

        public bool bAcessaAgenda
        {
            get { return _bAcessaAgenda; }
            set { _bAcessaAgenda = value; NotifyPropertyChanged(); }
        }


        private string _xTabelaPreco;

        public string xTabelaPreco
        {
            get { return _xTabelaPreco; }
            set { _xTabelaPreco = value; NotifyPropertyChanged(); }
        }

        private string _xCondPgto;

        public string xCondPgto
        {
            get { return _xCondPgto; }
            set { _xCondPgto = value; NotifyPropertyChanged(); }
        }
        #endregion


        #region Commands

        public ICommand PedidosCommand { get; set; }
        public ICommand AgendaCommand { get; set; }

        public ICommand FinanceiroCommand { get; set; }
        public ICommand ProdutosCommand { get; set; }


        public ICommand FoneEmailCommand { get; set; }
        public ICommand ContatosCommand { get; set; }
        public ICommand EnderecoCommand { get; set; }


        public ICommand DeleteCommand { get; set; }

        public ICommand AtualizarCommand { get; set; }

        public PageCliente pageCliente { get; set; }
        #endregion



        public ClienteApresentacaoNewViewModel()
        {
            AgendaCommand = new Command(() =>
            {
                var page = new PageListagemEventos(bUsaClienteEspecifico: true); 
                UtilNavidate.PushAsync(page);
            });

            PedidosCommand = new Command(() =>
            {
                PagePedidoNew.CurrentViewModel.currentModel = new PedidoVendaModel
                {
                    idClientesOffLine = currentModel.idClientesOffLine ?? 0,
                    idClientes = currentModel.idClientes
                };

                var page = new PageListarPedidos(bUsaClienteEspecifico: true);
                page.setCommand(GerarPedido);
                UtilNavidate.PushAsync(page);
            });

            FinanceiroCommand = new Command(() =>
            {
                PageFinanceiroCliente page = new PageFinanceiroCliente(currentModel.idClientesOffLine.GetValueOrDefault()); 
                UtilNavidate.PushAsync(page);
            });

            FoneEmailCommand = new Command(() =>
            {
                UtilNavidate.PushAsync(new PageTelefonesCliente(currentModel));
            });

            ContatosCommand = new Command(() =>
            {
                UtilNavidate.PushAsync(new PageListagemContato(currentModel));
            });

            EnderecoCommand = new Command(() =>
            {
                UtilNavidate.PushAsync(new PageListagemEndereco(currentModel));
            });
            DeleteCommand = new Command(Delete);

            ProdutosCommand = new Command(() =>
            {
                UtilNavidate.PushAsync(new PageListarProdutosByCliente(currentModel.idClientesOffLine ?? 0, currentModel.idClientes ?? 0, bUltimosProdutosAdquiridos: true));
            });

            AtualizarCommand = new Command(() =>
            {
                pageCliente.ViewModel.currentModel = currentModel;
                UtilNavidate.PushAsync(pageCliente);
            });

        }

        private async void Delete()
        {
            var removido = await ClienteRepository.Delete(currentModel);
            if (!removido) return;
            UtilNavidate.PopAsync();
        }


        public bool Initialize()
        {
            if (canExecuteInicial)
            {
                canExecuteInicial = false;

                Device.BeginInvokeOnMainThread(() =>
                {
                    currentModel = ClienteRepository.GetClienteModel(idClientesOffLine);
                    bAcessaAgenda = (App.planoAtual == Hlp.PedidoEletronico.Domain.Business.Bo.Planos.plbus 
                    || App.planoAtual == Hlp.PedidoEletronico.Domain.Business.Bo.Planos.plprem
                    || App.planoAtual == Hlp.PedidoEletronico.Domain.Business.Bo.Planos.pldeg) ? true : false;


                    PagePedidoNew.CurrentViewModel.currentModel = new PedidoVendaModel
                    {
                        idClientesOffLine = currentModel.idClientesOffLine ?? 0,
                        idClientes = currentModel.idClientes
                    };

                    CarregarDadosFK();
                    pageCliente = new PageCliente(currentModel);
                });
            }
            return canExecuteInicial;
        }

        private async void GerarPedido()
        {              
            UtilNavidate.PushAsync(new PagePedidoNew(PagePedidoNew.CurrentViewModel.currentModel, true));
        }

        private async void CarregarDadosFK()
        {
            try
            {
                await Task.Run(() =>
                {
                    xTabelaPreco = TabelaPrecoRepository.GetNameRegistro(currentModel.idTabelaPreco ?? 0);
                    xCondPgto = CondicaoPagamentoRepository.GetDisplay(currentModel.idCondicaoPagamento ?? 0);
                    xTransportadora = TransportadoraRepository.GetDisplay(currentModel.idTransportadora ?? 0);
                    if (currentModel.stProspeccao == "CP")
                        xProspeccao = "CLIENTE EM PROSPECÇÃO";
                    else
                        xProspeccao = "CLIENTE EFETIVADO";
                });
            }
            catch (Exception ex)
            {
                ex.TrakException();
            }
        }


    }
}
