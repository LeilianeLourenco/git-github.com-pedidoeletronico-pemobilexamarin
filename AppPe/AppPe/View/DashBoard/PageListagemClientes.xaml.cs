using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Model.Cadastros;
using Xamarin.HLP.Mobile.AppPE.View.Sincronizacao;
using Xamarin.HLP.Mobile.AppPE.ViewModel.DashBoard;

namespace Xamarin.HLP.Mobile.AppPE.View.DashBoard
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageListagemClientes : ContentPage
    {
        public string ImageIconListarAgenda => Device.OnPlatform("ApplicationBarListarAgenda.png", "ApplicationBarListarAgenda.png", "Assets/ApplicationBarListarAgenda.png");

        public PageListagemClientes(List<ClientesModel> lClientes)
        {
            InitializeComponent();

            ListaClientes.ItemsSource = lClientes;
        }

        private async void ListaClientes_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;

            PageInfoCliente page = new PageInfoCliente(e.SelectedItem as ClientesModel);

            await App.Navigation.PushPopupAsync(page, animate: true);
            ((ListView)sender).SelectedItem = null;
        }
    }
}
