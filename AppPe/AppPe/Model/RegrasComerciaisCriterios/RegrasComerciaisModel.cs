using SQLite;
using System;
using System.Collections.Generic;
using Xamarin.HLP.Mobile.AppPE.Common;

namespace Xamarin.HLP.Mobile.AppPE.Model.RegrasComerciais
{
    [Table(TableMobile.TB_REGRAS_COMERCIAIS)]
    public class RegrasComerciaisModel : ModelComum
    {
        [PrimaryKey, AutoIncrement]
        public long idRegraComercial { get; set; }
        /// <summary>
        /// Nome da regra
        /// </summary>
        public string xNomeRegra { get; set; }
        /// <summary>
        /// Data de inicio da regra
        /// </summary>
        public DateTime dtInicioRegra { get; set; }
        /// <summary>
        /// data de final da regra nullable
        /// </summary>
        public DateTime? dtFimRegra { get; set; }

        /// <summary>
        /// ID do último usuário que modificou ou incluiu a regra
        /// </summary>
        public string idAspNetUsersEdicao { get; set; }

        public int nSequencia { get; set; }

        /// <summary>
        /// Boleano se regra está ativa
        /// </summary>
        public bool bAtivo { get; set; }
        /// <summary>
        /// boleano de deleção da regra
        /// </summary>
        public bool bDeletado { get; set; }

        /// <summary>
        /// data de ultima alteração da regra
        /// </summary>
        public DateTime? dtUltimaAlteracao { get; set; }

        public int idEmpresa { get; set; }

        [Ignore] // SQLite não suporta relacionamento automático, será tratado manualmente
        public List<RcFaixasModel> lFaixas { get; set; }
    }
}
