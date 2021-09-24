using Xamarin.HLP.Mobile.AppPE.Common;

namespace Xamarin.HLP.Mobile.AppPE.Model
{
    public class CatalogoModel : ModelComum
    {
        private int _idClienteOffLine;

        public int idClienteOffLine
        {
            get { return _idClienteOffLine; }
            set { _idClienteOffLine = value; NotifyPropertyChanged(); }
        }




    }
}
