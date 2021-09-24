using System;
using System.Linq;
using System.Threading.Tasks;
using SQLite;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Model.Lancamento;

namespace Xamarin.HLP.Mobile.AppPE.Model
{

    [Table(TableMobile.TB_FORCEATUALIZACAO)]
    public class ForceAtualizacaoModel
    {
        [PrimaryKey]
        public string xId { get; set; }
    }


    /// <summary>
    /// Classe criada para rodar processos necessários após mudanças de banco de dados ou conceitos.
    /// </summary>
    public class AtualizacaoViewModelNecessaria : ViewModelComum<ForceAtualizacaoModel>
    {
        private string _xMessage;

        public string xMessage
        {
            get { return _xMessage; }
            set { _xMessage = value; }
        }

        public AtualizacaoViewModelNecessaria()
        {
            currentModel = new ForceAtualizacaoModel();
        }

        public async Task AtualizacaoNecessariaTask(string xId)
        {
            IsBusy = true;
            xMessage = "Configurando atualizações...";
            try
            {



                if (App.Data.Connection.Table<ForceAtualizacaoModel>().Any(c => c.xId == xId) == false)
                {
                    switch (xId)
                    {
                        case "0001":
                            {

                                AtualizaRepresentantePedido();
                                App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.UltimaSyncDateTime = DateTime.Today.AddYears(-50);
                                App.Data.Connection.Update(App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel);
                                App.Data.Connection.Insert(new ForceAtualizacaoModel { xId = xId });
                            }
                            break;
                        case "0002":
                            {
                                AtualizaRepresentantePedido();
                                App.Data.Connection.Insert(new ForceAtualizacaoModel { xId = xId });
                            }
                            break;
                        default:
                            break;
                    }
                }
                IsBusy = false;
                xMessage = "Análise completa";
            }
            catch (Exception ex)
            {
                ex.TrakException();
                IsBusy = false;
                xMessage = "Sincronize seus dados";
            }
        }



        /// <summary>
        /// 07/07/16 - Método criado para atualizar o campo tb_pedidovenda.idRepresentantePedido com o valor correto
        /// </summary>
        public static void AtualizaRepresentantePedido()
        {
            try
            {

                var pedidos = (from c in App.Data.Connection.Table<PedidoVendaModel>()
                    .Where(
                        c =>
                            c.idRepresentantePedido == null)
                               select new
                               {
                                   c.idPedidoVendaOffLine,
                                   c.idAspNetUsersRepresentante
                               }).ToList();

                if (pedidos.Any())
                {
                    foreach (var pedido in pedidos)
                    {
                        var Email =
                            App.Data.Connection.Table<AspNetUsersModel>()
                                .FirstOrDefault(c => c.Id == pedido.idAspNetUsersRepresentante);
                        if (Email != null)
                        {
                            var user =
                                App.Data.Connection.Table<EmpresaAspnetUsersModel>()
                                    .FirstOrDefault(c => c.xEmail.ToUpper() == Email.Email.ToUpper());

                            if (user != null)
                            {
                                var xQuery = $"Update TB_PEDIDOVENDA set idRepresentantePedido = '{user.idEmpresa_aspnetUsers}' where idPedidoVendaOffLine = {pedido.idPedidoVendaOffLine}";
                                App.Data.Connection.Execute(xQuery);
                            }
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                ex.TrakException();
            }
        }

    }
}
