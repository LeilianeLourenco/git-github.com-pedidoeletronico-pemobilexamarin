using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Common;

namespace Xamarin.HLP.Mobile.AppPE.Model.Chart.Horizontal
{
    public class ChartHorizontalModel : ModelComum
    {

        public ChartHorizontalModel()
        {
            Series = new List<SerieHorizontalModel>();
        }

        private string _title = "Meu dashboard";
        public string Title
        {
            get { return _title; }
            set { _title = value; NotifyPropertyChanged(); }
        }

        private Color _corTemplate = ColorStaticModel.AzulChart;
        public Color CorTemplate
        {
            get { return _corTemplate; }
            set { _corTemplate = value; NotifyPropertyChanged(); }
        }

        private List<SerieHorizontalModel> _series;
        public List<SerieHorizontalModel> Series
        {
            get { return _series; }
            set { _series = value; NotifyPropertyChanged(); }
        }

        public void RefreshAndShow(double width, double? _valorbase = null)
        {
            if (Series.Any())
            {
                double valorbase ;
                if (_valorbase == null)
                {
                    var max = Series.Max(c => c.Valor);
                    valorbase = (max* 1.2);
                }
                else
                    valorbase = (double) _valorbase;

                if (valorbase > 0)
                {
                    foreach (var serie in Series)
                    {
                        serie.WidthLine = (serie.Valor * width) / valorbase;
                    }
                }
            }
        }

    }


    public class SerieHorizontalModel : ModelComum
    {
        private string _Display;
        public string Display
        {
            get { return _Display; }
            set { _Display = value; NotifyPropertyChanged(); }
        }

        private double _valor;
        public double Valor
        {
            get { return _valor; }
            set { _valor = value; NotifyPropertyChanged(); }
        }

        private Color _CorLine ;
        public Color CorLine
        {
            get { return _CorLine; }
            set { _CorLine = value; NotifyPropertyChanged(); }
        }

        private double _WidthLine;
        public double WidthLine
        {
            get { return _WidthLine; }
            set { _WidthLine = value; NotifyPropertyChanged(); }
        }


    }


}
