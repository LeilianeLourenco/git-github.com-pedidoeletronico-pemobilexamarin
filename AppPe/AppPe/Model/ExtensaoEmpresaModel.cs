using SQLite;
using Xamarin.HLP.Mobile.AppPE.Common;

namespace Xamarin.HLP.Mobile.AppPE.Model
{

    [Table(TableMobile.TB_EXTENSAO_EMPRESA)]
    public class ExtensaoEmpresaModel
    {
        [PrimaryKey]
        public int idExtensaoEmpresa { get; set; }
        public int idEmpresa { get; set; }
        public bool? bControleEstoque { get; set; }
        public bool? bGeraOrcamento { get; set; }
        public bool? bCampanha { get; set; }
        public bool? bCampanhaIlimitada { get; set; }
        public bool? bTabelaEscalonada { get; set; }
        public bool? bFinanceiro { get; set; }
        public bool? bLimiteCredito { get; set; }
        public bool? bStatusVenda { get; set; }
        public bool? bMetas { get; set; }
        public bool? bComissao { get; set; }
        public bool? bRepraIlimitada { get; set; }
    }
}
