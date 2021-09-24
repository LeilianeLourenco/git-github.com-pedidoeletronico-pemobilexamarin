using System;
using SQLite;
using Xamarin.HLP.Mobile.AppPE.Common;

namespace Xamarin.HLP.Mobile.AppPE.Model.Cadastros
{
    [Table(TableMobile.TB_REPRESENTADA)]
    public class RepresentadaModel : ModelComum
    {
        public RepresentadaModel()
        {
            stComissaoIPI = false;
        }

        [PrimaryKey]
        public int? idRepresentada { get; set; }
        public int? idTransportadora { get; set; }
        public string xRazaoSocial { get; set; }
        public string xFantasia { get; set; }
        public string xCnpj { get; set; }
        public string xAnotacoes { get; set; }
        public bool stComissao { get; set; }
        public bool stControleEstoque { get; set; }
        public double? pComissao { get; set; }
        public int idEmpresa { get; set; }
        public string xTelefones { get; set; }
        public bool stComissaoIPI { get; set; }
        public bool stComissaoST { get; set; }
        public bool stAtivo { get; set; }
        public string xEmails { get; set; }
        public DateTime? dtUltimaAlteracao { get; set; }
        public string xOrdemComissao { get; set; }
        public string idAspnetUsersInclusao { get; set; }
        public string xRazaoTransportadora { get; set; }
        
        public bool bControlaEstoqueDisponivel { get; set; }

        public bool bPermiteAtivarRegistro { get; set; }

        /// <summary>
        /// 0 -  NINGUEM
        /// 1 - CLIENTE
        /// 2 - REPRESENTAÇÃO
        /// 3 - AMBOS
        /// </summary>
        public byte stEmailObrigatorioPara { get; set; }


    }
}
