using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Model.Cadastros;
using Xamarin.HLP.Mobile.AppPE.ViewModel.Agenda;

namespace Xamarin.HLP.Mobile.AppPE.View.Agenda
{ 
    public partial class PageEventoNew : ContentPage
    {
        public static EventoCadastroViewModel StaticViewModel { get; set; }

        public PageEventoNew(AtividadeAgendaModel objEventoModel)
        {
            try
            {
                InitializeComponent();
                NavigationPage.SetHasBackButton(this, false);

                if (objEventoModel.idAtividadeOffline == 0)
                {
                    objEventoModel.dtInicioEvento = DateTime.Now;
                    objEventoModel.dtFimEvento = DateTime.Now.AddHours(1);
                }

                StaticViewModel = ViewModel;
                ViewModel.currentModel = objEventoModel; 
            }
            catch (Exception ex)
            {
                App.Messages.ShowAsync(ex.Message);
            }
        
        }
        public EventoCadastroViewModel ViewModel => BindingContext as EventoCadastroViewModel;

        protected override void OnAppearing()
        {
            ViewModel.ExecuttingAnyCommand = false;  
            Device.StartTimer(UtilMethods.GetStartTime, ViewModel.Initialize);
            base.OnAppearing();
        }

        private void BindableObject_OnPropertyChanging(object sender, PropertyChangingEventArgs e)
        {

            if (ViewModel?.ItemCliente != null && ViewModel.ItemCliente.Id > 0)
            {
                try
                {
                    ViewModel.currentModel.idClienteOffline = ViewModel.ItemCliente.Id;
                    ViewModel.SetEnderecoCliente();
                }
                catch (Exception ex)
                {
                    ex.TrakException();
                } 
            }
        }

        private void BindableAtividade_OnPropertyChanging(object sender, PropertyChangingEventArgs e)
        {

            if (ViewModel?.ItemAtividade != null && ViewModel.ItemAtividade.Id > 0)
            {
                try
                {
                    if (string.IsNullOrEmpty(ViewModel.currentModel.xDescricaoAtividade))
                    { 
                        ViewModel.currentModel.xDescricaoAtividade = ViewModel.ItemAtividade.Display;
                    }

                    ViewModel.currentModel.idTipoAtividade = ViewModel.ItemAtividade.Id;
                }
                catch (Exception ex)
                {
                    ex.TrakException();
                }
            }
        }
    }
}