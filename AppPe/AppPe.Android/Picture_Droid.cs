using System;
using Android.Content;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Droid;
using File = Java.IO.File;
using Uri = Android.Net.Uri;


[assembly: Xamarin.Forms.Dependency(typeof(Picture_Droid))]
namespace Xamarin.HLP.Mobile.AppPE.Droid
{
    public class Picture_Droid : IPicture
    {
        public void SavePictureToDisk(string filename, byte[] imageData)
        {

            var ResourceDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string name = filename + ".jpg";
            var filePath = System.IO.Path.Combine(ResourceDirectory, name);

            if (System.IO.File.Exists(filePath)) return;
            try
            {
                System.IO.File.WriteAllBytes(filePath, imageData);
                var mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile);
                mediaScanIntent.SetData(Uri.FromFile(new File(filePath)));
                Xamarin.Forms.Forms.Context.SendBroadcast(mediaScanIntent);
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine(e.ToString());
            }
        }

        public bool IsExist(string filename)
        {
            try
            {
                var documentsDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                var jpgFilename = System.IO.Path.Combine(documentsDirectory, filename + ".jpg");
                return System.IO.File.Exists(jpgFilename);
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public ImageSource GetImageFromDisk(string filename)
        {
            try
            {
                ImageSource img = null;
                var documentsDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                var jpgFilename = System.IO.Path.Combine(documentsDirectory, filename.ToUpper() + ".jpg");
                if (System.IO.File.Exists(jpgFilename))
                {
                    img = ImageSource.FromFile(jpgFilename);
                }
                return img;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}