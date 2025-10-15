using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Controls.custom;
using Xamarin.HLP.Mobile.AppPE.Model;
using Xamarin.HLP.Mobile.AppPE.Model.Cadastros;
using Xamarin.HLP.Mobile.AppPE.Model.Cidades;
using Xamarin.HLP.Mobile.AppPE.ViewModel.Cadastro;

namespace Xamarin.HLP.Mobile.AppPE.View.Cliente
{
    public partial class PageEndereco : ContentPage
    {
        public PageEndereco(ClientesModel cliente, EnderecoModel endereco)
        {
            try
            {
                InitializeComponent();
                ViewModel.currentModel = endereco;
                ViewModel.currentModel.LEstadosBasicPickerModels = ViewModel.LEstadosBasicPickerModels;
                ViewModel.currentClienteModel = cliente;
                EntryCep.Completed += (sender, e) => { EntryName.Focus(); };
                EntryName.Completed += (sender, e) => { EntryNumero.Focus(); };
                EntryNumero.Completed += (sender, e) => { EntryComplemento.Focus(); };
                EntryComplemento.Completed += (sender, e) => { EntryBairro.Focus(); };
            }
            catch (Exception ex)
            {
                App.Messages.ShowAsync(ex.Message);
            }
           
            

        }

        public EnderecoViewModel ViewModel => BindingContext as EnderecoViewModel;


        


        private bool _canClose = true;
        protected override bool OnBackButtonPressed()
        {
            if (_canClose)
            {
                QuestionToBack();
            }
            return _canClose;
        }
        private async void QuestionToBack()
        {
            var answer = await App.Messages.ShowConfirmAsync("Deseja realmente sair do lançamento ?");
            if (answer)
            {
                _canClose = false;
                UtilNavidate.PopAsync();
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Device.StartTimer(UtilMethods.GetStartTime, ViewModel.Initialize);
            SvgImageCep.Command = new Command(execute: () =>
            {
                UtilCorreios.BuscaCep(ViewModel.currentModel);
            }, canExecute: () => !ViewModel.currentModel.isSearching);
            GoogleInsightsReportingConstants.TrakPage(GoogleInsightsReportingConstants.InPage.PAGE_ENDERECO);

            

        }

        private void MenuSaveItem_OnClicked(object sender, EventArgs e)
        {
            ViewModel.currentModel.SaveCommand.Execute(null);
        }

        private void PickerEstado_SelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = (BindablePicker)sender;
            var estado = picker.SelectedItem as BasicPickerModel;

            if (estado != null)
            {
                var cidades = App.Data.Connection
                 .Table<CidadesModel>()
                 .Where(c => c.uf == estado.XId)
                 .OrderBy(c => c.nome)
                 .ToList();

                var pickerItems = cidades.Select(c => new BasicPickerModel
                {
                    XId = c.codigoIBGE?.ToString(),
                    Display = c.nome
                }).ToList();

                PickerCidade.ItemsSource = pickerItems;

                PickerCidade.SelectedItem = null;
            }
        }
    }

}
