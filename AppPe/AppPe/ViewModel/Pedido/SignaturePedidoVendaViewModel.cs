using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Model.Repository;
using Xamarin.HLP.Mobile.AppPE.Services;

namespace Xamarin.HLP.Mobile.AppPE.ViewModel.Pedido
{
    public class SignaturePedidoVendaViewModel
    {     
        private readonly int idPedidoVenda;
        public SignaturePedidoVendaViewModel(int idPedidoVenda)
        { 
            this.idPedidoVenda = idPedidoVenda;
        }
        public IFileService _fileService => DependencyService.Get<IFileService>();
        public async void SalvarAssinatura(Stream image)
        {
            var _fileName = Guid.NewGuid().ToString();
            var _retornoCaminhoFile = await _fileService.SavePicture(_fileName, image, "PedidoVenda");
            PedidoRepository.SalvarCaminhoImagemAssinaturaPedidoVenda(_retornoCaminhoFile, idPedidoVenda);

            //await Shell.Current.GoToAsync("..");
        }
    }
}
