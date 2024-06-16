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
            searchResultList = await PatientService.SearchPatient(SearchPatientForm);
        }

        protected async Task GetPatient() 
        {
            PatientDTO = await PatientService.GetPatientById(PatientSearchId);
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
            var comment = CreatePatientForm.Validate();
            if(string.IsNullOrEmpty(comment))
            {
                NewPatientRequestDto patientRequestDto = new NewPatientRequestDto(
                                            CreatePatientForm.Patient_fName,
                                            CreatePatientForm.Patient_lName,
                                            CreatePatientForm.Patient_DoB,
                                            CreatePatientForm.Patient_PhoneNum,
                                            CreatePatientForm.Patient_Allergy
                                );

                try
                {
                    PatientDto? NewPatient = await PatientService.CreateNewPatient(patientRequestDto);
                    feedbackCreation = "New Patient created ID # " + NewPatient?.Patient_ID;
                    await Search(); //refresh list
                } catch (PatientServiceException ex)
                {
                    feedbackCreation = ex.Message;
                }
            } else feedbackCreation = comment;
            
        }

        protected async Task Update() 
        {
            var comment = EditPatientForm.Validate();
            if (string.IsNullOrEmpty(comment))
            {
                UpdatePatientRequestDto patientRequestDto = new UpdatePatientRequestDto(
                    PatientSearchId,
                    EditPatientForm.Patient_fName,
                    EditPatientForm.Patient_lName,
                    EditPatientForm.Patient_DoB,
                    EditPatientForm.Patient_PhoneNum,
                    EditPatientForm.Patient_Allergy
                );

                try
                {
                    PatientDto? NewPatient = await PatientService.UpdatePatient(patientRequestDto);
                    feedbackUpdate = "Patient ID #" + NewPatient?.Patient_ID + " updated";
                    await Search(); //refresh list
                }
                catch (PatientServiceException ex)
                {
                    feedbackUpdate = ex.Message;
                }
            }
            else feedbackUpdate = comment;

        }

        protected async Task Delete() 
        {
            try
            {
                await PatientService.DeletePatient(PatientSearchId);
                feedbackUpdate = "Patient ID #" + PatientSearchId + " deleted";
                await Search(); //refresh list
            }
            catch (PatientServiceException ex)
            {
                feedbackUpdate = ex.Message;
            }
        }
        

    }




    public class EditOrCreatePatientForm // this is data transfer obj, move info. from form in 
    {
        public string Patient_fName { get; set; } = string.Empty;
        public string Patient_lName { get; set; } = string.Empty;
        public DateOnly Patient_DoB { get; set; } = DateOnly.FromDateTime(DateTime.Now); //set the default date as today in the form
        public string Patient_PhoneNum { get; set; } = string.Empty;
        public string Patient_Allergy { get; set; } = string.Empty;// =string.Empty does not have any value yet

        public string Validate() //check all values are ok before 
        {
            if (string.IsNullOrEmpty(Patient_fName)) return "Missing a first name";
            if (string.IsNullOrEmpty(Patient_lName)) return "Missing a last name";
            if (string.IsNullOrEmpty(Patient_PhoneNum)) return "Missing a phone number";
            if (string.IsNullOrEmpty(Patient_Allergy))
            {
                Patient_Allergy = string.Empty; //this one is not mandatory so we're just making sure there's no null value
            }
            return string.Empty; 
        }
    }

    public class SearchPatientForm
    {
        public int Patient_Id { get; set; }
        public string Patient_fName { get; set; } //need to co-relate these to backend controller so to function
        public string Patient_lName { get; set; }
    }

}
