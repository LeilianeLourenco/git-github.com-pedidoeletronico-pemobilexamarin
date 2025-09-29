using System;
using System.Text;
using System.Threading.Tasks;
using Plugin.Permissions;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Model;
using Xamarin.HLP.Mobile.AppPE.View.MainPage;
using ZXing.Net.Mobile.Forms;
using Xamarin.HLP.Mobile.AppPE.Core.Criptografia.Interfaces;
using Xamarin.Essentials;

namespace Xamarin.HLP.Mobile.AppPE.Common
{
    public class UtilMethods
    {
        public static TimeSpan GetStartTime
            => (Device.RuntimePlatform == Device.Android) ? new TimeSpan(0, 0, 0, 0, 500) : new TimeSpan(0, 0, 0, 0, 50)
            ;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xNameImage">CAMINHO DA IMAGEM</param>
        /// <param name="tipo">P - PRODUTO | E - EMPRESA | U - User</param>
        /// <returns></returns>
        public static ImageSource GetLocalProdutoImageSource(string xNameImage, string tipo = "P")
        {
            if (string.IsNullOrEmpty(xNameImage) || xNameImage.Contains(@"/media/produto/") ||
                xNameImage.ToUpper().Contains("APPLICATIONDEFAULTIMAGE") ||
                xNameImage.ToUpper().Contains("/IMG-REPRESENTANTE-PADRAO"))
                return "ProdutoDefaultImage".ToImagemJPG();

            xNameImage = xNameImage.Replace("/produtos/img/", "").Replace("/imgs/", "").Replace(".png", "").Replace(".jpg", "");
            Forms.ImageSource image = null;
            if (!xNameImage.ToUpper().Equals("PRODUTODEFAULT"))
                image = App.Picture.GetImageFromDisk(xNameImage);

            if (image == null && tipo == "P")
                image = "ProdutoDefaultImage".ToImagemJPG();
            if (image == null && tipo == "E")
                image = "LogoHome".ToImagemPNG();
            if (image == null && tipo == "U")
                image = "UserDefaultImage".ToImagemPNG();

            return image;
        }



        public enum TipoImagem
        {
            Produto,
            CapaEmpresa,
            LogoEmpresa,
            Usuario
        }

        public static ImageSource GetLocalImage(string xPath, TipoImagem tipo = TipoImagem.Produto)
        {
            var xNameImage = "";
            xPath = xPath ?? "";
            if (tipo == TipoImagem.CapaEmpresa)
            {
                if (xPath.Contains("/Content/imgs/padroes/") || xPath.Contains("/Content/imagens/def/"))
                    xPath = "PEDEFAULTCOVERCOMBORDA";
                xNameImage = xPath.PathToNameImage();
                return ValidaImage(xPath, xNameImage, "PEDEFAULTCOVERCOMBORDA");
            }
            if (tipo == TipoImagem.LogoEmpresa)
            {
                if (xPath.Contains("/Content/imgs/padroes/"))
                    xPath = "PEDEFAULT";
                xNameImage = xPath.PathToNameImage();
                return ValidaImage(xPath, xNameImage, "PEDEFAULT");
            }
            if (tipo == TipoImagem.Produto)
            {
                if (xPath.Contains("/media/produto/Default"))
                    xPath = "ProdutoDefaultImage";
                xNameImage = xPath.PathToNameImage();
                return ValidaImage(xPath, xNameImage, "ProdutoDefaultImage");
            }
            if (tipo == TipoImagem.Usuario)
            {
                if (xPath.Contains("/Content/Images/Representante") || xPath.Contains("/Content/imgs/padroes/"))
                    xPath = "UserDefaultImage";
                xNameImage = xPath.PathToNameImage();
                return ValidaImage(xPath, xNameImage, "UserDefaultImage");
            }
            return "PEDEFAULTCOVERCOMBORDA".ToImagem();
        }


        private static ImageSource ValidaImage(string xPath, string xNameImage, string xDefault)
        {
            if (string.IsNullOrEmpty(xNameImage))
            {
                xNameImage = xDefault;
            }

            if (string.IsNullOrEmpty(xPath))
            {
                xPath = xDefault;
            }
            var bDefault = true;
            ImageSource image = null;
            if (!xNameImage.ToUpper().Equals(xDefault.ToUpper()))
            {
                if (!ImageExist(xPath))
                {
                    UtilHttp.SaveImagem(xPath);
                }
                bDefault = false;
            }
            if (!bDefault)
                image = App.Picture.GetImageFromDisk(xNameImage);
            return image ?? (image = xDefault.ToImagemPNG());
        }



        public static bool ImageExist(string xFile)
        {
            try
            {
                xFile = xFile.Replace("/imgs/", "").Replace(".png", "");
                return App.Picture.IsExist(xFile);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static string EncodeRoute(string actionName, string controllerName, string routeValues)
        {
            ICriptografiaUrlCore _cripCore;

            var queryString = routeValues;
            var ancor = new StringBuilder();
            if (controllerName != string.Empty)
            {
                ancor.Append(controllerName);
            }
            if (actionName != "Index")
            {
                ancor.Append("/" + actionName);
            }
            if (queryString != string.Empty)
            {
                ancor.Append("?q=" + App.Encoded.Encrypt(queryString));
            }
            return App.UrlReport + ancor;
        }

        public static ZXingScannerPage scanPage;

        public static async Task<PermissionStatus> PermissionLoc()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
            if (status != PermissionStatus.Granted)
                status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();

            return status;
        }

        public static async Task<bool> PermissionCamera()
        {
            var cameraPermission = new CameraPermission(CrossPermissions.Current);
            var bpermissao = await cameraPermission.RequestCameraPermissionIfNeeded();
            return bpermissao;
        }

        public static async Task<string> ReadBarCode()
        {
            string xRetorno = null;
            try
            {
                scanPage = new ZXingScannerPage();
                scanPage.AutoFocus();
                scanPage.OnScanResult += (result) =>
                    Device.BeginInvokeOnMainThread(() =>
                        {
                            xRetorno = result.Text;
                        }
                       );

                var main = Application.Current.MainPage as RootPage;
                if (main != null)
                {
                    await Task.Yield();
                    await main.Detail.Navigation.PushAsync(scanPage);
                }

                return xRetorno;
            }
            catch (Exception)
            {
                return xRetorno;
            }


        }


    }
}
