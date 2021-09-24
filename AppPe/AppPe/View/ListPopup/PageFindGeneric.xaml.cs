using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Model;
using Xamarin.HLP.Mobile.AppPE.ViewModel;

namespace Xamarin.HLP.Mobile.AppPE.View.ListPopup
{
    public partial class PageFindGeneric : TabbedPage
    {
        public PageFindGeneric(FindGenericModel bindcontext)
        {
            InitializeComponent();
            ViewModel.IsBusy = true;
            Task.Yield();
            ViewModel.currentModel = bindcontext;
            ContentPageLista.Title = bindcontext.Display;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ViewModel.Listar();
        }

        public ListagemGenericViewModel ViewModel => BindingContext as ListagemGenericViewModel;
        private void ListViewDados_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as BasicPickerModel;
            ListViewDados.SelectedItem = null;
            if (item == null) return;
            ViewModel.currentModel.SelectedItem = item;
            UtilNavidate.PopAsync();
        }
    }
}
