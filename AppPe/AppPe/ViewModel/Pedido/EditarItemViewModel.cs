using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Hlp.PedidoEletronico.Domain.Business.Enums;
using Hlp.PedidoEletronico.Domain.Business.Helpers;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Core.PedidoVenda;
using Xamarin.HLP.Mobile.AppPE.Model;
using Xamarin.HLP.Mobile.AppPE.Model.Cadastros;
using Xamarin.HLP.Mobile.AppPE.Model.Lancamento;
using Xamarin.HLP.Mobile.AppPE.Model.Lancamento.Behaviors;
using Xamarin.HLP.Mobile.AppPE.Model.Repository;
using Xamarin.HLP.Mobile.AppPE.View.Pedido;

namespace Xamarin.HLP.Mobile.AppPE.ViewModel.Pedido
{
    public class EditarItemViewModel : ViewModelComum<PedidoVendaItensModel>
    {
        BuscaPreco _buscaPreco = new BuscaPreco();

        #region Properties

        private ObservableCollection<BasicPickerModel> _listaTabelaPreco = new ObservableCollection<BasicPickerModel>();

        public ObservableCollection<BasicPickerModel> ListaTabelaPreco
        {
            get { return _listaTabelaPreco; }
            set { _listaTabelaPreco = value; NotifyPropertyChanged(); }
        }

        private BasicPickerModel _currentTabelaPreco;
        public BasicPickerModel CurrentTabelaPreco
        {
            get { return _currentTabelaPreco; }
            set
            {
                try
                {
                    if (value == null) return;

                    if (_currentTabelaPreco == null || _currentTabelaPreco.Id != value.Id)
                    {
                        _currentTabelaPreco = value;

                        TabelaPrecoSimplificada currentTabPreco = new TabelaPrecoSimplificada();

                        currentTabPreco = currentModel.lTabelaPreco.FirstOrDefault(c => c.idTabelaPreco == value.Id);
                        if (currentTabPreco == null) return;

                        if (currentModel.bProdutoVariacao)
                            currentTabPreco.vVenda = ReturnValorProdutoVariacao(value.Id);

                        currentModel.currentTabelaPreco = currentTabPreco;
                        if (currentModel.ItensGrade == null) return;
                        foreach (var item in currentModel.ItensGrade)
                        {
                            if (PagePedidoNew.CurrentViewModel.ItemCondicaoPgto.vDescCondicao.GetValueOrDefault() != 0
                                || PagePedidoNew.CurrentViewModel.vDescCondicao.GetValueOrDefault() != 0)
                            {
                                if (PagePedidoNew.CurrentViewModel.ItemCondicaoPgto.vDescCondicao.GetValueOrDefault() != 0)
                                    item.vDescontoDaCondicao = PagePedidoNew.CurrentViewModel.ItemCondicaoPgto.vDescCondicao;
                                else if (PagePedidoNew.CurrentViewModel.vDescCondicao.GetValueOrDefault() != 0)
                                    item.vDescontoDaCondicao = PagePedidoNew.CurrentViewModel.vDescCondicao.GetValueOrDefault();
                            }

                            item.currentTabelaPreco = currentTabPreco;
                            item.vUnitarioVendaSemImposto = currentModel.vUnitarioVendaSemImposto;

                            if (currentModel.vDescontoDaCondicao.GetValueOrDefault() > 0)
                            {
                                item.vUnitarioVendaComImpostos = item.vUnitarioVenda + (item.vUnitarioVenda * (item.vDescontoDaCondicao.GetValueOrDefault() / 100));
                                item.vUnitarioVendaComImpostosOriginal = item.vUnitarioVendaComImpostos;
                            }

                        }

                        if (PagePedidoNew.CurrentViewModel.ItemCondicaoPgto.vDescCondicao == 0)
                        {
                            vDesconto = pDesconto = 0;
                        }


                        vUnitarioVendaComImpostos = vUnitarioVenda = currentModel.currentTabelaPreco.vVenda;
                        _vUnitarioVendaSemImposto = currentModel.vUnitarioVendaSemImposto;

                        // caso a comissão seja por tabela de preço e a a tabela seja aterada.
                        if (HelperPedidoVenda.GetTipoComissao(xComissao: currentModel.stComissao) ==
                            TipoComissao.tabelapreco)
                        {
                            currentModel.pComissao = currentModel.pComissaoOriginal = currentModel.currentTabelaPreco.pComissao;
                            pComissao = currentModel.pComissao;
                        }

                        else
                        {
                            if (currentModel.currentTabelaPreco.bEscalonada)
                            {
                                pComissao =
                                    currentModel.currentTabelaPreco.SelectComissaoEscalonada(currentModel.pDesconto);
                            }
                        }

                        pStVenda = currentModel.currentTabelaPreco.pStVenda;
                        pIpiVenda = currentModel.currentTabelaPreco.pIpiVenda;

                        //tratamento feito para descontos que vierem aplicados na condição de pagamento
                        if (currentModel.vDesconto > 0 && vDesconto == 0)
                        {
                            pDesconto = currentModel.pDesconto;
                            vDesconto = currentModel.vDesconto;
                        }

                        if (vDesconto > 0)
                        {
                            vDesconto = (vUnitarioVenda * (pDesconto / 100)).ArredondarValorDecimal(nCasasDecimais: 2);
                            vUnitarioVendaComImpostos -= vDesconto;
                        }

                        if (currentModel.vDescontoDaCondicao.GetValueOrDefault() > 0)
                        {
                            vUnitarioVendaComImpostos = vUnitarioVenda + (vUnitarioVenda * (currentModel.vDescontoDaCondicao.GetValueOrDefault() / 100));
                        }

                        //recalculo 
                        PedidoVendaCalculos.AtualizaValores(currentModel);

                        //tratamento no valor unitário
                        //após trocar de tabela ele preenchia o valor cheio sem desconto e zuava o total sem imposto.

                        xDescontoMaximo = $"desconto permitido {currentModel.currentTabelaPreco.pDescontoMaximo}%";

                        NotifyPropertyChanged();
                    }
                }
                catch (Exception ex)
                {
                    ex.TrakException();
                }
            }
        }

