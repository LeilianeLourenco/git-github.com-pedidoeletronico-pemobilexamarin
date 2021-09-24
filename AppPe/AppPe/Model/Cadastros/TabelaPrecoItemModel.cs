using System;
using SQLite;
using Xamarin.HLP.Mobile.AppPE.Common;

namespace Xamarin.HLP.Mobile.AppPE.Model.Cadastros
{
    [Table(TableMobile.TB_TABELAPRECOITEM)]
    public class TabelaPrecoItemModel
    {
        [PrimaryKey()]
        public int? idTabelaPrecoItem { get; set; }
        public double? pLucro { get; set; }
        public double? vVenda { get; set; }
        [NotNull]
        public int idProduto { get; set; }
        [NotNull]
        public int idTabelaPreco { get; set; }
        [NotNull]
        public int idEmpresa { get; set; }

        public decimal? qProdutoCampanha { get; set; }
        public DateTime? dtUltimaAlteracao { get; set; }
        public decimal? pDescontoMaximo { get; set; }
        public decimal? pComissao { get; set; }
        public decimal pIpiVenda { get; set; }
        public decimal pStVenda { get; set; }
        public decimal vVendaComImpostos { get; set; }
    }
}
