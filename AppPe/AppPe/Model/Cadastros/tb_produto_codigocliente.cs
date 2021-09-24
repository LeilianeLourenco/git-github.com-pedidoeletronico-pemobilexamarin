using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace Xamarin.HLP.Mobile.AppPE.Model.Cadastros
{
    public class tb_produto_codigocliente
    {
        [PrimaryKey]
        public int idCodigo { get; set; }
        public int idProduto { get; set; }
        public string xCodigoProdutoCliente { get; set; }
        public int idClientes { get; set; }
        public int idEmpresa { get; set; }
        public DateTime? dtUltimaAlteracao { get; set; }
    }
}
