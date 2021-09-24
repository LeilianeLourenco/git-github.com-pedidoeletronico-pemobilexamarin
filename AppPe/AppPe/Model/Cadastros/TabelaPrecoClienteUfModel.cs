using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.HLP.Mobile.AppPE.Common;

namespace Xamarin.HLP.Mobile.AppPE.Model.Cadastros
{
    [Table(TableMobile.tb_tabelapreco_uf_cliente)]
    public class TabelaPrecoClienteUfModel
    {
        [PrimaryKey()]
        public int idTabelaPrecoUfCliente { get; set; }
        [NotNull()]
        public int idTabelaPreco { get; set; }
        public string xUF { get; set; }
        [NotNull()]
        public int idEmpresa { get; set; }
        public DateTime? dtUltimaAlteracao { get; set; }
    }
}
