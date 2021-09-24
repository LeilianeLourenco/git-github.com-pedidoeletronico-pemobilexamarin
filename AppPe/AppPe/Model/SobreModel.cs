using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Common;

namespace Xamarin.HLP.Mobile.AppPE.Model
{
    public class SobreModel : ModelComum
    {
        private string _versao;
        public string versao
        {
            get { return _versao; }
            set { _versao = value; NotifyPropertyChanged(); }
        }


        private ImageSource _imagePe;
        public ImageSource imagePe
        {
            get { return _imagePe; }
            set { _imagePe = value; NotifyPropertyChanged(); }
        }





    }
}
