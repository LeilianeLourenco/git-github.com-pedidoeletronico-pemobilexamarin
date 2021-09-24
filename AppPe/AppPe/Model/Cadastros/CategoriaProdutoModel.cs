using SQLite;
using Xamarin.HLP.Mobile.AppPE.Common;

namespace Xamarin.HLP.Mobile.AppPE.Model.Cadastros
{
    [Table(TableMobile.TB_CATEGORIA)]
    public class CategoriaProdutoModel : ModelComum
    {
        [PrimaryKey()]
        public int? idCategoria { get; set; }

        private string _xCategoria = "";
        public string xCategoria
        {
            get { return _xCategoria != null ? _xCategoria.ToUpper() : _xCategoria; }
            set { _xCategoria = value; }
        }

        private string _xRotaCategoria = "";
        [Ignore]
        public string xRotaCategoria
        {
            get { return _xRotaCategoria.ToUpper(); }
            set { _xRotaCategoria = value; }
        }

        private int? _idCategoriaPai;
        public int? idCategoriaPai
        {
            get { return _idCategoriaPai; }
            set { _idCategoriaPai = value == 0 ? null : value; }
        }

        public int idEmpresa { get; set; }

        private bool _stAtivo = true;
        public bool stAtivo
        {
            get { return _stAtivo; }
            set { _stAtivo = value; }
        }

        public int? idRepresentacao { get; set; }

        [Ignore]
        public CategoriaProdutoModel objCategoriaPai { get; set; }

    }
}
