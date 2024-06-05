using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace HospitalFrontEnd.Data
{
    public record DrugDto(
        int Drug_ID,
        string Drug_Name,
        string Drug_Dosage,
        string Drug_AllergyList
    );
    public class DrugService
    {
        private static HttpClient _client = new()
        {
            BaseAddress = new Uri("https://localhost:7099/api/Drug/")

        };

        public DrugService()
        {
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<List<DrugDto>?> GetDrugList()
        {
            HttpResponseMessage response = await _client.GetAsync("GetAllDrugs");
            if (response.IsSuccessStatusCode)
            {
                var rawJson = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<DrugDto>>(rawJson);
            }
            return null;
        }
    }
}
