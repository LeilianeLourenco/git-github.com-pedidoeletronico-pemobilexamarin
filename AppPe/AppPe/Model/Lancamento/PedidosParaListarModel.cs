using System;

namespace Xamarin.HLP.Mobile.AppPE.Model.Lancamento
{
    public class PedidosParaListarModel
    {
        public int idPedidoVendaOffLine { get; set; }
        public string idAspNetUsersRepresentante { get; set; }
        public string filtro { get; set; }

        public DateTime dEmissao { get; set; }
    }
}
