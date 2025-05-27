using SQLite;
using System;
using System.Collections.Generic;
using Xamarin.HLP.Mobile.AppPE.Common;

namespace Xamarin.HLP.Mobile.AppPE.Model.RegrasComerciais
{
    [Table(TableMobile.TB_REGRAS_COMERCIAIS)]
    public class RegrasComerciaisModel : ModelComum
    {
        [PrimaryKey]
        public long idRegraComercial { get; set; }
        public int nSequencia { get; set; }
        public string xNomeRegra { get; set; }
        public int idEmpresa { get; set; }

        [Ignore] // SQLite não suporta relacionamento automático, será tratado manualmente
        public List<RcFaixasModel> lFaixas { get; set; }
    }
}