        public double ReturnValorProdutoVariacao(int idTabelaPreco)
        {
            var tabelaPreco = currentModel.lTabelaPreco.FirstOrDefault(c => c.idTabelaPreco == idTabelaPreco);

            var porcentagem = tabelaPreco?.pIndice ?? 0;
            var vVenda = currentModel.ItensGrade?.FirstOrDefault().vVendaOriginal ?? 0;

            var valor = (vVenda * porcentagem) / 100;

            return vVenda + valor;
        }

        private List<BasicPickerModel> _listaLocalEstoque = new List<BasicPickerModel>();
        public List<BasicPickerModel> ListaLocalEstoque
        {
            get { return _listaLocalEstoque; }
            set { _listaLocalEstoque = value; NotifyPropertyChanged(); }
        }

        private BasicPickerModel _currentLocalEstoque;
        public BasicPickerModel CurrentLocalEstoque
        {
            get { return _currentLocalEstoque; }
            set
            {
                try
                {
                    if (value == null) return;

                    _currentLocalEstoque = value;
                    var currentLocalEstoque = currentModel.lLocaisEstoque.FirstOrDefault(c => c.idLocalEstoque == value.Id);
                    if (currentLocalEstoque == null) return;

                    currentModel.currentLocalEstoque = currentLocalEstoque;

                    if (currentModel.ItensGrade?.Count() > 0)
                    {
                        foreach (var grade in currentModel.ItensGrade)
                        {
                            if (grade.idGradeCor == null && grade.idGradeTamanho == null)
                            {
                                currentModel.vQtdEstoque = ProdutoRepository.ObterEstoqueProduto(idEmpresa: currentModel.idEmpresa, idProduto: currentModel.idProduto ?? 0, idLocalEstoque: currentLocalEstoque.idLocalEstoque);
                                currentModel.xQtdEstoque = $"Disponível: {currentModel.vQtdEstoque}";
                                currentModel.idLocalEstoque = currentLocalEstoque.idLocalEstoque;
                            }
                            else
                            {
                                grade.vQtdEstoque = ProdutoRepository.ObterEstoqueGradeCorTamanhoProduto(currentModel.idEmpresa, currentModel.idProduto ?? 0, grade.idGradeCor, grade.idGradeTamanho, currentLocalEstoque.idLocalEstoque);
                                currentModel.xQtdEstoque = $"Disponível: {currentModel.vQtdEstoque}";
                                currentModel.idLocalEstoque = currentLocalEstoque.idLocalEstoque;
                            }
                        }
                    }
                    else
                    {
                        currentModel.vQtdEstoque = ProdutoRepository.ObterEstoqueProduto(idEmpresa: currentModel.idEmpresa, idProduto: currentModel.idProduto ?? 0, idLocalEstoque: currentLocalEstoque.idLocalEstoque);
                        currentModel.idLocalEstoque = currentLocalEstoque.idLocalEstoque;
                    }

                    NotifyPropertyChanged();

                }
                catch (Exception ex)
                {
                    ex.TrakException();
                }
            }
        }

