using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Common;

namespace Xamarin.HLP.Mobile.AppPE.Model.Lancamento
{
    public class PedidoVendaListarModel : ModelComum
    {
        #region Listar novo

        public string xDisplayIntegracao { get; set; }

        private int? _idPedidoVenda;
        public int? idPedidoVenda
        {
            get { return _idPedidoVenda; }
            set { _idPedidoVenda = value; NotifyPropertyChanged(); }
        }

        private int _idPedidoVendaOffLine;
        public int idPedidoVendaOffLine
        {
            get { return _idPedidoVendaOffLine; }
            set { _idPedidoVendaOffLine = value; NotifyPropertyChanged(); }
        }

        private bool _estoqueInvalido;
        public bool EstoqueInvalido
        {
            get { return _estoqueInvalido; }
            set { _estoqueInvalido = value; NotifyPropertyChanged(); }
        }

        private double _VTotal;
        public double VTotal
        {
            get
            {
              return _VTotal;
            }
            set { _VTotal = value; NotifyPropertyChanged(); }
        }

        private double _vOutrasPed;

        public double vOutrasPed
        {
            get { return _vOutrasPed; }
            set { _vOutrasPed = value; NotifyPropertyChanged(); }
        }

        private double _vFretePed;

        public double vFretePed
        {
            get { return _vFretePed; }
            set { _vFretePed = value; NotifyPropertyChanged(); }
        }

        private double _vSeguroPed;

        public double vSeguroPed
        {
            get { return _vSeguroPed; }
            set { _vSeguroPed = value; NotifyPropertyChanged(); }
        }

        private double _vSubTotal;

        public double vSubTotal
        {
            get { return _vSubTotal; }
            set { _vSubTotal = value; NotifyPropertyChanged(); }
        }

        private byte _stLancamento;
        /// <summary>
        /// 0 - Orçamento
        /// 1 - Pedido
        /// </summary>
        public byte stLancamento
        {
            get { return _stLancamento; }
            set { _stLancamento = value; NotifyPropertyChanged(); }
        }

        private int? _idPedidoDisplay;
        public int? idPedidoDisplay
        {
            get { return _idPedidoDisplay; }
            set
            {
                _idPedidoDisplay = value;
                if (value == 0)
                    _idPedidoDisplay = null;
                NotifyPropertyChanged();
            }
        }
        public int idStatus { get; set; }

        private string _displayCliente;
        public string DisplayCliente
        {
            get { return (_displayCliente ?? "").ToUpper(); }
            set { _displayCliente = value; NotifyPropertyChanged(); }
        }

        private string _xObsInterna;
        public string xObsInterna
        {
            get { return (_xObsInterna ?? "").ToUpper(); }
            set { _xObsInterna = value; NotifyPropertyChanged(); }
        }

        private int? _idRepresentadaPdf;

        public int? idRepresentadaPdf
        {
            get { return _idRepresentadaPdf; }
            set { _idRepresentadaPdf = value; NotifyPropertyChanged(); }
        }

        /// <summary>
        /// 0 - aberto
        /// 1 - cancelado
        /// 2 - vendido
        /// </summary>
        public byte stPedidoVenda { get; set; }

        private string _displayPrazo;
        public string DisplayPrazo
        {
            get { return (_displayPrazo ?? "").ToUpper(); }
            set { _displayPrazo = value; NotifyPropertyChanged(); }
        }

        private string _DisplayForma;
        public string DisplayForma
        {
            get { return (_DisplayForma ?? "").ToUpper(); }
            set { _DisplayForma = value; NotifyPropertyChanged(); }
        }
        
        private string _displayEmail;
        public string DisplayEmail
        {
            get { return (_displayEmail ?? "").ToLower(); }
            set { _displayEmail = value; NotifyPropertyChanged(); }
        }

        #region Tratamentos
        public bool CanRemove => idPedidoVenda == null;

        private string _XdEmissao;

        public string XdEmissao
        {
            get { return _XdEmissao; }
            set { _XdEmissao = value; NotifyPropertyChanged(); }
        }


        public string PedidoDisplayCompleto => idPedidoDisplay != null ? "#" + idPedidoDisplay.ToString().PadLeft(6, '0') : "#";

        public string xDisplayPedidoNew => (App.tipouser == App.TipoUser.NORMAL) ? PedidoDisplayCompleto : $"#{xDisplayIntegracao ?? ""}";

        public string TipoLancamento => (stLancamento == 0) ? "ORÇAMENTO" : "PEDIDO";

        public string DisplaySincronizacao
        {
            get
            {
                if (App.tipouser == App.TipoUser.OMIE && string.IsNullOrEmpty(xDisplayIntegracao) && idPedidoVenda.GetValueOrDefault() > 0)
                {
                    return "Sincronizado no pedidoeletronico.com";
                } 
                if (EstoqueInvalido)
                {
                    return "Estoque inválido";
                }
                return idPedidoVenda != null ? "Sincronizado com sucesso" : "Não sincronizado";
            }
        }
        #endregion

        private string _xSigla;

        public string xSigla
        {
            get { return _xSigla ?? "--"; }
            set { _xSigla = value; NotifyPropertyChanged(); }
        }


        private string _xCor;

        public string xCor
        {
            get { return _xCor; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    _xCor = "33B5E5";
                }
                else
                {
                    _xCor = value;
                }
            }
        }


        public Color ColorSigla => Color.FromHex((xCor ?? "33B5E5").Replace("#", ""));

        #endregion

        #region Cores
        public Color ColorTipoLancamento => (stLancamento == 0) ? ColorStaticModel.Orcamento : ColorStaticModel.Pedido;

        public Color ColorSincronizacao
        {
            get
            {
                if (EstoqueInvalido)
                {
                    return ColorStaticModel.VermelhoPrincipal;
                }
                if (idPedidoVenda != null)
                {
                    return ColorStaticModel.AzulPrincipal;
                }
                return ColorStaticModel.CinzaClaro;
            }
        }


        private int _iEstoqueInvalido;

        public int iEstoqueInvalido
        {
            get { return _iEstoqueInvalido; }
            set
            {
                _iEstoqueInvalido = value;

                EstoqueInvalido = value > 0;
            }
        }


        public Color ColorStatusPedido
        {
            get
            {
                if (stPedidoVenda == 0)
                {
                    return ColorStaticModel.Aberto;
                }
                if (stPedidoVenda == 2)
                {
                    return ColorStaticModel.Vendido;
                }
                return ColorStaticModel.Cancelado;
            }
        }

        private string _xNomeStatus;

        public string xNomeStatus
        {
            get { return _xNomeStatus; }
            set { _xNomeStatus = value; NotifyPropertyChanged(); }
        }


        public string DisplayStatusPedido
        {
            get
            {
                if (stPedidoVenda == 0)
                {
                    return "A";
                }
                if (stPedidoVenda == 1)
                {
                    return "C";
                }
                return "V";
            }
        }


        #endregion
    }
}
