using HospitalFrontEnd.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Diagnostics.CodeAnalysis;

namespace HospitalFrontEnd.Components
{
    public class TreatmentPlanComponent : ComponentBase
    {
        public TreatmentPlanDto? TreatmentPlanDto { get; set; }

        public TreatmentEntryDto? TreatmentEntryDto { get; set; }
        public EditOrCreateTreatmentPlanForm CreateTreatmentPlanForm { get; set; } = new EditOrCreateTreatmentPlanForm();
        public EditOrCreateTreatmentPlanForm EditTreatmentPlanForm { get; set; } = new EditOrCreateTreatmentPlanForm();
        public PrescriptionForm PrescriptionForm { get; set; } = new PrescriptionForm();
        public ScheduleTestForm ScheduleTestForm { get; set; } = new ScheduleTestForm();
        public TreatmentPlanEntryForm UpdateEntryForm { get; set; } = new TreatmentPlanEntryForm();

        public SearchTreatmentPlanForm SearchTreatmentPlanForm { get; set; } = new SearchTreatmentPlanForm();

        public Boolean displayable = false;
        public string feedbackSearch = string.Empty;
        public string feedbackCreation = string.Empty;
        public string feedbackUpdate = string.Empty;
        public IEnumerable<TreatmentPlanDto> planList = new List<TreatmentPlanDto>();
        public IEnumerable<TreatmentEntryDto> entryList = new List<TreatmentEntryDto>();
        public IEnumerable<PatientDto> patientList = new List<PatientDto>();
        public IEnumerable<StaffDto> staffList = new List<StaffDto>();
        public IEnumerable<DrugDto> drugList = new List<DrugDto>();

        [Parameter] //for single value to track , use this parameter. otherwise, methods includs many feilds
        public int TreatmentPlanSearchId { get; set; }
        [Parameter]
        public int TreatmentPlanPatientId { get; set; }
        [Parameter]
        public int SelectedEntryId { get; set; }


        [Inject]
        protected TreatmentPlanService TreatmentService { get; set; }

        [Inject]
        protected PatientService PatientService { get; set; }
        [Inject]
        protected StaffService StaffService { get; set; }
        [Inject]
        protected DrugService DrugService { get; set; }


        protected async Task Search()
        {
            try
            {
                var returnedList = await TreatmentService.SearchTreatmentPlan(SearchTreatmentPlanForm);
                if(returnedList != null)
                {
                    planList = returnedList;
                } else
                {
                    planList = new List<TreatmentPlanDto>();
                }
            } catch(TreatmentPlanServiceException ex)
            {
                feedbackSearch = ex.Message;
            }


        }

