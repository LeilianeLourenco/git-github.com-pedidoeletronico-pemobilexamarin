using Newtonsoft.Json;

namespace Xamarin.HLP.Mobile.AppPE.Common
{
    public class TypeSerializer
    {

        public static T Clone<T>(T obj) where T : class
        {
            var s = JsonConvert.SerializeObject(obj);
            var r = JsonConvert.DeserializeObject<T>(s);
            return r;
        }

    }
}
