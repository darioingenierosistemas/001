using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text;

//using System.Collections.Generic;
//using System.Linq;

//using Android.App;
//using Android.Content;
//using Android.OS;
//using Android.Runtime;
//using Android.Views;
//using Android.Widget;



namespace une_etp.REST
{
    public static class Client
    {

        public async static Task<T> GetRequest<T>(this string url)
        {
            try
            {

                HttpClient client = new HttpClient();
                var response = await client.GetAsync(url);
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(json);

            }
            catch
            {
                return default(T);
            }
        }

        public async static Task<string> GetRequestUSER<T>(this string url)
        {
            try
            {

                HttpClient client = new HttpClient();
                var response = await client.GetAsync(url);
                var json = await response.Content.ReadAsStringAsync();
                string var = json.ToString();
                return var;

            }
            catch
            {
                return default(string);
            }
        }

        public async static Task<T> PostRequest<T>(string url, string data)
        {
            try
            {            
                HttpContent httpContent = new StringContent(data, Encoding.UTF8, "application/json");
                HttpClient client = new HttpClient();
                var response = await client.PostAsync(url, httpContent);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(jsonString);
                }
            }
            catch
            {
                
            }

            return default(T);


        }
    }
}