using SQLite;
using System;
using System.Collections.Generic;
using Xamarin.HLP.Mobile.AppPE.Common;

namespace Xamarin.HLP.Mobile.AppPE.Model.RegrasComerciais
{
    [Table(TableMobile.TB_REGRAS_COMERCIAIS_FAIXAS_CRITERIO_VINCULO)]
    public class RcFaixasCriterioVinculoModel : ModelComum
    {
        [PrimaryKey]
        public long id { get; set; }
        public DateTime? dtUltimaAlteracao { get; set; }
        public long idRegraComercialFaixaCriterio { get; set; }
        public long idRegraComercialFaixaCriterioVinculo { get; set; }
        public long? idVinculo { get; set; }
        public string xVinculo { get; set; }
    }
}
