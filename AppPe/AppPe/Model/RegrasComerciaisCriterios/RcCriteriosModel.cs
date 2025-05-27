using SQLite;
using System;
using System.Collections.Generic;
using Xamarin.HLP.Mobile.AppPE.Common;

namespace Xamarin.HLP.Mobile.AppPE.Model.RegrasComerciais
{
    [Table(TableMobile.TB_REGRAS_COMERCIAIS_CRITERIOS)]
    public class RcCriteriosModel : ModelComum
    {   
        [PrimaryKey]
        public long idRegraComercialFaixaCriterio { get; set; }
        public DateTime? dtUltimaAlteracao { get; set; }
        public long idRegraComercialFaixa { get; set; }
        public int nSequenciaCriterio { get; set; }
        public byte stCondicao { get; set; }
        public byte stQualRegra { get; set; }

        [Ignore]
        public List<RcFaixasCriterioVinculoModel> lClientes { get; set; }

        [Ignore]
        public List<RccCategoriaProdutoModel> lCategoriasProduto { get; set; }

        [Ignore]
        public List<RccCondicaoPagamentoModel> lCondicoesPagamento { get; set; }

        [Ignore]
        public List<RccProdutosModel> lProdutos { get; set; }

        [Ignore]
        public List<RccRamoAtividadesModel> lRamosAtividade { get; set; }

        [Ignore]
        public List<RccRepresentadasModel> lRepresentadas { get; set; }

        [Ignore]
        public List<RccTabelaPrecoModel> lTabelasPreco { get; set; }

        [Ignore]
        public List<RccUfModel> lUfs { get; set; }
    }
}