        #region Calculaveis
        private double _vUnitarioVenda;
        /// <summary>
        /// Valor unitario sem desconto e com impostos
        /// </summary>
        public double vUnitarioVenda
        {
            get { return _vUnitarioVenda; }
            set
            {
                _vUnitarioVenda = value;
                if (currentModel.ItensGrade == null) return;
                foreach (var item in currentModel.ItensGrade)
                {
                    //se o desconto da condição não existir continuo a lógica normal que era antes
                    if (item.vDescontoDaCondicao == null)
                        item.vUnitarioVenda = value;
                }
                NotifyPropertyChanged();
            }
        }

        private double _vUnitarioVendaSemImposto;
        /// <summary>
        /// Valor unitario sem desconto e com impostos
        /// </summary>
        public double vUnitarioVendaSemImposto
        {
            get { return _vUnitarioVendaSemImposto; }
            set
            {
                _vUnitarioVendaSemImposto = value;
                NotifyPropertyChanged();
            }
        }


        private double _vUnitarioVendaComImpostos;
        /// <summary>
        /// valor unitário com impostos e descontos
        /// </summary>
        public double vUnitarioVendaComImpostos
        {
            get { return _vUnitarioVendaComImpostos; }
            set
            {
                if (Math.Abs(_vUnitarioVendaComImpostos - value) <= 0) return;

                _vUnitarioVendaComImpostos = value;
                if (currentModel.ItensGrade == null) return;
                foreach (var item in currentModel.ItensGrade)
                {
                    item.vUnitarioVendaComImpostos = value;
                }
                NotifyPropertyChanged();
                NotifyPropertyChanged("xValorPromocional");
            }
        }


        private double _pDesconto;
        public double pDesconto
        {
            get { return _pDesconto; }
            set
            {
                if (Math.Abs(_pDesconto - value) <= 0) return;
                if (value > 100)
                {
                    value = 100;
                }

                _pDesconto = value;
                currentModel.pDesconto = value;
                if (currentModel.ItensGrade == null) return;
                foreach (var item in currentModel.ItensGrade)
                {
                    item.pDesconto = value;
                }
                NotifyPropertyChanged();
            }
        }

        private double _vDesconto;
        public double vDesconto
        {
            get { return _vDesconto; }
            set
            {
                if (Math.Abs(_vDesconto - value) <= 0) return;
                _vDesconto = value; NotifyPropertyChanged();
            }
        }

        private double? _pIpiVenda;
        public double? pIpiVenda
        {
            get { return _pIpiVenda; }
            set
            {
                var valor = Math.Abs((_pIpiVenda ?? 0) - (value ?? 0));
                if (valor <= 0) return;

                _pIpiVenda = value;
                if (currentModel.ItensGrade == null) return;
                foreach (var item in currentModel.ItensGrade)
                {
                    item.pIpiVenda = value;
                }
                NotifyPropertyChanged();
            }
        }

        private double? _pStVenda;
        public double? pStVenda
        {
            get { return _pStVenda; }
            set
            {
                var valor = Math.Abs((_pStVenda ?? 0) - (value ?? 0));

                if (valor <= 0) return;

                _pStVenda = value;
                if (currentModel.ItensGrade == null) return;
                foreach (var item in currentModel.ItensGrade)
                {
                    item.pStVenda = value;
                }
                NotifyPropertyChanged();
            }
        }

