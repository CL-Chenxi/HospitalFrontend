using HospitalFrontEnd.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Diagnostics.CodeAnalysis;

namespace HospitalFrontEnd.Components
{
    public class TreatmentPlanComponent : ComponentBase
    {
        public TreatmentPlanDto? TreatmentPlanDto { get; set; }
        public EditOrCreateTreatmentPlanForm CreateTreatmentPlanForm { get; set; } = new EditOrCreateTreatmentPlanForm();
        public EditOrCreateTreatmentPlanForm EditTreatmentPlanForm { get; set; } = new EditOrCreateTreatmentPlanForm();
        public TreatmentPlanEntryForm PrescriptionForm { get; set; } = new TreatmentPlanEntryForm();
        public TreatmentPlanEntryForm ScheduleTestForm { get; set; } = new TreatmentPlanEntryForm();
        public TreatmentPlanEntryForm TestResultForm { get; set; } = new TreatmentPlanEntryForm();

        public SearchTreatmentPlanForm SearchTreatmentPlanForm { get; set; } = new SearchTreatmentPlanForm();

        public Boolean displayable = false;
        public string feedbackSearch = string.Empty;
        public string feedbackCreation = string.Empty;
        public string feedbackUpdate = string.Empty;
        public IEnumerable<TreatmentPlanDto> searchResultList = new List<TreatmentPlanDto>();
        public IEnumerable<TreatmentEntryDto> entryList = new List<TreatmentEntryDto>();

        [Parameter] //for single value to track , use this parameter. otherwise, methods includs many feilds
        public int TreatmentPlanSearchId { get; set; }
        [Parameter]
        public int TreatmentPlanPatientId { get; set; }
        [Parameter]
        public List<string> StatusDropdown {  get; set; } //filled up on page load (see TreatmentPlanCreate.razor)
        [Parameter]
        public List<string> TypeDropdown { get; set; }


        [Inject]
        private TreatmentPlanService TreatmentService { get; set; }



        protected async Task Search()
        {
            var returnedList = await TreatmentService.SearchTreatmentPlan<TreatmentPlanDto>(SearchTreatmentPlanForm);
            if(returnedList != null)
            {
                searchResultList = returnedList;
            } else
            {
                searchResultList = new List<TreatmentPlanDto>();
            }

        }

        protected async Task GetTreatmentPlanById()
        {
            var dto = await TreatmentService.GetTreatmentPlanById<TreatmentPlanDto>(TreatmentPlanSearchId);
            if (dto == null)
            {
                displayable = false;
                TreatmentPlanDto = null;  //does not exist in current context
                EditTreatmentPlanForm = new EditOrCreateTreatmentPlanForm();
                feedbackSearch = "No Treatment Plan Found";
            }
            else
            {
                TreatmentPlanDto = dto;
                EditTreatmentPlanForm.Patient_Id = TreatmentPlanDto.patient.Patient_ID;
                EditTreatmentPlanForm.Plan_Date = TreatmentPlanDto.Plan_Date; //if DB find sth, user wants to change, so need to edit
                EditTreatmentPlanForm.Plan_Status = TreatmentPlanDto.Plan_Status;
                EditTreatmentPlanForm.Plan_Observation = TreatmentPlanDto.Plan_Observation;
                EditTreatmentPlanForm.Plan_CycleLen = TreatmentPlanDto.Plan_Cycle;
                EditTreatmentPlanForm.Staff_Id = TreatmentPlanDto.staff.Staff_ID;
                await GetTreatmentHistory();
                displayable = true; //this makes form to appear
                feedbackSearch = string.Empty;

            }
        }

        protected async Task GetTreatmentHistory()
        {
            var list = await TreatmentService.GetEntriesForPlan(TreatmentPlanSearchId);
            if(list != null)
            {
                entryList = list;
            } else { 
                entryList = new List<TreatmentEntryDto>();
            }

        }

        protected async Task GetAllPlansForPatients()
        {
            var returnedList = await TreatmentService.GetAllTreatmentPlansForPatient<TreatmentPlanDto>(TreatmentPlanPatientId);
            if(returnedList != null)
            {
                searchResultList = returnedList;
            } else
            {
                searchResultList = new List<TreatmentPlanDto>();
            }
        }

