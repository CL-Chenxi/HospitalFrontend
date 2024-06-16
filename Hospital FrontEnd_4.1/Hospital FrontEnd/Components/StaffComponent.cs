using HospitalFrontEnd.Data;
using Microsoft.AspNetCore.Components;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace HospitalFrontEnd.Components
{
    public class StaffComponent : ComponentBase
    {
        protected SearchStaffForm SearchStaffForm { get; set; } = new SearchStaffForm();
        protected IEnumerable<StaffDto> searchResultList;

        public Boolean displayable = false;
        public string feedbackSearch = string.Empty;
        public string feedbackCreate = string.Empty;
        public string feedbackUpdate = string.Empty;

        protected IEnumerable<StaffDto> StaffList = new List<StaffDto>();
        protected CreateOrUpdateStaffForm CreateStaffForm { get; set; } = new CreateOrUpdateStaffForm();
        protected CreateOrUpdateStaffForm UpdateStaffForm { get; set; } = new CreateOrUpdateStaffForm();
        protected int SelectedStaffId { get; set; }
        protected Boolean StaffStatus { get; set; }

        protected List<int> Grades = [1, 2, 3, 4, 5];
 
        [Inject]
        protected StaffService StaffService { get; set; }  

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

        protected async Task GetStaffList()
        {
            StaffList = await StaffService.GetStaffList();
            if(StaffList == null) StaffList = new List<StaffDto>();
        }


        protected async Task Create()
        {
            var comment = CreateStaffForm.Validate();
            if (comment == string.Empty)
            {
                NewStaffRequest request = new NewStaffRequest(
                    CreateStaffForm.Staff_fName, CreateStaffForm.Staff_lName, CreateStaffForm.Staff_Phone, CreateStaffForm.Staff_Grade
                );
                try
                {
                    var newStaff = await StaffService.CreateNewStaff(request);
                    feedbackCreate = "New staff created Id #" + newStaff?.Staff_ID;
                    await GetStaffList(); //refresh list
                }
                catch (StaffServiceException ex)
                {
                    feedbackCreate = ex.Message;
                }
            }
            else feedbackCreate = comment;
        }

        protected async Task Update()
        {
            var comment = UpdateStaffForm.Validate();
            if(comment == string.Empty)
            {
                UpdateStaffRequest request = new UpdateStaffRequest(
                    SelectedStaffId, UpdateStaffForm.Staff_fName, UpdateStaffForm.Staff_lName, UpdateStaffForm.Staff_Phone, UpdateStaffForm.Staff_Grade
                    );

                try
                {
                    var updatedStaff = await StaffService.UpdateStaff(request);
                    feedbackUpdate = "Updated Staff Id #" + updatedStaff?.Staff_ID;
                    await GetStaffList(); //refresh list
                } catch(StaffServiceException ex)
                {
                    feedbackUpdate = ex.Message;
                }
            } else feedbackUpdate = comment;
        }

        protected async Task Delete()
        {
            try
            {
                await StaffService.DeleteStaff(SelectedStaffId);
                feedbackUpdate = "Deleted Staff Id #" + SelectedStaffId;
                await GetStaffList(); //refresh list
            }
            catch (StaffServiceException ex)
            {
                feedbackUpdate = ex.Message;
            }
        }

        protected async Task UpdateStaffStatus(Boolean activation)
        {
            try
            {
                StaffService.ChangeStatus(activation, SelectedStaffId);
                if (activation) feedbackUpdate = "Staff ID #" + SelectedStaffId + " made available";
                else feedbackUpdate = "Staff ID #" + SelectedStaffId + " made unavailable";
                await GetStaffList(); //refresh the list
            }
            catch (StaffServiceException ex)
            {
                feedbackUpdate = ex.Message;
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

    public class CreateOrUpdateStaffForm
    {
        [MaxLength(50)]
        public string Staff_fName { get; set; }
        [MaxLength(50)]
        public string Staff_lName { get; set; }
        [MaxLength(25)]
        public string Staff_Phone { get; set; }
        public int Staff_Grade { get; set; }

        public string Validate()
        {
            if (string.IsNullOrEmpty(Staff_fName)) return "Missing a First Name";
            if (Staff_fName.Length > 50) return "First name too long";
            if (string.IsNullOrEmpty(Staff_lName)) return "Missing a Last Name";
            if (Staff_lName.Length > 50) return "Last name too long";
            if (string.IsNullOrEmpty(Staff_Phone)) return "Missing a phone number";
            if (Staff_Phone.Length > 25) return "Phone number too long";
            if (Staff_Grade < 1 || Staff_Grade > 5) return "Staff grade inconsistent";

            return string.Empty;
        }
    }

}
