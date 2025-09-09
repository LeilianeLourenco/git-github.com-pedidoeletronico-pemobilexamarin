using SQLite;
using Xamarin.HLP.Mobile.AppPE.Common;

namespace Xamarin.HLP.Mobile.AppPE.Model
{
    [Table(TableMobile.CurrentUserLogin)]
    public class CurrentUserLoginModel
    {
        [PrimaryKey]
        public string Id { get; set; }

        public string Email { get; set; }

        public int idEmpresa { get; set; }

        /// <summary>
        /// Tipo da pagina de produtos no pedido
        /// 0-SIMPLES
        /// 1-SIMPLES + IMG
        /// 2-COMPLETA
        /// </summary>
        public byte TipoPageProdutos { get; set; } = 1;

        public Enumerations.TipoVisualizacao GeTipoVisualizacao()
        {
            if (TipoPageProdutos == 1)
                TipoPageProdutos = 0;

            if (TipoPageProdutos == 0)
            {
                return Enumerations.TipoVisualizacao.LISTA;
            }
            //if (TipoPageProdutos == 1)
            //{
            //    return Enumerations.TipoVisualizacao.CAROUSEL;
            //}
            if (TipoPageProdutos == 2)
            {
                return Enumerations.TipoVisualizacao.CLEAN;
            }
            return Enumerations.TipoVisualizacao.CLEAN;
        }

        public bool bLogado { get; set; } = true;

        public bool bBloqueado { get; set; }

        public bool bUltimoUserLogado { get; set; } = false;

        public int idEmpresaLogada { get; set; }

        public double vMetaCorrente { get; set; }

    }
}
