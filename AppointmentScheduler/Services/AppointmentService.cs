using AppointmentScheduler.Data;
using AppointmentScheduler.Models.ViewModels;

namespace AppointmentScheduler.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly ApplicationDbContext dbContext;

        public AppointmentService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public List<DoctorVM> GetDoctorlist()
        {
            var doctors = (from user in dbContext.Users
                           join userRoles in dbContext.UserRoles on user.Id equals userRoles.UserId
                           join roles in dbContext.Roles.Where(x => x.Name == Utilities.Helper.Doctor) on userRoles.RoleId equals roles.Id
                           select new DoctorVM
                           {
                               Id = user.Id,
                               Name = user.Name,

                           }).ToList();
            return doctors;
        }

        public List<PatientVM> GetPatientlist()
        {
            var patients = (from user in dbContext.Users
                           join userRoles in dbContext.UserRoles on user.Id equals userRoles.UserId
                           join roles in dbContext.Roles.Where(x => x.Name == Utilities.Helper.Patient) on userRoles.RoleId equals roles.Id
                           select new PatientVM
                           {
                               Id = user.Id,
                               Name = user.Name,

                           }).ToList();
            return patients;
        }
    }
}
