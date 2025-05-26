using SQLite;
using System;
using System.Collections.Generic;
using Xamarin.HLP.Mobile.AppPE.Common;

namespace Xamarin.HLP.Mobile.AppPE.Model.RegrasComerciais
{    
    [Table(TableMobile.TB_REGRAS_COMERCIAIS_CRITERIOS_UF)]
    public class RccUfModel : ModelComum
    {
        [PrimaryKey]
        public long idCriterioUF { get; set; }

        /// <summary>
        /// data de alteração da Regras Comerciais Criterios Clientes
        /// </summary>
        public DateTime? dtUltimaAlteracao { get; set; }

        public string xUfCriterio { get; set; }

        /// <summary>
        /// critério relacionado
        /// </summary>
        public long idCriterio { get; set; }       
    }
}
