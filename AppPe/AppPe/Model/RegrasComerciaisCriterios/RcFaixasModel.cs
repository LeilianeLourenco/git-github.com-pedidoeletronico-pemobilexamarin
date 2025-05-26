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
        public long idRegraFaixa { get; set; }
        /// <summary>
        /// boleano de deleção da regra
        /// </summary>
        public bool bDeletado { get; set; }
        /// <summary>
        /// numero sequencia
        /// </summary>
        public int nSequenciaRegra { get; set; }

        /// <summary>
        /// data de ultima alteração da regra
        /// </summary>
        public DateTime? dtUltimaAlteracao { get; set; }

        /// <summary>
        /// utilizado para o acréscimo ou desconto
        /// </summary>  
        public decimal? nPercentual { get; set; }

        /// <summary>
        /// 0 - acrécismo
        /// 1 - desconto
        /// </summary>
        public byte? stTipoPercentual { get; set; }

        /// <summary>
        /// Relação tb_regras_comerciais e faixas
        /// </summary>
        public long idRegraComercial { get; set; }

        [Ignore]
        public List<RegrasComerciaisCriteriosModel> lCriterios { get; set; }
    }
}
