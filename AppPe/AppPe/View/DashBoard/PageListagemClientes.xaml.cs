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
using Xamarin.HLP.Mobile.AppPE.ViewModel.DashBoard;

namespace Xamarin.HLP.Mobile.AppPE.View.DashBoard
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageListagemClientes : ContentPage
    {
        public PageListagemClientes(List<ClientesModel> lClientes)
        {
            InitializeComponent();

            ListaClientes.ItemsSource = (System.Collections.IEnumerable)lClientes;
        }
      
    }  
}
