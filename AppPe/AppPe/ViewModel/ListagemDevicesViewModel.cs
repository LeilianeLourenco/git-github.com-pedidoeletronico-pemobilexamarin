using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Model;

namespace Xamarin.HLP.Mobile.AppPE.ViewModel
{
    public class ListagemDevicesViewModel : ViewModelComum<DeviceModel>
    {
        public ListagemDevicesViewModel()
        {
            PesquisarCommand = new Command(Listar);
        }

        public void Listar()
        {
            IsBusy = true;
            Task.Yield();
          
        }

    }
}
