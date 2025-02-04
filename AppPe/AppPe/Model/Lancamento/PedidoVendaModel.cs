using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using Xamarin.HLP.Mobile.AppPE.Common;

namespace Xamarin.HLP.Mobile.AppPE.Model.Lancamento
{
    [Table(TableMobile.TB_PEDIDOVENDA)]
    public class PedidoVendaModel : ModelComum
    {
        public PedidoVendaModel()
        {
            dtPrevisto = dEmissao = DateTime.Now.ToUniversalTime();
            dtValidadeOrcamento = DateTime.Now.SqlMinDateTime();
        }

        [PrimaryKey, AutoIncrement]
        public int? idPedidoVendaOffLine { get; set; }

        /// <summary>
        /// PK NUVEM
        /// </summary>
        public int? idPedidoVenda { get; set; }

        public int? idPedidoDisplay { get; set; }

        //OS 35349 - Jessica Barbieri
        public bool bPedidoFechadoIncorretamente;

        //public byte stPedidoVenda { get; set; } = 0;

        private byte _stPedidoVenda = 0;

        public string xMeuID { get; set; }

        /// <summary>
        /// 0 - aberto
        /// 1 - cancelado 
        /// 2 - vendido
        /// </summary>
        public byte stPedidoVenda
        {
            get { return _stPedidoVenda; }
            set { _stPedidoVenda = value; NotificaItens(); }
        }


        //[NotNull]
        public int? idClientes { get; set; }

        private int _idClientesOffLine;

        public int idClientesOffLine
        {
            get { return _idClientesOffLine; }
            set
            {
                _idClientesOffLine = value;
                if (SaveCommand != null)
                    SaveCommand.ChangeCanExecute();
            }
        }

        public string idAspnetUsers { get; set; }

        private string _idAspNetUsersRepresentante;

        [Obsolete]
        public string idAspNetUsersRepresentante
        {
            get { return _idAspNetUsersRepresentante ?? idAspnetUsers; }
            set { _idAspNetUsersRepresentante = value; NotifyPropertyChanged(); }
        }

        private int? _idRepresentantePedido;
        public int? idRepresentantePedido
        {
            get { return _idRepresentantePedido; }
            set { _idRepresentantePedido = value; NotifyPropertyChanged(); }
        }

        public DateTime _dEmissao;
        public DateTime dEmissao
        {
            get { return _dEmissao; }
            set
            {
                _dEmissao = value;

                NotifyPropertyChanged();
                idEmissao = value.ToInt();
                XdEmissao = value.ToString("dd/MM/yyyy");
            }
        }

        private string _xEndereco;

        public string xEndereco
        {
            get { return _xEndereco; }
            set
            {
                _xEndereco = value; NotifyPropertyChanged();               
            }
        }

        public DateTime? _dtFaturamento;
        public DateTime? dtFaturamento
        {
            get { return _dtFaturamento; }
            set
            {
                _dtFaturamento = value;
                NotifyPropertyChanged();
                idFaturamento = value.GetValueOrDefault().ToLocalTime().ToInt();
                XdtFaturamento = value.GetValueOrDefault().ToString("dd/MM/yyyy");
            }
        }

        private DateTime? _dtUltimaAlteracao;

        public DateTime? dtUltimaAlteracao
        {
            get { return _dtUltimaAlteracao; }
            set { _dtUltimaAlteracao = value; NotificaItens(); }
        }

        private int? _idCondicaoPagamento;

        public int? idCondicaoPagamento
        {
            get { return _idCondicaoPagamento; }
            set
            {
                _idCondicaoPagamento = value; NotifyPropertyChanged();
                if (SaveCommand != null)
                    SaveCommand.ChangeCanExecute();
            }
        }

        private int? _idTransportadora;

        public int? idTransportadora
        {
            get { return _idTransportadora; }
            set { _idTransportadora = value; NotifyPropertyChanged(); }
        }


