using System;
using System.IO;
using Foundation;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Xamarin.HLP.Mobile.AppPE.iOS;

[assembly: Xamarin.Forms.Dependency(typeof(Picture_iOS))]
namespace Xamarin.HLP.Mobile.AppPE.iOS
{
    public class Picture_iOS : IPicture
    {
        public async void SavePictureToDisk(string filename, byte[] imageData)
        {
            var imgSrc = ImageSource.FromStream(() => new MemoryStream(imageData));

            var renderer = new StreamImagesourceHandler();
            var photo = await renderer.LoadImageAsync(imgSrc);
            var documentsDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string jpgFilename = System.IO.Path.Combine(documentsDirectory, filename + ".jpg");
            NSData imgData = photo.AsJPEG();
            NSError err = null;
            if (imgData.Save(jpgFilename, false, out err))
            {
                Console.WriteLine("saved as " + jpgFilename);
            }
            else
            {
                Console.WriteLine("NOT saved as " + jpgFilename + " because" + err.LocalizedDescription);
            }

        }
        public bool IsExist(string filename)
        {
            var documentsDirectory = Environment.GetFolderPath
                (Environment.SpecialFolder.Personal);
            var jpgFilename = System.IO.Path.Combine(documentsDirectory, filename + ".jpg");

            return File.Exists(jpgFilename);
        }


        public Forms.ImageSource GetImageFromDisk(string filename)
        {
            var documentsDirectory = Environment.GetFolderPath
                (Environment.SpecialFolder.Personal);
            var jpgFilename = System.IO.Path.Combine(documentsDirectory, filename.ToUpper() + ".jpg");
            return ImageSource.FromFile(jpgFilename);
        }
    }
}
