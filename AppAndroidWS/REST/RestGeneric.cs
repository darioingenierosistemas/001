using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;


namespace une_etp.REST
{
   public class RestGeneric
    {

        public async Task<string> Get<T>(string url)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders
                 .Accept
             .Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await client.GetAsync(url);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return await response.Content.ReadAsStringAsync();

                }
            }
            catch (Exception ex)
            {
                throw new System.Exception(ex.Message);

            }


            return default(string);
        }
        public async Task<string> Post<T>(string url, string data)
        {
            try
            {
                HttpContent httpContent = new StringContent(data, Encoding.UTF8, "application/json");
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = client.PostAsync(url, httpContent).Result;
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {
                throw new System.Exception(ex.Message);

            }

            return default(string);
        }
    }
}
