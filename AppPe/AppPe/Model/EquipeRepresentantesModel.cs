using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.HLP.Mobile.AppPE.Common;

namespace Xamarin.HLP.Mobile.AppPE.Model
{
    [Table(TableMobile.TB_EQUIPE_REPRESENTANTES)]
    public class EquipeRepresentantesModel : ModelComum
    {
        [PrimaryKey()]
        public int idEquipeRepresentante { get; set; }
        public int idEmpresa_aspnetusers { get; set; }
        public int idEquipe { get; set; }      
        public int idEmpresa { get; set; }      
    }
}
