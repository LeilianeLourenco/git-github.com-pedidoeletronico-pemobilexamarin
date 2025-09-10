using System;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Model.Cadastros;
using Xamarin.HLP.Mobile.AppPE.ViewModel.Agenda;

namespace Xamarin.HLP.Mobile.AppPE.View.Agenda
{ 
    public partial class PageAnexosEvento : ContentPage
    {
        public PageAnexosEvento(EventoCadastroViewModel viewModel)
        {
            try
            {
                InitializeComponent();
                BindingContext = viewModel;
            }
            catch (Exception ex)
            {
                App.Messages.ShowAsync(ex.Message);
            }
        
        }
    }
}