using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.HLP.Mobile.AppPE.Core.PedidoVenda;
using Xamarin.HLP.Mobile.AppPE.Model;
using Xamarin.HLP.Mobile.AppPE.Model.Cadastros;
using Xamarin.HLP.Mobile.AppPE.Model.Repository;
using Xamarin.HLP.Mobile.AppPE.ViewModel.Cadastro;
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
            var lista = editarViewModel.ListaTabelaPreco;

            _todosOriginal = new ObservableCollection<BasicPickerModel>(lista ?? new ObservableCollection<BasicPickerModel>());
            _todos = new ObservableCollection<BasicPickerModel>(_todosOriginal);
            listaTabelaPreco.ItemsSource = _todos;
            BindingContext = editarViewModel;
        }

        public EditarItemViewModel ViewModel => BindingContext as EditarItemViewModel;

        private void OnSearchCompleted(object sender, EventArgs e)
        {
            if (BindingContext is EditarItemViewModel vm)
            {
                var filtro = searchEntry.Text?.ToLower() ?? string.Empty;

                if (!string.IsNullOrEmpty(filtro))
                {
                    vm.BuscarTabelaPrecoFiltro(filtro);

                    _todos.Clear();
                    foreach (var item in vm.currentModel.lTabelaPreco)
                    {
                        _todos.Add(new BasicPickerModel
                        {
                            Id = item.idTabelaPreco,
                            Display = item.xTabelaPreco
                        });
                    }

                    listaTabelaPreco.ItemsSource = _todos;
                }
                else
                {
                    _todos.Clear();
                    foreach (var item in _todosOriginal)
                    {
                        _todos.Add(item);
                    }

                    listaTabelaPreco.ItemsSource = _todos;
                }
            }
        }

        private void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is BasicPickerModel selecionado)
            {
                ItemSelecionado?.Invoke(selecionado);
                listaTabelaPreco.SelectedItem = null;
                PopupNavigation.Instance.PopAsync();
            }
        }

        private void OnFecharClicked(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PopAsync();
        }
    }
}
