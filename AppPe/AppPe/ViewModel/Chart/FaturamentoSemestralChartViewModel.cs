using System.Linq;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Controls.xaml.ChartControls;
using Xamarin.HLP.Mobile.AppPE.Model.Chart.Horizontal;
using Xamarin.HLP.Mobile.AppPE.Model.Repository;

namespace Xamarin.HLP.Mobile.AppPE.ViewModel.Chart
{
    public class FaturamentoSemestralChartViewModel : ViewModelComum<ChartHorizontalModel>
    {

        public FaturamentoSemestralChartViewModel()
        {
            currentModel = new ChartHorizontalModel
            {
                Title = "buscando informações..."
            };
            IsBusy = true;
        }

        public bool CarregarChart(StackLayout ChartStack, bool bTodosUsuarios)
        {
            if (canExecuteInicial)
            {
                if (ChartStack.Children.Any())
                {
                    ChartStack.Children.Clear();
                    IsBusy = true;
                }
                canExecuteInicial = false;

                if(PedidoRepository.GetDataRelatorio(App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.objEmpresaModel.idEmpresa.GetValueOrDefault()) == 0)
                {
                    currentModel = PedidoRepository.GetChartFaturamentoInLine(bTodosUsuarios);
                }
                else
                {
                    currentModel = PedidoRepository.GetChartFaturamentoPorDataFaturamentoInLine(bTodosUsuarios); 
                }


                foreach (var serie in currentModel.Series)
                {
                    var chart = new GridLineChart(serie);
                    ChartStack.Children.Add(chart);
                }
                currentModel.RefreshAndShow(ChartStack.Width);
                // tratamento

                IsBusy = false;
            }
            return canExecuteInicial;
        }




    }
}
