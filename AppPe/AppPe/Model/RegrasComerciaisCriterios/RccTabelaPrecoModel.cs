using SQLite;
using System;
using System.Collections.Generic;
using Xamarin.HLP.Mobile.AppPE.Common;

namespace Xamarin.HLP.Mobile.AppPE.Model.RegrasComerciais
{    
    [Table(TableMobile.TB_REGRAS_COMERCIAIS_CRITERIOS_TABELAPRECO)]
    public class RccTabelaPrecoModel : ModelComum
    {                
        [PrimaryKey, AutoIncrement]
        public long idCriterioTabelaPreco { get; set; }

        /// <summary>
        /// data de ultima alteração da regra
        /// </summary>
        public DateTime? dtUltimaAlteracao { get; set; }

        /// <summary>
        /// critério relacionado
        /// </summary>
        public long idCriterio { get; set; }     

        /// <summary>
        /// Relação tb_produto e Regras Comerciais criterios produtos
        /// </summary>
        public int idTabelaPreco { get; set; }      
    }
}

