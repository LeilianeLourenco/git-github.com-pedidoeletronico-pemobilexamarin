using SQLite;
using System;
using System.Collections.Generic;
using Xamarin.HLP.Mobile.AppPE.Common;

namespace Xamarin.HLP.Mobile.AppPE.Model.RegrasComerciais
{    
    [Table(TableMobile.TB_REGRAS_COMERCIAIS_CRITERIOS_CLIENTES)]
    public class RccClientesModel : ModelComum
    {
        [PrimaryKey]
        public long idCriterioCliente { get; set; }

        /// <summary>
        /// critério relacionado
        /// </summary>
        public long idCriterio { get; set; }

        /// <summary>
        /// data de alteração da Regras Comerciais Criterios Clientes
        /// </summary>
        public DateTime? dtUltimaAlteracao { get; set; }
        /// <summary>
        /// Data de inicio da regra
        /// </summary>
        public int idClientes { get; set; }
    }
}
