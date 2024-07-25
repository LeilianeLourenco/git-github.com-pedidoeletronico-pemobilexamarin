using SQLite;
using System;
using Xamarin.HLP.Mobile.AppPE.Common;

namespace Xamarin.HLP.Mobile.AppPE.Model.Cadastros
{
    [Table(TableMobile.TB_ATIVIDADES)]
    public class AtividadeAgendaModel : ModelComum
    {
        [PrimaryKey, AutoIncrement]
        public int idAtividadeOffline { get; set; }


        private int? _idAtividade;
        public int? idAtividade
        {
            get { return _idAtividade; }
            set
            {
                _idAtividade = value; NotifyPropertyChanged();
            }
        }

         
        private bool _bEventoCancelado;
        public bool bEventoCancelado
        {
            get { return _bEventoCancelado; }
            set
            {
                _bEventoCancelado = value; NotifyPropertyChanged();
            }
        }

        private int _idEmpresa;
        public int idEmpresa
        {
            get { return _idEmpresa; }
            set
            {
                _idEmpresa = value; NotifyPropertyChanged();
            }
        }

        private string _xdtInicioEvento;
        public string xDtInicioEvento
        {
            get { return _xdtInicioEvento; }
            set
            {
                _xdtInicioEvento = value; NotifyPropertyChanged();
            }
        }
        
        private string _idAspnetUsers;
        public string idAspnetUsers
        {
            get { return _idAspnetUsers; }
            set
            {
                _idAspnetUsers = value; NotifyPropertyChanged();
            }
        }

        private string _xVendedorVinculado;
        public string xVendedorVinculado
        {
            get { return _xVendedorVinculado; }
            set
            {
                _xVendedorVinculado = value; NotifyPropertyChanged();
            }
        }

        private string _xDescricaoAtividade;
        public string xDescricaoAtividade
        {
            get { return _xDescricaoAtividade; }
            set
            {
                _xDescricaoAtividade = value; NotifyPropertyChanged(); 
            }
        }


        private string _xAnotacaoPrivada;
        public string xAnotacaoPrivada
        {
            get { return _xAnotacaoPrivada; }
            set
            {
                _xAnotacaoPrivada = value; NotifyPropertyChanged();
            }
        }

        private string _xEnderecoCompleto;
        public string xEnderecoCompleto
        {
            get { return _xEnderecoCompleto; }
            set
            {
                _xEnderecoCompleto = value; NotifyPropertyChanged();
            }
        }

        private int? _idCliente;
        public int? idCliente
        {
            get { return _idCliente; }
            set
            {
                _idCliente = value; NotifyPropertyChanged();
            }
        }


        private int? _idClienteOffLine;
        public int? idClienteOffline
        {
            get { return _idClienteOffLine; }
            set
            {
                _idClienteOffLine = value; NotifyPropertyChanged();
            }
        }

        private bool _bRealizado;
        public bool bRealizado
        {
            get { return _bRealizado; }
            set
            {
                _bRealizado = value; NotifyPropertyChanged();
            }
        }

        private bool _bExcluido;
        public bool bExcluido
        {
            get { return _bExcluido; }
            set
            {
                _bExcluido = value; NotifyPropertyChanged();
            }
        }

        private int _idTipoAtividade;
        public int idTipoAtividade
        {
            get { return _idTipoAtividade; }
            set
            {
                _idTipoAtividade = value; NotifyPropertyChanged();
            }
        }

         

        private DateTime? _dtInicioEvento = DateTime.Now.SqlMinDateTime();
        public DateTime? dtInicioEvento
        {
            get { return _dtInicioEvento; }
            set
            {
                _dtInicioEvento = value;
                NotifyPropertyChanged(); 
                xDtInicioEvento = value?.ToLocalTime().ToString("dd/MM/yyyy");
            }
        }


        private DateTime? _dtFimEvento = DateTime.Now.SqlMinDateTime();
        public DateTime? dtFimEvento
        {
            get { return _dtFimEvento; }
            set { _dtFimEvento = value; NotifyPropertyChanged(); }
        }



        private DateTime? _dtEmissaoAtividade = DateTime.Now.SqlMinDateTime();
        public DateTime? dtEmissaoAtividade
        {
            get { return _dtEmissaoAtividade; }
            set { _dtEmissaoAtividade = value; NotifyPropertyChanged(); }
        }



        private DateTime? _dtUltimaAlteracao;
        public DateTime? dtUltimaAlteracao
        {
            get { return _dtUltimaAlteracao; }
            set
            {
                _dtUltimaAlteracao = value != null ? ((DateTime)value).ToDateTimeSync() : (DateTime?)null;
            }
        }

        public DateTime dtTempoCheck { get; set; }
        public string xLocalCheckIn { get; set; }
        public string xLocalCheckOut { get; set; }
    }
}