        private int? _idRedespacho;
        public int? idRedespacho
        {
            get { return _idRedespacho; }
            set
            {
                _idRedespacho = value; NotifyPropertyChanged();
            }
        }


        private string _xPedidoCompra;
        public string xPedidoCompra
        {
            get { return _xPedidoCompra; }
            set
            {
                _xPedidoCompra = value; NotifyPropertyChanged();
            }
        }

        [NotNull]
        public int idEmpresa { get; set; }

        private byte _stLancamento = 1;
        /// <summary>
        /// 0 - Orçamento
        /// 1 - Pedido
        /// </summary>
        public byte stLancamento
        {
            get { return _stLancamento; }
            set
            {
                TipoLancamento = (value == 0) ? "ORÇAMENTO" : "PEDIDO";
                _stLancamento = value;
            }
        }


        private string _xObsInterna = String.Empty;
        public string xObsInterna
        {
            get { return _xObsInterna ?? ""; }
            set
            {
                _xObsInterna = value;
                NotifyPropertyChanged();
            }
        }


        private string _xFormaPagamento = String.Empty;
        public string xFormaPagamento
        {
            get { return _xFormaPagamento ?? ""; }
            set
            {
                _xFormaPagamento = value;
                NotifyPropertyChanged();
            }
        }


        private string _xInfAdicional = String.Empty;
        public string xInfAdicional
        {
            get { return _xInfAdicional ?? ""; }
            set
            {
                _xInfAdicional = value;
                NotifyPropertyChanged();
            }
        }

        private bool _stEnviadoRepresentacao;
        public bool stEnviadoRepresentacao
        {
            get { return _stEnviadoRepresentacao; }
            set { _stEnviadoRepresentacao = value; NotifyPropertyChanged(); }
        }

        private bool _stEnviadoCliente;
        public bool stEnviadoCliente
        {
            get { return _stEnviadoCliente; }
            set { _stEnviadoCliente = value; NotifyPropertyChanged(); }
        }

        [IgnoreDataMember]
        public bool bChangedStatus { get; set; }

        private int? _idOrcamentoOrigem;
        public int? idOrcamentoOrigem
        {
            get { return _idOrcamentoOrigem; }
            set { _idOrcamentoOrigem = value; NotifyPropertyChanged(); }
        }

        public int? idOrcamentoOrigemOffLine { get; set; }


        public int? idStatus { get; set; }

        [IgnoreDataMember]
        public int? idStatusOld { get; set; }

        public string xDisplayIntegracao { get; set; }
        public bool bPedidoComAlteracao { get; set; }
        public string xLocalRepresentante { get; set; }

        private int? _idPedidoOrigem;
        public int? idPedidoOrigem
        {
            get { return _idPedidoOrigem; }
            set { _idPedidoOrigem = value; NotifyPropertyChanged(); }
        }

        private string _xMotivoCancelamento = string.Empty;
        public string xMotivoCancelamento
        {
            get { return _xMotivoCancelamento; }
            set { _xMotivoCancelamento = value; NotifyPropertyChanged(); }
        }


        private DateTime? _dtValidadeOrcamento;
        public DateTime? dtValidadeOrcamento
        {
            get { return _dtValidadeOrcamento; }
            set { _dtValidadeOrcamento = value; NotifyPropertyChanged(); }
        }


        public double VTotal { get; set; }

        public bool bControlaEstoque { get; set; }

        public string xCodigoCategoria { get; set; }

        public int? nCodigoContaCorrente { get; set; }
        public string xInfoAdicionaisParaNF { get; set; }

        public double vTotalProduto { get; set; }

        private DateTime? _dtPrevisto;
        public DateTime? dtPrevisto
        {
            get { return _dtPrevisto; }
            set { _dtPrevisto = value; NotifyPropertyChanged(); }
        }

        public string stFretePed { get; set; }
        public byte? stVindoDeIntegracao { get; set; }



