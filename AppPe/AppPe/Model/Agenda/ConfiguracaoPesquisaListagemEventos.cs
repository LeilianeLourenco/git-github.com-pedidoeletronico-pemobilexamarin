using Xamarin.HLP.Mobile.AppPE.Common;

namespace Xamarin.HLP.Mobile.AppPE.Model.Agenda
{
    public class ConfiguracaoPesquisaListagemEventos : NotifyCommon
    { 
        public ConfiguracaoPesquisaListagemEventos()
        { 
        }

        private bool _bTrazerRealizados = false;

        public bool bTrazerRealizados
        {
            get { return _bTrazerRealizados; }
            set
            {
                if (_bTrazerRealizados != value)
                    canExecuteInicial = true;
                _bTrazerRealizados = value;
                NotifyPropertyChanged();
            }
        }

        private bool _bTrazerCancelados = false;

        public bool bTrazerCancelados
        {
            get { return _bTrazerCancelados; }
            set
            {
                if (_bTrazerCancelados != value)
                    canExecuteInicial = true;
                _bTrazerCancelados = value;
                NotifyPropertyChanged();
            }
        }

        private bool _bTrazerTodosRepresentantes = false;

        public bool bTrazerTodosRepresentantes
        {
            get { return _bTrazerTodosRepresentantes; }
            set
            {
                if (_bTrazerTodosRepresentantes != value)
                    canExecuteInicial = true;
                _bTrazerTodosRepresentantes = value;
                NotifyPropertyChanged();
            }
        }



        private bool _bOrdernarCrescente = false;

        public bool bOrdernarCrescente
        {
            get { return _bOrdernarCrescente; }
            set
            {
                if (_bOrdernarCrescente != value)
                    canExecuteInicial = true;
                _bOrdernarCrescente = value;
                NotifyPropertyChanged();
            }
        }
        

        public bool bNeedRefresh { get; set; }
    }
}
