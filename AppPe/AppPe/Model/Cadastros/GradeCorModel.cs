using SQLite;
using Xamarin.HLP.Mobile.AppPE.Common;

namespace Xamarin.HLP.Mobile.AppPE.Model.Cadastros
{
    [Table(TableMobile.TB_GRADECOR)]
    public class GradeCorModel
    {
        [PrimaryKey()]
        public int? idGradeCor { get; set; }
        [NotNull]
        public string xNome { get; set; }
        [NotNull]
        public string xCor { get; set; }
        [NotNull]
        public int idProduto { get; set; }
        [NotNull]
        public bool stAtivo { get; set; }

        public int idEmpresa { get; set; }
        public override string ToString()
        {
            return this.idGradeCor.ToString();
        }
    }
}
