using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Model.Cadastros;
using Xamarin.HLP.Mobile.AppPE.View.Cliente;

namespace Xamarin.HLP.Mobile.AppPE.ViewModel.Cadastro
{
    public class ListagemContatoViewModel : NotifyCommon
    {

        public ICommand NovoCommand { get; set; }
        

        private ClientesModel _currentModel = new ClientesModel();

        public ClientesModel currentModel
        {
            get { return _currentModel; }
            set { _currentModel = value; NotifyPropertyChanged(); }
        }
        public ListagemContatoViewModel()
        {
            NovoCommand = new Command(() =>
            {
                UtilNavidate.PushAsync(new PageContato(currentModel, new ContatoModel()));
            });
        }

    }
}
