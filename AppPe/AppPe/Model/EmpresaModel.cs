using System;
using System.Runtime.Serialization;
using SQLite;
using Xamarin.HLP.Mobile.AppPE.Common;

namespace Xamarin.HLP.Mobile.AppPE.Model
{
    [Table(TableMobile.TB_EMPRESA)]
    public class EmpresaModel : ModelComum
    {
        [PrimaryKey()]
        public int? idEmpresa { get; set; }

        private string _xRazaoSocial;

        [NotNull]
        public string xRazaoSocial
        {
            get { return (_xRazaoSocial != null ? _xRazaoSocial.ToUpper() : ""); }
            set { _xRazaoSocial = value; }
        }

        [NotNull]
        public string idAspnetUser { get; set; }

        public string imLogoMarca { get; set; }

        public int? nDiasValidadeOrcamento { get; set; }

        [Ignore]
        public int idEmpresa_AspnetUsers { get; set; }

        public string xCaminhoImgCapa { get; set; }

        private string _xFantasia;

        public string xFantasia
        {
            get { return _xFantasia ?? xRazaoSocial; }
            set { _xFantasia = value; NotifyPropertyChanged(); }
        }

        public string xCnpj { get; set; }
        public bool? bExibirAnotacaoEmpresaNoPedido { get; set; }
        public string xAnotacao { get; set; }

        public string xEmails { get; set; }
        public string xTelefones { get; set; }
        public string cNumero { get; set; }
        public string xBairro { get; set; }
        public string xCep { get; set; }
        public string xCidade { get; set; }
        public string xEndereco { get; set; }
        public string xEstado { get; set; }
        public string xComplemento { get; set; }
        public string xOmieAppKey { get; set; }
        public string xBlingApiKey { get; set; }
        public string xOmieAppSecret { get; set; }
        public int? idEcommerceTiny { get; set; }
        public bool bSincronizaAutoPedidos { get; set; }
        public byte? stDataRelatorios { get; set; }

        [Ignore]
        [IgnoreDataMember]
        public string xEnderecoCompleto =>
            $"Rua: {xEndereco ?? "não informado"} - Bairro: {xBairro ?? "não informado"}, Numero:{cNumero ?? "não informado"}";


        [Ignore]
        [IgnoreDataMember]
        public string xCidadeCompleto => $"{xCidade ?? "XXXXX"}/{xEstado ?? "XX"} - Cep:{xCep ?? "XXXX"}";


        /// <summary>
        /// Propriedade apenas para trazer a data do servidor azure.
        /// </summary>
        [Ignore]
        public DateTime dtServer { get; set; }


        public bool stForcaLimiteCreditoCliente { get; set; }
    }
}
