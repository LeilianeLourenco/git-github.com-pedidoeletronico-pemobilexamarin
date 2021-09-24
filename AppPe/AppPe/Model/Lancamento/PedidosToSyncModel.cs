using System;

namespace Xamarin.HLP.Mobile.AppPE.Model.Lancamento
{
    public class PedidosToSyncModel
    {
        public int idPedidoVenda { get; set; }
        private DateTime _dtUltimaAlteracao;

        public DateTime dtUltimaAlteracao
        {
            get { return _dtUltimaAlteracao.ToDateTimeSync(); }
            set { _dtUltimaAlteracao = value; }
        }
        
    }
}
