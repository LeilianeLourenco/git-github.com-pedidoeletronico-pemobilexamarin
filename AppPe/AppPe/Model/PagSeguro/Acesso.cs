namespace Xamarin.HLP.Mobile.AppPE.Model.PagSeguro
{
    public class Acesso
    {
        public TipoAcessoPermitido stAcessoPermitido { get; set; }

        /// <summary>
        /// 1	plstarter
        /// 2	plsbus
        /// 3	plbus
        /// 4	plprem
        /// 5	plfree
        /// 6	pldeg
        /// </summary>
        public int idProdutoPlanoAtual { get; set; }
    }

    public enum TipoAcessoPermitido
    {
        ok,
        naopossuiplano,
        vencido,
    }

    public class AcessoModel
    {
        public int idEmpresa { get; set; }
        public string idUsuario { get; set; }
    }
}
