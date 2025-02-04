using System;
using System.Runtime.Serialization;
using SQLite;
using Xamarin.HLP.Mobile.AppPE.Common;

namespace Xamarin.HLP.Mobile.AppPE.Model.Financeiro
{
    [Table(TableMobile.TB_RECEBIMENTOTITULOS_POST)]
    public class RecebimentoTitulosPostModel : ModelComum
    {

        public int idRecebimentoTituloOffLine { get; set; }
        [PrimaryKey, AutoIncrement]
        public int idRecebimentoTitulo { get; set; }


        //[PrimaryKey, AutoIncrement]
        //public int idRecebimentoTituloOffLine { get; set; }
        //public int idRecebimentoTitulo { get; set; }


        public int idEmpresa { get; set; }
        public int nSequencia { get; set; }
        private DateTime _dtEmissao;

        public DateTime dtEmissao
        {
            get { return _dtEmissao; }
            set
            {
                _dtEmissao = value;
                idtEmissao = value.ToInt();
            }
        }

        public int idtEmissao { get; set; }
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

        public int idtVencimento { get; set; }

        public string xDtsVencimento { get; set; }
        public decimal vTitulo { get; set; }
        public string xTitulo { get; set; }
        public decimal vRecebido { get; set; }
        public int idPedidoVenda { get; set; }

        /// <summary>
        /// maximo de 500 caracteres
        /// </summary>
        public string xInformacoesAdicionais { get; set; }
        public DateTime dtBaseComissao { get; set; }
        public decimal vBaseComissao { get; set; }
        public decimal pComissao { get; set; }
        public string idPedidoDisplay { get; set; }
        public int nParcela { get; set; }

        [Ignore]
        [IgnoreDataMember]
        public byte stDuplicata => vRecebido == 0 ? (byte)0 : (vRecebido >= vTitulo ? (byte)1 : (byte)2);

        [Ignore]
        [IgnoreDataMember]
        public string xStatusTitulo => stDuplicata == 0 ? "Em Aberto" : (stDuplicata == 1 ? "Baixado" : "Parcialmente Aberto");

        [IgnoreDataMember]
        [Ignore]
        public string xNumeroTitulo => $"{idPedidoDisplay} / {nSequencia}";
    }
}
