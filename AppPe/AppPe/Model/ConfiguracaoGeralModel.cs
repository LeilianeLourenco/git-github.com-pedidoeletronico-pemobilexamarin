using SQLite;
using System;
using Xamarin.HLP.Mobile.AppPE.Common;

namespace Xamarin.HLP.Mobile.AppPE.Model
{
    [Table(TableMobile.TB_CONFIGURACOES_GERAIS)]
    public class ConfiguracaoGeralModel
    {
        [PrimaryKey()]
        public int? idConfiguracaoGeral { get; set; }
        public int idEmpresa { get; set; }          
        public bool bUtilizaLimiteMinimoVendas { get; set; }
        public bool bForcarMinimoVendas { get; set; }

        /// <summary>
        /// Por qual cadastro será aplicado o limite mínimo de vendas ( 1 - Pedido / 2 - Produto / 3 - Cliente / 4 - Lista Preço / 5 - duplicatas )
        /// </summary>  
        public byte stCadastroLimiteVendasEmpresa { get; set; }

        /// <summary>
        /// Por qual cadastro será aplicado o limite mínimo de vendas ( 1 - Quantidade / 2 - Valor )
        /// </summary>
        public byte stCalculoLimiteVendasEmpresa { get; set; }
        public bool bBloquearVisualizacaoEstoqueVendedor { get; set; }
        public double dValorLimiteMinimo { get; set; }
        public DateTime? dtUltimaAlteracao { get; set; }
        public bool bMostraFaixaTabelaEscalonada { get; set; } 
        public int? idStatusVendaDefault { get; set; }
        public int? idStatusOrcamentoDefault { get; set; } 
        public string xInformacaoContrato { get; set; } 
        public bool? bMostraProdutosVariacoesNaVenda { get; set; }
        public bool bBloqueiaValorProdutoApp { get; set; }
        public bool bNaoAvaliarApp { get; set; }
        public DateTime? dtAvaliouApp { get; set; }
        public decimal dAcrescimoMensal { get; set; }
        public bool bExibirValorPorPeso { get; set; }
        public bool bBloquearPedidosClienteEmAprovacao { get; set; }
    }


    [Table(TableMobile.TB_CONFIGURACOES_ESPECIFICAS)]
    public class ConfiguracaoEspecificaModel
    {
        [PrimaryKey()]
        public int idConfiguracaoEspecifica { get; set; }

        public int idEmpresa { get; set; }

        /// <summary>
        /// issues 1751
        /// </summary>
        public bool bAplicaMelhoriaUnicaUm { get; set; }
        public bool bAplicaMelhoriaRosaMaria { get; set; }
        public bool bAplicaMelhoriaBloqueiaEnderecoReceita { get; set; }
        public bool? bAplicaMelhoriaEscolherRepresentacaoPdf { get; set; }
        

        /// <summary>
        /// issues 1786
        /// </summary>
        public bool bAplicaMelhoriaUnicaDois { get; set; }


        /// <summary>
        /// criado para as empresas em especifico serem redirecionadas para a tela nova de pedido de venda
        /// campo temporário
        /// </summary>
        public bool bUtilizaTelaNova { get; set; }
        public bool? bAplicaMelhoriaTl { get; set; }
        public bool? bAplicaMelhoriaEmailsTelefoneTl { get; set; }
        
    }
    public class ValidacaoConfiguracaoMinimoVendas
    {
        public bool bValidado { get; set; }
        public string xMensagemValidacao { get; set; }
    }
}
