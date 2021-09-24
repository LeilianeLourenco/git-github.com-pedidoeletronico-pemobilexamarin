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
using Xamarin.HLP.Mobile.AppPE.ViewModel.Home;

namespace Xamarin.HLP.Mobile.AppPE.View.Home
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageHomeNew : ContentPage
    {
        public static HomeNewViewModel ViewModelStatic;
        public PageHomeNew()
        {
            InitializeComponent();
            ViewModelStatic = ViewModel;
            ViewModel.page = this;
            GridEmpresa.Command = new Command(ViewModel.TrocarEmpresa);
            GridSincronizar.Command = new Command(ViewModel.Sincronizar);
            GridDash.Command = new Command(ViewModel.IrParaDashBoard);
        }


        public HomeNewViewModel ViewModel => BindingContext as HomeNewViewModel;

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            ViewModel.ExecuttingAnyCommand = false;

            //ViewModel.canExecuteInicial = true;

            if (ViewModel.canExecuteInicial)
            {
                ViewModel.RefreshConexao(await App.IsConected());
            }            
            Device.StartTimer(UtilMethods.GetStartTime, ViewModel.Initialize);
           
        }


    }


}
