using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.HLP.Mobile.AppPE.Common;

namespace Xamarin.HLP.Mobile.AppPE.Model.Cadastros
{
    [Table(TableMobile.tb_tabelapreco_ramoatividade_cliente)]
    public class TabelaPrecoClienteRamoModel
    {
        [PrimaryKey()]
        public int idTabelaPrecoRamoAtividadeCliente { get; set; }
        [NotNull()]
        public int idTabelaPreco { get; set; }
        [NotNull()]
        public int idRamoAtividade { get; set; }
        [NotNull()]
        public int idEmpresa { get; set; }
        public DateTime? dtUltimaAlteracao { get; set; }
    }
}
