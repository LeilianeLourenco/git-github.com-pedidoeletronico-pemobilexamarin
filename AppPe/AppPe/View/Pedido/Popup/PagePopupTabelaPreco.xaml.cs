using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Shapes;
using Xamarin.Forms.Xaml;
using Xamarin.HLP.Mobile.AppPE.Model;
using Xamarin.HLP.Mobile.AppPE.Model.Cadastros;
using Xamarin.HLP.Mobile.AppPE.View.Pedido;
using Xamarin.HLP.Mobile.AppPE.ViewModel.Pedido;

namespace Xamarin.HLP.Mobile.AppPE.View.Popup
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PagePopupTabelaPreco : PopupPage
    {
        public event Action<BasicPickerModel> ItemSelecionado;

        private List<TabelaPrecoSimplificada> _listaOriginal = new List<TabelaPrecoSimplificada>();
        private ObservableCollection<BasicPickerModel> _lista = new ObservableCollection<BasicPickerModel>();

        public PagePopupTabelaPreco(EditarItemViewModel editarViewModel)
        {
            InitializeComponent();
            BindingContext = editarViewModel;

            _listaOriginal = new List<TabelaPrecoSimplificada>(PagePedidoNew.CurrentViewModel.currentModel
                .CurrentItemModel.lTabelaPreco.ToList());

            _lista = new ObservableCollection<BasicPickerModel>(editarViewModel.ListaTabelaPreco);
            listaTabelaPreco.ItemsSource = _lista;

            //InicializarTabelas();
            //listaTabelaPreco.SelectedItem = null;
        }

        public EditarItemViewModel ViewModel => BindingContext as EditarItemViewModel;

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            var viewmodel = PagePedidoNew.CurrentViewModel.currentModel.CurrentItemModel;

            if (!_listaOriginal.Any(x => x.idTabelaPreco == viewmodel.tabelaPrecoSelecionada.idTabelaPreco))
                _listaOriginal.Add(viewmodel.tabelaPrecoSelecionada);
            
            viewmodel.lTabelaPreco = _listaOriginal;
            //var item = viewmodel.tabelaPrecoSelecionada;

            //ViewModel.CurrentTabelaPreco = new BasicPickerModel
            //{
            //    Id = item.idTabelaPreco,
            //    Display = item.xTabelaPreco
            //};
        }

        private void OnSearchCompleted(object sender, EventArgs e)
        {
            if (BindingContext is EditarItemViewModel vm)
            {
                //var item = PagePedidoNew.CurrentViewModel.currentModel.CurrentItemModel;
                //item.lTabelaPreco.Clear();

                var filtro = searchEntry.Text?.ToLower() ?? string.Empty;
                vm.BuscarTabelaPreco(filtro);

                _lista.Clear();

                foreach (var lin in vm.currentModel.lTabelaPreco)
                {
                    _lista.Add(new BasicPickerModel
                    {
                        Id = lin.idTabelaPreco,
                        Display = lin.xTabelaPreco
                    });
                }
            }
        }

        private void OnItemSelected(object sender, SelectionChangedEventArgs e)
        {
            if (BindingContext is EditarItemViewModel vm)
            {
                if (e.CurrentSelection.FirstOrDefault() is BasicPickerModel selecionado)
                {
                    var viewmodel = PagePedidoNew.CurrentViewModel.currentModel.CurrentItemModel;
                    var itemSelecionado = viewmodel.lTabelaPreco.FirstOrDefault(x => x.idTabelaPreco == selecionado.Id);

                    PagePedidoNew.CurrentViewModel.currentModel.CurrentItemModel.tabelaPrecoSelecionada = itemSelecionado;
                    ItemSelecionado?.Invoke(selecionado);

                    ((CollectionView)sender).SelectedItem = null;
                    PopupNavigation.Instance.PopAsync();
                }
            }
        }

        private void OnFecharClicked(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PopAsync();
        }
    }
}