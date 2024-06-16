using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace HospitalFrontEnd.Data
{
    public record DrugDto(
        int Drug_ID,
        string Drug_Name,
        string Drug_Dosage,
        string Drug_AllergyList,
        Boolean Drug_Available
    );

    public record NewDrugRequest(
        string Drug_Name,
        string Drug_Dosage,
        string Drug_AllergyList
        );
    public record UpdateDrugRequest(
            int Drug_ID,
            string Drug_Name,
            string Drug_Dosage,
            string Drug_AllergyList
        );

    public class DrugServiceException : Exception
    {
        public DrugServiceException() { }
        public DrugServiceException(string message) : base(message) { }
    }
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
            var rawContent = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<List<DrugDto>>(rawContent);
            }
            else return null;
        }

        public async Task<DrugDto?> CreateNewDrug(NewDrugRequest request)
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PostAsync("AddDrug", content);
            var rawContent = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<DrugDto>(rawContent);
            }
            else throw new DrugServiceException(rawContent);
        }

        public async Task<DrugDto?> UpdateDrug(UpdateDrugRequest request)
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PutAsync("UpdateDrug", content);
            var rawJson = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<DrugDto>(rawJson);
            }
            else
                throw new DrugServiceException(rawJson);
        }

        public async Task DeleteDrug(int DrugId)
        {
            HttpResponseMessage response = await _client.DeleteAsync("DeleteDrug?drug_id=" + DrugId);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
                throw new DrugServiceException(content);
        }

        public async Task ChangeStatus(Boolean activate, int DrugId)
        {
            String parameter = String.Empty;
            if (activate) parameter = "ReactivateDrug?drug_id=" + DrugId;
            else parameter = "DeactivateDrug?drug_id=" + DrugId;
            HttpResponseMessage response = await _client.PutAsync(parameter, new StringContent(String.Empty));
            if (!response.IsSuccessStatusCode)
                throw new StaffServiceException(await response.Content.ReadAsStringAsync());
        }
    }
}