        private double _pComissao;
        public double pComissao
        {
            get { return _pComissao; }
            set
            {
                if (Math.Abs(_pComissao - value) <= 0) return;

                _pComissao = value;
                if (currentModel.ItensGrade == null) return;
                foreach (var item in currentModel.ItensGrade)
                {
                    item.pComissao = value;
                }
                NotifyPropertyChanged();
            }
        }

        private double _vComissao;
        public double vComissao
        {
            get { return _vComissao; }
            set
            {
                if (Math.Abs(_pComissao - value) < 0) return;
                _vComissao = value; NotifyPropertyChanged();
            }
        }


        private double _vSubTotal;
        public double vSubTotal
        {
            get { return _vSubTotal; }
            set
            {
                _vSubTotal = value;
                NotifyPropertyChanged();
            }
        }

        private double _vSubTotalSemImpostos;
        public double vSubTotalSemImpostos
        {
            get { return _vSubTotalSemImpostos; }
            set
            {
                _vSubTotalSemImpostos = value;
                NotifyPropertyChanged();
            }
        }

        private double _vValorPorPeso;
        public double vValorPorPeso
        {
            get { return _vValorPorPeso; }
            set
            {
                _vValorPorPeso = value;
                NotifyPropertyChanged();
            }
        }

        #endregion

        private double _vQtdeTotal;

        /// <summary>
        /// Campo para mostrar o total de quantidade do item, (grade ou sem grade) campo não utilizado para calculo
        /// </summary>
        public double vQtdeTotal
        {
            get { return _vQtdeTotal; }
            set { _vQtdeTotal = value; NotifyPropertyChanged(); }
        }

        private bool _bUsaLocaisEstoque = false;

        public bool bUsaLocaisEstoque
        {
            get { return _bUsaLocaisEstoque; }
            set
            {
                _bUsaLocaisEstoque = value;
                NotifyPropertyChanged();
            }
        }

        private bool _bExibirValorPorPeso = false;

        public bool bExibirValorPorPeso
        {
            get { return _bExibirValorPorPeso; }
            set
            {
                _bExibirValorPorPeso = value;
                NotifyPropertyChanged();
            }
        }


        private string _xDescontoMaximo;
        public string xDescontoMaximo
        {
            get { return _xDescontoMaximo; }
            set { _xDescontoMaximo = value; NotifyPropertyChanged(); }
        }


        /// <summary>
        /// Utilizada no carrossel de imagens da PageEditarItem
        /// </summary>
        private List<ImageSource> _listaImagens;
        public List<ImageSource> ListaImagens
        {
            get { return _listaImagens; }
            set { _listaImagens = value; NotifyPropertyChanged(); }
        }

        public string xValorPromocional
        {
            get
            {

                var valorBase = "";
                if (vUnitarioVendaComImpostos < currentModel.vVendaDef)
                    valorBase = currentModel.vVendaDef.ToCurrencyStringPtBr() + " / ";
                return valorBase;
            }
        }

        #endregion

        public EditarItemViewModel()
        {
            try
            {
                if (IsBusy)
                    return;

                IsBusy = true;
                if (PagePedidoNew.CurrentViewModel.currentModel.CurrentItemModel == null) return;
                currentModel = PagePedidoNew.CurrentViewModel.currentModel.CurrentItemModel;

                StaticModel.StaticEditarItemViewModel = this;
                ListaTabelaPreco = new ObservableCollection<BasicPickerModel>();
                ListaLocalEstoque = new List<BasicPickerModel>();


                foreach (var tabelaPrecoSimplificada in currentModel.lTabelaPreco)
                {
                    ListaTabelaPreco.Add(new BasicPickerModel
                    {
                        Id = tabelaPrecoSimplificada.idTabelaPreco,
                        Display = tabelaPrecoSimplificada.xTabelaPreco
                    });
                }

                foreach (var localestoque in currentModel.lLocaisEstoque)
                {
                    ListaLocalEstoque.Add(new BasicPickerModel
                    {
                        Id = localestoque.idLocalEstoque,
                        Display = localestoque.xNomeLocal
                    });
                }


                InitializeDados();
                IsBusy = false;
                //Device.StartTimer(new TimeSpan(0, 0, 0, 0, 250), InitializeDados);
            }
            catch (Exception ex)
            {
                ex.TrakException();
            }
        }


