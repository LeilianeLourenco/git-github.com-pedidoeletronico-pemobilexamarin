using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Windows.Input;
using SQLite;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Model.Cadastros;
using Xamarin.HLP.Mobile.AppPE.Model.Estoque;
using Xamarin.HLP.Mobile.AppPE.Model.Repository;
using Xamarin.HLP.Mobile.AppPE.View.Pedido;
using Xamarin.HLP.Mobile.AppPE.ViewModel.Pedido;

namespace Xamarin.HLP.Mobile.AppPE.Model.Lancamento
{
    [Table(TableMobile.TB_PEDIDOVENDAITENS)]
    public class PedidoVendaItensModel : ModelComum
    {
        public PedidoVendaItensModel()
        {
            EditItemPedidoCommand = new Command(EditItem);
            TableEscalonada = new Command(EditItemEscalonada);
            RemoveItemCommand = new Command(RemoveItem);
            this.bTabelasCarregadas = false;
            this.bLocaisCarregados = false;
        }

        public string xNomeVariacao { get; set; }

        public List<VariacaoModel> _lTiposVariacoes;

        [Ignore]
        [IgnoreDataMember]
        public List<VariacaoModel> lTiposVariacoes
        {
            get { return _lTiposVariacoes; }
            set { _lTiposVariacoes = value; NotifyPropertyChanged(); }
        }

        public int lIdsProduto { get; set; }

        public decimal? dPesoBruto { get; set; }
        public bool bTabelasCarregadas { get; set; }
        public bool stVendaSemEstoque { get; set; }


        public bool bLocaisCarregados { get; set; }

        /// <summary>
        /// utilizado para controle de preços
        /// </summary>
        public bool bProdutoComValorDiferente { get; set; }
        public bool bDescMaximoPermitido { get; set; }


        //OS 35349 - Jessica Barbieri
        public bool bPedidoFechadoIncorretamente;

        public double? idCondicaoPagamento { get; set; } //alterado

        #region Primary Keys and Foring Keys

        /// <summary>
        /// FK NUVEM
        /// </summary>
        public int? idPedidoVenda { get; set; }

        /// <summary>
        /// FK LOCAL
        /// </summary>
        public int? idPedidoVendaOffLine { get; set; }

        [PrimaryKey, AutoIncrement]
        public int? idPedidoVendaItemOffLine { get; set; }

        [IgnoreDataMember]
        public int idClientesOffLine { get; set; }

        public int idProdutoOffLine { get; set; }

        #endregion

        private double _vUnitarioVenda;

        /// <summary>
        /// campo não mais passivo de alteração pelo usuário
        /// </summary>
        [NotNull]
        public double vUnitarioVenda
        {
            get { return _vUnitarioVenda; }
            set
            {
                _vUnitarioVenda = value;
                NotifyPropertyChanged();
                NotifyTotalizadores();
            }
        }

        private double _pDesconto;

