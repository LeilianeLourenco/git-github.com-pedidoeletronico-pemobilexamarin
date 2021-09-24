using SQLite;
using Xamarin.HLP.Mobile.AppPE.Common;

namespace Xamarin.HLP.Mobile.AppPE.Model.Cadastros
{
    [Table(TableMobile.TB_GRADETAMANHO)]
    public class GradeTamanhoModel
    {
        [PrimaryKey()]
        public int? idGradeTamanho { get; set; }
        [NotNull]
        public string xNome { get; set; }
        [NotNull]
        public int idProduto { get; set; }

        [NotNull]
        public bool stAtivo { get; set; }

        public int idEmpresa { get; set; }

    }
}
