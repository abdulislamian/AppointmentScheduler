using AppointmentScheduler.Models.ViewModels;

namespace AppointmentScheduler.Services
{
    public interface IAppointmentService
    {
        public Task<List<DoctorVM>> GetDoctorlist();
        public Task<List<PatientVM>> GetPatientlist();
        public Task<int> AddUpdate(AppointmentVM appointment);
        public List<AppointmentVM> DoctorEventsById(string doctorId);
        public List<AppointmentVM> PatientEventsById(string patientId);
        public Task<AppointmentVM> GetById(int id);
        public Task<int> Delete(int id);

        public Task<int> ConfirmEvent(int id);
    }
}
