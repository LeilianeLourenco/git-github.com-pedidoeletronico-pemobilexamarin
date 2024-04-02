using System;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Model.Cadastros;
using Xamarin.HLP.Mobile.AppPE.Model.Lancamento;
using Xamarin.HLP.Mobile.AppPE.ViewModel.Pedido;

namespace Xamarin.HLP.Mobile.AppPE.View.Pedido
{
    public partial class PageSelectDate : ContentPage
    {
        public PageSelectDate(PedidoVendaModel pedido, SelectDateViewModel.tipolancamento tipo)
        {


            InitializeComponent();
            ViewModel.Tipolancamento = tipo;
            ViewModel.pedido = pedido;

            if (tipo == SelectDateViewModel.tipolancamento.PEDIDO)
            {
                ViewModel.date = pedido.dEmissao;
                ViewModel.time = pedido.dEmissao.TimeOfDay;
            }
            else if (tipo == SelectDateViewModel.tipolancamento.ORCAMENTO)
            {
                ViewModel.date = pedido.dtValidadeOrcamento ?? DateTime.Now;
                ViewModel.time = (pedido.dtValidadeOrcamento ?? DateTime.Now).TimeOfDay;
            }
            else if (tipo == SelectDateViewModel.tipolancamento.PREVISAO_ENTREGA)
            {
                ViewModel.date = pedido.dtPrevisto ?? DateTime.Now;
                ViewModel.time = (pedido.dtPrevisto ?? DateTime.Now).TimeOfDay;
            }

        }

        public PageSelectDate(AtividadeAgendaModel evento, SelectDateViewModel.tipolancamento tipo)
        {


            InitializeComponent();
            ViewModel.Tipolancamento = tipo;
            ViewModel.atividade = evento;

            if (tipo == SelectDateViewModel.tipolancamento.INICIO_EVENTO)
            {
                ViewModel.date = evento.dtInicioEvento ?? DateTime.Now;
                ViewModel.time = (evento.dtInicioEvento ?? DateTime.Now).ToLocalTime().TimeOfDay;
            }
            else if (tipo == SelectDateViewModel.tipolancamento.FIM_EVENTO)
            {
                ViewModel.date = evento.dtFimEvento ?? DateTime.Now;
                ViewModel.time = (evento.dtFimEvento ?? DateTime.Now).ToLocalTime().TimeOfDay;
            }
        }

        public SelectDateViewModel ViewModel => BindingContext as SelectDateViewModel;

    }
}
