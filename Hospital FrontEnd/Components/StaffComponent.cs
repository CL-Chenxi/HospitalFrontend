using HospitalFrontEnd.Data;
using Microsoft.AspNetCore.Components;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace HospitalFrontEnd.Components
{
    public class StaffComponent : ComponentBase
    {
        public SearchStaffForm SearchStaffForm { get; set; } = new SearchStaffForm();

        public Boolean displayable = false;
        public string feedbackSearch = string.Empty;
        public IEnumerable<StaffDto> searchResultList;

        
        [Inject]
        private StaffService StaffService { get; set; }  

        protected async Task Search()
        {
            searchResultList = await StaffService.SearchStaff<StaffDto>(SearchStaffForm);
            if (searchResultList == null)
            {
                displayable = false;
                searchResultList = new List<StaffDto>();  //does not exist in current context
                feedbackSearch = "No staff Found";
            }
            else
            {
                displayable = true; //this makes form to appear
                feedbackSearch = string.Empty;

            }
        }

    }

    public class SearchStaffForm
    {
        [AllowNull]
        public int? Staff_Id { get; set; }
        [MaxLength(50)]
        [AllowNull]
        public string? First_Name { get; set; }
        [MaxLength(50)]
        [AllowNull]
        public string? Last_Name { get; set; }
    }

}
