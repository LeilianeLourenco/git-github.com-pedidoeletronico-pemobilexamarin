using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Model;
using Xamarin.HLP.Mobile.AppPE.Model.Cadastros;
using Xamarin.HLP.Mobile.AppPE.ViewModel.Cadastro;

namespace Xamarin.HLP.Mobile.AppPE.View.Cliente
{
    public partial class PageTelefonesCliente : ContentPage
    {
        public PageTelefonesCliente(ClientesModel objcliente)
        {
            InitializeComponent();
            ViewModel.currentModel = objcliente;
            this.Title = objcliente.xFantasia;
        }

        private async void ListViewFoneEmail_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as EnderecoFoneClienteContatoModel;
            if (item == null) return;

            if (item.hasEmail && item.hasFone) // os dois
            {
                var resposta = await App.Messages.ShowQuestionMessageSendEmailOrCall();

                if (resposta != "")
                {
                    if (resposta.Equals("LIGAR"))
                        ViewModel.Call(item);
                    else
                        ViewModel.SendEmail(item);
                }
            }
            else if (item.hasEmail && !item.hasFone) // só email
                ViewModel.SendEmail(item);
            else if (item.hasFone && !item.hasEmail) // só telefone
                ViewModel.Call(item);
            ListEmailFone.SelectedItem = null;
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();
            ViewModel.Initialize();
        }

        public TelefoneClienteViewModel ViewModel => BindingContext as TelefoneClienteViewModel;
    }
}