        protected async Task GetTreatmentPlanById()
        {
            try
            {
                var dto = await TreatmentService.GetTreatmentPlanById(TreatmentPlanSearchId);
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
            catch (TreatmentPlanServiceException ex)
            {
                feedbackSearch = ex.Message;
            }

        }

        protected async Task GetTreatmentHistory()
        {
            try
            {
                var list = await TreatmentService.GetEntriesForPlan(TreatmentPlanSearchId);
                if(list != null)
                {
                    entryList = list;
                } else { 
                    entryList = new List<TreatmentEntryDto>();
                }
            }
            catch (TreatmentPlanServiceException ex)
            {
                feedbackSearch = ex.Message;
            }
        }

        protected async Task GetAllPlansForPatients()
        {
            try
            {
                var returnedList = await TreatmentService.GetAllTreatmentPlansForPatient(TreatmentPlanPatientId);
                if(returnedList != null)
                {
                    planList = returnedList;
                } else
                {
                    planList = new List<TreatmentPlanDto>();
                }
            }
            catch (TreatmentPlanServiceException ex)
            {
                feedbackSearch = ex.Message;
            }

        }

        protected async Task Create()
        {
            var comment = CreateTreatmentPlanForm.Validate();
            if (comment == string.Empty)
            {
                try
                {
                    NewPlanRequest planRequest = new NewPlanRequest(
                        CreateTreatmentPlanForm.Patient_Id,
                        CreateTreatmentPlanForm.Staff_Id,
                        CreateTreatmentPlanForm.Plan_CycleLen,
                        CreateTreatmentPlanForm.Plan_Status,
                        CreateTreatmentPlanForm.Plan_Date,
                        CreateTreatmentPlanForm.Plan_Observation
                        );
                    TreatmentPlanDto? plan = await TreatmentService.CreateNewTreatmentPlan(planRequest);
                    feedbackCreation = "New treatment plan created, ID #" + plan?.Plan_ID;
                }
                catch (TreatmentPlanServiceException ex)
                {
                    feedbackCreation = ex.Message;
                }
            }
            else feedbackCreation = comment;


        }

        protected async Task Update()
        {
            var comment = EditTreatmentPlanForm.Validate();
            if(comment == string.Empty)
            {
                try
                {
                    UpdatePlanRequest planRequest = new UpdatePlanRequest(
                        TreatmentPlanSearchId,
                        EditTreatmentPlanForm.Patient_Id,
                        EditTreatmentPlanForm.Staff_Id,
                        EditTreatmentPlanForm.Plan_CycleLen,
                        EditTreatmentPlanForm.Plan_Status,
                        TreatmentPlanDto.Plan_Date,
                        EditTreatmentPlanForm.Plan_Observation
                        );
                    TreatmentPlanDto? plan = await TreatmentService.UpdateTreatmentPlan(planRequest);
                    feedbackUpdate = "Treatment plan ID #" + TreatmentPlanSearchId + " updated";
                    await GetTreatmentPlanById(); //refresh
                    await GetTreatmentHistory();
                } 
                catch(TreatmentPlanServiceException ex)
                {
                    feedbackUpdate= ex.Message;
                }
            } else feedbackUpdate = comment;
            

        }

        protected async Task Delete()
        {
            try
            {
                await TreatmentService.DeleteTreatmentPlan(TreatmentPlanSearchId);
            }
            catch (TreatmentPlanServiceException ex)
            {
                feedbackUpdate = ex.Message;
            }
        }

        protected async Task AddPrescription()
        {
            var comment = PrescriptionForm.Validate();
            if (comment == string.Empty)
            {
                try
                {
                    if (TreatmentPlanDto != null)
                    {
                        NewEntryRequest entryRequest = new NewEntryRequest(
                            TreatmentPlanDto.Plan_ID,
                            TreatmentPlanDto.staff.Staff_ID, //in the future, replace with ID of whoever is logged on
                            DateOnly.FromDateTime(DateTime.Now),
                            PrescriptionForm.EntryType,
                            PrescriptionForm.Comment,
                            PrescriptionForm.Drug_ID,
                            PrescriptionForm.Posology,
                            string.Empty
                        );
                        TreatmentEntryDto? entry = await TreatmentService.AddPlanEntry(entryRequest);
                        feedbackUpdate = "Created entry ID #" + entry?.Entry_ID + " for plan #" + TreatmentPlanDto.Plan_ID;
                        await GetTreatmentHistory();
                    }
                    else { feedbackUpdate = "No plan selected"; }

                }
                catch (TreatmentPlanServiceException ex)
                {
                    feedbackUpdate = ex.Message;
                }
            }
            else feedbackUpdate = comment;
            

        }
        protected async Task AddTest()
        {
            var comment = ScheduleTestForm.Validate();
            if (comment == string.Empty)
            {
                try
                {
                    if (TreatmentPlanDto != null)
                    {
                        NewEntryRequest entryRequest = new NewEntryRequest(
                        TreatmentPlanDto.Plan_ID,
                        TreatmentPlanDto.staff.Staff_ID, //in the future, replace with ID of whoever is logged on
                        DateOnly.FromDateTime(DateTime.Now),
                        ScheduleTestForm.EntryType,
                        ScheduleTestForm.Comment,
                        null, null, string.Empty
                            );
                        TreatmentEntryDto? entry = await TreatmentService.AddPlanEntry(entryRequest);
                        feedbackUpdate = "Created entry ID #" + entry?.Entry_ID + " for plan #" + TreatmentPlanDto.Plan_ID;
                        await GetTreatmentHistory();
                    }
                    else { feedbackUpdate = "No plan selected"; }

                }
                catch (TreatmentPlanServiceException ex)
                {
                    feedbackUpdate = ex.Message;
                }
            } else feedbackUpdate = comment;
        }

        protected async Task GetEntryByID()
        {
            try
            {
                TreatmentEntryDto = await TreatmentService.GetEntryById(SelectedEntryId);
                if(TreatmentEntryDto != null)
                {
                    UpdateEntryForm.Entry_ID = TreatmentEntryDto.Entry_ID;
                    UpdateEntryForm.Plan_ID = TreatmentEntryDto.Plan_ID;
                    UpdateEntryForm.Staff_ID = TreatmentEntryDto.Staff.Staff_ID;
                    UpdateEntryForm.Entry_Date = TreatmentEntryDto.Last_Update;
                    UpdateEntryForm.EntryType = TreatmentEntryDto.Entry_Type;
                    UpdateEntryForm.Comment = TreatmentEntryDto.Comment;
                    if(TreatmentEntryDto.Drug != null)
                    {
                        UpdateEntryForm.Drug_ID = TreatmentEntryDto.Drug.Drug_ID;
                    }
                    UpdateEntryForm.Posology = TreatmentEntryDto.Posology;
                    UpdateEntryForm.UploadLink = TreatmentEntryDto.Upload_Link;
                    displayable = true;
                }
            } catch (TreatmentPlanServiceException ex)
            {
                displayable = false;
                feedbackSearch = ex.Message;
            }
            
        }

        protected async Task UpdateEntry()
        {
            try
            {
                if(TreatmentEntryDto != null)
                {

                    UpdateEntryRequest updateRequest;
                    if(TreatmentEntryDto.Entry_Type != "PRESCRIPTION")
                    {
                        updateRequest = new UpdateEntryRequest(
                                TreatmentEntryDto.Entry_ID,
                                TreatmentEntryDto.Plan_ID,
                                UpdateEntryForm.Staff_ID, //in the future, replace with staff_id of whoever is logger in
                                DateOnly.FromDateTime(DateTime.Now),
                                TreatmentEntryDto.Entry_Type,
                                UpdateEntryForm.Comment,
                                null, null,
                                UpdateEntryForm.UploadLink
                        );
                    } else
                    {
                        updateRequest = new UpdateEntryRequest(
                        TreatmentEntryDto.Entry_ID,
                        TreatmentEntryDto.Plan_ID,
                        UpdateEntryForm.Staff_ID, //in the future, replace with staff_id of whoever is logger in
                        DateOnly.FromDateTime(DateTime.Now),
                        TreatmentEntryDto.Entry_Type,
                        UpdateEntryForm.Comment,
                        UpdateEntryForm.Drug_ID,
                        UpdateEntryForm.Posology,
                        UpdateEntryForm.UploadLink
                        );
                    }

                    await TreatmentService.UpdatePlanEntry(updateRequest);
                    feedbackUpdate = "Updated entry ID #" + TreatmentEntryDto.Entry_ID + " for plan #" + TreatmentEntryDto.Plan_ID;
                    await GetEntryByID();
                }
            }
            catch (TreatmentPlanServiceException ex)
            {
                feedbackUpdate = ex.Message;
            }


        }

        protected async Task DeleteEntry()
        {
            if (TreatmentEntryDto != null)
            {
                try
                {
                    await TreatmentService.DeleteTreatmentPlanEntry(SelectedEntryId);
                    feedbackUpdate = "Deleted entry ID #" + SelectedEntryId + " for plan #" + TreatmentEntryDto.Plan_ID;
                }
                catch (TreatmentPlanServiceException ex)
                {
                    feedbackUpdate = ex.Message;
                }
            }
        }

    }



