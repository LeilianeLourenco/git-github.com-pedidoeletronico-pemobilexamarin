using SQLite;
using System;
using Xamarin.HLP.Mobile.AppPE.Common;

namespace Xamarin.HLP.Mobile.AppPE.Model
{
    [Table(TableMobile.TB_JORNADA_TRABALHO)]
    public class JornadaModel
    {
        [PrimaryKey()]
        public int idJornada { get; set; }
        public bool bDeletado { get; set; }
        public string xNomeJornada { get; set; }
    }

    [Table(TableMobile.TB_JORNADA_TRABALHO_HORARIOS)]
    public class JornadaHorariosModel
    {
        [PrimaryKey()]
        public int idJornadaDia { get; set; }

        public int idJornada { get; set; }

        public TimeSpan tHorarioInicio { get; set; }
        public TimeSpan tHorarioFim { get; set; }

        public byte nDiaSemana { get; set; }
    }

}
