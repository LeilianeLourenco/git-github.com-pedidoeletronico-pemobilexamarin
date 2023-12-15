using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Xamarin.HLP.Mobile.AppPE.Services
{
    public interface IFileService
    {
        Task<ImageSource> GetImage(string fileName);
        string GetImageBase64(string fileName);
        Task<string> SavePicture(string name, Stream data, string location = "temp");
    }
}
