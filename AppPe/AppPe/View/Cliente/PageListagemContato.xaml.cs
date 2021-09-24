using System;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Model;
using Xamarin.HLP.Mobile.AppPE.Model.Cadastros;
using Xamarin.HLP.Mobile.AppPE.Model.Repository;
using Xamarin.HLP.Mobile.AppPE.ViewModel.Cadastro;

namespace Xamarin.HLP.Mobile.AppPE.View.Cliente
{
    public partial class PageListagemContato : ContentPage
    {
        public PageListagemContato(ClientesModel objClientesModel)
        {
            InitializeComponent();
            ViewModel.currentModel = objClientesModel;
            //this.Title = objClientesModel.xFantasia;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ViewModel.currentModel.lContato =
                new ObservableCollection<ContatoModel>(ViewModel.currentModel.lContato);
        }

        public ListagemContatoViewModel ViewModel => BindingContext as ListagemContatoViewModel;

        private void ListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                if (e.SelectedItem == null) return;
                ViewModel.currentModel.objContatoModel = PageCliente.StaticViewModel.currentModel.lContato.FirstOrDefault(c =>
                {
                    var basicPickerModel = e.SelectedItem as BasicPickerModel;
                    return basicPickerModel != null && c.idGuid == basicPickerModel.XId;
                });
                ListContato.SelectedItem = null;
                UtilNavidate.PushAsync(new PageContato(ViewModel.currentModel, ViewModel.currentModel.objContatoModel));
            }
            catch (Exception ex)
            {
                GoogleInsightsReportingConstants.TrakException("PageApresentacaoCliente.ListView_OnItemSelected", ex.Message, true);
            }

        }

        private async void MenuItemContato_OnClicked(object sender, EventArgs e)
        {
            var menuItem = sender as MenuItem;
            var item = menuItem?.BindingContext as BasicPickerModel;
            if (item == null) return;
            if (!await UtilMessages.Exclusao()) return;
            var registro = ViewModel.currentModel.lContato.FirstOrDefault(c => c.idGuid == item.XId);
            if (ViewModel.currentModel.idClientesOffLine != null)
                ContatoRepository.Delete(registro);
            ViewModel.currentModel.lContato.Remove(registro);
            ViewModel.currentModel.lContato =
                new ObservableCollection<ContatoModel>(ViewModel.currentModel.lContato);
        }
    }
}
