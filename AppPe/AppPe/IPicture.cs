namespace Xamarin.HLP.Mobile.AppPE
{
    public interface IPicture
    {
        void SavePictureToDisk(string filename, byte[] imageData);

        bool IsExist(string filename);
        Forms.ImageSource GetImageFromDisk(string filename);
    }
}
