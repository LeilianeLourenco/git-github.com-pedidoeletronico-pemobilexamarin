using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Annotations;

namespace Xamarin.HLP.Mobile.AppPE.Common
{
    public class NotifyCommon : INotifyPropertyChanged
    {
        public bool canExecuteInicial { get; set; } = true;

        public bool ExecuttingAnyCommand { get; set; } = false;

        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void PublicNotifyPropertyChanged(string _propertyName)
        {
            NotifyPropertyChanged(_propertyName);
        }


        public string ImageIconSearch => Device.OnPlatform("ApplicationBarSearch.png", "ApplicationBarSearch.png", "Assets/ApplicationBarSearch.png");
        public string ImageIconSave => Device.OnPlatform("ApplicationBarSave.png", "ApplicationBarSave.png", "Assets/ApplicationBarSave.png");
        public string ImageIconAdvancedSearch => Device.OnPlatform("PesquisaAvancada.png", "PesquisaAvancada.png", "Assets/ApplicationBarSave.png");
        public string ImageIconCancel => Device.OnPlatform("ApplicationBarCancel.png", "ApplicationBarCancel.png", "Assets/ApplicationBarCancel.png");
        public string ImageIconEdit => Device.OnPlatform("ApplicationBarEdit.png", "ApplicationBarEdit.png", "Assets/ApplicationBarEdit.png");
        public string ImageIconPedido => Device.OnPlatform("ApplicationBarListarPedidos.png", "ApplicationBarListarPedidos.png", "Assets/ApplicationBarListarPedido.png");
        public string ImageIconDelete => Device.OnPlatform("ApplicationBarDelete.png", "ApplicationBarDelete.png", "Assets/ApplicationBarDelete.png");
        public string ImageIconAdd => Device.OnPlatform("ApplicationBarAdd.png", "ApplicationBarAdd.png", "Assets/ApplicationBarAdd.png");
        public string ImageIconSync => Device.OnPlatform("ApplicationBarSync.png", "ApplicationBarSync.png", "Assets/ApplicationBarSync.png");
        public string ImageIconListarRepresentantes => Device.OnPlatform("ApplicationBarListarClientes.png", "ApplicationBarListarClientes.png", "Assets/ApplicationBarListarClientes.png");
        
    }

    public class SearchCommom : NotifyCommon
    {
        public SearchCommom()
        {
            
        }
        public ICommand SearchCommand { get; set; }
        public ICommand LoadItensCommand { get; set; }

        private bool _bFind = false;


        public bool bFind
        {
            get { return _bFind; }
            set { _bFind = value; NotifyPropertyChanged(); }
        }

        private bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }
            set { _isBusy = value; NotifyPropertyChanged(); }
        }

        

        private string _xFiltro;

        public string xFiltro
        {
            get { return _xFiltro; }
            set { _xFiltro = value; NotifyPropertyChanged(); }
        }

        public bool CloseIsBusy()
        {
            App.ParamBackButtonPressed?.SetParameter(true);
            return IsBusy = false;
        }

    }




}
