using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using Xamarin.HLP.Mobile.AppPE.Common;

namespace Xamarin.HLP.Mobile.AppPE.Model.Empresa
{

    [Table(TableMobile.TB_EMPRESA_ASPNETUSERS_METAS)]
    public class EmpresaAspnetUsers_Meta
    {
        [PrimaryKey]
        public int idEmpresaAspnetUsers_Meta { get; set; }

        public DateTime dtInicioMeta { get; set; }

        public decimal vMeta { get; set; }

    }
}
