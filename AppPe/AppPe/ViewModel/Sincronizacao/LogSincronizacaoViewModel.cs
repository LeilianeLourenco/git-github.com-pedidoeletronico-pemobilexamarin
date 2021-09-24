using System.Collections.ObjectModel;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Model;

namespace Xamarin.HLP.Mobile.AppPE.ViewModel.Sincronizacao
{
    public class LogSincronizacaoViewModel : NotifyCommon
    {


        private ObservableCollection<AlertaSincronizacao> _lAlertaSincronizacao;
        public ObservableCollection<AlertaSincronizacao> LAlertaSincronizacao
        {
            get { return _lAlertaSincronizacao; }
            set { _lAlertaSincronizacao = value; NotifyPropertyChanged(); }
        }

    }
}