        private double _vDescontoPed;
        /// <summary>
        /// Utilizado
        /// </summary>
        public double vDescontoPed
        {
            get { return _vDescontoPed; }
            set { _vDescontoPed = value; NotifyPropertyChanged(); }
        }


        private double _vFretePed;
        /// <summary>
        /// Utilizado
        /// </summary>
        public double vFretePed
        {
            get { return _vFretePed; }
            set { _vFretePed = value; NotifyPropertyChanged(); }
        }



        private double _vOutrasPed;
        /// <summary>
        /// Utilizado
        /// </summary>
        public double vOutrasPed
        {
            get { return _vOutrasPed; }
            set { _vOutrasPed = value; NotifyPropertyChanged(); }
        }


        private double _vSeguroPed;
        /// <summary>
        /// Utilizado
        /// </summary>
        public double vSeguroPed
        {
            get { return _vSeguroPed; }
            set { _vSeguroPed = value; NotifyPropertyChanged(); }
        }

        private byte _stCalculoMinimoVenda;

        public byte stCalculoMinimoVenda
        {
            get { return _stCalculoMinimoVenda; }
            set { _stCalculoMinimoVenda = value; NotifyPropertyChanged(); }
        }

        private byte _stCadastroMinimoVenda;

        public byte stCadastroMinimoVenda
        {
            get { return _stCadastroMinimoVenda; }
            set { _stCadastroMinimoVenda = value; NotifyPropertyChanged(); }
        }

        private bool _bForcarMinimoVendas = false;

        public bool bForcarMinimoVendas
        {
            get { return _bForcarMinimoVendas; }
            set { _bForcarMinimoVendas = value; NotifyPropertyChanged(); }
        }

        private bool _bBloquearVisualizacaoEstoqueVendedor = false;

        public bool bBloquearVisualizacaoEstoqueVendedor
        {
            get { return _bBloquearVisualizacaoEstoqueVendedor; }
            set { _bBloquearVisualizacaoEstoqueVendedor = value; NotifyPropertyChanged(); }
        }


        private bool _bMostraFaixaEscalonada = false;

        public bool bMostraFaixaEscalonada
        {
            get { return _bMostraFaixaEscalonada; }
            set { _bMostraFaixaEscalonada = value; NotifyPropertyChanged(); }
        }

        private bool _bAplicaMelhoriaEscolherRepresentacaoPdf = false;

        public bool bAplicaMelhoriaEscolherRepresentacaoPdf
        {
            get { return _bAplicaMelhoriaEscolherRepresentacaoPdf; }
            set { _bAplicaMelhoriaEscolherRepresentacaoPdf = value; NotifyPropertyChanged(); }
        }

        private int? _idRepresentadaPdf;

        public int? idRepresentadaPdf
        {
            get { return _idRepresentadaPdf; }
            set { _idRepresentadaPdf = value; NotifyPropertyChanged(); }
        }


        private double _vLimiteMinimoVenda;

        public double vLimiteMinimoVenda
        {
            get { return _vLimiteMinimoVenda; }
            set { _vLimiteMinimoVenda = value; NotifyPropertyChanged(); }
        }

        private double _vLimiteMinimoVendaCliente;

        public double vLimiteMinimoVendaCliente
        {
            get { return _vLimiteMinimoVendaCliente; }
            set { _vLimiteMinimoVendaCliente = value; NotifyPropertyChanged(); }
        }

        private bool _bUsaMinimoVendas = false;

        public bool bUsaMinimoVendas
        {
            get { return _bUsaMinimoVendas; }
            set { _bUsaMinimoVendas = value; NotifyPropertyChanged(); }
        }

        private string _xMinimoVendas;

        public string xMinimoVendas
        {
            get { return _xMinimoVendas; }
            set { _xMinimoVendas = value; NotifyPropertyChanged(); }
        }


        #region Contrato


