using SQLite;
using Xamarin.HLP.Mobile.AppPE.Common;

namespace Xamarin.HLP.Mobile.AppPE.Model
{
    [Table(TableMobile.TB_SINCRONIZACAOESTOQUE)]
    public class SincronizacaoInicialEstoque
    {
        [PrimaryKey(), AutoIncrement]
        public int? idSincEstIni { get; set; }

        public bool bSincronizado { get; set; }

        public int idEmpresa { get; set; }
    }
}
