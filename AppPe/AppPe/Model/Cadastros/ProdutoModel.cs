using System;
using System.Runtime.Serialization;
using SQLite;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.HLP.Mobile.AppPE.Model.Estoque;

namespace Xamarin.HLP.Mobile.AppPE.Model.Cadastros
{
    [Table(TableMobile.TB_PRODUTO)]
    public class ProdutoModel : ModelComum
    {

        public ProdutoModel()
        {
        }

        public int? idProduto { get; set; }

        private int? _idProdutoOffLine;
        [PrimaryKey, AutoIncrement]
        public int? idProdutoOffLine
        {
            get { return _idProdutoOffLine; }
            set { _idProdutoOffLine = value; NotifyPropertyChanged(); }
        }

        private string _cAlternativo;
        [NotNull]
        public string cAlternativo
        {
            get { return (_cAlternativo ?? "").ToUpper(); }
            set { _cAlternativo = value; NotifyPropertyChanged(); }
        }

        private string _xNome;
        [NotNull]
        public string xNome
        {
            get { return (_xNome ?? "").ToUpper(); }
            set
            {
                _xNome = value; NotifyPropertyChanged();
                xDisplaySemCaracter = value.RemoverAcentos();
            }
        }

        //public image? imProduto { get; set; }
        public string xUnidade { get; set; }

        private double _vCompra;
        public double vCompra
        {
            get { return _vCompra; }
            set { _vCompra = value; NotifyPropertyChanged(); }
        }

        private double _pIcms;
        public double pIcms
        {
            get { return _pIcms; }
            set { _pIcms = value; NotifyPropertyChanged(); }
        }

        private double _pIpi;
        public double pIpi
        {
            get { return _pIpi; }
            set { _pIpi = value; NotifyPropertyChanged(); }
        }

        private double _pSt;
        public double pSt
        {
            get { return _pSt; }
            set { _pSt = value; NotifyPropertyChanged(); }
        }

        private double _pOutras;
        public double pOutras
        {
            get { return _pOutras; }
            set { _pOutras = value; NotifyPropertyChanged(); }
        }

        private double _vCusto;
        public double vCusto
        {
            get { return _vCusto; }
            set { _vCusto = value; NotifyPropertyChanged(); }
        }

        private double _vVenda;
        [NotNull]
        public double vVenda
        {
            get { return _vVenda; }
            set { _vVenda = value; NotifyPropertyChanged(); }
        }

        private double _pLucro;
        [NotNull]
        public double pLucro
        {
            get { return _pLucro; }
            set { _pLucro = value; NotifyPropertyChanged(); }
        }

        private double? _pIpiVenda;
        public double? pIpiVenda
        {
            get { return _pIpiVenda; }
            set { _pIpiVenda = value; NotifyPropertyChanged(); }
        }

        private double? _pStVenda;
        public double? pStVenda
        {
            get { return _pStVenda; }
            set { _pStVenda = value; NotifyPropertyChanged(); }
        }

        private double? _vEstoqueMax;
        public double? vEstoqueMax
        {
            get { return _vEstoqueMax; }
            set { _vEstoqueMax = value; NotifyPropertyChanged(); }
        }

        private double? _vEstoqueMin;

        public double? vEstoqueMin
        {
            get { return _vEstoqueMin; }
            set { _vEstoqueMin = value; NotifyPropertyChanged(); }
        }

        private double? _vLimiteMinimoVenda;

        public double? vLimiteMinimoVenda
        {
            get { return _vLimiteMinimoVenda; }
            set { _vLimiteMinimoVenda = value; NotifyPropertyChanged(); }
        }


        //private List<ImagemModel> _lImagens;
        //[Ignore]
        //public List<ImagemModel> lImagens
        //{
        //    get { return _lImagens; }
        //    set { _lImagens = value; NotifyPropertyChanged(); }
        //}

        public string xAnotacao { get; set; }
        public string xFabricante { get; set; }

        private double? _pComissao;

        public double? pComissao
        {
            get { return _pComissao; }
            set { _pComissao = value; NotifyPropertyChanged(); }
        }

        [NotNull]
        public int idRepresentada { get; set; }
        [NotNull]
        public int idCategoria { get; set; }
        [NotNull]
        public int idEmpresa { get; set; }


        private bool _stVendaSemEstoque;
        [NotNull]
        public bool stVendaSemEstoque
        {
            get { return _stVendaSemEstoque; }
            set { _stVendaSemEstoque = value; NotifyPropertyChanged(); }
        }


        private bool _stControleEstoque;
        public bool stControleEstoque
        {
            get
            {
                return _stControleEstoque;
            }
            set { _stControleEstoque = value; NotifyPropertyChanged(); }
        }

