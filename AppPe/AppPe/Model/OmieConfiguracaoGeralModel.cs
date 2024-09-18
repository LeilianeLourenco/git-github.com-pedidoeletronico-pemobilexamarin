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
        public bool bEnviarEmailBoleto { get; set; }    
        public bool bZeraIpiSubirPedido { get; set; }     
        public bool bZeraImpostosSubirPedido { get; set; }     
        public bool bIpiDesconto { get; set; }       
        public bool bOrdenarPorAmbiente { get; set; }
        public bool bManterDtFaturamento { get; set; }
        public bool bReservaProduto { get; set; }
        public bool bCancelaPedParcial { get; set; }        
        public bool bBloqueiaValorVendaProduto { get; set; }
        public string xCodigoProjeto { get; set; }
        public string xTipoDocumento { get; set; }
        public bool bAplicarAdmEmClienteSemVendedor { get; set; }
        public int idEmpresa { get; set; }   
        public bool bBloqueiaGetPedidoVenda { get; set; }
        public bool bBloqueiaAlteracoesPedidoVenda { get; set; }
        public bool? bEscolheEmpresaSincPedido { get; set; }     
        public bool? bEnviarParcelasDoPedido { get; set; }
        public bool bSalvarTituloComDtPrevisao { get; set; }
        public bool bEnviarObsPrivadas { get; set; }
        public bool bEnviarInfoAdicionais { get; set; }
        public bool bEnviarAssinaturaPedido { get; set; }
        public bool bEnviarInfoAdicionaisParaNf { get; set; }
    }
}
