using System;
using System.Net.Http;
using System.Threading.Tasks;
using static ProjectCore._Common;

namespace ProjectCore
{
    class _Client
    {
        public async Task<string> SendRequest()
        {
            HttpClient client = new HttpClient();
            //client.Timeout = new TimeSpan(0, 0, 6);
            RequestPacket request = new RequestPacket() { Code = REQUEST_CODES.REGISTER_CLIENT };
            string data = _Common.ConvertData(request);
            StringContent content = new StringContent(data);
            var response = await client.PostAsync("http://localhost:8080/",content);
            var str = await response.Content.ReadAsStringAsync();
            return str;
        }
    }
}
