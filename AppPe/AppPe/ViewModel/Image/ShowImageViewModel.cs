using System.Collections.Generic;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Model.Cadastros;
using Xamarin.HLP.Mobile.AppPE.Model.Repository;


namespace Xamarin.HLP.Mobile.AppPE.ViewModel.Image
{
    public class ShowImageViewModel : ViewModelComum<ImagemModel>
    {

        public Xamarin.HLP.Mobile.AppPE.Controls.custom.ItemsView CompCatagalogo { get; set; }

        private int _idProduto;

        public int idProduto
        {
            get { return _idProduto;  }
            set { _idProduto = value; NotifyPropertyChanged(); }
        }

        private double _width;

        public double width
        {
            get { return _width; }
            set { _width = value; }
        }


        public ShowImageViewModel()
        {
            currentModel = new ImagemModel();
            RegistrosAll = new List<ImagemModel>();

        }

        public bool OnApparing()
        {
            if (canExecuteInicial)
            {
                CompCatagalogo.Clear();
                canExecuteInicial = false;
                if (idProduto != 0)
                {
                    var result = ImagemRepository.GetAllImages(idProduto);

                    foreach (var imagemModel in result)
                    {
                        var filename = imagemModel.xFilePath.Replace("/imgs/", "").Replace(".png", "");
                        if (!App.Picture.IsExist(filename))
                        {
                            UtilHttp.SaveImagem(imagemModel.xFilePath);
                        }
                        imagemModel.image = UtilMethods.GetLocalProdutoImageSource(imagemModel.xFilePath);
                        imagemModel.width = width;
                        RegistrosAll.Add(imagemModel);
                    }
                    CompCatagalogo.NotifyPropertyChangedSource();
                }

            }
            return canExecuteInicial;
        }

    }
}
