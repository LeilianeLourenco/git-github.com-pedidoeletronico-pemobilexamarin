using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Controls.custom;
using Xamarin.HLP.Mobile.AppPE.Controls.xaml;
using Xamarin.HLP.Mobile.AppPE.Model;
using Xamarin.HLP.Mobile.AppPE.Model.Cadastros;
using Xamarin.HLP.Mobile.AppPE.ViewModel.Cadastro;

namespace Xamarin.HLP.Mobile.AppPE.View.Cliente
{
    public partial class PageContato : ContentPage
    {
        public PageContato(ClientesModel cliente, ContatoModel objcontato)
        {
            InitializeComponent();
            ViewModel.currentModel = objcontato;
            ViewModel.currentClienteModel = cliente;
            EntryNome.Completed += (sender, e) => { BindableDpto.Focus(); };
            //BindableDpto.SelectedIndexChanged += (sender, e) => { EntryCargo.Focus(); };
            EntryCargo.Completed += (sender, e) => { EntryFone.Focus(); };
            EntryFone.Completed += (sender, e) => { EntryEmail.Focus(); };
            EntryNome.Focus();
        }
        public ContatoViewModel ViewModel => BindingContext as ContatoViewModel;
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
            GoogleInsightsReportingConstants.TrakPage(GoogleInsightsReportingConstants.InPage.PAGE_CONTATO);
            

        }

        private void EntryFone_OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var control = sender as ExtendedEntry;

            FoneEmailControl.FormatarPhone(control);
        }

        private void EntryEmail_OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var control = sender as ExtendedEntry;

            FoneEmailControl.ValidaEmail(control);
        }

        private void MenuItem_OnClicked(object sender, EventArgs e)
        {
            ViewModel.SaveCommand.Execute(null);
        }

        private void MenuItemDelete_OnClicked(object sender, EventArgs e)
        {
           ViewModel.DeleteCommand.Execute(null);
        }
    }
}
