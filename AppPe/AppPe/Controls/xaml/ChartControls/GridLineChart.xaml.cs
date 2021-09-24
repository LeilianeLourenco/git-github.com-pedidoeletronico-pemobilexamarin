using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Model.Chart.Horizontal;

namespace Xamarin.HLP.Mobile.AppPE.Controls.xaml.ChartControls
{
    public partial class GridLineChart : Grid
    {
        public GridLineChart(SerieHorizontalModel viewmodel)
        {
            InitializeComponent();
            BindingContext = viewmodel;
        }
    }
}
