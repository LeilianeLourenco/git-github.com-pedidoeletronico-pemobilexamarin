using SQLite;
using Xamarin.HLP.Mobile.AppPE.Common;

namespace Xamarin.HLP.Mobile.AppPE.Model.Cidades
{
    [Table(TableMobile.TB_CIDADES)]
    public class CidadesModel : ModelComum
    {
        [PrimaryKey, AutoIncrement]
        public int idCidade { get; set; }
        public int? codigoIBGE { get; set; }
        public string nome { get; set; }
        public string uf { get; set; }
    }

    public class CidadeIBGE
    {
        public int? id { get; set; }
        public string nome { get; set; }
        public Microrregiao microrregiao { get; set; }
    }

    public class Microrregiao
    {
        public Mesorregiao mesorregiao { get; set; }
    }

    public class Mesorregiao
    {
        public UF UF { get; set; }
    }

    public class UF
    {
        public string sigla { get; set; }
    }
}
