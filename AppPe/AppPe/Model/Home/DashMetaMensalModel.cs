using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.HLP.Mobile.AppPE.Common;

namespace Xamarin.HLP.Mobile.AppPE.Model.Home
{
    public class DashMetaMensalModel : NotifyCommon
    {
        private double _dVendido = 0;
        public double dVendido
        {
            get { return _dVendido; }
            set { _dVendido = value; NotifyPropertyChanged(); }
        }


        private double _dFaltante = 0;
        public double dFaltante
        {
            get { return _dFaltante; }
            set { _dFaltante = value; NotifyPropertyChanged(); }
        }

        private double _dMeta = 0;
        public double dMeta
        {
            get { return _dMeta; }
            set
            {
                _dMeta = value;

                xMeta = $"Meta = {value.ToCurrencyStringPtBr()}";

                NotifyPropertyChanged();
            }
        }

        private string _xMeta = $"Meta = R$ 0,00";
        public string xMeta
        {
            get { return _xMeta; }
            set { _xMeta = value; NotifyPropertyChanged(); }
        }



        private string _pVendido = "0 %";
        public string pVendido
        {
            get { return _pVendido; }
            set { _pVendido = value; NotifyPropertyChanged(); }
        }


        private string _pFaltante = "0 %";
        public string pFaltante
        {
            get { return _pFaltante; }
            set { _pFaltante = value; NotifyPropertyChanged(); }
        }

        private string _xDisplay1;

        public string xDisplay1
        {
            get { return _xDisplay1; }
            set { _xDisplay1 = value; NotifyPropertyChanged(); }
        }

        private string _xDisplay2;
        public string xDisplay2
        {
            get { return _xDisplay2; }
            set { _xDisplay2 = value; NotifyPropertyChanged(); }
        }


        private double _WidthGridBoxDashVendido;

        public double WidthGridBoxDashVendido
        {
            get { return _WidthGridBoxDashVendido; }
            set { _WidthGridBoxDashVendido = value; NotifyPropertyChanged(); }
        }

        private double _WidthGridBoxDashFaltante;
        public double WidthGridBoxDashFaltante
        {
            get { return _WidthGridBoxDashFaltante; }
            set { _WidthGridBoxDashFaltante = value; NotifyPropertyChanged(); }
        }




    }
}
