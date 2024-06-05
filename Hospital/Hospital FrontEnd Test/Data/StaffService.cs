using System.Net.Http.Headers;
using HospitalFrontEnd.Components;
using Newtonsoft.Json;


namespace HospitalFrontEnd.Data
{
    public record StaffDto(
        int Staff_ID,
        string Staff_fName,
        string Staff_lName,
        string Staff_PhoneNum,
        int Staff_Grade
    );

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
            if (response.IsSuccessStatusCode)
            {
                var rawJson = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<StaffDto>>(rawJson);
            }
            return null;
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

    }

}

