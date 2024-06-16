using System.Net.Http.Headers;
using System.Text;
using HospitalFrontEnd.Components;
using Newtonsoft.Json;


namespace HospitalFrontEnd.Data
{
    public record TreatmentPlanDto(
            int Plan_ID,
            PatientDto patient,
            StaffDto staff,
            int Plan_Cycle,
            string Plan_Status,
            DateOnly Plan_Date,
            string Plan_Observation,
            List<TreatmentEntryDto> Plan_entries
        );

    public record TreatmentEntryDto(
            int Entry_ID,
            int Plan_ID,
            StaffDto Staff,
            DateOnly Last_Update,
            string Entry_Type,
            string Comment,
            DrugDto? Drug,
            string? Posology,
            string? Upload_Link
        );


    public record NewPlanRequest(
            int Patient_ID,
            int Staff_ID,
            int Plan_Cycle,
            string Plan_Status,
            DateOnly Plan_Date,
            string Plan_Observation
        );

    public record NewEntryRequest(
            int Plan_ID,
            int Staff_ID,
            DateOnly Last_Update,
            string Entry_Type,
            string Comment,
            int? Drug_ID,
            string? Posology,
            string? Upload_Link
        );

    public record UpdatePlanRequest(
            int Plan_ID,
            int Patient_ID,
            int Staff_ID,
            int Plan_Cycle,
            string Plan_Status,
            DateOnly Plan_Date,
            string Plan_Observation
        );

    public record UpdateEntryRequest(
            int Entry_ID,
            int Plan_ID,
            int Staff_ID,
            DateOnly Last_Update,
            string Entry_Type,
            string Comment,
            int? Drug_ID,
            string? Posology,
            string? Upload_Link
        );

    public class TreatmentPlanServiceException : Exception
    {
        public TreatmentPlanServiceException() { }
        public TreatmentPlanServiceException(string message) : base( message ) { }
    }

    public class TreatmentPlanService
    {

        private static HttpClient _client = new()
        {
            BaseAddress = new Uri("https://localhost:7099/api/TreatmentPlan/")

        };

        public TreatmentPlanService()
        {
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<TreatmentPlanDto?> GetTreatmentPlanById(int TreatmentPlanId)  
        {
            string paramStr = "GetTreatmentPlanById?plan_id=" + TreatmentPlanId; 
            HttpResponseMessage response = await _client.GetAsync(paramStr);
            var rawJson = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<TreatmentPlanDto>(rawJson);
            }
            else throw new TreatmentPlanServiceException(rawJson);
        }

        public async Task<IEnumerable<TreatmentEntryDto>?> GetEntriesForPlan(int TreatmentPlanId)
        {
            string paramStr = "GetAllEntriesForPlan?plan_id=" + TreatmentPlanId;
            HttpResponseMessage response = await _client.GetAsync(paramStr);
            var rawJson = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<IEnumerable<TreatmentEntryDto>>(rawJson);
            }
            else throw new TreatmentPlanServiceException(rawJson); 
        }

        public async Task<IEnumerable<TreatmentPlanDto>?> GetAllTreatmentPlansForPatient(int PatientId)  
        {
            string paramStr = "GetAllTreatmentPlansForPatient?patient_id=" + PatientId;
            HttpResponseMessage response = await _client.GetAsync(paramStr);
            var rawJson = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                
                return JsonConvert.DeserializeObject<IEnumerable<TreatmentPlanDto>>(rawJson);
            }
            else throw new TreatmentPlanServiceException(rawJson);
        }

        public async Task<IEnumerable<TreatmentPlanDto>?> SearchTreatmentPlan(SearchTreatmentPlanForm searchFormDto)
        {
            List<string> paramList = new List<string>();
            if (searchFormDto.Patient_Id > 0)
            {
                paramList.Add( "patient_id=" + searchFormDto.Patient_Id);
            }
            if (searchFormDto.Staff_Id > 0)
            {
                paramList.Add("staff_id=" + searchFormDto.Patient_Id);
            }
            if (searchFormDto.Plan_Id > 0)
            {
                paramList.Add("plan_id=" + searchFormDto.Patient_Id);
            }
            string queryStr = "SearchTreatmentPlan?" + string.Join('&', paramList);
            HttpResponseMessage response = await _client.GetAsync(queryStr);
            if (response.IsSuccessStatusCode)
            {
                var rawJson = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<TreatmentPlanDto>>(rawJson);
            }
            return null;
        }

        public async Task<TreatmentPlanDto?> CreateNewTreatmentPlan(NewPlanRequest createTreatmentPlanRequest)
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(createTreatmentPlanRequest), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PostAsync("CreateNewTreatmentPlan", content);
            var rawJson = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                
                TreatmentPlanDto? newlyCreatedPlan = JsonConvert.DeserializeObject<TreatmentPlanDto>(rawJson);
                return newlyCreatedPlan;
            }
            else
            {
                throw new TreatmentPlanServiceException(rawJson);
            }
        }

        public async Task<TreatmentEntryDto?> AddPlanEntry(NewEntryRequest newEntryRequest)
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(newEntryRequest), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PostAsync("AddTreatmentPlanEntry", content);
            var rawJson = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                TreatmentEntryDto? NewlyCreatedEntry = JsonConvert.DeserializeObject<TreatmentEntryDto>(rawJson);
                return NewlyCreatedEntry;
            }
            else
            {
                throw new TreatmentPlanServiceException(rawJson);
            }
        }

        public async Task<TreatmentEntryDto?> UpdatePlanEntry(UpdateEntryRequest updateRequest)
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(updateRequest), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PutAsync("UpdateTreatmentPlanEntry", content);
            var rawJson = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                TreatmentEntryDto? NewlyUpdated = JsonConvert.DeserializeObject<TreatmentEntryDto>(rawJson);
                return NewlyUpdated;
            }
            else
            {
                throw new TreatmentPlanServiceException(rawJson);
            }
        }

        public async Task<TreatmentEntryDto?> GetEntryById(int entry_id)
        {
            string paramStr = "GetTreatmentPlanEntry?entry_id=" + entry_id;
            HttpResponseMessage response = await _client.GetAsync(paramStr);
            var rawJson = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {

                TreatmentEntryDto? entry = JsonConvert.DeserializeObject<TreatmentEntryDto>(rawJson);
                return entry;
            }
            else throw new TreatmentPlanServiceException(rawJson);
        }



        public async Task<TreatmentPlanDto?> UpdateTreatmentPlan(UpdatePlanRequest request)
        { 
            StringContent content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PutAsync("UpdateTreatmentPlan", content);  //its PutAsync here , not Get
            var rawJson = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {

                TreatmentPlanDto? NewlyUpdated = JsonConvert.DeserializeObject<TreatmentPlanDto>(rawJson);
                return NewlyUpdated;
            }
            else
            {
                throw new TreatmentPlanServiceException(rawJson);
            }

        }

        public async Task DeleteTreatmentPlan(int planId)   //formate changed here again
        {
            HttpResponseMessage response = await _client.DeleteAsync("DeleteTreatmentPlan?plan_id=" + planId);
            var rawJson = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new TreatmentPlanServiceException(rawJson);
            }
            

        }

        public async Task DeleteTreatmentPlanEntry(int entryId)   
        {
            HttpResponseMessage response = await _client.DeleteAsync("DeleteTreatmentPlanEntry?entry_id=" + entryId);
            var rawJson = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new TreatmentPlanServiceException(rawJson);
            }


        }
    }

}


