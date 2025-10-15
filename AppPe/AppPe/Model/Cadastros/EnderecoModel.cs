using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Windows.Input;
using SQLite;
using Xamarin.HLP.Mobile.AppPE.Common;

namespace Xamarin.HLP.Mobile.AppPE.Model.Cadastros
{
    [Table(TableMobile.TB_ENDERECO)]
    public class EnderecoModel : ModelComum
    {
        public EnderecoModel()
        {
            LtipoEnderecoBasicPickerModels.Add(new BasicPickerModel { XId = "CO", Display = "COMERCIAL" });
            LtipoEnderecoBasicPickerModels.Add(new BasicPickerModel { XId = "FA", Display = "FATURAMENTO" });
            LtipoEnderecoBasicPickerModels.Add(new BasicPickerModel { XId = "CB", Display = "COBRANÇA" });
            LtipoEnderecoBasicPickerModels.Add(new BasicPickerModel { XId = "EN", Display = "ENTREGA" });
            LtipoEnderecoBasicPickerModels.Add(new BasicPickerModel { XId = "PA", Display = "PARTICULAR" });
            stEndereco = "CO";
            Guid g;
            g = Guid.NewGuid();
            idGuid = g.ToString();

        }

        [Ignore]
        public string idGuid { get; set; }

        public int? idEndereco { get; set; }

        [PrimaryKey, AutoIncrement]
        public int? idEnderecoOffLine { get; set; }

        private string _stEndereco;
        [NotNull]
        public string stEndereco
        {
            get { return _stEndereco; }
            set
            {
                _stEndereco = value; base.NotifyPropertyChanged();
                var item = LtipoEnderecoBasicPickerModels.FirstOrDefault(c => c.XId == value);
                if (item != null)
                {
                    XTipoEndereco = item.Display;
                }
            }
        }

        private string _xCep;
        public string xCep
        {
            get { return _xCep; }
            set
            {
                _xCep = value; base.NotifyPropertyChanged();
            }
        }
        private string _xEndereco;
        public string xEndereco
        {
            get { return _xEndereco; }
            set
            {
                _xEndereco = value; base.NotifyPropertyChanged(); NotifyPropertyChanged("xDisplay");
            }
        }
        private int? _cNumero;
        public int? cNumero
        {
            get { return _cNumero; }
            set
            {
                _cNumero = value; base.NotifyPropertyChanged();
            }
        }

        private string _xComplemento;
        public string xComplemento
        {
            get { return _xComplemento; }
            set
            {
                _xComplemento = value; base.NotifyPropertyChanged();
            }
        }

        private string _xBairro;
        public string xBairro
        {
            get { return _xBairro; }
            set
            {
                _xBairro = value; base.NotifyPropertyChanged();
            }
        }
        private string _xCidade;
        public string xCidade
        {
            get { return _xCidade; }
            set
            {
                _xCidade = value; base.NotifyPropertyChanged(); NotifyPropertyChanged("xDisplay");
            }
        }

        private string _xEstado;
        public string xEstado
        {
            get { return _xEstado; }
            set
            {
                _xEstado = value; base.NotifyPropertyChanged(); NotifyPropertyChanged("xDisplay");
            }
        }

        public int? idClientes { get; set; }

        public int? idClientesOffLine { get; set; }

        public int? idRepresentada { get; set; }

        public int? idTransportadora { get; set; }
        public int? idEmpresa_aspnetUsers { get; set; }

        private string _xLatitude;

        public string xLatitude
        {
            get { return _xLatitude; }
            set { _xLatitude = value; NotifyPropertyChanged(); }
        }

        private string _xLongitude;
        public string xLongitude
        {
            get { return _xLongitude; }
            set { _xLongitude = value; NotifyPropertyChanged(); }
        }


        public DateTime? dtUltimaAlteracao { get; set; }

        public int idEmpresa { get; set; }

        public DateTime? dtCadastro { get; set; }

        private bool _stPrincipal;

        public bool stPrincipal
        {
            get { return _stPrincipal; }
            set { _stPrincipal = value; NotifyPropertyChanged(); }
        }


