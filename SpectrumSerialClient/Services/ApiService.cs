using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpectrumSerialClient.Services
{
    public class ApiService
    {
        public async Task<string> GetDocument(string address, string baseAddress = "http://192.168.0.10:5120/")
        {
            HttpClient client = new HttpClient();

            var res = await client.GetAsync(baseAddress + address);

            if (res.IsSuccessStatusCode)
            {
                string data = await res.Content.ReadAsStringAsync();

                return data;
            }

            return "error";
        }
    }
}
