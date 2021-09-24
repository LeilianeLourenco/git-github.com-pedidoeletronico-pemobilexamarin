using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SQLite;
using Xamarin.HLP.Mobile.AppPE.Common;

namespace Xamarin.HLP.Mobile.AppPE.Model.Cadastros.Escalonada
{
    [Table(TableMobile.TB_TABELAESCALONADA)]
    public class TabelaEscalonadaModel
    {
        [PrimaryKey()]
        public int idTabelaPrecoEscalonada { get; set; }

        public string xNomeTabela { get; set; }

        public int? idTabelaPrecoVinculo { get; set; }

        public int idEmpresa { get; set; }

        public string idAspnetUsersInclusao { get; set; }
        public DateTime? dtUltimaAlteracao { get; set; }

        public bool stExibeCampanhas { get; set; }

        public bool stAtivo { get; set; }


        [Ignore]
        [IgnoreDataMember]
        public List<TabelaEscalonadaFaixaComissaoModel> lFaixaComissao { get; set; }
    }
}
