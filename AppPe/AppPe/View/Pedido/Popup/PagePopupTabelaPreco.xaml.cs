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

        private ObservableCollection<BasicPickerModel> _todos;
        private ObservableCollection<BasicPickerModel> _todosOriginal;
        public event Action<BasicPickerModel> ItemSelecionado;

        public PagePopupTabelaPreco(EditarItemViewModel editarViewModel)
        {
            InitializeComponent();
            BindingContext = editarViewModel;

            var lista = editarViewModel.ListaTabelaPreco ?? new ObservableCollection<BasicPickerModel>();
            _todosOriginal = new ObservableCollection<BasicPickerModel>(lista);
            _todos = new ObservableCollection<BasicPickerModel>(_todosOriginal);
            listaTabelaPreco.ItemsSource = _todos;

            listaTabelaPreco.SelectedItem = null; 
        }

        public EditarItemViewModel ViewModel => BindingContext as EditarItemViewModel;

        private void OnSearchCompleted(object sender, EventArgs e)
        {
            string filtro = searchEntry.Text?.ToLower() ?? string.Empty;

            _todos.Clear();

            if (!string.IsNullOrWhiteSpace(filtro))
            {
                var filtrados = _todosOriginal
                    .Where(x => x.Display?.ToLower().Contains(filtro) == true)
                    .ToList();

                foreach (var item in filtrados)
                    _todos.Add(item);
            }
            else
            {
                foreach (var item in _todosOriginal)
                    _todos.Add(item);
            }

            listaTabelaPreco.SelectedItem = null;
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
