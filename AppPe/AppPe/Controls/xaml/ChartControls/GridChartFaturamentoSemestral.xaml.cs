using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.ViewModel.Chart;

namespace Xamarin.HLP.Mobile.AppPE.Controls.xaml.ChartControls
{
    public partial class GridChartFaturamentoSemestral : Grid
    {


        public GridChartFaturamentoSemestral()
        {
            InitializeComponent();
        }

        public void InitiDados()
        {
            //Device.StartTimer(new TimeSpan(0, 0, 0, 1), CarregarChart);
            CarregarChart();
        }

        public bool bTodosUsuarios { get; set; }

        private bool CarregarChart()
        {
            if (ViewModel.canExecuteInicial)
            {
                ViewModel.CarregarChart(ChartStack, bTodosUsuarios);
            }
            return ViewModel.canExecuteInicial;
        }



        public FaturamentoSemestralChartViewModel ViewModel => BindingContext as FaturamentoSemestralChartViewModel;
    }
}