        private bool bInitViewModelDados = true;

        public bool InitializeDados()
        {
            if (bInitViewModelDados)
            {
                bInitViewModelDados = false;
                try
                {
                    if (ListaTabelaPreco.Count > 0)
                    {
                        if (currentModel.ItensGrade == null)
                        {
                            if (currentModel.HasGrade && !currentModel.bProdutoVariacao && currentModel.ItensGrade == null)
                                currentModel.ItensGrade =
                                    new ObservableCollection<PedidoVendaItensModel>(
                                        ProdutoRepository.GetGradeItem(currentModel));
                            else if (!currentModel.HasGrade && !currentModel.bProdutoVariacao)
                                currentModel.ItensGrade = new ObservableCollection<PedidoVendaItensModel> { currentModel };

                            else if (currentModel.bProdutoVariacao)
                            {
                                currentModel.ItensGrade = new ObservableCollection<PedidoVendaItensModel> { currentModel };
                                currentModel.ItensVariacao = new ObservableCollection<PedidoVendaItensModel>(
                                        ProdutoRepository.GetVariacaoItem(currentModel));

                            }

                            CurrentTabelaPreco = ListaTabelaPreco.FirstOrDefault(c => c.Id == currentModel.idTabelaPreco);

                        }
                        else
                        {
                            _currentTabelaPreco =
                                ListaTabelaPreco.FirstOrDefault(c => c.Id == currentModel.idTabelaPreco);
                        }

                        if (currentModel.ItensGrade != null)
                        {
                            var itemBase = currentModel.ItensGrade.FirstOrDefault(c => c.vQtdItem > 0) ??
                                           currentModel.ItensGrade.FirstOrDefault();


                            //linha adicionada porque vem do últimas compras
                            // eu apenas sobrescrevo os valores.
                            if (itemBase.vUltimaVenda > 0)
                            {
                                itemBase.vUnitarioVenda = itemBase.vUltimaVenda;
                                itemBase.vUnitarioVendaComImpostos = itemBase.vUltimaVenda;
                                itemBase.vUnitarioVendaComImpostosOriginal = itemBase.vUltimaVenda;
                                itemBase.vUltimaVenda = itemBase.vUltimaVenda;
                                itemBase.vVenda = itemBase.vUnitarioVendaSemImposto;
                                itemBase.vSubTotal = itemBase.vUltimaVenda * itemBase.vQtdItem;
                            }



                            _vUnitarioVenda = itemBase.vUnitarioVenda;
                            _vUnitarioVendaComImpostos = itemBase.vUnitarioVendaComImpostos;

                            _vUnitarioVendaSemImposto = itemBase.vUnitarioVendaSemImposto;

                            if (_vUnitarioVendaComImpostos > 0 && itemBase.dPesoBruto > 0)
                                _vValorPorPeso = _vUnitarioVendaComImpostos / (double)(itemBase.dPesoBruto ?? 0);
                            else
                                _vValorPorPeso = _vUnitarioVendaComImpostos;

                            var _tblAtual = currentModel.currentTabelaPreco;

                            _pDesconto = itemBase.pDesconto;

                            if (_tblAtual != null)
                            {
                                _pIpiVenda = _tblAtual.pIpiVenda;
                                _pStVenda = _tblAtual.pStVenda;
                            }

                            _pComissao = itemBase.pComissao;


                            //tratamento de itens com grade e sem grade
                            if (itemBase.idGradeCor.GetValueOrDefault() > 0 && itemBase.idGradeTamanho.GetValueOrDefault() > 0)
                            {
                                _vDesconto = itemBase.vDesconto;
                                _vComissao = currentModel.ItensGrade.Where(c => c.vQtdItem > 0).Sum(c => c.vComissao);
                            }
                            else
                            {
                                _vDesconto = currentModel.ItensGrade.Sum(c => c.vDesconto);
                                _vComissao = currentModel.ItensGrade.Sum(c => c.vComissao);
                            }
                        }

                        if (currentModel.ListaImagens?.Count() > 0)
                        {
                            ListaImagens = currentModel.ListaImagens;
                        }
                    }

                    bExibirValorPorPeso = PagePedidoNew.CurrentViewModel.bExibirValorPorPeso;

                    if (ListaLocalEstoque.Where(t => t.Id > 0).Count() > 0)
                    {
                        bUsaLocaisEstoque = true;
                        CurrentLocalEstoque = _currentLocalEstoque = ListaLocalEstoque.FirstOrDefault(c => c.Id == currentModel.idLocalEstoque);
                    }
                    else
                    {
                        bUsaLocaisEstoque = false;
                    }
                }
                catch (Exception ex)
                {
                    ex.TrakException();
                }

            }

            return bInitViewModelDados;
        }


