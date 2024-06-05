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

    public class NewPatientRequestDto {
        public string Patient_fName { get; set; }
        public string Patient_lName { get; set; }
        public DateOnly Patient_DoB { get; set; }
        public string Patient_PhoneNum { get; set; }
        public string Patient_Allergy { get; set; }
    };

    public class UpdatePatientRequestDto
    {
        public int Patient_ID { get; set; }
        public string Patient_fName { get; set; }
        public string Patient_lName { get; set; }
        public DateOnly Patient_DoB { get; set; }
        public string Patient_PhoneNum { get; set; }
        public string Patient_Allergy { get; set; }
    };

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

        /*
         * unused
        public async Task<IEnumerable<PatientDto>?> GetPatientList<PatientDto>() // why ? after patientdto
        { // what does this function do?
            HttpResponseMessage respons = await _client.GetAsync("GetPatientList");
            if (respons.IsSuccessStatusCode)
            {
                var rawJson = await respons.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<PatientDto>>(rawJson);
            }
            return null;
        }
        */
        public async Task<PatientDto?> GetPatientById<PatientDto>(int patientId)   
        {
            HttpResponseMessage response = await _client.GetAsync("GetPatientById?patient_id=" + patientId);
            if (response.IsSuccessStatusCode)
            {
                var rawJson = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<PatientDto>(rawJson);
            }
            return default;
        }

        public async Task<IEnumerable<PatientDto?>?> SearchPatient<PatientDto>(SearchPatientForm searchFormDto)
        {
            string queryString = "SearchPatient?";
            if (!string.IsNullOrEmpty(searchFormDto.Patient_fName))
            {
                queryString += "First_Name=" + searchFormDto.Patient_fName;
                if (!string.IsNullOrEmpty(searchFormDto.Patient_lName))
                {
                    queryString += "&Last_Name=" + searchFormDto.Patient_lName;
                }
            } else if(!string.IsNullOrEmpty(searchFormDto.Patient_lName))
            {
                queryString += "Last_Name=" + searchFormDto.Patient_lName;
            }
            HttpResponseMessage response = await _client.GetAsync(queryString);
            if (response.IsSuccessStatusCode)
            {
                var rawJson = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<PatientDto>>(rawJson);
            }
            return null;
        }

        public async Task<string> CreateNewPatient<NewPatientRequestDto>(NewPatientRequestDto createPatientDTO)  
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(createPatientDTO), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PostAsync("CreatePatient", content);
            if (response.IsSuccessStatusCode)
            {
                var rawJson = await response.Content.ReadAsStringAsync();
                PatientDto? newlyCreatedPatient = JsonConvert.DeserializeObject<PatientDto>(rawJson);
                return "Patient Created (id #" + newlyCreatedPatient?.Patient_ID + ")";
            }
            else
            {
                return "Error " + response.StatusCode;
            }

        }

        public async Task<string> UpdatePatient<UpdatePatientRequestDto>(UpdatePatientRequestDto editPatientFormDto)  
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(editPatientFormDto), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PutAsync("UpdatePatient", content);  //its PutAsync here , not Get
            if (response.IsSuccessStatusCode)
            {
                return "Patient Updated";
            }
            else
            {
                return "Error " + response.StatusCode;
            }
        }

        public async Task<string> DeletePatient<UpdatePatientRequestDto>(int patientId)   //formate changed here again
        {
            HttpResponseMessage response = await _client.DeleteAsync("DeletePatient?patient_id=" + patientId);
            if (response.IsSuccessStatusCode)
            {
                return "Patient Deleted";
            }
            else
            {
                return "Error " + response.StatusCode;
            }

        }
    }

}
    
