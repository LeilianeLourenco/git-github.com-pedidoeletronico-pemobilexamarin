using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Model;
using Xamarin.HLP.Mobile.AppPE.Model.Cadastros;
using Xamarin.HLP.Mobile.AppPE.Model.Repository;

namespace Xamarin.HLP.Mobile.AppPE.ViewModel.Produto
{
    public class ProdutoViewModel : ViewModelComum<ProdutoModel>
    {

        public bool FindyBarCode { get; set; } = Device.RuntimePlatform == Device.iOS ||
                                                 Device.RuntimePlatform == Device.Android;  
        #region Properties

        private double _vVendaComImpostos;
        public double vVendaComImpostos
        {
            get { return _vVendaComImpostos; }
            set { _vVendaComImpostos = value; NotifyPropertyChanged(); }
        }

        private readonly Hlp.PedidoEletronico.Domain.Business.Calculos.Produto prodCalc =
            new Hlp.PedidoEletronico.Domain.Business.Calculos.Produto();

        private List<BasicPickerModel> _lUnidMedidaBasicPickerModel = new List<BasicPickerModel>();
        public List<BasicPickerModel> lUnidMedidaBasicPickerModel
        {
            get { return _lUnidMedidaBasicPickerModel; }
            set { _lUnidMedidaBasicPickerModel = value; NotifyPropertyChanged(); }
        }

        private BasicPickerModel _currentUnidMedidaBasicPickerModel;
        public BasicPickerModel currentUnidMedidaBasicPickerModel
        {
            get { return _currentUnidMedidaBasicPickerModel; }
            set
            {
                _currentUnidMedidaBasicPickerModel = value; NotifyPropertyChanged();
                if (value == null) return;
                currentModel.idUnidadeMedida = value.Id;
                currentModel.xUnidade = value.Display;
            }
        }


        private List<BasicPickerModel> _lRepresentadasBasicPickerModels;
        public List<BasicPickerModel> LRepresentadasBasicPickerModels
        {
            get { return _lRepresentadasBasicPickerModels; }
            set { _lRepresentadasBasicPickerModels = value; NotifyPropertyChanged(); }
        }

        private BasicPickerModel _currentRepresentadaBasicPickerModel;
        public BasicPickerModel currentRepresentadaBasicPickerModel
        {
            get { return _currentRepresentadaBasicPickerModel; }
            set
            {
                _currentRepresentadaBasicPickerModel = value; NotifyPropertyChanged();
                if (value == null) return;
                currentModel.idRepresentada = value.Id;
            }
        }


        private List<BasicPickerModel> _lCategoriaBasicPickerModels;
        public List<BasicPickerModel> LCategoriaBasicPickerModels
        {
            get { return _lCategoriaBasicPickerModels; }
            set { _lCategoriaBasicPickerModels = value; NotifyPropertyChanged(); }
        }

        private BasicPickerModel _currentCategoriaBasicPickerModel;
        public BasicPickerModel currentCategoriaBasicPickerModel
        {
            get { return _currentCategoriaBasicPickerModel; }
            set
            {
                _currentCategoriaBasicPickerModel = value; NotifyPropertyChanged();
                if (value == null) return;
                currentModel.idCategoria = value.Id;
            }
        }

        private bool _showCustos;
        public bool showCustos
        {
            get { return _showCustos; }
            set { _showCustos = value; NotifyPropertyChanged(); }
        }

        #endregion




        public ProdutoViewModel()
        {
            Title = "Cadastro de produto";

            var id = StaticModel.StaticProdutoModel.idProdutoOffLine;
            currentModel = id == null ? new ProdutoModel() : ProdutoRepository.GetProduto((int)id);
            CarregarListas();
            CalcularValorVendaComImpostos();

        }

        private void CarregarListas()
        {
            lUnidMedidaBasicPickerModel = UnidadeMedidaRepository.GetListToBasicPickerModel();
            var un = lUnidMedidaBasicPickerModel.FirstOrDefault(c => c.Id == currentModel.idUnidadeMedida);
            currentUnidMedidaBasicPickerModel = un ?? lUnidMedidaBasicPickerModel.FirstOrDefault(c => c.Display.ToUpper().Contains("UNIDADE"));

            LRepresentadasBasicPickerModels = RepresentadaRepository.GetListToBasicPickerModel();
            var representada = LRepresentadasBasicPickerModels.FirstOrDefault(c => c.Id == currentModel.idRepresentada);
            currentRepresentadaBasicPickerModel = representada ?? LRepresentadasBasicPickerModels.FirstOrDefault();

            CarregarCategoria();
        }