        private bool _stAtivo = true;
        [NotNull]
        public bool stAtivo
        {
            get { return _stAtivo; }
            set
            {
                _stAtivo = value;
                NotifyPropertyChanged();
            }
        }

        public string idAspNetUserInclusao { get; set; }

        public byte stAtualizado { get; set; } = 1;

        private DateTime? _dtUltimaAlteracao;

        public DateTime? dtUltimaAlteracao
        {
            get { return _dtUltimaAlteracao; }
            set
            {
                _dtUltimaAlteracao = value != null ? ((DateTime)value).ToDateTimeSync() : (DateTime?)null;
            }
        }


        private static readonly string imgdefault = Device.OnPlatform("ApplicationDefaultImage.jpg", "ApplicationDefaultImage.jpg", "Assets/ApplicationDefaultImage.jpg");

        private string _xFileImagePrincipal = imgdefault;
        public string xFileImagePrincipal
        {
            get { return _xFileImagePrincipal ?? imgdefault; }
            set
            {
                _xFileImagePrincipal = value; NotifyPropertyChanged();


            }
        }

        private ObservableCollection<ListEstoqueProduto> _lEstoqueProduto = new ObservableCollection<ListEstoqueProduto>();
        [Ignore]
        [IgnoreDataMember]
        public ObservableCollection<ListEstoqueProduto> lEstoqueProduto
        {
            get { return _lEstoqueProduto; }
            set { _lEstoqueProduto = value; NotifyPropertyChanged(); }
        }

        private string _dtUltimaSincronizacaoEstoque;

        public string dtUltimaSincronizacaoEstoque
        {
            get { return _dtUltimaSincronizacaoEstoque; }
            set { _dtUltimaSincronizacaoEstoque = value; NotifyPropertyChanged(); }
        }

        private List<ImageSource> _listaImagens;
        [Ignore]
        [IgnoreDataMember]
        public List<ImageSource> ListaImagens
        {
            get { return _listaImagens; }
            set { _listaImagens = value; NotifyPropertyChanged(); }
        }





        [IgnoreDataMember]
        [Ignore]
        public ImageSource GetImageSource => UtilMethods.GetLocalProdutoImageSource(xFileImagePrincipal ?? "");

        private DateTime _dtCadastro;
        public DateTime dtCadastro
        {
            get { return _dtCadastro; }
            set { _dtCadastro = value; }
        }

        public int idUnidadeMedida { get; set; }
        public int hasGrade { get; set; }

        [IgnoreDataMember]
        public bool bProblemaSincronizacao { get; set; }


        private bool _bUtilizaEstoqueMinMax;
        public bool bUtilizaEstoqueMinMax
        {
            get { return _bUtilizaEstoqueMinMax; }
            set
            {
                _bUtilizaEstoqueMinMax = value; NotifyPropertyChanged();
            }
        }
        private string _cEan;

        public string cEan
        {
            get { return _cEan; }
            set { _cEan = value; NotifyPropertyChanged(); }
        }

        public string xNcm { get; set; }


        public string xDisplaySemCaracter { get; set; }

        private bool? _bExibirAnotacaoNoPedido;
        public bool? bExibirAnotacaoNoPedido
        {
            get { return _bExibirAnotacaoNoPedido ?? false; }
            set { _bExibirAnotacaoNoPedido = value; NotifyPropertyChanged(); }
        }




        #region relacionados a integração omie

        //Código da Situação Tributária do PIS.
        public string xCodCSTPIS { get; set; }
        //Código da Situação Tributária do COFINS.
        public string xCodCSTCOFINS { get; set; }
        //Alíquota do ICMS Operação Própria.
        public decimal? dAliqOPICMSST { get; set; }
        //Código da Situação Tributária do IPI.
        public string xCodCSTIPI { get; set; }
        //Código da Situação Tributária do ICMS.
        public string xCodCSTICMS { get; set; }
        //Origem.
        public string xOrigem { get; set; }
        //CFOP.
        public string xCFOP { get; set; }

        public string xCFOP_INTER { get; set; }

        #endregion

        #region Integração BLING

        public decimal? dPesoLiq { get; set; }
        public decimal? dPesoBruto { get; set; }
        private string _cEanEmb;

        public string cEanEmb
        {
            get { return _cEanEmb; }
            set { _cEanEmb = value; NotifyPropertyChanged(); }
        }

        public string xNomeDet { get; set; }
        public string cCest { get; set; }
        public string cOrigem { get; set; }

        public bool? bExibirCatalogo { get; set; }
        public bool? bDestaqueCatalogo { get; set; }

        [Ignore]
        public int? QtdMaxProdCatalogoPermitido { get; set; }

        public string xDetalhesCatalogo { get; set; }

        public string xTamanhosCatalogo { get; set; }

        public string xInfTecnicasCatalogo { get; set; }

        #endregion


    }
}
