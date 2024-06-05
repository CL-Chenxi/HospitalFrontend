using HospitalFrontEnd.Data;
using Microsoft.AspNetCore.Components;

namespace HospitalFrontEnd.Components
{
    public class PatientComponent : ComponentBase
    {
        public PatientDto? PatientDTO { get; set; }   // 
        public EditOrCreatePatientForm CreatePatientForm { get; set; } = new EditOrCreatePatientForm();
        public EditOrCreatePatientForm EditPatientForm { get; set; } = new EditOrCreatePatientForm();
        public SearchPatientForm SearchPatientForm { get; set; } = new SearchPatientForm();

        public Boolean displayable = false;
        public string feedbackSearch = string.Empty;
        public string feedbackCreation = string.Empty;
        public string feedbackUpdate = string.Empty;
        public IEnumerable<PatientDto> searchResultList;

        [Parameter] //for single value to track , use this parameter. otherwise, methods includs many feilds
        public int PatientSearchId { get; set; }


        [Inject]
        private PatientService PatientService { get; set; }

        protected async Task Search()
        {
            searchResultList = await PatientService.SearchPatient<PatientDto>(SearchPatientForm);

        }

        protected async Task GetPatient() 
        {
            PatientDTO = await PatientService.GetPatientById<PatientDto>(PatientSearchId);
            if (PatientDTO == null) 
            {
                displayable = false;
                PatientDTO = null;  //does not exist in current context
                EditPatientForm = new EditOrCreatePatientForm();
                feedbackSearch = "No Patient Found";
            }
            else 
            {
                EditPatientForm.Patient_fName = PatientDTO.Patient_fName; //if DB find sth, user wants to change, so need to edit
                EditPatientForm.Patient_lName = PatientDTO.Patient_lName;
                EditPatientForm.Patient_DoB = PatientDTO.Patient_DoB;
                EditPatientForm.Patient_PhoneNum = PatientDTO.Patient_PhoneNum;
                EditPatientForm.Patient_Allergy = PatientDTO.Patient_Allergy;
                displayable = true; //this makes form to appear
                feedbackSearch = string.Empty;

            }
        }
        
        protected async Task Create() 
        {
            NewPatientRequestDto patientRequestDto = new NewPatientRequestDto();
            patientRequestDto.Patient_fName = CreatePatientForm.Patient_fName;
            patientRequestDto.Patient_lName = CreatePatientForm.Patient_lName;
            patientRequestDto.Patient_DoB = CreatePatientForm.Patient_DoB;
            patientRequestDto.Patient_PhoneNum = CreatePatientForm.Patient_PhoneNum;
            patientRequestDto.Patient_Allergy = CreatePatientForm.Patient_Allergy;
            feedbackCreation = await PatientService.CreateNewPatient<NewPatientRequestDto>(patientRequestDto);
        }

        protected async Task Update() 
        {
            UpdatePatientRequestDto patientRequestDto = new UpdatePatientRequestDto();
            patientRequestDto.Patient_ID = PatientSearchId;
            patientRequestDto.Patient_fName = EditPatientForm.Patient_fName;
            patientRequestDto.Patient_lName = EditPatientForm.Patient_lName;
            patientRequestDto.Patient_DoB = EditPatientForm.Patient_DoB;
            patientRequestDto.Patient_PhoneNum = EditPatientForm.Patient_PhoneNum;
            patientRequestDto.Patient_Allergy = EditPatientForm.Patient_Allergy;
            feedbackUpdate = await PatientService.UpdatePatient<UpdatePatientRequestDto>(patientRequestDto);
        }

        protected async Task Delete() 
        {
            feedbackUpdate = await PatientService.DeletePatient<UpdatePatientRequestDto>(PatientSearchId);
        }    
    }




    public class EditOrCreatePatientForm // this is data transfer obj, move info. from form in 
    {
        public string Patient_fName { get; set; } = string.Empty;
        public string Patient_lName { get; set; } = string.Empty;
        public DateOnly Patient_DoB { get; set; } = DateOnly.FromDateTime(DateTime.Now); //set the default date as today in the form
        public string Patient_PhoneNum { get; set; } = string.Empty;
        public string Patient_Allergy { get; set; } = string.Empty;// =string.Empty does not have any value yet

        public EditOrCreatePatientForm() { }// constructor?
    }

    public class SearchPatientForm
    {
        public int Patient_Id { get; set; }
        public string Patient_fName { get; set; } //need to co-relate these to backend controller so to function
        public string Patient_lName { get; set; }
    }

}
