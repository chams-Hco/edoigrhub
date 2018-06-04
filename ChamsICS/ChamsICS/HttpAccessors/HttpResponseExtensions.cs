using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ChamsICSWebService.HttpAccessors
{ 
    public static class HttpResponseExtensions
    {
        public async static Task<T> ContentAsTypeAsync<T>(this HttpResponseMessage response)
        {
            var data = await response.Content.ReadAsStringAsync();
            return string.IsNullOrEmpty(data) ?
                            default(T) :
                            JsonConvert.DeserializeObject<T>(data);
        }

        public async static Task<string> ContentAsJsonAsync(this HttpResponseMessage response)
        {
            var data = await response.Content.ReadAsStringAsync();
            return JsonConvert.SerializeObject(data);
        }

        public async static Task<string> ContentAsStringAsync(this HttpResponseMessage response)
        {
            return await response.Content.ReadAsStringAsync();
        }
    }
}
