using System;
using SQLite;
using Xamarin.HLP.Mobile.AppPE.Common;

namespace Xamarin.HLP.Mobile.AppPE.Model.Financeiro
{
    [Table(TableMobile.TB_RECEBIMENTOTITULOS_MOVIMENTACOES)]
    public class RecebimentoTitulosMovimentacaoModel : ModelComum
    {
        public RecebimentoTitulosMovimentacaoModel()
        {

        }
        public int idRecebimentoTituloMovimentacaoOffLine { get; set; }
        [PrimaryKey]
        public int idRecebimentoTituloMovimentacao { get; set; }


        //[PrimaryKey, AutoIncrement]
        //public int idRecebimentoTituloMovimentacaoOffLine { get; set; }
        //public int idRecebimentoTituloMovimentacao { get; set; }


        public int nMovimentacao { get; set; }
        private DateTime _dtVencimento;

        public DateTime dtVencimento
        {
            get { return _dtVencimento; }
            set
            {
                _dtVencimento = value;
                idtVencimento = value.ToInt();
            }
        }

        private DateTime _dtRecebimento;

        public DateTime dtRecebimento
        {
            get { return _dtRecebimento; }
            set
            {
                _dtRecebimento = value;
                idtRecebimento = value.ToInt();
            }
        }

        public decimal vReceber { get; set; }
        public decimal vRecebido { get; set; }
        /// <summary>
        /// maximo de 500 caracteres
        /// </summary>
        public string xObservacaoMovimentacao { get; set; }
        public int idEmpresaAspNetUsersInclusao { get; set; }
        public int idEmpresaAspNetusersAlteracao { get; set; }
        public int idRecebimentoTitulo { get; set; }

        public DateTime dtUltimaAlteracao { get; set; }
        public int idEmpresa { get; set; }

        public int idtVencimento { get; set; }
        public int idtRecebimento { get; set; }





    }
}
