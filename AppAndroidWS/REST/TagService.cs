using System;
using System.Threading.Tasks;
using Android.Util;


namespace une_etp.REST
{
    public class TagService
    {
        public async Task<string> GetTapsAsync()
        {
            string result = string.Empty;
            try
            {
                RestGeneric restGeneric = new RestGeneric();
                result = await restGeneric.Get<string>("");
            }
            catch (Exception ex)
            {
                Log.Info("Error", ex.Message);
                throw new System.Exception(ex.Message);
            }
            return result;
        }
    }
}