        public void CarregarCategoria()
        {
            LCategoriaBasicPickerModels = CategoriaRepository.GetListToBasicPickerModel(currentModel.idRepresentada);
            var categoria = LCategoriaBasicPickerModels.FirstOrDefault(c => c.Id == currentModel.idCategoria);
            if (categoria == null && currentModel.idCategoria != 0)
            {
                LCategoriaBasicPickerModels.Add(new BasicPickerModel { XId = "0" , Display = "Sem categoria", Id = 0});
            }
            currentCategoriaBasicPickerModel = categoria ??
                                               LCategoriaBasicPickerModels.FirstOrDefault(
                                                   c => c.Display.ToUpper().Contains("PROD"));
        }

        public void Save()
        {
            try
            {
                var isNew = currentModel.idProdutoOffLine == null;

                if (currentModel.idProduto == null || currentModel.idProduto == 0)
                {
                    currentModel.bExibirCatalogo = false;
                }

                ProdutoRepository.Save(objProdutoModel: currentModel);

                if (isNew && StaticModel.StaticFindProdutoModel != null)
                {
                    // manipulação da page de pesquisa para ja vir pesquisado o registro salvo
                    StaticModel.StaticFindProdutoModel.RegistrosToSearch.Add(new BasicPickerModel
                    {
                        Id = currentModel.idProdutoOffLine ?? 0,
                        Display = currentModel.xNome,
                        Detail = currentCategoriaBasicPickerModel.Display
                    });
                    StaticModel.StaticFindProdutoModel.Filtro = currentModel.xNome;
                }

                // altero as informações do item selecionado no listar de PRODUTO
                if (!isNew)
                {
                    if (StaticModel.StaticFindProdutoModel != null)
                    {
                        StaticModel.StaticFindProdutoModel.SelectedItem.Display = currentModel.xNome;
                        StaticModel.StaticFindProdutoModel.SelectedItem.Detail = currentCategoriaBasicPickerModel.Display;
                    }
                    StaticModel.StaticProdutoModel.needRefresh = true;
                }
                UtilNavidate.PopAsync();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool CanSave()
        {
            return !string.IsNullOrEmpty(currentModel.xNome) &&
                   currentModel.idUnidadeMedida != 0 &&
                   currentModel.idRepresentada != 0 &&
                   currentModel.idCategoria != 0;
        }

        #region CALCULOS

        public void CalculoCustoProduto()
        {
            try
            {
                currentModel.vCusto = (double)prodCalc.CalcularCustoProduto(
                    currentModel.vCompra.ObjectToDecimalNullable(),
                    currentModel.pIcms.ObjectToDecimalNullable(),
                    currentModel.pSt.ObjectToDecimalNullable(),
                    currentModel.pIpi.ObjectToDecimalNullable(),
                    currentModel.pOutras.ObjectToDecimalNullable());
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        public void CalcularValorVenda()
        {
            currentModel.vVenda = (double)prodCalc.CalcularValorVendaProduto(vCusto: currentModel.vCusto.ObjectToDecimalNullable(),
                pLucro: currentModel.pLucro.ToDecimalPtBr());
        }

        public void CalcularPorcLucro()
        {
            currentModel.pLucro = (double)prodCalc.CalcularPorcLucroVenda(vVenda: currentModel.vVenda.ToDecimalPtBr(),
                vCusto: currentModel.vCusto.ObjectToDecimalNullable());
        }

        public void CalcularValorVendaComImpostos()
        {
            vVendaComImpostos =
                (double)prodCalc.CalcularValorVendaProdutoComImpostos(vVenda: currentModel.vVenda.ToDecimalPtBr(),
                    pIpiVenda: currentModel.pIpiVenda.ToDecimalPtBr(), pStVenda: currentModel.pStVenda.ToDecimalPtBr());
        }
        //public void CalculoValorVendaByLucro()
        //{
        //    currentModel.vVenda = ((currentModel.pLucro / 100) * currentModel.vCusto) + currentModel.vCusto;
        //}

        //public void CalculoLucroByValorVenda()
        //{
        //    currentModel.pLucro = currentModel.vCusto > 0 ? ((currentModel.vVenda - currentModel.vCusto) / currentModel.vCusto) * 100
        //            : 100;
        //}
        #endregion




    }
}
