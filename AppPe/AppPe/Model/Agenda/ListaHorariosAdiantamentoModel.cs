using Xamarin.HLP.Mobile.AppPE.Common;

namespace Xamarin.HLP.Mobile.AppPE.Model.Agenda
{
    public class ListaHorariosAdiantamentoModel : NotifyCommon
    {
        private string _xDisplay;
        public string xDisplay
        {
            get { return _xDisplay; }
            set { _xDisplay = value; NotifyPropertyChanged(); }
        }

        private byte _stOpcao;
        public byte stOpcao
        {
            get { return _stOpcao; }
            set { _stOpcao = value; NotifyPropertyChanged(); }
        }   
    }
}
