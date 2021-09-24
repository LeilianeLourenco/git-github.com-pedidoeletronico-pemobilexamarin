using System;
using SQLite;
using Xamarin.HLP.Mobile.AppPE.Common;

namespace Xamarin.HLP.Mobile.AppPE.Model.Cadastros.Escalonada
{
    [Table(TableMobile.TB_TABELAESCALONADA_REPRESENTANTE)]
    public class TabelaEscalonadaRepresentanteModel
    {
        [PrimaryKey()]
        public int idTabelaEscalonadaRepresentante { get; set; }
        public int idRepresentante { get; set; }
        public string xNomeRepresentante { get; set; }
        public string idAspnetUsersInclusao { get; set; }
        public DateTime? dtUltimaAlteracao { get; set; }
        public int? idEmpresa { get; set; }

        public int? idTabelaEscalonada { get; set; }
    }
}
