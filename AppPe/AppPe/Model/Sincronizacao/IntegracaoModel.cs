using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using Xamarin.HLP.Mobile.AppPE.Common;

namespace Xamarin.HLP.Mobile.AppPE.Model.Sincronizacao
{
    [Table(TableMobile.TB_INTEGRACAO)]
    public class IntegracaoModel : ModelComum
    {
        [PrimaryKey, AutoIncrement]
        public int idIntegracao { get; set; }
        public string xTabela { get; set; }
        public string xLogIntegracao { get; set; }
        public DateTime? dtUltimaSincronizacao { get; set; }
        public int idEmpresa { get; set; }
    }
}
