using System;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Model;
using Xamarin.HLP.Mobile.AppPE.Model.Cadastros;
using Xamarin.HLP.Mobile.AppPE.Model.Repository;
using Xamarin.HLP.Mobile.AppPE.View.MainPage;
using Xamarin.HLP.Mobile.AppPE.ViewModel.Produto;
using ZXing.Net.Mobile.Forms;

namespace Xamarin.HLP.Mobile.AppPE.View.Produto
{
    public partial class PageProduto : TabbedPage
    {
        public PageProduto(ProdutoModel objProdutoModel)
        {
            StaticModel.StaticProdutoModel = objProdutoModel;
            InitializeComponent();
            NavigationPage.SetHasBackButton(this, false);
            _ViewModel.currentModel = objProdutoModel;
            ToolbarItemSave.Command = new Command(AfterSave);
            EntryName.Completed += (sender, e) => { EntryCodAlternativo.Focus(); };
            EntryValorVenda.Completed += (sender, e) => { EntryComissao.Focus(); };
            EntryValorCompra.Completed += (sender, e) => { EntrySt.Focus(); };
            EntrySt.Completed += (sender, e) => { EntryIcms.Focus(); };
            EntryIcms.Completed += (sender, e) => { EntryIpi.Focus(); };
            EntryIpi.Completed += (sender, e) => { EntryOutros.Focus(); };
            EntryOutros.Completed += (sender, e) => { EntryLucro.Focus(); };
            EntryLucro.Completed += (sender, e) => { EntrypIpiVenda.Focus(); };
            EntrypIpiVenda.Completed += (sender, e) => { EntrypStVenda.Focus(); };

            BtncEan.Command = new Command(() =>
            {
                ReadBarCode("cEan");
            });

            BtncEanEmb.Command = new Command(() =>
            {
                ReadBarCode("cEanEmb");
            });
        }

        public ProdutoViewModel _ViewModel => BindingContext as ProdutoViewModel;

        private void EntryValores_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var entry = sender as Entry;
            if (entry == null) return;
            if (entry.IsFocused)
            {
                _ViewModel.CalculoCustoProduto();
                _ViewModel.CalcularValorVenda();
                _ViewModel.CalcularValorVendaComImpostos();
            }
        }

        private void EntryFieldsToValorVenda_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var entry = sender as Entry;
            if (entry == null) return;
            if (entry.IsFocused)
            {
                _ViewModel.CalcularValorVenda();
                _ViewModel.CalcularValorVendaComImpostos();
            }
        }

        private void EntryValorVenda_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var entry = sender as Entry;
            if (entry == null) return;
            if (entry.IsFocused)
            {
                _ViewModel.CalcularPorcLucro();
                _ViewModel.CalcularValorVendaComImpostos();
            }
        }

        private async void AfterSave()
        {
            try
            {
                if (await ValidaForm())
                {
                    if (_ViewModel.CanSave())
                    {
                        _ViewModel.Save();
                    }
                }
            }
            catch (Exception ex)
            {
                ex.TrakException();
            }
        }

        private async Task<bool> ValidaForm()
        {
            try
            {
                const string msg = "O campo de {0} é obrigatório";
                var bValido = true;

                if (_ViewModel.currentUnidMedidaBasicPickerModel != null)
                    _ViewModel.currentModel.idUnidadeMedida = _ViewModel.currentUnidMedidaBasicPickerModel.Id;
                if (_ViewModel.currentRepresentadaBasicPickerModel != null)
                    _ViewModel.currentModel.idRepresentada = _ViewModel.currentRepresentadaBasicPickerModel.Id;
                if (_ViewModel.currentCategoriaBasicPickerModel != null)
                    _ViewModel.currentModel.idCategoria = _ViewModel.currentCategoriaBasicPickerModel.Id;

                if (string.IsNullOrEmpty(_ViewModel.currentModel.xNome))
                {
                    await App.Messages.ShowAsync(string.Format(msg, "Nome"));
                    EntryName.Focus();
                    bValido = false;
                }
                else if ((_ViewModel.currentModel.xNome ?? "").Length < 2)
                {
                    await App.Messages.ShowAsync("A Descrição do produto precisa ter ao menos dois caracteres.");
                    EntryName.Focus();
                    bValido = false;
                }
                else if (_ViewModel.currentModel.idUnidadeMedida == 0)
                {
                    await App.Messages.ShowAsync(string.Format(msg, "Unidade de medida"));
                    BindableUnidadeMedida.Focus();
                    bValido = false;
                }
                else if (_ViewModel.currentModel.idRepresentada == 0)
                {
                    await App.Messages.ShowAsync(string.Format(msg, "Representação"));
                    BindableRepresentada.Focus();
                    bValido = false;
                }
                else if (_ViewModel.currentModel.idCategoria == 0)
                {
                    await App.Messages.ShowAsync(string.Format(msg, "Categoria"));
                    BindableCategoria.Focus();
                    bValido = false;
                }
                else if (ProdutoRepository.CodigoAlternativoExiste(_ViewModel.currentModel))
                {
                    await App.Messages.ShowAsync("Código Alternativo já existe no sistema, impossível salvar outro com a mesma nomenclatura.");
                    EntryCodAlternativo.Focus();
                    bValido = false;
                }
                else if (ProdutoRepository.NomeExiste(_ViewModel.currentModel))
                {
                    await App.Messages.ShowAsync("Nome do produto já existe no sistema, impossível salvar outro com a mesma nomenclatura.");
                    EntryName.Focus();
                    bValido = false;
                }
                else if (_ViewModel.currentModel.bUtilizaEstoqueMinMax && (_ViewModel.currentModel.vEstoqueMax < _ViewModel.currentModel.vEstoqueMin))
                {
                    await App.Messages.ShowAsync("Estoque máximo não pode ser menor que estoque mínimo.");
                    this.CurrentPage = PageEstoque;
                    EntryEstoqueMax.Focus();
                    bValido = false;
                }
                return bValido;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void BindableRepresentada_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (BindableCategoria != null)
            {
                _ViewModel.CarregarCategoria();
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            //Insights.Track(InsightsReportingConstants.PAGE_PRODUTO);
            GoogleInsightsReportingConstants.TrakPage(GoogleInsightsReportingConstants.InPage.PAGE_PRODUTO);
        }

        public async void ReadBarCode(string tipo)
        {
            try
            {
                if (await UtilMethods.PermissionCamera())
                {
                    ZXingScannerPage scanPage = new ZXingScannerPage();
                    scanPage.AutoFocus();
                    scanPage.OnScanResult += (result) =>
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            if (tipo.Equals("cEan"))
                            {
                                _ViewModel.currentModel.cEan = result.Text;
                            }
                            else
                            {
                                _ViewModel.currentModel.cEanEmb = result.Text;
                            }
                            UtilNavidate.PopAsync();
                        });
                    UtilNavidate.PushAsync(scanPage);
                }
            }
            catch (Exception ex)
            {
                ex.TrakException();
            }


        }
    }
}