        private string _xInformacaoContrato;

        public string xInformacaoContrato
        {
            get { return _xInformacaoContrato; }
            set { _xInformacaoContrato = value; NotifyPropertyChanged(); }
        }



        #endregion


        public int? idContato { get; set; }
        public string xNomeContato { get; set; }


        private bool _bAguardandoAprovacao = false;

        public bool bAguardandoAprovacao
        {
            get { return _bAguardandoAprovacao; }
            set { _bAguardandoAprovacao = value; NotifyPropertyChanged(); }
        }

        private bool _bValidadoMinimoVendas = true;

        public bool bValidadoMinimoVendas
        {
            get { return _bValidadoMinimoVendas; }
            set { _bValidadoMinimoVendas = value; NotifyPropertyChanged(); }
        }

        public string xAssinatura { get; set; }

        #region Propriedades que não fazem parte da base de dados

        [Ignore]
        public string xAssinaturaBase64 { get; set; }

        [IgnoreDataMember]
        public string XdEmissao { get; set; } = "";


        [IgnoreDataMember]
        public string XdtFaturamento { get; set; } = "";



        [IgnoreDataMember]
        public int idEmissao { get; set; }



        [IgnoreDataMember]
        public int idFaturamento { get; set; }

        private bool _stValidaEnvioParaRepresentada = false;
        [IgnoreDataMember]
        public bool stValidaEnvioParaRepresentada
        {
            get { return _stValidaEnvioParaRepresentada; }
            set { _stValidaEnvioParaRepresentada = value; NotifyPropertyChanged(); }
        }

        private ObservableCollection<PedidoVendaItensModel> _lItens = new ObservableCollection<PedidoVendaItensModel>();
        [Ignore]
        public ObservableCollection<PedidoVendaItensModel> lItens
        {
            get
            {
                return _lItens;
            }
            set
            {
                _lItens = value;
                NotifyPropertyChanged();
            }
        }

        [Ignore]
        [IgnoreDataMember]
        public List<PedidoVendaItensModel> ItensRemovidos { get; set; } = new List<PedidoVendaItensModel>();

        private PedidoVendaItensModel _currentItemModel;
        [IgnoreDataMember]
        [Ignore]
        public PedidoVendaItensModel CurrentItemModel
        {
            get { return _currentItemModel; }
            set { _currentItemModel = value; NotifyPropertyChanged(); }
        }

        private bool _EstoqueInvalido = false;
        [IgnoreDataMember]
        [Ignore]
        public bool EstoqueInvalido
        {
            get { return _EstoqueInvalido; }
            set { _EstoqueInvalido = value; NotifyPropertyChanged(); }
        }

        private string _TipoLancamento = "Pedido";
        //propriedade salva na base local somente para pesquisa
        [IgnoreDataMember]
        [Ignore]
        public string TipoLancamento
        {
            get { return _TipoLancamento; }
            set { _TipoLancamento = value; NotifyPropertyChanged(); }
        }


        public DateTime? _dtInicial;
        public DateTime? dtInicial
        {
            get { return _dtInicial; }
            set
            {
                _dtInicial = value;
                NotifyPropertyChanged();
            }
        }

        public DateTime? _dtFinal;
        public DateTime? dtFinal
        {
            get { return _dtFinal; }
            set
            {
                _dtFinal = value;
                NotifyPropertyChanged();
            }
        }

        public int? _nParcelas;
        public int? nParcelas
        {
            get { return _nParcelas; }
            set
            {
                _nParcelas = value;
                NotifyPropertyChanged();
            }
        }

        public int GetNextValidAgrupamento()
        {
            try
            {
                if (lItens.Any())
                {
                    var max = lItens.Max(c => c.idItemAgrupamento ?? 0);
                    return max + 1;
                }
                return 1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void NotificaItens()
        {
            NotifyPropertyChanged("lItens");
        }
    }
}

#endregion

