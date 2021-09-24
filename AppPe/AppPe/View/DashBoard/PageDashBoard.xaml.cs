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
using Xamarin.HLP.Mobile.AppPE.ViewModel.DashBoard;

namespace Xamarin.HLP.Mobile.AppPE.View.DashBoard
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageDashBoard : ContentPage
    {
        public PageDashBoard()
        {
            InitializeComponent();

            btnTotais.Command = new Command(() =>
            {

                viewmodel.bShowTodos = !viewmodel.bShowTodos;
                RefreshDashBoardFaturamento();
            });

            btntualizaChart.Command = new Command(RefreshDashBoardFaturamento);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Device.StartTimer(UtilMethods.GetStartTime, Initialize);
        }

        public bool Initialize()
        {
            if (viewmodel.canExecuteInicial)
            {
                viewmodel.canExecuteInicial = false;
                RefreshDashBoardFaturamento();
            }
            return viewmodel.canExecuteInicial;
        }

        public DashBoardViewModel viewmodel => BindingContext as DashBoardViewModel;

        private async void RefreshDashBoardFaturamento()
        {
            await Task.Run(() =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    ChartFaturamento.ViewModel.canExecuteInicial = true;
                    ChartFaturamento.bTodosUsuarios = viewmodel.bShowTodos;
                    ChartFaturamento.InitiDados();
                    viewmodel.RefreshDashBoardDados();
                });
            });
        }


    }



  

}
