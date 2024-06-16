using System.Net.Http.Headers;
using System.Text;
using HospitalFrontEnd.Components;
using Newtonsoft.Json;


namespace HospitalFrontEnd.Data
{
    //same definition as in Back end
    //used to receive result from back end controller
    public record PatientDto(
        int Patient_ID,
        string Patient_fName,
        string Patient_lName,
        DateOnly Patient_DoB,
        string Patient_PhoneNum,
        string Patient_Allergy
    );

    public record NewPatientRequestDto(
         string Patient_fName,
         string Patient_lName,
         DateOnly Patient_DoB,
         string Patient_PhoneNum,
         string Patient_Allergy
    ); 
    

    public record UpdatePatientRequestDto(
         int Patient_ID,
         string Patient_fName,
         string Patient_lName,
         DateOnly Patient_DoB,
         string Patient_PhoneNum,
         string Patient_Allergy
    );
  

    public class PatientServiceException : Exception
    {
        public PatientServiceException() { }
        public PatientServiceException(string message) : base(message) { }
    }

    public class PatientService
    {

        private static HttpClient _client = new()
        {
            BaseAddress = new Uri("https://localhost:7099/api/Patient/")

        };

        public PatientService()
        {
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }


        public async Task<IEnumerable<PatientDto>?> GetPatientList()
        { 
            HttpResponseMessage response = await _client.GetAsync("GetPatientList");
            var rawJson = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                
                return JsonConvert.DeserializeObject<IEnumerable<PatientDto>>(rawJson);
            }
            else throw new PatientServiceException(rawJson);
        }

        public async Task<PatientDto?> GetPatientById(int patientId)   
        {
            HttpResponseMessage response = await _client.GetAsync("GetPatientById?patient_id=" + patientId);
            var rawJson = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                
                return JsonConvert.DeserializeObject<PatientDto>(rawJson);
            }
            else throw new PatientServiceException(rawJson);
        }

        public async Task<IEnumerable<PatientDto>?> SearchPatient(SearchPatientForm searchFormDto)
        {
            
            StringBuilder builder = new StringBuilder();
            List<string> queryParam = new List<string>();
            if (!string.IsNullOrEmpty(searchFormDto.Patient_fName))
            {
                queryParam.Add("First_Name=" + searchFormDto.Patient_fName);
            }
            if (!string.IsNullOrEmpty(searchFormDto.Patient_lName))
            {
                queryParam.Add("Last_Name=" + searchFormDto.Patient_lName);
            }
            if (searchFormDto.Patient_Id > 0)
            {
                queryParam.Add("Patient_Id=" + searchFormDto.Patient_Id);
            }
            string queryString = "SearchPatient?"+builder.AppendJoin("&", queryParam);

            HttpResponseMessage response = await _client.GetAsync(queryString);
            var rawJson = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<IEnumerable<PatientDto>>(rawJson);
            }
            else throw new PatientServiceException(rawJson); 
        }

        public async Task<PatientDto?> CreateNewPatient(NewPatientRequestDto createPatientDTO)  
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(createPatientDTO), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PostAsync("CreatePatient", content);
            var rawJson = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                var newlyCreatedPatient = JsonConvert.DeserializeObject<PatientDto>(rawJson);
                return newlyCreatedPatient;
            }
            else
            {
                throw new PatientServiceException(rawJson);
            }

        }

        public async Task<PatientDto?> UpdatePatient(UpdatePatientRequestDto editPatientFormDto)  
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(editPatientFormDto), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PutAsync("UpdatePatient", content);  //its PutAsync here , not Get
            var rawJson = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                var newlyCreatedPatient = JsonConvert.DeserializeObject<PatientDto>(rawJson);
                return newlyCreatedPatient;
            }
            else
            {
                throw new PatientServiceException(rawJson);
            }
        }

        public async Task DeletePatient(int patientId)   //formate changed here again
        {
            HttpResponseMessage response = await _client.DeleteAsync("DeletePatient?patient_id=" + patientId);
            var rawJson = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new PatientServiceException(rawJson);
            }

        }
    }

}
    