    public class EditOrCreateTreatmentPlanForm // this is data transfer obj, move info. from form in 
    {
        public int Patient_Id { get; set; } 
        public int Staff_Id { get; set; } 
        public int Plan_CycleLen { get; set; } = 0;
        public DateOnly Plan_Date { get; set; } = DateOnly.FromDateTime(DateTime.Now); //set the default date as today in the form
        public string Plan_Status { get; set; } = "OPEN"; //= PlanStatus.New.ToString();
        public string Plan_Observation { get; set; } = string.Empty;

        public string Validate()
        {
            if (Patient_Id == null || Patient_Id < 1) return "Missing Patient Id";
            if (Staff_Id == null || Staff_Id < 1) return "Missing Staff Id";
            return string.Empty;
        }
    }

    public class PrescriptionForm
    {
        public string EntryType { get; set; } = "PRESCRIPTION";
        public int Drug_ID { get; set; }
        public string Posology { get; set; } = string.Empty;
        public string Comment { get; set; } = string.Empty;

        public string Validate()
        {
            if (string.IsNullOrEmpty(EntryType)) return "Missing entry type";
            if (Drug_ID == null || Drug_ID < 1) return "Missing drug id";
            if (string.IsNullOrEmpty(Posology)) return "Missing posology";
            if (string.IsNullOrEmpty(Comment)) Comment = string.Empty;
            return string.Empty;
        }
    }

    public class ScheduleTestForm
    {
        public string EntryType { get; set; }
        public string Comment { get; set; } = string.Empty;
        public string UploadLink { get; set; } = string.Empty;

        public string Validate()
        {
            if (string.IsNullOrEmpty(EntryType)) return "Missing entry type";
            if (string.IsNullOrEmpty(Comment)) Comment = string.Empty;
            if (string.IsNullOrEmpty(UploadLink)) UploadLink = string.Empty;
            return string.Empty;
        }
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
        public string Posology { get; set; } = string.Empty;
        [AllowNull]
        public string Comment { get; set; } = string.Empty;
        [AllowNull]
        public string UploadLink { get; set; } = string.Empty;

        public string Validate()
        {
            if (Entry_ID == null || Entry_ID < 1) return "Missing Plan Id";
            if (Plan_ID == null || Plan_ID < 1) return "Missing Plan Id";
            if (Staff_ID == null || Staff_ID < 1) return "Missing Staff Id";
            if (string.IsNullOrEmpty(EntryType)) return "Missing entry type";
            if (EntryType == "PRESCRIPTION")
            {
                if (Drug_ID == null || Drug_ID < 1) return "Missing drug id";
                if (string.IsNullOrEmpty(Posology)) return "Missing posology";
            } 
            return string.Empty;
        }
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

