using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace HospitalFrontEnd.Data
{
    public class UtilService
    {
        private static HttpClient _client = new()
        {
            BaseAddress = new Uri("https://localhost:7099/api/Database/")
        };

        public UtilService()
        {
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<List<string>?> GetStatusList()
        {
            HttpResponseMessage response = await _client.GetAsync("GetStatusList");
            if (response.IsSuccessStatusCode)
            {
                var rawJson = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<string>>(rawJson);
            }
            return default;
        }

        public async Task<List<string>?> GetEntryTypeList()
        {
            HttpResponseMessage response = await _client.GetAsync("GetEntryTypeList");
            if (response.IsSuccessStatusCode)
            {
                var rawJson = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<string>>(rawJson);
            }
            return default;
        }
    }
}
