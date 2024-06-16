using HospitalFrontEnd.Data;
using Microsoft.AspNetCore.Components;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace HospitalFrontEnd.Components
{
    public class DrugComponent : ComponentBase
    {
        protected IEnumerable<DrugDto>? DrugList { get; set; }

        public Boolean displayable = false;
        public string feedbackSearch = string.Empty;
        public string feedbackCreate = string.Empty;
        public string feedbackUpdate = string.Empty;

        protected CreateOrUpdateDrugForm CreateDrugForm { get; set; } = new CreateOrUpdateDrugForm();
        protected CreateOrUpdateDrugForm UpdateDrugForm { get; set; } = new CreateOrUpdateDrugForm();

        protected int SelectedDrugId {  get; set; }
        protected Boolean SelectedDrugStatus {  get; set; }

        [Inject]
        protected DrugService DrugService { get; set; }

        protected async Task GetDrugList()
        {
            DrugList = await DrugService.GetDrugList();
        }

        protected async Task Create()
        {
            feedbackCreate = CreateDrugForm.Validate();
            if(feedbackCreate == string.Empty) 
            {
                NewDrugRequest request = new NewDrugRequest(CreateDrugForm.Drug_Name, CreateDrugForm.Drug_Dosage, CreateDrugForm.Drug_Allergies);
                try
                {
                    DrugDto? NewDrug =  await DrugService.CreateNewDrug(request);
                    feedbackCreate = "New Drug inserted ID #" + NewDrug?.Drug_ID;
                    await GetDrugList(); //refresh the list
                } catch (DrugServiceException ex)
                {
                    feedbackCreate = ex.Message;
                }
            }

        }

        protected async Task Update()
        {
            feedbackUpdate = UpdateDrugForm.Validate();
            if(feedbackUpdate == string.Empty)
            {
                UpdateDrugRequest request = new UpdateDrugRequest(SelectedDrugId, UpdateDrugForm.Drug_Name, UpdateDrugForm.Drug_Dosage, UpdateDrugForm.Drug_Allergies);
                try
                {
                    DrugDto? NewDrug = await DrugService.UpdateDrug(request);
                    feedbackUpdate = "Drug updated ID #" + NewDrug?.Drug_ID;
                    await GetDrugList(); //refresh the list
                }
                catch (DrugServiceException ex)
                {
                    feedbackUpdate = ex.Message;
                }
            }
        }

        protected async Task Delete()
        {
            try
            {
                await DrugService.DeleteDrug(SelectedDrugId);
                feedbackUpdate = "Deleted Drug ID #" + SelectedDrugId;
                await GetDrugList(); //refresh the list
            } catch (DrugServiceException ex)
            {
                feedbackUpdate = ex.Message;
            }
        }

        protected async Task UpdateDrugStatus()
        {
            try
            {
                await DrugService.ChangeStatus(!SelectedDrugStatus, SelectedDrugId);
                if(SelectedDrugStatus) feedbackUpdate = "Drug ID #" + SelectedDrugId + " made unavailable"; 
                else feedbackUpdate = "Drug ID #" + SelectedDrugId + " made available";
                await GetDrugList(); //refresh the list
                SelectedDrugStatus = !SelectedDrugStatus;
            } catch(DrugServiceException ex)
            {
                feedbackUpdate = ex.Message;
            }

        }

    }

    public class CreateOrUpdateDrugForm
    {
        [MaxLength(50)]
        public string Drug_Name { get; set; }
        [MaxLength(50)]
        public string Drug_Dosage { get; set; }
        [MaxLength(50)]
        [AllowNull]
        public string Drug_Allergies { get; set; } = string.Empty;

        //verify that nothing is missing before we do anything
        public string Validate()
        {
            if (string.IsNullOrEmpty(Drug_Name)) return "Missing a name";
            if (string.IsNullOrEmpty(Drug_Dosage)) return "Missing a dosage";
            if (string.IsNullOrEmpty(Drug_Allergies))
            {
                Drug_Allergies = string.Empty;//avoid accidentally inserting a null value
            }
            return string.Empty;
        }
    }
}