        public double pDesconto
        {
            get { return _pDesconto; }
            set
            {
                _pDesconto = value;
                NotifyPropertyChanged();
                NotifyTotalizadores();
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

        private string _stComissao;
        //Campo de tipo criado como string por seguir modelagem já estabelecida em campo de ordem de comissão Representação
        public string stComissao
        {
            get { return _stComissao; }
            set
            {
                _stComissao = value;
                NotifyPropertyChanged();
            }
        }

        private bool _stDescontaIpiComissao;

        public bool stDescontaIpiComissao
        {
            get { return _stDescontaIpiComissao; }
            set
            {
                _stDescontaIpiComissao = value;
                NotifyPropertyChanged();
            }
        }

        private double _vLimiteMinimoVenda;

        public double vLimiteMinimoVenda
        {
            get { return _vLimiteMinimoVenda; }
            set { _vLimiteMinimoVenda = value; NotifyPropertyChanged(); }
        }

        private double _vUltimaVenda;

        /// <summary>
        /// utilizado na tela de ultimos produtos adquiridos do cliente para trazer a info da ultima venda
        /// </summary>

        public double vUltimaVenda
        {
            get { return _vUltimaVenda; }
            set { _vUltimaVenda = value; NotifyPropertyChanged(); }
        }

        private double _vQtdUltimaVenda;

        /// <summary>
        /// utilizado na tela de ultimos produtos adquiridos do cliente para trazer a info da ultima venda
        /// </summary>

        public double vQtdUltimaVenda
        {
            get { return _vQtdUltimaVenda; }
            set { _vQtdUltimaVenda = value; NotifyPropertyChanged(); }
        }

        private double? _vQtdEstoque;

        public double? vQtdEstoque
        {
            get { return _vQtdEstoque; }
            set { _vQtdEstoque = value; NotifyPropertyChanged(); }
        }

        private string _xQtdEstoque;

        public string xQtdEstoque
        {
            get
            {
                if (vQtdEstoque != null)
                {
                    _xQtdEstoque = $"Disponível: {vQtdEstoque}";
                }
                return _xQtdEstoque;
            }
            set { _xQtdEstoque = value; NotifyPropertyChanged(); }
        }

        private double? _vValorPorPeso;

        public double? vValorPorPeso
        {
            get { return _vValorPorPeso; }
            set { _vValorPorPeso = value; NotifyPropertyChanged(); }
        }

        private string _xValorPorPeso;

        public string xValorPorPeso
        {
            get
            {            
                double valorPorPeso = 0;

                if (vUnitarioVendaComImpostos > 0 && dPesoBruto > 0)
                    valorPorPeso = vUnitarioVendaComImpostos / (double)(dPesoBruto ?? 0);
                else
                    valorPorPeso = vUnitarioVendaComImpostos;

                _xValorPorPeso = $"Valor por peso: {valorPorPeso.ToCurrencyStringPtBr()}";
                return _xValorPorPeso;
            }
            set { _xValorPorPeso = value; NotifyPropertyChanged(); }
        }

        private string _xUltimaVendaInfo;

        public string xUltimaVendaInfo
        {
            get
            {
                if (vQtdUltimaVenda == 0 || vUltimaVenda == 0)
                    return string.Empty;

                return _xUltimaVendaInfo = $"Última venda: {vQtdUltimaVenda}x {vUltimaVenda.ToCurrencyStringPtBr()}";
            }
            set { _xUltimaVendaInfo = value; NotifyPropertyChanged(); }
        }

        private string _xQtdEstoqueValor;

        public string xQtdEstoqueValor
        {
            get
            {
                if (vQtdEstoque != null)
                {
                    _xQtdEstoqueValor = $"{vQtdEstoque.GetValueOrDefault().ArredondarValorDecimal(2)}";
                }
                return _xQtdEstoqueValor;
            }
            set { _xQtdEstoqueValor = value; NotifyPropertyChanged(); }
        }

        private bool _stDescontaStComissao;

        public bool stDescontaStComissao
        {
            get { return _stDescontaStComissao; }
            set
            {
                _stDescontaStComissao = value;
                NotifyPropertyChanged();
            }
        }

        private double _pComissao;

        public double pComissao
        {
            get { return _pComissao; }
            set
            {
                _pComissao = value;
                NotifyPropertyChanged();
            }
        }

        private double _vComissao;

        /// <summary>
        /// % da comissao * qtdade item * valor base 
        /// </summary>
        public double vComissao
        {
            get { return _vComissao; }
            set
            {
                _vComissao = value;
                NotifyPropertyChanged();
            }
        }

        private double _pComissaoOriginal;

        public double pComissaoOriginal
        {
            get { return _pComissaoOriginal; }
            set
            {
                _pComissaoOriginal = value;
                NotifyPropertyChanged();
            }
        }

        private double _vVenda;

        /// <summary>
        /// Campo padrão da tabela de preço - não é alteravel ( com impostos )
        /// </summary>

        [NotNull]
        public double vVenda
        {
            get { return _vVenda; }
            set
            {
                _vVenda = value;
                NotifyPropertyChanged();
            }
        }

        private double _vVendaOriginal;

        /// <summary>
        /// Valor do cadastro do produto
        /// </summary>

        public double vVendaOriginal
        {
            get { return _vVendaOriginal; }
            set
            {
                _vVendaOriginal = value;
                NotifyPropertyChanged();
            }
        }

        private double _vCusto;

        /// <summary>
        /// Valor de compra do produto
        /// </summary>

        public double vCusto
        {
            get { return _vCusto; }
            set
            {
                _vCusto = value;
                NotifyPropertyChanged();
            }
        }

        private string _xInfAdicionais;

        public string xInfAdicionais
        {
            get { return _xInfAdicionais ?? ""; }
            set
            {
                _xInfAdicionais = value ?? "";
                NotifyPropertyChanged();
            }
        }

        private int _idLocalEstoque;

        public int idLocalEstoque
        {
            get { return _idLocalEstoque; }
            set
            {
                _idLocalEstoque = value;
                NotifyPropertyChanged();
            }
        }

        private int _idTabelaPreco;

        public int idTabelaPreco
        {
            get { return _idTabelaPreco; }
            set
            {
                _idTabelaPreco = value;
                NotifyPropertyChanged();
            }
        }

        public int? idProduto { get; set; }
        public DateTime? dtUltimaAlteracao { get; set; }

        private double _vQtdItem;

        public double vQtdItem
        {
            get { return _vQtdItem; }
            set
            {
                if (value == _vQtdItem) return;

                _vQtdItem = value;
                NotifyPropertyChanged();
                NotifyTotalizadores();
            }
        }

        private string _xSigla;

        public string xSigla
        {
            get { return _xSigla; }
            set
            {
                _xSigla = value;
                NotifyPropertyChanged();
            }
        }

        public int? idGradeCor { get; set; }
        public int? idGradeTamanho { get; set; }
        public int? idItemAgrupamento { get; set; }
        public int idEmpresa { get; set; }

        private double? _pIpiVenda;

        public double? pIpiVenda
        {
            get { return _pIpiVenda; }
            set
            {
                _pIpiVenda = value;
                NotifyPropertyChanged();
            }
        }

        private double? _pStVenda;

        public double? pStVenda
        {
            get { return _pStVenda; }
            set
            {
                _pStVenda = value;
                NotifyPropertyChanged();
            }
        }

        private double _vDesconto;

        public double vDesconto
        {
            get { return _vDesconto; }
            set
            {
                _vDesconto = value;
                NotifyPropertyChanged();
                NotifyTotalizadores();
            }
        }

        private double _vJuros;

        public double vJuros
        {
            get { return _vJuros; }
            set
            {
                _vJuros = value;
                NotifyTotalizadores();
                NotifyPropertyChanged();
            }
        }

        private double _vUnitarioVendaComImpostosOriginal;

        public double vUnitarioVendaComImpostosOriginal
        {
            get { return _vUnitarioVendaComImpostosOriginal; }
            set
            {
                _vUnitarioVendaComImpostosOriginal = value;
                NotifyTotalizadores();
                NotifyPropertyChanged();
            }
        }

        private double _vUnitarioVendaComImpostos;

        public double vUnitarioVendaComImpostos
        {
            get { return _vUnitarioVendaComImpostos; }
            set
            {
                _vUnitarioVendaComImpostos = value;
                NotifyTotalizadores();
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(xValorPorPeso)); 
            }
        }


        private double _vUnitarioVendaSemImposto;
        /// <summary>
        /// Valor unitario com desconto e sem impostos
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

        #region propriedades que não fazem parte da TABLE OFICIAL

        /// <summary>
        /// EditarItemInListCleanCommand está sendo executado ou não
        /// </summary>
        [IgnoreDataMember]
        [Ignore]
        public bool editting { get; set; } = false;


        private List<ImageSource> _listaImagens;
        [Ignore]
        [IgnoreDataMember]
        public List<ImageSource> ListaImagens
        {
            get { return _listaImagens; }
            set { _listaImagens = value; NotifyPropertyChanged(); }
        }



        [IgnoreDataMember]
        public int QtdeGrade { get; set; }

        [IgnoreDataMember]
        public int nCasasDecimais { get; set; }

        private string _xFileImagePrincipal = "ApplicationDefaultImage.jpg";

        [IgnoreDataMember]
        public string xFileImagePrincipal
        {
            get { return _xFileImagePrincipal; }
            set
            {
                _xFileImagePrincipal = value;
                NotifyPropertyChanged();
            }
        }

        private string _xDescricao;

        [IgnoreDataMember]
        public string xDescricao
        {
            get { return (_xDescricao ?? "").ToUpper(); }
            set
            {
                _xDescricao = value;
                NotifyPropertyChanged();
            }
        }

        [IgnoreDataMember]
        public int SeqToLastSales { get; set; }

        [IgnoreDataMember]
        [Ignore]
        public string xDescricaoToEstoque { get; set; }

        private bool _itemValid = true;

        [IgnoreDataMember]
        [Ignore]
        [Obsolete]
        public bool itemValid
        {
            get { return _itemValid; }
            set
            {
                _itemValid = value;
                NotifyPropertyChanged();
                if (SaveCommand != null)
                    SaveCommand.ChangeCanExecute();
            }
        }



        private string _cAlternativo;

        public string cAlternativo
        {
            get { return (_cAlternativo ?? "").ToUpper(); }
            set
            {
                _cAlternativo = value;
                NotifyPropertyChanged();
            }
        }


        private bool _ItemJaIncluso;

        [IgnoreDataMember]
        [Ignore]
        public bool ItemJaIncluso
        {
            get { return _ItemJaIncluso; }
            set
            {
                _ItemJaIncluso = value;
                NotifyPropertyChanged();
            }
        }




        [Ignore]
        [IgnoreDataMember]
        public ICommand EditItemPedidoCommand { get; set; }

        [Ignore]
        [IgnoreDataMember]
        public ICommand TableEscalonada { get; set; }

        [Ignore]
        [IgnoreDataMember]
        public ICommand RemoveItemCommand { get; set; }


        private ObservableCollection<PedidoVendaItensModel> _itensGrade;

        [Ignore]
        [IgnoreDataMember]
        public ObservableCollection<PedidoVendaItensModel> ItensGrade
        {
            get { return _itensGrade; }
            set
            {
                _itensGrade = value;
                NotifyPropertyChanged();
            }
        }

        private ObservableCollection<PedidoVendaItensModel> _itensVariacao;

        [Ignore]
        [IgnoreDataMember]
        public ObservableCollection<PedidoVendaItensModel> ItensVariacao
        {
            get { return _itensVariacao; }
            set
            {
                _itensVariacao = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Propriedade para mostrar se o produto tem algum tipo de grade
        /// </summary>
        [Ignore]
        [IgnoreDataMember]
        public bool HasGrade => QtdeGrade > 0 || bProdutoVariacao;
        public bool bProdutoVariacao { get; set; }

        private bool _bPrecoAtualizado = false;

        [Ignore]
        [IgnoreDataMember]
        public bool bPrecoAtualizado
        {
            get { return _bPrecoAtualizado; }
            set
            {
                _bPrecoAtualizado = value;
                NotifyPropertyChanged();
            }
        }


        private Color _xCor = Color.FromHex("#FFFFFF");

        [Ignore]
        [IgnoreDataMember]
        public Color xCor
        {
            get { return _xCor; }
            set { _xCor = value; }
        }


        private bool _bBloquearVisualizacaoEstoqueVendedor = false;

        [Ignore]
        [IgnoreDataMember]
        public bool bBloquearVisualizacaoEstoqueVendedor
        {
            get { return _bBloquearVisualizacaoEstoqueVendedor; }
            set
            {
                _bBloquearVisualizacaoEstoqueVendedor = value;
                NotifyPropertyChanged();
            }
        }

        private bool _bExibirValorPorPeso = false;

        [Ignore]
        [IgnoreDataMember]
        public bool bExibirValorPorPeso
        {
            get { return _bExibirValorPorPeso; }
            set
            {
                _bExibirValorPorPeso = value;
                NotifyPropertyChanged();
            }
        }

        private double _vVendaDef;

        /// <summary>
        /// Valor de venda default do produto com os impostos
        /// </summary>
        public double vVendaDef
        {
            get { return _vVendaDef; }
            set
            {
                _vVendaDef = value;
                NotifyPropertyChanged();
            }
        }


        private double? _vDescontoDaCondicao;

        /// <summary>
        /// Uso como auxiliar para calculos no descontoitembehavior
        /// </summary>
        public double? vDescontoDaCondicao
        {
            get { return _vDescontoDaCondicao; }
            set
            {
                _vDescontoDaCondicao = value;
                NotifyPropertyChanged();
            }
        }

        public int idRepresentada { get; set; }

        private List<TabelaPrecoSimplificada> _lTabelaPreco = new List<TabelaPrecoSimplificada>();

        [Ignore]
        public List<TabelaPrecoSimplificada> lTabelaPreco
        {
            get { return _lTabelaPreco; }
            set
            {
                _lTabelaPreco = value;
                NotifyPropertyChanged();
            }
        }

        private TabelaPrecoSimplificada _tabelaPrecoSelecionada = new TabelaPrecoSimplificada();

        [Ignore]
        public TabelaPrecoSimplificada tabelaPrecoSelecionada
        {
            get { return _tabelaPrecoSelecionada; }
            set
            {
                _tabelaPrecoSelecionada = value;
                NotifyPropertyChanged();
            }
        }

        private List<LocalEstoqueSimplificado> _lLocaisEstoque = new List<LocalEstoqueSimplificado>();

        [Ignore]
        public List<LocalEstoqueSimplificado> lLocaisEstoque
        {
            get { return _lLocaisEstoque; }
            set
            {
                _lLocaisEstoque = value;
                NotifyPropertyChanged();
            }
        }


        private LocalEstoqueSimplificado _currentLocalEstoque;

        [Ignore]
        public LocalEstoqueSimplificado currentLocalEstoque
        {
            get { return _currentLocalEstoque; }
            set
            {
                _currentLocalEstoque = value;
                NotifyPropertyChanged();
            }
        }

        private TabelaPrecoSimplificada _currentTabelaPreco;

        [Ignore]
        public TabelaPrecoSimplificada currentTabelaPreco
        {
            get { return _currentTabelaPreco; }
            set
            {
                var _tabelaPrecoAntiga = idTabelaPreco;
                _currentTabelaPreco = value;
                if (value == null || value.idTabelaPreco == idTabelaPreco) return;
                idTabelaPreco = value.idTabelaPreco;

                //linha adicionada pois caso a tabela de preço esteja zerada
                //na tela de últimos produtos no cliente traz 0
                if (value.vVenda <= 0)
                    value.vVenda = vVenda;


                //processo utilizado apenas para qunado o usuário ficar digitando imposto no produto
                vUnitarioVendaSemImposto = value.vUnitario;

                //OS 35400 - Jessica Barbieri
                var cond = CondicaoPagamentoRepository.GetItem(PagePedidoNew.CurrentViewModel.ItemCondicaoPgto.Id);
                if (cond.vDescCondicao != null)
                {
                    vUnitarioVenda = value.vVenda;
                    if (vDescontoDaCondicao != cond.vDescCondicao)
                        pDesconto -= cond.vDescCondicao.GetValueOrDefault();

                    vDesconto = (vUnitarioVenda * (pDesconto / 100)).ArredondarValorDecimal(nCasasDecimais: 2);
                    vUnitarioVendaComImpostos = vUnitarioVenda - vDesconto;
                    vUnitarioVendaComImpostosOriginal = vUnitarioVenda - vDesconto;
                    vSubTotal = vUnitarioVendaComImpostos * vQtdItem;
                    if (pDesconto < 0 || vDesconto < 0)
                    {
                        pDesconto = 0;
                        vDesconto = 0;
                    }

                    vDescontoDaCondicao = cond.vDescCondicao;
                }
                else
                {
                    //foi feito o if else para não atrapalhar quem já usava,
                    // esse vunitário venda possui um  calculo no pedidovendacalculo que atrapalha qdo aplica desconto quando o cara usa condição com valores.
                    vUnitarioVenda = vUnitarioVendaComImpostos = value.vVenda;
                    if (vDesconto > 0)
                    {
                        vDesconto = (vUnitarioVendaComImpostos * (pDesconto / 100)).ArredondarValorDecimal(nCasasDecimais: 2);
                        vUnitarioVendaComImpostos -= vDesconto;
                        vUnitarioVendaComImpostosOriginal = vUnitarioVendaComImpostos;
                    }
                }

                currentTabelaPreco.bBloquearValorProdutoApp = ConfiguracaoGeralRepositorio.GetConfiguracaoEmpresa().bBloqueiaValorProdutoApp;
                NotifyPropertyChanged();
            }
        }

        [Ignore]
        [IgnoreDataMember]
        public double dEstoque { get; set; }

        [Ignore]
        [IgnoreDataMember]
        public string xDisplayEstoque => string.Format("{0}  {1}Estoque atual: {2}",
            idProduto.ToString() + (idGradeCor ?? 0).ToString() + (idGradeTamanho ?? 0).ToString() + ";" +
            (xDescricaoToEstoque ?? "").Replace(";", ""),
            Environment.NewLine,
            dEstoque);


        /// <summary>
        /// Propriedade para dizer se deve ou não mostrar a cor da pagina de edição do item
        /// </summary>
        [Ignore]
        [IgnoreDataMember]
        public bool HasGradeCor => idGradeCor != null;


        private ImageSource _imageProduto;
        [Ignore]
        [IgnoreDataMember]
        public ImageSource ImageProduto => _imageProduto ?? (_imageProduto = UtilMethods.GetLocalProdutoImageSource(xFileImagePrincipal));

        [Ignore]
        [IgnoreDataMember]
        public string xEdit => HasGrade ? "EDITAR GRADE" : "EDITAR";


        [Ignore]
        [IgnoreDataMember]
        public string xValorPrincipal
        {
            get
            {
                var retorno = 0.ToCurrencyStringPtBr();

                try
                {
                    //retorno = ((ItensGrade != null && ItensGrade.Any()) ? _itensGrade.Where(c => c.vQtdItem > 0).Select(c => c.vUnitarioVendaComImpostos).FirstOrDefault().ToCurrencyStringPtBr() : vUnitarioVendaComImpostos.ToCurrencyStringPtBr());
                    retorno = ((ItensGrade != null && ItensGrade.Any())
                        ? _itensGrade.FirstOrDefault().vUnitarioVendaComImpostos.ToCurrencyStringPtBr()
                        : vUnitarioVendaComImpostos.ToCurrencyStringPtBr());
                    //retorno = ((ItensGrade != null && ItensGrade.Any())
                    //    ? _itensGrade.FirstOrDefault().vUnitarioVenda.ToCurrencyStringPtBr()
                    //    : vUnitarioVenda.ToCurrencyStringPtBr());
                }
                catch (Exception ex)
                {
                }
                return retorno;
            }
        }


        private bool _bUsaMinimoVendas;

        public bool bUsaMinimoVendas
        {
            get { return _bUsaMinimoVendas; }
            set { _bUsaMinimoVendas = value; NotifyPropertyChanged(); }
        }


        private string _xMinimoVendas;
        public string xMinimoVendas
        {
            get { return _xMinimoVendas; }
            set
            {
                _xMinimoVendas = value;
                NotifyPropertyChanged();
            }
        }

        [Ignore]
        [IgnoreDataMember]
        public string xValorPromocional
        {
            get
            {
                var valorBase = "";
                if (vUnitarioVendaComImpostos < vVendaDef)
                    valorBase = vVendaDef.ToCurrencyStringPtBr() + " / ";
                return valorBase;
            }
        }

        [Ignore]
        [IgnoreDataMember]
        public string xValorSubTotal
        {
            get
            {
                var retorno = 0.ToCurrencyStringPtBr();
                try
                {
                    if (ItensGrade != null && ItensGrade.Any())
                        retorno = ItensGrade.Where(c => c.vQtdItem > 0).Sum(c => c.vSubTotal).ToCurrencyStringPtBr();
                    else
                        retorno = vSubTotal.ToCurrencyStringPtBr();
                }
                catch (Exception ex)
                {

                }
                return retorno;
            }
        }



        [Ignore]
        [IgnoreDataMember]
        public string xQtde
        {
            get
            {
                var qtde = (ItensGrade != null && ItensGrade.Any()) ? _itensGrade.Sum(c => c.vQtdItem) : vQtdItem;
                var retorno = (nCasasDecimais == 0
                                  ? qtde.ToString()
                                  : qtde.ToString("#0.".PadRight(nCasasDecimais + 3, '0'))) + "x"; //33967

                return retorno;
            }
        }

        private string _xDetalheItem;

        [Ignore]
        [IgnoreDataMember]
        public string xDetalheItem
        {
            get { return _xDetalheItem; }
            set
            {
                _xDetalheItem = value;
                NotifyPropertyChanged();
            }
        }

        public void NotifyTotalizadores()
        {

            if (PagePedidoNew.CurrentViewModel != null)
            {
                //verifico se é o item atual a ser editado, ou se é um item já salvo
                if (PagePedidoNew.CurrentViewModel.currentModel.CurrentItemModel == this || idPedidoVendaOffLine != null)
                {
                    NotifyPropertyChanged("xQtde");
                    NotifyPropertyChanged("xValorPromocional");
                    NotifyPropertyChanged("xValorPrincipal");
                    NotifyPropertyChanged("xValorSubTotal");
                }
            }
        }

        #endregion

        public void SetDetalheItem()
        {
            if (ItensGrade != null && ItensGrade.Any())
            {
                var qtde = ItensGrade.Sum(c => c.vQtdItem);
                xDetalheItem =
                    qtde <= 0
                        ? null
                        : $"{xQtde} - {xValorSubTotal}";
            }
            else
            {
                xDetalheItem =
                    vQtdItem <= 0
                        ? null
                        : $"{xQtde} - {xValorSubTotal}";
            }

        }


        public decimal SetValorDesconto(double vDesconto, double vResto)
        {
            var _vTabela = this.vVenda;
            var _vDesconto = Math.Round(d: (decimal)vDesconto, decimals: 4, mode: MidpointRounding.AwayFromZero);

            var _pDesconto = Math.Round(d: (_vDesconto * 100) / (decimal)_vTabela, decimals: 4, mode: MidpointRounding.AwayFromZero);

            this.pDesconto = (double)_pDesconto;
            this.vDesconto = (double)_vDesconto;

            if (_vTabela != this.vUnitarioVendaComImpostos)
            {
                this.vUnitarioVendaComImpostos = _vTabela;
                this.vUnitarioVendaComImpostosOriginal = _vTabela;
            }
            this.vUnitarioVendaComImpostos = _vTabela - (double)_vDesconto;
            this.vUnitarioVendaComImpostosOriginal = _vTabela - (double)_vDesconto;
            this.vSubTotal = (double)Math.Round(d: (((decimal)this.vUnitarioVendaComImpostos * (decimal)this.vQtdItem) + (decimal)vResto), decimals: 2, mode: MidpointRounding.AwayFromZero);

            return (decimal)this.vSubTotal;
        }

        public void SetValorTotal(double vSubTotalAux)
        {
            this.vSubTotal = vSubTotalAux;
        }

        public async void EditItem()
        {
            try
            {
                if (PageListarProdutosNew.currentViewModel == null)
                    PageListarProdutosNew.currentViewModel = new ViewModel.Pedido.ListarProdutosNewViewModel();

                if (!editting && !PageListarProdutosNew.currentViewModel.IsBusy)
                {
                    editting = true;
                    var viewmodel = PagePedidoNew.CurrentViewModel;
                    var item = PagePedidoNew.CurrentViewModel.currentModel;

                    if (viewmodel != null && item != null)
                    {
                        PagePedidoNew.CurrentViewModel.currentModel.CurrentItemModel = this;
                        if (PageListarProdutosNew.currentViewModel != null)
                            PageListarProdutosNew.currentViewModel.itemSelected = this;

                        if (viewmodel.currentModel.CurrentItemModel.bTabelasCarregadas == false)
                        {
                            await Task.Run(() =>
                            {
                                TabelaPrecoRepository.SetTabelaPrecoByProduto(viewmodel.currentModel.CurrentItemModel,
                                viewmodel.currentModel.idClientesOffLine,
                                ClienteRepository.GetIdClienteNuvem(viewmodel.currentModel.idClientesOffLine),
                                viewmodel.currentModel.idRepresentantePedido ?? 0);
                                ProdutoRepository.SetComissao(item: viewmodel.currentModel.CurrentItemModel);
                            });
                        }


                        if (viewmodel.currentModel.CurrentItemModel.bLocaisCarregados == false)
                        {
                            await Task.Run(() =>
                            {
                                PedidoRepository.SetLocalEstoque(viewmodel.currentModel.CurrentItemModel,
                                ClienteRepository.GetIdClienteNuvem(viewmodel.currentModel.idClientesOffLine),
                                viewmodel.currentModel.idRepresentantePedido ?? 0);
                            });
                        }

                        var anotacao = ProdutoRepository.GetAnotacaoProduto(this.idProdutoOffLine);

                        if (!string.IsNullOrEmpty(anotacao))
                        {
                            if (!xInfAdicionais.ToUpper().Contains(anotacao.ToUpper()))
                            {
                                xInfAdicionais += string.IsNullOrEmpty(xInfAdicionais) ? anotacao : Environment.NewLine + anotacao;
                            }
                        }

                        currentTabelaPreco.bBloquearValorProdutoApp = ConfiguracaoGeralRepositorio.GetConfiguracaoEmpresa().bBloqueiaValorProdutoApp;

                        await Task.Yield(); // Delay(300);
                        UtilNavidate.PushAsync(new PageEditarItem());
                    }
                }
            }
            catch (Exception ex)
            {
                ex.TrakException();
            }

        }

        public async void EditItemEscalonada()
        {
            try
            {
                var _modelItem = this;
                if (_modelItem != null)
                {
                    var valorVenda = _modelItem.vUnitarioVendaComImpostos;
                    var faixaComissao = _modelItem.currentTabelaPreco.lFaixaComissao;
                    var idProduto = _modelItem.idProdutoOffLine;
                    var idEmpresa = _modelItem.idEmpresa;


                    if (!editting && !PageListarProdutosNew.currentViewModel.IsBusy)
                    {
                        if (_modelItem.currentTabelaPreco.lFaixaComissao?.Count() == 0)
                        {
                            await App.Messages.ShowAsync("Não foi encontrado nenhuma faixa de tabela escalonada relacionado a esse item, verifique de modificar a tabela de preço do item para buscar uma tabela escalonada relacionada!");
                        }
                        else
                        {
                            editting = true;
                            await Task.Yield(); // Delay(300);
                            UtilNavidate.PushAsync(new PageListarTabelaEscalonada(valorVenda, idProduto, faixaComissao, idEmpresa, _modelItem));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ex.TrakException();
            }

        }

        public async void RemoveItem()
        {
            try
            {
                if (PagePedidoNew.CurrentViewModel != null && PagePedidoNew.CurrentViewModel.currentModel != null)
                {
                    if (PagePedidoNew.CurrentViewModel.currentModel.lItens.Any(c => c == this))
                    {
                        if (HasGrade)
                        {
                            foreach (var c in this.ItensGrade)
                            {
                                if (c.idPedidoVendaItemOffLine != null && c.idPedidoVendaItemOffLine > 0)
                                {
                                    PagePedidoNew.CurrentViewModel.currentModel.ItensRemovidos.Add(c);
                                }
                            }
                        }
                        else
                        {
                            if (this.idPedidoVendaItemOffLine != null && this.idPedidoVendaItemOffLine > 0)
                            {
                                PagePedidoNew.CurrentViewModel.currentModel.ItensRemovidos.Add(this);
                            }
                        }

                        PagePedidoNew.CurrentViewModel.currentModel.lItens.Remove(this);


                        PagePedidoNew.CurrentViewModel.AtualizaTotalizadoresPedido();
                    }
                }
            }
            catch (Exception ex)
            {
                ex.TrakException();
            }
        }

    }
}