        protected async Task Create()
        {
            NewPlanRequest planRequest = new NewPlanRequest(
                CreateTreatmentPlanForm.Patient_Id,
                CreateTreatmentPlanForm.Staff_Id,
                CreateTreatmentPlanForm.Plan_CycleLen,
                CreateTreatmentPlanForm.Plan_Status,
                CreateTreatmentPlanForm.Plan_Date,
                CreateTreatmentPlanForm.Plan_Observation
                );
            feedbackCreation = await TreatmentService.CreateNewTreatmentPlan<NewPlanRequest>(planRequest);
            //todo isolate returned Id (and inject in TreatmentPlanSearchId)
        }

        protected async Task Update()
        {
            UpdatePlanRequest planRequest = new UpdatePlanRequest(
                TreatmentPlanSearchId,
                EditTreatmentPlanForm.Patient_Id,
                EditTreatmentPlanForm.Staff_Id,
                EditTreatmentPlanForm.Plan_CycleLen,
                EditTreatmentPlanForm.Plan_Status,
                CreateTreatmentPlanForm.Plan_Date,
                EditTreatmentPlanForm.Plan_Observation
                );
            feedbackUpdate = await TreatmentService.UpdateTreatmentPlan<UpdatePlanRequest>(planRequest);
        }

        protected async Task Delete()
        {
            feedbackUpdate = await TreatmentService.DeleteTreatmentPlan<UpdatePlanRequest>(TreatmentPlanSearchId);
        }

        protected async Task AddPrescription()
        {
            NewEntryRequest entryRequest = new NewEntryRequest(
                TreatmentPlanDto.Plan_ID,
                TreatmentPlanDto.staff.Staff_ID, //in the future, replace with ID of whoever is logged on
                DateOnly.FromDateTime(DateTime.Now),
                "PRESCRIPTION",
                PrescriptionForm.Comment,
                PrescriptionForm.Drug_ID, 
                PrescriptionForm.Posology, 
                String.Empty
            );
            feedbackUpdate = await TreatmentService.AddPlanEntry<NewEntryRequest>(entryRequest);
        }
        protected async Task AddTest()
        {
            NewEntryRequest entryRequest = new NewEntryRequest(
                    TreatmentPlanDto.Plan_ID,
                    TreatmentPlanDto.staff.Staff_ID, //in the future, replace with ID of whoever is logged on
                    DateOnly.FromDateTime(DateTime.Now),
                    ScheduleTestForm.EntryType,
                    ScheduleTestForm.Comment,
                    null, null, String.Empty
                );
            feedbackUpdate = await TreatmentService.AddPlanEntry(entryRequest);
        }

        protected async Task UpdateTest()
        {
            TreatmentEntryDto originalEntry = await TreatmentService.GetEntryById<TreatmentEntryDto>(TestResultForm.Entry_ID);
            if(originalEntry != null)
            {
                UpdateEntryRequest updateRequest = new UpdateEntryRequest(
                    originalEntry.Entry_ID,
                    originalEntry.Plan_ID,
                    TestResultForm.Staff_ID, //in the future, replace with staff_id of whoever is logger in
                    DateOnly.FromDateTime(DateTime.Now),
                    originalEntry.Entry_Type,
                    TestResultForm.Comment,
                    null, null,
                    TestResultForm.UploadLink
                    );
                feedbackUpdate = await TreatmentService.UpdatePlanEntry(updateRequest);
            }

        }

    }



    public class EditOrCreateTreatmentPlanForm // this is data transfer obj, move info. from form in 
    {
        public int Patient_Id { get; set; } 
        public int Staff_Id { get; set; } 
        public int Plan_CycleLen { get; set; }
        public DateOnly Plan_Date { get; set; } = DateOnly.FromDateTime(DateTime.Now); //set the default date as today in the form
        public string Plan_Status { get; set; } = "OPEN"; //= PlanStatus.New.ToString();
        public string Plan_Observation { get; set; } = string.Empty;

    }

    public class TreatmentPlanEntryForm
    {
        public int Entry_ID {get; set;}
        public int Plan_ID { get; set; }
        public int Staff_ID { get; set; }
        public DateOnly Entry_Date { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        public string EntryType { get; set; }

        [AllowNull]
        public int Drug_ID { get; set; }
        [AllowNull]
        public string Posology { get; set; }
        [AllowNull]
        public string Comment { get; set; } = String.Empty;
        [AllowNull]
        public string UploadLink { get; set; }
    }

    public class SearchTreatmentPlanForm //search options
    {
        [AllowNull]
        public int? Plan_Id { get; set; }
        [AllowNull]
        public int? Patient_Id { get; set; } 
        [AllowNull]
        public int? Staff_Id { get; set; } 

    }
}

