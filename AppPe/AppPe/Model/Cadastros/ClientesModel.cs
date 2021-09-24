using System;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using SQLite;
using Xamarin.HLP.Mobile.AppPE.Common;

namespace Xamarin.HLP.Mobile.AppPE.Model.Cadastros
{
    [Table(TableMobile.TB_CLIENTES)]
    public class ClientesModel : ModelComum
    {
        public ClientesModel()
        {
            stJuridico = 1;
            stPeriodoVisitaCliente = 0;
            stProspeccao = "CP";
        }

        [PrimaryKey(), AutoIncrement()]
        public int? idClientesOffLine { get; set; }

        private int? _idClientes;
        public int? idClientes
        {
            get { return _idClientes ?? 0; }
            set
            {
                _idClientes = value;
            }
        }

        private string _idAspnetUsersInclusao;

        private byte _stJuridico;


        /// <summary>
        /// 0- CPF
        /// 1 - CNPJ
        /// </summary>
        [NotNull]
        public byte stJuridico
        {
            get { return _stJuridico; }
            set
            {
                _stJuridico = value; NotifyPropertyChanged();

                if (value == 0)
                {
                    bMostraCPF = true;
                    bMostraCNPJ = false;
                }
                else
                {
                    bMostraCNPJ = true;
                    bMostraCPF = false;
                }
            }

        }

        private bool _bMostraCPF;

        [IgnoreDataMember]
        [Ignore]
        public bool bMostraCPF
        {
            get { return _bMostraCPF; }
            set { _bMostraCPF = value; NotifyPropertyChanged(); }
        }

        private bool _bMostraCNPJ;

        [IgnoreDataMember]
        [Ignore]
        public bool bMostraCNPJ
        {
            get { return _bMostraCNPJ; }
            set { _bMostraCNPJ = value; NotifyPropertyChanged(); }
        }



        private bool _stAtivo = true;
        public bool stAtivo
        {
            get { return _stAtivo; }
            set { _stAtivo = value; }
        }

        private string _cAlternativo;
        public string cAlternativo
        {
            get { return _cAlternativo; }
            set
            {
                _cAlternativo = value; NotifyPropertyChanged();

            }
        }

        private string _xRazaoSocial = "";
        [NotNull]
        public string xRazaoSocial
        {
            get { return _xRazaoSocial; }
            set
            {
                _xRazaoSocial = value; NotifyPropertyChanged();
                xDisplaySemCaracter = value.RemoverAcentos();
            }
        }

