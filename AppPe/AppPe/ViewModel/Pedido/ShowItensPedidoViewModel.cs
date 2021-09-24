using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Core.PedidoVenda.Implementacoes;
using Xamarin.HLP.Mobile.AppPE.Core.PedidoVenda.Interfaces;
using Xamarin.HLP.Mobile.AppPE.Model.Lancamento;
using Xamarin.HLP.Mobile.AppPE.Model.Repository;
using Xamarin.HLP.Mobile.AppPE.Model.Repository.Interfaces.PedidoVenda;
using Xamarin.HLP.Mobile.AppPE.View.Pedido;

namespace Xamarin.HLP.Mobile.AppPE.ViewModel.Pedido
{
    public class ShowItensPedidoViewModel : NotifyCommon
    {
        public ICommand CloseCommand { get; set; }

        private PedidoVendaModel _currentModel = new PedidoVendaModel();

        public PedidoVendaModel currentModel
        {
            get { return _currentModel; }
            set { _currentModel = value; NotifyPropertyChanged(); }
        }

        private int _countItens = 0;

        public int CountItens
        {
            get { return _countItens; }
            set
            {
                _countItens = value;
                NotifyPropertyChanged();
            }
        }

        public ShowItensPedidoViewModel()
        {
            CloseCommand = new Command(UtilNavidate.PopPopupNew);
        }

        public bool Initialize()
        {
            if (canExecuteInicial)
            {
                canExecuteInicial = false;

                IItensImpressaoPedido _buscaItensImpressao = new ItensImpressaoPedido();
                currentModel = _buscaItensImpressao.RetornarItensParaImpressao(id: PageDetalhesPedido.viewmodelStatic.currentModel.idPedidoVendaOffLine);

                //currentModel = PedidoRepository.GetPedidoVendaModel(PageDetalhesPedido.viewmodelStatic.currentModel.idPedidoVendaOffLine);
                CountItens = currentModel.lItens.Count;
            }
            return canExecuteInicial;
        }
    }
}
