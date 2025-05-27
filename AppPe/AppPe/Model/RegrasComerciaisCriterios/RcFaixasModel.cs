using SQLite;
using System;
using System.Collections.Generic;
using Xamarin.HLP.Mobile.AppPE.Common;

namespace Xamarin.HLP.Mobile.AppPE.Model.RegrasComerciais
{
    [Table(TableMobile.TB_REGRAS_COMERCIAIS_FAIXAS)]
    public class RcFaixasModel : ModelComum
    {
        [PrimaryKey]
        public long idRegraComercialFaixa { get; set; }
        public DateTime? dtUltimaAlteracao { get; set; }
        public long idRegraComercial { get; set; }
        public byte? stTipoPercentual { get; set; }
        public int nSequenciaRegra { get; set; }
        public decimal? nPercentual { get; set; }
        public bool bDeletado { get; set; }

        [Ignore]
        public List<RcCriteriosModel> lCriterios { get; set; }
    }
}
