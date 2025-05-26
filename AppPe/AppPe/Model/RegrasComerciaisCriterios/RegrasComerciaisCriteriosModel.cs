using SQLite;
using System;
using System.Collections.Generic;
using Xamarin.HLP.Mobile.AppPE.Common;

namespace Xamarin.HLP.Mobile.AppPE.Model.RegrasComerciais
{
    [Table(TableMobile.TB_REGRAS_COMERCIAIS_CRITERIOS)]
    public class RegrasComerciaisCriteriosModel : ModelComum
    {
        public RegrasComerciaisCriteriosModel()
        {

        }

        [PrimaryKey]
        public long idCriterio { get; set; }

        /// <summary>
        /// Qual cadastro vai buscar?
        /// 0 - Cliente
        /// 1 - Ramo Atividade Cliente        
        /// 2 - Produto
        /// 3 - Categoria Produto
        /// 4 - UF
        ///  5 - Representada
        ///  6 - condição
        ///  7 - tabela preco
        /// </summary>
        public byte stQualRegra { get; set; }

        /// <summary>
        /// Sittuação de condição regra Comerciais Criterios
        /// </summary>
        public byte stCondicao { get; set; }

        /// <summary>
        /// data de alteração da Regra
        /// </summary>
        public DateTime? dtUltimaAlteracao { get; set; }

        /// <summary>
        /// utilizado para a ordenação do critério
        /// </summary>
        public int nSequenciaCriterio { get; set; }


        /// <summary>
        /// Relação tb_regras_comerciais_faixas e Criterios
        /// </summary>
        public long idRegraFaixa { get; set; }

        [Ignore]
        public List<RccClientesModel> lClientes { get; set; }

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
