using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.Serialization;
using SQLite;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Common;

namespace Xamarin.HLP.Mobile.AppPE.Model.Cadastros
{
    [Table(TableMobile.TB_CLIENTES_CONDICOESPAGAMENTO)]
    public class ClientesCondicoesPagamentoModel : ModelComum
    {
        [PrimaryKey(), AutoIncrement()]
        public int idCondicaoCliente { get; set; }
        public int idCondicaoPagamento { get; set; }    
        public int idCliente { get; set; }      
        public bool bPrincipal { get; set; }
        public int idEmpresa { get; set; }
        public DateTime? dtUltimaAlteracao { get; set; }
    }
}
