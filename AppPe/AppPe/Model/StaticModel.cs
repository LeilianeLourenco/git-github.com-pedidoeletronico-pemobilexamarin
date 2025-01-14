using System.Collections.Generic;
using Xamarin.HLP.Mobile.AppPE.Model.Cadastros;
using Xamarin.HLP.Mobile.AppPE.Model.Repository;
using Xamarin.HLP.Mobile.AppPE.View.Home;
using Xamarin.HLP.Mobile.AppPE.ViewModel.Pedido;

namespace Xamarin.HLP.Mobile.AppPE.Model
{
    public class StaticModel
    {
        public static void SetNewPageHome()
        {
            _PageHome = new PageHomeNew();
        }

        private static PageHomeNew _PageHome;
        public static PageHomeNew PageHome => _PageHome ?? (_PageHome = new PageHomeNew());


        ///// <summary>
        ///// Current Cliente em edição
        ///// </summary>
        //public static ClientesModel StaticClientesModel { get; set; }

        /// <summary>
        /// Current PRODUTO em edição
        /// </summary>
        public static ProdutoModel StaticProdutoModel { get; set; }

        /// <summary>
        /// Current model da ViewModel Listar Clientes
        /// </summary>
        public static FindGenericModel StaticFindClienteModel { get; set; }

        /// <summary>
        /// Current model da ViewModel Listar Produtos
        /// </summary>
        public static FindGenericModel StaticFindProdutoModel { get; set; }



        /// <summary>
        /// View Model atual do Pedido Venda 
        /// </summary>
        //public static PedidoViewModel StaticPedidoViewModel = new PedidoViewModel();

        public static EditarItemViewModel StaticEditarItemViewModel { get; set; }

        private static List<TabelaPrecoModel> _lTabelasPrecoCampanhas = null;
        public static List<TabelaPrecoModel> lTabelasPrecoCampanhas
        {
            get
            {
                _lTabelasPrecoCampanhas = null;

                return _lTabelasPrecoCampanhas ??
                       (_lTabelasPrecoCampanhas = TabelaPrecoRepository.GetTabelasCampanhas(0, 1,
                           App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa));
            }
            set { _lTabelasPrecoCampanhas = value; }
        }

    }



}
