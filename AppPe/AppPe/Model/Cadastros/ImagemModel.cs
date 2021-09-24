using System.Runtime.Serialization;
using SQLite;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Common;

namespace Xamarin.HLP.Mobile.AppPE.Model.Cadastros
{
    [Table(TableMobile.TB_IMAGEM)]
    public class ImagemModel : ModelComum
    {
        [PrimaryKey()]
        public int? idImagem { get; set; }
        [NotNull]
        public int idProduto { get; set; }
        [NotNull]
        public string xFilePath { get; set; }
        [NotNull]
        public bool stPrincipal { get; set; }
        [NotNull]
        public int idEmpresa { get; set; }

    
        private string _base64Image;
        [Ignore]
        public string base64Image
        {
            get { return _base64Image; }
            set { _base64Image = value; }
        }


        private ImageSource _image;
        [Ignore]
        [IgnoreDataMember]
        public ImageSource image
        {
            get { return _image; }
            set { _image = value; NotifyPropertyChanged(); }
        }


        private double _width;
        [Ignore]
        [IgnoreDataMember]
        public double width
        {
            get { return _width; }
            set { _width = value; }
        }




    }
}
