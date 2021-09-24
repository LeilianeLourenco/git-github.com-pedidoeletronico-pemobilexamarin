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
    public partial class PageListagemEndereco : ContentPage
    {
        public PageListagemEndereco(ClientesModel objClientesModel)
        {
            InitializeComponent();
            ViewModel.currentModel = objClientesModel;
            //this.Title = ViewModel.currentModel.xFantasia;
        }

        public ListagemEnderecoViewModel ViewModel => BindingContext as ListagemEnderecoViewModel;

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ViewModel.currentModel.lEndereco = new ObservableCollection<EnderecoModel>(ViewModel.currentModel.lEndereco);
        }

        private void ListEndereco_OnItemSelected(object sender, EventArgs e)
        {
            try
            {
                var menuItem = sender as MenuItem;
                var item = menuItem?.BindingContext as BasicPickerModel;

                if (item == null) return;
                ViewModel.currentModel.objEndereco = ViewModel.currentModel.lEndereco.FirstOrDefault(c =>
                {
                    var basicPickerModel = item;
                    return basicPickerModel != null && c.idGuid == basicPickerModel.XId;
                });
                ListEndereco.SelectedItem = null;

                if (ViewModel.currentModel.objEndereco.bBuscaFeitoDaReceita == true && ViewModel.currentModel.objEndereco.bAplicaMelhoriaBloqueiaEnderecoReceita == true)
                {
                    App.Messages.ShowAsync("Endereço da receita não pode ser modificado ou excluído, operação não permitida!");
                    return;
                }

                UtilNavidate.PushAsync(new PageEndereco(ViewModel.currentModel, ViewModel.currentModel.objEndereco));
            }
            catch (Exception ex)
            {
                GoogleInsightsReportingConstants.TrakException("PageApresentacaoCliente.ListEndereco_OnItemSelected", ex.Message, true);
            }
        }

        private async void MenuItemEndereco_OnClicked(object sender, EventArgs e)
        {
            var menuItem = sender as MenuItem;
            var item = menuItem?.BindingContext as BasicPickerModel;
            if (item == null) return;


            if (!await UtilMessages.Exclusao()) return;
            var registro = ViewModel.currentModel.lEndereco.FirstOrDefault(c => c.idGuid == item.XId);



            if (registro.bBuscaFeitoDaReceita == true && registro.bAplicaMelhoriaBloqueiaEnderecoReceita == true)
            {
                await App.Messages.ShowAsync("Endereço da receita não pode ser modificado ou excluído, operação não permitida!");
                return;
            }




            if (ViewModel.currentModel.idClientesOffLine != null)
                EnderecoRepository.Delete(registro);
            ViewModel.currentModel.lEndereco.Remove(registro);
            ViewModel.currentModel.lEndereco =
                new ObservableCollection<EnderecoModel>(ViewModel.currentModel.lEndereco);
        }
    }
}