        private bool? _bBuscaFeitoDaReceita;

        public bool? bBuscaFeitoDaReceita
        {
            get { return _bBuscaFeitoDaReceita; }
            set { _bBuscaFeitoDaReceita = value; NotifyPropertyChanged(); }
        }
        

        /// <summary>
        /// Propriedade usada para salvar o id do usuario que cadastrou o cliente
        /// </summary>
        public string idAspnetUsers { get; set; }


        #region Ignore
        [IgnoreDataMember]
        [Ignore]
        public ICommand commandRemove { get; set; }

        private bool _bDeleted = false;
        [IgnoreDataMember]
        [Ignore]
        public bool bDeleted
        {
            get { return _bDeleted; }
            set { _bDeleted = value; base.NotifyPropertyChanged(); }
        }

        [Ignore]
        [IgnoreDataMember]
        public string xDisplay => (xEndereco ?? "") + ", " + (xCidade ?? "") + "/" + (xEstado ?? "");

        private List<BasicPickerModel> _lTipoEnderecoBasicPickerModels = new List<BasicPickerModel>();
        [Ignore]
        [IgnoreDataMember]
        public List<BasicPickerModel> LtipoEnderecoBasicPickerModels
        {
            get { return _lTipoEnderecoBasicPickerModels; }
            set { _lTipoEnderecoBasicPickerModels = value; NotifyPropertyChanged(); }
        }

        private string _xTipoEndereco;
        [Ignore]
        [IgnoreDataMember]
        public string XTipoEndereco
        {
            get { return _xTipoEndereco; }
            set { _xTipoEndereco = value; NotifyPropertyChanged(); }
        }


        [IgnoreDataMember]
        private bool _bAplicaMelhoriaBloqueiaEnderecoReceita;

        [IgnoreDataMember]
        public bool bAplicaMelhoriaBloqueiaEnderecoReceita
        {
            get { return _bAplicaMelhoriaBloqueiaEnderecoReceita; }
            set { _bAplicaMelhoriaBloqueiaEnderecoReceita = value; }
        }

        private List<BasicPickerModel> _lEstatdosBasicPickerModels = new List<BasicPickerModel>();
        [Ignore]
        [IgnoreDataMember]
        public List<BasicPickerModel> LEstadosBasicPickerModels
        {
            get { return _lEstatdosBasicPickerModels; }
            set { _lEstatdosBasicPickerModels = value; NotifyPropertyChanged(); }
        }

        private BasicPickerModel _estadoBasicPickerModel;
        [Ignore]
        [IgnoreDataMember]
        public BasicPickerModel EstadoBasicPickerModel
        {
            get { return _estadoBasicPickerModel; }
            set
            {
                _estadoBasicPickerModel = value; NotifyPropertyChanged();
                if (value != null)
                {
                    xEstado = value.XId;
                }
            }
        }

        private BasicPickerModel _cidadeBasicPickerModel;
        [Ignore]
        [IgnoreDataMember]
        public BasicPickerModel CidadeBasicPickerModel
        {
            get { return _cidadeBasicPickerModel; }
            set
            {
                _cidadeBasicPickerModel = value; NotifyPropertyChanged();
                if (value != null)
                {
                    xCidade = value.Display;
                }
            }
        }

        private bool _isSearching = false;
        [Ignore]
        [IgnoreDataMember]
        public bool isSearching
        {
            get { return _isSearching; }
            set { _isSearching = value; NotifyPropertyChanged(); }
        }


        #endregion








    }


    /// <summary>
    /// Modelagem utilizada para pegar as informações no buscacep. atualmente é o site viacep.com.br
    /// </summary>
    public class EnderecoCepModel
    {
        public string cep { get; set; }
        public string logradouro { get; set; }
        public string complemento { get; set; }
        public string bairro { get; set; }
        public string localidade { get; set; }
        public string uf { get; set; }
        public string unidade { get; set; }
        public string ibge { get; set; }
        public string gia { get; set; }


    }
}
