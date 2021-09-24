using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Model.Agenda;

namespace Xamarin.HLP.Mobile.AppPE.ViewModel.Agenda
{
    public class ConfiguracaoPesquisaListagemEventosViewModel : SearchCommom
    { 

        public ConfiguracaoPesquisaListagemEventos configDaListagem { get; set; }

        public ConfiguracaoPesquisaListagemEventosViewModel()
        { 
            EfetivarPesquisaCommand = new Command(() =>
            { 
                configDaListagem.bTrazerRealizados = currentModel.bTrazerRealizados;
                configDaListagem.bTrazerCancelados = currentModel.bTrazerCancelados;
                configDaListagem.bTrazerTodosRepresentantes = currentModel.bTrazerTodosRepresentantes; 
                configDaListagem.bOrdernarCrescente = currentModel.bOrdernarCrescente;
                configDaListagem.bNeedRefresh = true;
                UtilNavidate.PopAsync();
            });
             

        }
        public Command EfetivarPesquisaCommand { get; set; }

        public bool bChangeOrdenacao { get; set; }

        private ConfiguracaoPesquisaListagemEventos _currentModel = new ConfiguracaoPesquisaListagemEventos();

        public ConfiguracaoPesquisaListagemEventos currentModel
        {
            get { return _currentModel; }
            set { _currentModel = value; NotifyPropertyChanged(); }
        }
         



        public bool Initialize()
        {
            if (canExecuteInicial)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    canExecuteInicial = false;
                    IsBusy = true; 
                    IsBusy = false;
                });
            }
            return canExecuteInicial;
        }
    }
}
