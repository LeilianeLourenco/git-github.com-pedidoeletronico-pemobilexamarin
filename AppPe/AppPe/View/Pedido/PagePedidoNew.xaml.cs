using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using TEditor;
using TEditor.Abstractions;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Controls.xaml;
using Xamarin.HLP.Mobile.AppPE.Model.Lancamento;
using Xamarin.HLP.Mobile.AppPE.Model.Repository;
using Xamarin.HLP.Mobile.AppPE.ViewModel.Pedido;
using PropertyChangingEventArgs = Xamarin.Forms.PropertyChangingEventArgs;

namespace Xamarin.HLP.Mobile.AppPE.View.Pedido
{
    public partial class PagePedidoNew : TabbedPage
    {

        private static PedidoNewViewModel _currentViewModel = null;

        public static PedidoNewViewModel CurrentViewModel
        {
            get
            {
                _currentViewModel = _currentViewModel ?? new PedidoNewViewModel();
                return _currentViewModel;
            }
            set { _currentViewModel = value; }
        }



        public PagePedidoNew(PedidoVendaModel currentModel, bool bPedidoByCliente = false)
        {
            InitializeComponent();
            NavigationPage.SetHasBackButton(this, false);

            SessionLancamento.Remove(TxtRepresentanteCell);
            ViewModel.currentModel = currentModel ?? new PedidoVendaModel();
            ViewModel.bPedidoByCliente = bPedidoByCliente;

            CurrentViewModel = ViewModel;

            ViewModel.DateOrcamentoVisibilityCommand = new Command(DateOrcamentoVisibility);

            ViewModel.DateOrcamentoVisibilityCommand.Execute(null);

            ViewModel.RepresentacaoPdfVisibilityCommand = new Command(RepresentacoesPdfVisibility);

            ViewModel.RepresentacaoPdfVisibilityCommand.Execute(null);

            //ViewModel.IsBusy = true;

            ButtonMostrarItens.Command = new Command(() =>
            {
                CurrentPage = PageItens;
            });
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();
            ViewModel.ExecuttingAnyCommand = false;
            Device.StartTimer(UtilMethods.GetStartTime, ViewModel.Initialize); 
        }


        public void DateOrcamentoVisibility()
        {
            try
            {
                if (ViewModel.currentModel.stLancamento == 1)
                {
                    if (SessionLancamento.Contains(DateOrcamentoCell))
                        SessionLancamento.Remove(DateOrcamentoCell);
                }
                else
                {
                    if (!SessionLancamento.Contains(DateOrcamentoCell))
                        SessionLancamento.Insert(2, DateOrcamentoCell);
                }

            }
            catch (Exception ex)
            {
                ex.TrakException();
            }
        }

        /// <summary>
        /// melhoria especifica onde o usuário escolhe a representação que vai pro pdf
        /// </summary>
        public void RepresentacoesPdfVisibility()
        {
            try
            {
                if (ViewModel.currentModel.bAplicaMelhoriaEscolherRepresentacaoPdf == false)
                {
                    if (SectionAdicionais.Contains(TextCellRepresentacoesPdf))
                        SectionAdicionais.Remove(TextCellRepresentacoesPdf);
                }
                else
                {
                    if (!SectionAdicionais.Contains(TextCellRepresentacoesPdf))
                        SectionAdicionais.Insert(10, TextCellRepresentacoesPdf);
                }

            }
            catch (Exception ex)
            {
                ex.TrakException();
            }
        }

        public PedidoNewViewModel ViewModel => BindingContext as PedidoNewViewModel;

        private void BindableObject_OnPropertyChanging(object sender, PropertyChangingEventArgs e)
        {
            var _entry = sender as TextCell;

            if (ViewModel?.ItemCliente != null && ViewModel.ItemCliente.Id > 0 && (_entry.Detail != ViewModel.ItemCliente.Display || ViewModel.ItemCondicaoPgto.Id == 0))
            {
                try
                {
                    if (ViewModel.canExecuteInicial == false && ViewModel.IsBusy == false)
                    {
                        ViewModel.currentModel.idClientesOffLine = ViewModel.ItemCliente.Id;
                        ViewModel.currentModel.xInfAdicional += "";
                        ViewModel.SetConiguracoesDoCliente();
                    }
                    if (App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.stAdministrador)
                    {
                        if (SessionLancamento.Contains(TxtRepresentanteCell) == false)
                        {
                            SessionLancamento.Insert(SessionLancamento.Count == 7 ? 6 : 5, TxtRepresentanteCell);
                        }
                    }

                }
                catch (Exception ex)
                {
                    ex.TrakException();
                }

            }
        }

        private void StatusTextCell_OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (ViewModel.canExecuteInicial == false && ViewModel.IsBusy == false)
                if (ViewModel?.ItemSatus != null)
                {
                    ViewModel.VerificaStatusCancelado();
                }
        }

        private void TxtRepresentanteCell_OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (ViewModel.canExecuteInicial == false && ViewModel.IsBusy == false)
                if (ViewModel?.ItemRepresentante != null)
                {
                    ViewModel.currentModel.idRepresentantePedido = ViewModel.ItemRepresentante.Id;
                }
        }

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


        #region Editor

        async void Editor_OnClicked(object sender, EventArgs e)
        {
            UtilNavidate.PushModalAsync(new NavigationPage(new ContentPage { Content = new TEditorHtmlView(CurrentViewModel.currentModel), BackgroundColor = Color.White }));
            //await ShowTEditor();
        } 

        #endregion




    }

    /// <summary>
    /// View do campo de contrato para o usuário
    /// </summary>
    public class TEditorHtmlView : StackLayout
    {
        //create bindable property, html
        public string Html { get; set; } 
        WebView _displayWebView;
        public TEditorHtmlView(PedidoVendaModel currentModel)
        {
            this.Orientation = StackOrientation.Vertical;
            this.Children.Add(new Button
            {
                Text = "Atualizar Contrato",
                HeightRequest = 100,
                Command = new Command(async (obj) =>
                {
                    await ShowTEditor(currentModel);
                })
            });
            _displayWebView = new WebView() { HeightRequest = 500 }; 
            _displayWebView.Source = new HtmlWebViewSource() { Html = currentModel.xInformacaoContrato };
            this.Children.Add(_displayWebView);
        }

        async Task ShowTEditor(PedidoVendaModel currentModel)
        {
            try
            {
                TEditorResponse response = await CrossTEditor.Current.ShowTEditor(currentModel.xInformacaoContrato);
                if (response.IsSave)
                {
                    if (response.HTML != null)
                    {
                        currentModel.xInformacaoContrato = response.HTML.ToString();
                        _displayWebView.Source = new HtmlWebViewSource() { Html = response.HTML };
                    }
                }
            }
            catch (Exception ex)
            { 
                throw ex;
            }
          
        }
    }
}