        private string _xFantasia = "";
        public string xFantasia
        {
            get { return _xFantasia; }
            set
            {
                _xFantasia = value; NotifyPropertyChanged();
            }
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

        private string _xCpfCnpj;
        public string xCpfCnpj
        {
            get { return _xCpfCnpj; }
            set
            {

                _xCpfCnpj = value; NotifyPropertyChanged();
            }
        }

        public void ValidaCpfCnpj()
        {
            NotifyPropertyChanged("xCpfCnpj");
        }


        private string _xRgIe;
        public string xRgIe
        {
            get { return _xRgIe; }
            set
            {
                _xRgIe = value; NotifyPropertyChanged();
            }
        }


        private string _xUrlInstagram;
        public string xUrlInstagram
        {
            get { return _xUrlInstagram; }
            set
            {
                _xUrlInstagram = value; NotifyPropertyChanged();
            }
        }

        private string _xUrlFacebook;
        public string xUrlFacebook
        {
            get { return _xUrlFacebook; }
            set
            {
                _xUrlFacebook = value; NotifyPropertyChanged();
            }
        }

        private string _xUrlLinkedin;
        public string xUrlLinkedin
        {
            get { return _xUrlLinkedin; }
            set
            {
                _xUrlLinkedin = value; NotifyPropertyChanged();
            }
        }


        private int _idRamoAtividade;
        [NotNull]
        public int idRamoAtividade
        {
            get { return _idRamoAtividade; }
            set
            {
                _idRamoAtividade = value;
                NotifyPropertyChanged();
            }
        }

        private string _stProspeccao = "CE";
        /// <summary>
        /// CP - CE
        /// </summary>
        public string stProspeccao
        {
            get { return _stProspeccao; }
            set
            {
                _stProspeccao = value; NotifyPropertyChanged();
            }
        }

        private int? _qVisitaCliente;
        public int? qVisitaCliente
        {
            get { return _qVisitaCliente; }
            set
            {
                _qVisitaCliente = value; NotifyPropertyChanged();
            }
        }

        private byte? _stPeriodoVisitaCliente;
        public byte? stPeriodoVisitaCliente
        {
            get { return _stPeriodoVisitaCliente; }
            set
            {
                _stPeriodoVisitaCliente = value; NotifyPropertyChanged();
            }
        }

        private int? _idTransportadora;
        public int? idTransportadora
        {
            get { return _idTransportadora; }
            set
            {
                _idTransportadora = value; NotifyPropertyChanged();
            }
        }

        private int? _idCondicaoPagamento;
        public int? idCondicaoPagamento
        {
            get { return _idCondicaoPagamento; }
            set
            {
                _idCondicaoPagamento = value; NotifyPropertyChanged();
            }
        }

        private string _xAnotacao;
        public string xAnotacao
        {
            get { return _xAnotacao; }
            set
            {
                _xAnotacao = value; NotifyPropertyChanged();
            }
        }

        private int? _idTabelaPreco;
        public int? idTabelaPreco
        {
            get { return _idTabelaPreco; }
            set
            {
                _idTabelaPreco = value; NotifyPropertyChanged();
            }
        }

        private DateTime? _dEfetivacao = DateTime.Now.SqlMinDateTime();
        public DateTime? dEfetivacao
        {
            get { return _dEfetivacao; }
            set { _dEfetivacao = value; NotifyPropertyChanged(); }
        }

        private int _idEmpresa;
        public int idEmpresa
        {
            get { return _idEmpresa; }
            set { _idEmpresa = value; NotifyPropertyChanged(); }
        }

        private string _xTelefones;
        public string xTelefones
        {
            get { return _xTelefones; }
            set
            {
                _xTelefones = value; NotifyPropertyChanged();
            }
        }
        private string _xemails;

        public string xEmails
        {
            get { return _xemails; }
            set { _xemails = value; NotifyPropertyChanged(); }
        }

        private double? _vLimiteCredito = 0;
        public double? vLimiteCredito
        {
            get { return _vLimiteCredito; }
            set { _vLimiteCredito = value; NotifyPropertyChanged(); }
        }

        private double? _vLimiteMinimoVendas;

        public double? vLimiteMinimoVendas
        {
            get { return _vLimiteMinimoVendas; }
            set { _vLimiteMinimoVendas = value; NotifyPropertyChanged(); }
        }

        private int? _idEmpresa_aspnetUsers;
        public int? idEmpresa_aspnetUsers
        {
            get { return _idEmpresa_aspnetUsers; }
            set { _idEmpresa_aspnetUsers = value; NotifyPropertyChanged(); }
        }

        public DateTime? dtUltimaAlteracao { get; set; }

        private DateTime _dtCadastro;
        public DateTime dtCadastro
        {
            get { return _dtCadastro; }
            set { _dtCadastro = value; }
        }

        [IgnoreDataMember]
        public bool bProblemaSincronizacao { get; set; }




        /// <summary>
        /// Propriedade usada para salvar o id do usuario que cadastrou o cliente
        /// </summary>
        public string idAspnetUsers { get; set; }

        public string idAspNetUserInclusao { get; set; }

        public byte stAtualizado { get; set; } = 1;


        /* métodos e propriedades que não fazem parte da tabela ( apenas regras. ) */
        private TransportadorasModel _objTransportadoraModel = new TransportadorasModel();
        [Ignore]
        [IgnoreDataMember]
        public TransportadorasModel objTransportadoraModel
        {
            get { return _objTransportadoraModel; }
            set
            {
                _objTransportadoraModel = value; NotifyPropertyChanged();
                if (value != null)
                    idTransportadora = value.idTransportadora;
            }
        }

        private CondicaoPagamentoModel _objCondicaoPagamentoModel = new CondicaoPagamentoModel();
        [Ignore]
        [IgnoreDataMember]
        public CondicaoPagamentoModel objCondicaoPagamentoModel
        {
            get { return _objCondicaoPagamentoModel; }
            set
            {
                _objCondicaoPagamentoModel = value; NotifyPropertyChanged();
                if (value != null)
                    idCondicaoPagamento = value.idCondicaoPagamento;
            }
        }

        private EnderecoModel _objEndereco = new EnderecoModel();
        [Ignore]
        [IgnoreDataMember]
        public EnderecoModel objEndereco
        {
            get { return _objEndereco; }
            set
            {
                _objEndereco = value; NotifyPropertyChanged();
            }
        }

        private ObservableCollection<ContatoModel> _lContato = new ObservableCollection<ContatoModel>();
        [Ignore]
        [IgnoreDataMember]
        public ObservableCollection<ContatoModel> lContato
        {
            get { return _lContato; }
            set { _lContato = value; NotifyPropertyChanged(); }
        }

        private ContatoModel _objContatoModel = new ContatoModel();
        [Ignore]
        [IgnoreDataMember]
        public ContatoModel objContatoModel
        {
            get { return _objContatoModel; }
            set
            {
                _objContatoModel = value;
                NotifyPropertyChanged();
            }
        }

        private ObservableCollection<EnderecoModel> _lEndereco = new ObservableCollection<EnderecoModel>();
        [Ignore]
        public ObservableCollection<EnderecoModel> lEndereco
        {
            get { return _lEndereco; }
            set { _lEndereco = value; NotifyPropertyChanged(); }
        }


        private string _xDisplaySemCaracter;

        public string xDisplaySemCaracter
        {
            get { return _xDisplaySemCaracter; }
            set { _xDisplaySemCaracter = value; NotifyPropertyChanged(); }
        }


        private bool? _bExibirAnotacaoNoPedido;

        public bool? bExibirAnotacaoNoPedido
        {
            get { return _bExibirAnotacaoNoPedido ?? false; }
            set { _bExibirAnotacaoNoPedido = value; NotifyPropertyChanged(); }
        }


        public enum TipoTela
        {
            cadasatro,
            pedido
        }


    }
}
