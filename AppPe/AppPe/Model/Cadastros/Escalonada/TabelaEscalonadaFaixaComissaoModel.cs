using System;
using SQLite;
using Xamarin.HLP.Mobile.AppPE.Common;

namespace Xamarin.HLP.Mobile.AppPE.Model.Cadastros.Escalonada
{
    [Table(TableMobile.TB_TABELAESCALONADA_FAIXACOMISSAO)]
    public class TabelaEscalonadaFaixaComissaoModel
    {
        [PrimaryKey()]
        public int idTabelaEscalonadaFaixaComissao { get; set; }
        public double pInicioFaixa { get; set; }
        public double pFimFaixa { get; set; }
        public double pComissao { get; set; }
        public string idAspnetUsersInclusao { get; set; }
        public DateTime? dtUltimaAlteracao { get; set; }
        public int? idEmpresa { get; set; }
        public bool bMostraFaixaTabelaEscalonada { get; set; }
        public int? idTabelaEscalonada { get; set; }

    }
}
