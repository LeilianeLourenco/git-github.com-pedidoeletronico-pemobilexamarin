
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Model.Agenda;
using Xamarin.HLP.Mobile.AppPE.ViewModel.Agenda;

namespace Xamarin.HLP.Mobile.AppPE.View.Agenda
{
    public partial class PageAdiantamentoHorarios : ContentPage
    {
        public PageAdiantamentoHorarios(ListaHorariosAdiantamentoModel item)
        { 
            InitializeComponent(); 
            ViewModel.itemCadastro = item; 
        }


        public AdiantamentoHorariosViewModel ViewModel => BindingContext as AdiantamentoHorariosViewModel;

        protected override void OnAppearing()
        {
            base.OnAppearing();

            Device.StartTimer(UtilMethods.GetStartTime, ViewModel.Initialize);
        }

        private void ListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var listItemModel = e.SelectedItem as ListaHorariosAdiantamentoModel;
            if (listItemModel != null)
            { 
                UtilNavidate.PopAsync(); 
            }
        }
    }
}