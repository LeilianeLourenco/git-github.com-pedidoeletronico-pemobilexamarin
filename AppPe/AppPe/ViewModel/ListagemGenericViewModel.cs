using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Model;

namespace Xamarin.HLP.Mobile.AppPE.ViewModel
{
    public class ListagemGenericViewModel : ViewModelComum<FindGenericModel>
    {

        public ListagemGenericViewModel()
        {
            PesquisarCommand = new Command(Listar);
        }


        public void Listar()
        {
            IsBusy = true;
            Task.Yield();
            currentModel.Listar(currentModel.Filtro);
            IsBusy = false;
            isVisibleListView = currentModel.ItensFiltrados.Any();
        }

    }
}
