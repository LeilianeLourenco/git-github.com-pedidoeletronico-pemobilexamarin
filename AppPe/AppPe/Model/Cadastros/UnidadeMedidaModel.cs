using SQLite;
using Xamarin.HLP.Mobile.AppPE.Common;

namespace Xamarin.HLP.Mobile.AppPE.Model.Cadastros
{
    [Table(TableMobile.TB_UNIDADEMEDIDA)]
    public class UnidadeMedidaModel
    {
        public int idEmpresa { get; set; }
        [PrimaryKey()]
        public int? idUnidadeMedida { get; set; }
        [NotNull]
        public string xUnidadeMedida { get; set; }
        public string xSigla { get; set; }
        public int nCasasDecimais { get; set; }
        public bool stAtivo { get; set; }
        public string idAspnetUsersInclusao { get; set; }
    }
}
