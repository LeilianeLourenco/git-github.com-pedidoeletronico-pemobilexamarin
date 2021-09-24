using System;
using Xamarin.HLP.Mobile.AppPE.Common;

namespace Xamarin.HLP.Mobile.AppPE.Model.Chart
{
    public class FaturamentoMensalChart : ModelComum
    {
        public string Titulo { get; set; }

        private double _currentValor = 0;
        public double currentValor
        {
            get { return _currentValor; }
            set { _currentValor = value; NotifyPropertyChanged(); }
        }

        private DateTime _currentMes;
        public DateTime currentMes
        {
            get { return _currentMes; }
            set { _currentMes = value; NotifyPropertyChanged(); }
        }
        


    }
}
