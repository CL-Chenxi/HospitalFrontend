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

        public async Task<TreatmentPlanDto?> GetTreatmentPlanById<TreatmentPlanDto>(int TreatmentPlanId)  
        {
            string paramStr = "GetTreatmentPlanById?plan_id=" + TreatmentPlanId; 
            HttpResponseMessage response = await _client.GetAsync(paramStr);
            if (response.IsSuccessStatusCode)
            {
                var rawJson = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<TreatmentPlanDto>(rawJson);
            }
            return default;
        }

        public async Task<IEnumerable<TreatmentEntryDto>?> GetEntriesForPlan(int TreatmentPlanId)
        {
            string paramStr = "GetAllEntriesForPlan?plan_id=" + TreatmentPlanId;
            HttpResponseMessage response = await _client.GetAsync(paramStr);
            if (response.IsSuccessStatusCode)
            {
                var rawJson = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<TreatmentEntryDto>>(rawJson);
            }
            return default;
        }

        public async Task<IEnumerable<TreatmentPlanDto>?> GetAllTreatmentPlansForPatient<TreatmentPlanDto>(int PatientId)   //TreatmentPlanId is a new variable? same as line 137?
        {
            string paramStr = "GetAllTreatmentPlansForPatient?patient_id=" + PatientId;
            HttpResponseMessage response = await _client.GetAsync(paramStr);
            if (response.IsSuccessStatusCode)
            {
                var rawJson = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<TreatmentPlanDto>>(rawJson);
            }
            return null;
        }

        public async Task<IEnumerable<TreatmentPlanDto>?> SearchTreatmentPlan<TreatmentPlanDto>(SearchTreatmentPlanForm searchFormDto)
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

        public async Task<string> CreateNewTreatmentPlan<NewPlanRequest>(NewPlanRequest createTreatmentPlanRequest)
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(createTreatmentPlanRequest), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PostAsync("CreateNewTreatmentPlan", content);
            if (response.IsSuccessStatusCode)
            {
                var rawJson = await response.Content.ReadAsStringAsync();
                TreatmentPlanDto? newlyCreatedPlan = JsonConvert.DeserializeObject<TreatmentPlanDto>(rawJson);
                return "Treatment Plan Created (id#" + newlyCreatedPlan?.Plan_ID + ")";
            }
            else
            {
                return "Error " + response.StatusCode;
            }
        }

        public async Task<string> AddPlanEntry<NewEntryRequest>(NewEntryRequest newEntryRequest)
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(newEntryRequest), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PostAsync("AddTreatmentPlanEntry", content);
            if (response.IsSuccessStatusCode)
            {
                return "Entry added";
            }
            else
            {
                return "Error " + response.StatusCode;
            }
        }

        public async Task<string> UpdatePlanEntry<UpdateEntryRequest>(UpdateEntryRequest updateRequest)
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(updateRequest), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PutAsync("UpdateTreatmentPlanEntry", content);
            if (response.IsSuccessStatusCode)
            {
                return "Entry updated";
            }
            else
            {
                return "Error " + response.StatusCode;
            }
        }

        public async Task<TreatmentPlanEntryDto?> GetEntryById<TreatmentPlanEntryDto>(int entry_id)
        {
            string paramStr = "GetTreatmentPlanEntry?entry_id=" + entry_id;
            HttpResponseMessage response = await _client.GetAsync(paramStr);
            if (response.IsSuccessStatusCode)
            {
                var rawJson = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<TreatmentPlanEntryDto>(rawJson);
            }
            return default;
        }



        public async Task<string> UpdateTreatmentPlan<UpdatePlanRequest>(UpdatePlanRequest editTreatmentPlanFormDto)
        { 
            StringContent content = new StringContent(JsonConvert.SerializeObject(editTreatmentPlanFormDto), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PutAsync("UpdateTreatmentPlan", content);  //its PutAsync here , not Get
            if (response.IsSuccessStatusCode)
            {
                return "Treatment Plan Updated";
            }
            else
            {
                return "Error " + response.StatusCode;
            }

        }

        public async Task<string> DeleteTreatmentPlan<UpdatePlanRequest>(int planId)   //formate changed here again
        {
            HttpResponseMessage response = await _client.DeleteAsync("DeleteTreatmentPlan?plan_id=" + planId);
            if (response.IsSuccessStatusCode)
            {
                return "Treatment Plan Deleted";
            }
            else
            {
                return "Error " + response.StatusCode;
            }

        }
    }

}