        public async Task<bool> ValidateCamposTask(bool zerarvalores,
           ValorUnitarioComImpostosBehaviors valorUnitario,
           DescontoItemBehaviors pdesconto,
           DescontoItemBehaviors vdesconto)
        {
            var bretorno = false;
            await Task.Run(() =>
            {
                if (valorUnitario != null &&
                    pdesconto != null &&
                    vdesconto != null)
                {
                    bretorno = (
                        valorUnitario.IsValid
                        && pdesconto.IsValid
                        && vdesconto.IsValid);
                    if (zerarvalores && !bretorno)
                    {
                        if (!pdesconto.IsValid || !vdesconto.IsValid || !valorUnitario.IsValid)
                        {
                            vDesconto = pDesconto = 0;
                            vUnitarioVendaComImpostos = vUnitarioVenda;
                            foreach (var item in currentModel.ItensGrade)
                            {
                                item.vDesconto = item.pDesconto = 0;
                                item.vUnitarioVendaComImpostos = vUnitarioVenda;
                                item.vUnitarioVendaComImpostosOriginal = vUnitarioVenda;
                                //var vqtde = item.vQtdItem;
                                //item.vQtdItem = 0;
                                //item.vQtdItem = vqtde;
                            }
                            DescontoItemBehaviors.AtualizaComissao(this);
                        }

                        //if (!valorUnitario.IsValid)
                        //{
                        //    vUnitarioVendaComImpostos =
                        //        PedidoVendaCalculos.CalculoValorUnitarioComImpostos(
                        //            currentModel.currentTabelaPreco.vUnitario,
                        //            pStVenda ?? 0, pIpiVenda ?? 0);
                        //}

                        PedidoVendaCalculos.AtualizaValores(currentModel);
                    }
                }
            });
            return bretorno;
        }

        public void BuscarTabelaPrecoFiltro(string filtro)
        {
            List<PedidoVendaItensModel> itens = new List<PedidoVendaItensModel>();

            var idClientesOffLine = PagePedidoNew.CurrentViewModel.currentModel.idClientesOffLine;
            var idClientes = PagePedidoNew.CurrentViewModel.currentModel.idClientes;
            var idRepresentante = PagePedidoNew.CurrentViewModel.currentModel.idRepresentantePedido;
            var idTabelaPrecoCondicao = PagePedidoNew.CurrentViewModel.idTabelaPrecoCondicao;

            var item = PagePedidoNew.CurrentViewModel.currentModel.CurrentItemModel;
            itens.Add(item);

            if (string.IsNullOrEmpty(filtro))
                filtro = null;

            this._buscaPreco.Buscar(
                        itens: itens,
                        idClienteOff: idClientesOffLine,
                        idCliente: idClientes ?? 0,
                        idRepresentante: idRepresentante ?? 0,
                        idEmpresa: App.EnvironmentPE.idEmpresaLogada,
                        idTabelaPrecoCondicao: idTabelaPrecoCondicao,
                        filtro: filtro
                    );

            //ListaTabelaPreco.Clear();

            //foreach (var tabelaPrecoSimplificada in currentModel.lTabelaPreco)
            //{
            //    ListaTabelaPreco.Add(new BasicPickerModel
            //    {
            //        Id = tabelaPrecoSimplificada.idTabelaPreco,
            //        Display = tabelaPrecoSimplificada.xTabelaPreco
            //    });
            //}
        }
    }
}
