using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.iOS;

[assembly: Dependency(typeof(Encoded))]
namespace Xamarin.HLP.Mobile.AppPE.iOS
{
    public class Encoded : IEncoded
    {
        public string Encrypt(string plainText)
        {
            var key = "ldcm246964#";
            byte[] EncryptKey = { };
            byte[] IV = { 55, 34, 87, 64, 87, 195, 54, 21 };
            EncryptKey = System.Text.Encoding.UTF8.GetBytes(key.Substring(0, 8));
            var des = new DESCryptoServiceProvider();
            var inputByte = Encoding.UTF8.GetBytes(plainText);
            var mStream = new MemoryStream();
            var cStream = new CryptoStream(mStream, des.CreateEncryptor(EncryptKey, IV), CryptoStreamMode.Write);
            cStream.Write(inputByte, 0, inputByte.Length);
            cStream.FlushFinalBlock();
            return Convert.ToBase64String(mStream.ToArray());
        }
    }
}
