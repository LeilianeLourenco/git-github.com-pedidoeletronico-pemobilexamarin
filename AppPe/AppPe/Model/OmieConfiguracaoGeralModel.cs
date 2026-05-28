using SQLite;
using System;
using Xamarin.HLP.Mobile.AppPE.Common;

namespace Xamarin.HLP.Mobile.AppPE.Model
{
   
    [Table(TableMobile.TB_OMIE_CONFIGURACOES_GERAIS)]
    public class OmieConfiguracaoGeralModel
    {
        [PrimaryKey()]
        public int idOmieConfigGeral { get; set; }
        public int idEmpresa { get; set; }

        public bool bEnviarEmailBoleto { get; set; }

        /// <summary>
        /// Processo que zera o ipi dos produtos e mantém o valor original para que na omie
        /// seja calculado o ipi novamente e evitar duplicar.
        /// </summary>
        public bool bZeraIpiSubirPedido { get; set; }

        /// <summary>
        /// Processo que zera o st e ipi dos produtos e mantém o valor original para que na
        /// omie seja calculado o st e ipi novamente e evitar duplicar.
        /// </summary>
        public bool bZeraStSubirPedido { get; set; }

        /// <summary>
        /// Mantida por compatibilidade — não usada no código do mobile, mas a coluna SQLite
        /// já foi criada em versões anteriores.
        /// </summary>
        public bool bZeraImpostosSubirPedido { get; set; }

        /// <summary>
        /// Processo que remove o desconto do cálculo do IPI.
        /// </summary>
        public bool bIpiDesconto { get; set; }

        /// <summary>
        /// Processo que envia o pedido com os itens ordenados pelos ambientes.
        /// </summary>
        public bool bOrdenarPorAmbiente { get; set; }

        /// <summary>
        /// Bloqueia atualizar o valor de venda do cadastro do produto na sinc de prod da omie.
        /// </summary>
        public bool bBloqueiaValorVendaProduto { get; set; }

        /// <summary>
        /// Código do projeto que fica atrelado ao pedido de venda (infadicionais.codProj).
        /// </summary>
        public string xCodigoProjeto { get; set; }

        /// <summary>
        /// Seta o documento que quer mandar no tipo de documento da api do pedido.
        /// </summary>
        [MaxLength(50)]
        public string xTipoDocumento { get; set; }

        /// <summary>
        /// Esse campo serve para aplicar o representante adm nos clientes quando vier sem
        /// vendedor na omie.
        /// </summary>
        public bool bAplicarAdmEmClienteSemVendedor { get; set; }

        /// <summary>
        /// Propriedade para bloquear os pedidos da Omie para o PE.
        /// </summary>
        public bool bBloqueiaGetPedidoVenda { get; set; }
        public bool bCancelaPedParcial { get; set; }
        public bool bReservaProduto { get; set; }

        /// <summary>
        /// Propriedade que bloqueia qualquer alteração feita de pedido integrado na omie.
        /// </summary>
        public bool bBloqueiaAlteracoesPedidoVenda { get; set; }

        /// <summary>
        /// Quando true, esse campo é responsável por barrar os pedidos no robo e esses pedidos
        /// só serem sincados quando o cliente entra na tela de listagem de pedidos e escolhe
        /// pra qual empresa ele quer mandar o pedido — serve pra múltiplas empresas.
        /// </summary>
        public bool? bEscolheEmpresaSincPedido { get; set; }

        /// <summary>
        /// Quando true, as parcelas contidas no pedido são enviadas para Omie, sendo assim a
        /// condição de pagamento ser por condição especial.
        /// </summary>
        public bool? bEnviarParcelasDoPedido { get; set; }

        public bool bSalvarTituloComDtPrevisao { get; set; }
        public bool bEnviarObsPrivadas { get; set; }
        public bool bEnviarInfoAdicionais { get; set; }
        public bool bEnviarAssinaturaPedido { get; set; }

        /// <summary>
        /// Quando true, ao atualizar a data de previsão do pedido na Omie, a data de
        /// faturamento não será atualizada aqui no pe.
        /// </summary>
        public bool bManterDtFaturamento { get; set; }

        public bool bEnviarInfoAdicionaisParaNf { get; set; }
        public bool bExibirAnotacaoProduto { get; set; }
        public bool bExibirCampoNotaParaConsumoFinal { get; set; }
        public bool bEnviarQuantidadeVolume { get; set; }
        public bool bAtualizarStatusTabelaPreco { get; set; }

        public bool bIgnoraPrevisaoEstoqueOmie { get; set; }

        /// <summary>
        /// Habilita a funcionalidade de PIX OmieCash para a empresa. Quando true, o app exibe
        /// o toggle "Gerar PIX" em pedidos de 1 parcela e o backend envia a parcela como
        /// adiantamento PIX.
        /// </summary>
        public bool bUtilizaPixOmieCash { get; set; }
    }
}
