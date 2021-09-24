using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.HLP.Mobile.AppPE.Common;

namespace Xamarin.HLP.Mobile.AppPE.Model.Cadastros
{
    [Table(TableMobile.tb_clienteramosatividade)]
    public class ClienteRamosAtividade
    {
        [PrimaryKey()]
        public int idClienteRamoAtividade { get; set; }
        [NotNull]
        public int idCliente { get; set; }
        [NotNull]
        public int idRamoAtividade { get; set; }
        public DateTime? dtUltimaAlteracao { get; set; }
        [NotNull]
        public int idEmpresa { get; set; }

    }
}
