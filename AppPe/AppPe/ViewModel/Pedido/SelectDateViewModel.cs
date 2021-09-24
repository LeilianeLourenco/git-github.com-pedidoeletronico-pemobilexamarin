using System;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Model.Cadastros;
using Xamarin.HLP.Mobile.AppPE.Model.Lancamento;

namespace Xamarin.HLP.Mobile.AppPE.ViewModel.Pedido
{
    public class SelectDateViewModel : NotifyCommon
    {

        private DateTime _date;

        public DateTime date
        {
            get { return _date; }
            set { _date = value; NotifyPropertyChanged(); }
        }



        private TimeSpan _time;

        public TimeSpan time
        {
            get { return _time; }
            set { _time = value; NotifyPropertyChanged(); }
        }

        public ICommand VoltarCommand { get; set; }


        public tipolancamento Tipolancamento { get; set; }

        public PedidoVendaModel pedido { get; set; }
        public AtividadeAgendaModel atividade { get; set; }
        public enum tipolancamento
        {
            PEDIDO,
            ORCAMENTO,
            PREVISAO_ENTREGA,
            INICIO_EVENTO,
            FIM_EVENTO
        };

        public SelectDateViewModel()
        {
            VoltarCommand = new Command(() =>
            {
                if (Tipolancamento == tipolancamento.PEDIDO)
                {
                    pedido.dEmissao = new DateTime(date.Year, date.Month, date.Day, time.Hours, time.Minutes, time.Seconds, 0, DateTimeKind.Local);
                }
                else if (Tipolancamento == tipolancamento.ORCAMENTO)
                {
                    pedido.dtValidadeOrcamento = new DateTime(date.Year, date.Month, date.Day, time.Hours, time.Minutes, time.Seconds, 0, DateTimeKind.Local);
                }
                else if (Tipolancamento == tipolancamento.PREVISAO_ENTREGA)
                {
                    pedido.dtPrevisto = new DateTime(date.Year, date.Month, date.Day, time.Hours, time.Minutes, time.Seconds, 0, DateTimeKind.Local);
                }
                else if (Tipolancamento == tipolancamento.INICIO_EVENTO)
                { 
                    atividade.dtInicioEvento = new DateTime(date.Year, date.Month, date.Day, time.Hours, time.Minutes, time.Seconds, 0, DateTimeKind.Local);
                    atividade.dtFimEvento = atividade.dtInicioEvento?.AddMinutes(30);
                }
                else if (Tipolancamento == tipolancamento.FIM_EVENTO)
                {
                    atividade.dtFimEvento = new DateTime(date.Year, date.Month, date.Day, time.Hours, time.Minutes, time.Seconds, 0, DateTimeKind.Local);
                    if (atividade.dtFimEvento < atividade.dtInicioEvento)
                    {
                        App.Messages.ShowAsync("Data final não pode ser menor que a data inicial do evento");
                        return;
                    }
                    else 
                        atividade.dtFimEvento = new DateTime(date.Year, date.Month, date.Day, time.Hours, time.Minutes, time.Seconds, 0, DateTimeKind.Local);
                }
                UtilNavidate.PopModalAsync();
            });
        }
    }
}
