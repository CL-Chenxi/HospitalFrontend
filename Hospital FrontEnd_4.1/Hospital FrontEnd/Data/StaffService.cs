using System.Net.Http.Headers;
using System.Text;
using HospitalFrontEnd.Components;
using Newtonsoft.Json;


namespace HospitalFrontEnd.Data
{
    public record StaffDto(
        int Staff_ID,
        string Staff_fName,
        string Staff_lName,
        string Staff_PhoneNum,
        int Staff_Grade,
        Boolean Staff_Active
    );

    public record NewStaffRequest
    (
        string Staff_fName,
        string Staff_lName,
        string Staff_Phone,
        int Staff_Grade
    );

    public record UpdateStaffRequest
    (
        int Staff_ID,
        string Staff_fName,
        string Staff_lName,
        string Staff_Phone,
        int Staff_Grade
    );

    public class StaffServiceException : Exception
    {
        public StaffServiceException() { }
        public StaffServiceException( string message ) : base( message ) { }
    }

    public class StaffService
    {

        private static HttpClient _client = new()
        {
            BaseAddress = new Uri("https://localhost:7099/api/Staff/")

        };

        public StaffService()
        {
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<StaffDto?> GetStaffById<StaffDto>(int StaffId)  
        {
            HttpResponseMessage response = await _client.GetAsync("GetStaffById?staff_id=" + StaffId);
            if (response.IsSuccessStatusCode)
            {
                var rawJson = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<StaffDto>(rawJson);
            }
            return default;
        }

        public async Task<List<StaffDto>?> GetStaffList()
        {
            HttpResponseMessage response = await _client.GetAsync("GetStaffList");
            var rawJson = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<List<StaffDto>>(rawJson);
            }
            else return null;
        }

        public async Task<IEnumerable<StaffDto?>?> SearchStaff<StaffDto>(SearchStaffForm searchFormDto)
        {
            string queryString = "SearchStaff?";
            if (!string.IsNullOrEmpty(searchFormDto.First_Name))
            {
                queryString += "First_Name=" + searchFormDto.First_Name;
                if (!string.IsNullOrEmpty(searchFormDto.Last_Name))
                {
                    queryString += "&Last_Name=" + searchFormDto.Last_Name;
                }
            }
            else if (!string.IsNullOrEmpty(searchFormDto.Last_Name))
            {
                queryString += "Last_Name=" + searchFormDto.Last_Name;
            }
            HttpResponseMessage response = await _client.GetAsync(queryString);
            if (response.IsSuccessStatusCode)
            {
                var rawJson = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<StaffDto>>(rawJson);
            }
            return null;
        }

        public async Task<StaffDto?> CreateNewStaff(NewStaffRequest request)
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PostAsync("AddStaff", content);
            var rawJson = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<StaffDto>(rawJson);
            }
            else 
                throw new StaffServiceException(rawJson);
        }

        public async Task<StaffDto?> UpdateStaff(UpdateStaffRequest request)
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PutAsync("UpdateStaff", content);
            var rawJson = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<StaffDto>(rawJson);
            }
            else
                throw new StaffServiceException(rawJson);
        }

        public async Task DeleteStaff(int StaffId)
        {
            HttpResponseMessage response = await _client.DeleteAsync("DeleteStaff?staff_id="+StaffId);
            var raw = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
                throw new StaffServiceException(raw);
        }

        public async Task ChangeStatus(Boolean activate, int StaffId)
        {
            String parameter = string.Empty;
            if (activate) parameter = "ReactivateStaff?staff_id=" + StaffId;
            else parameter = "DeactivateStaff?staff_id=" + StaffId;
            HttpResponseMessage response = await _client.PutAsync(parameter, new StringContent(string.Empty));
            var raw = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
                throw new StaffServiceException(raw);
        }

    }

}

