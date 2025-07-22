using PdfSharpCore.Pdf.Filters;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.HLP.Mobile.AppPE.Core.PedidoVenda;
using Xamarin.HLP.Mobile.AppPE.Model;
using Xamarin.HLP.Mobile.AppPE.ViewModel.Pedido;

namespace Xamarin.HLP.Mobile.AppPE.View.Popup
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PagePopupTabelaPreco : PopupPage
    {
        BuscaPreco _buscaPreco = new BuscaPreco();

        private ObservableCollection<BasicPickerModel> _todos = new ObservableCollection<BasicPickerModel>();
        public event Action<BasicPickerModel> ItemSelecionado;

        public PagePopupTabelaPreco(EditarItemViewModel editarViewModel)
        {
            InitializeComponent();
            BindingContext = editarViewModel;

            listaTabelaPreco.ItemsSource = _todos;
            Inicializar();

            listaTabelaPreco.SelectedItem = null;
        }

        public EditarItemViewModel ViewModel => BindingContext as EditarItemViewModel;

        public void Inicializar()
        {
            if (BindingContext is EditarItemViewModel vm)
            {
                vm.BuscarTabelaPrecoFiltro(null);

                foreach (var item in vm.currentModel.lTabelaPreco)
                {
                    _todos.Add(new BasicPickerModel
                    {
                        Id = item.idTabelaPreco,
                        Display = item.xTabelaPreco
                    });
                }
            }
        }

        private void OnSearchCompleted(object sender, EventArgs e)
        {
            if (BindingContext is EditarItemViewModel vm)
            {
                string filtro = searchEntry.Text?.ToLower() ?? string.Empty;

                _todos.Clear();

                vm.BuscarTabelaPrecoFiltro(filtro);

                foreach (var item in vm.currentModel.lTabelaPreco)
                {
                    _todos.Add(new BasicPickerModel
                    {
                        Id = item.idTabelaPreco,
                        Display = item.xTabelaPreco
                    });
                }

                listaTabelaPreco.ItemsSource = _todos;
                listaTabelaPreco.SelectedItem = null;
            }
        }

        private void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is BasicPickerModel selecionado)
            {
                listaTabelaPreco.SelectedItem = null;

                ItemSelecionado?.Invoke(selecionado);
                PopupNavigation.Instance.PopAsync();
            }
        }

        private void OnFecharClicked(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PopAsync();
        }
    }
}
