using SQLite;
using System;
using Xamarin.HLP.Mobile.AppPE.Common;

namespace Xamarin.HLP.Mobile.AppPE.Model.Agenda
{

    [Table(TableMobile.TB_TIPOATIVIDADESCRM)]
    public class TipoAtividadeAgendaModel
    {
        [PrimaryKey]
        public int idTipoAtividade { get; set; }

        public int idEmpresa { get; set; }

        public bool stAtivo { get; set; }

         
        public bool bVerificaLocalizacao { get; set; }

        public DateTime? dtUltimaAlteracao { get; set; }

         
        public string xDescricaoAtividade { get; set; }

          
        public string xCor { get; set; }
         
        public string xIcon { get; set; }
    }
}
