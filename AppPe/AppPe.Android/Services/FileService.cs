using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Service.QuickSettings;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Droid.Services;
using Xamarin.HLP.Mobile.AppPE.Services;

[assembly: Dependency(typeof(FileService))]
namespace Xamarin.HLP.Mobile.AppPE.Droid.Services
{
    public class FileService : IFileService
    {
        public async Task<string> SavePicture(string name, Stream data, string location = "temp")
        {
            var _internalPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var documentsPath = Path.Combine(_internalPath, "Assinaturas", location);

            var remove = $"/Assinaturas/{location}/";
            if (name.Contains(remove))
                name = name.Replace(remove, "");

            if (!Directory.Exists(documentsPath))
                Directory.CreateDirectory(documentsPath);

            string filePath = Path.Combine(documentsPath, name);

            if (File.Exists(filePath))
                File.Delete(filePath);

            byte[] bArray = new byte[data.Length];
            using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate))
            {
                using (data)
                {
                    await data.ReadAsync(bArray, 0, (int)data.Length);
                }
                int length = bArray.Length;
                await fs.WriteAsync(bArray, 0, length);
            }

            return await Task.FromResult(filePath.Replace(_internalPath, ""));
        }

        public async Task<ImageSource> GetImage(string fileName)
        {
            ImageSource img = null;
            var _internalPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var documentsPath = $"{_internalPath}{fileName}";

            if (File.Exists(documentsPath))
                img = ImageSource.FromFile(documentsPath);

            return await Task.FromResult(img);
        }

        public string GetImageBase64(string fileName)
        {
            string xBase64 = string.Empty;
            var _internalPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var documentsPath = $"{_internalPath}{fileName}";

            if (File.Exists(documentsPath))            
                xBase64 = Convert.ToBase64String(File.ReadAllBytesAsync(documentsPath).Result);
            
            return xBase64;
        }
    }
}