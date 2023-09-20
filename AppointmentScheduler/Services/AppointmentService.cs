using AppointmentScheduler.Data;
using AppointmentScheduler.Models;
using AppointmentScheduler.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using System.Globalization;

namespace AppointmentScheduler.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly ApplicationDbContext dbContext;
        SignInManager<ApplicationUser> _signInManager;
        UserManager<ApplicationUser> _userManager;
        RoleManager<IdentityRole> _roleManager;

        public AppointmentService(ApplicationDbContext dbContext, SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager)
        {
            this.dbContext = dbContext;
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<int> AddUpdate(AppointmentVM model)
        {
            string dateFormat = "M/d/yyyy h:mm tt";
            var startDate = DateTime.ParseExact(model.StartDate, dateFormat, CultureInfo.InvariantCulture);
            var endDate = startDate.AddMinutes(Convert.ToDouble(model.Duration));

            if (model != null && model.Id > 0)
            {
                //Updation
                return 1;
            }
            else
            {
                if (model != null)
                {
                    //Insertion
                    Appointment appointment = new Appointment()
                    {
                        Title = model.Title,
                        StartDate = startDate,
                        EndDate = endDate,
                        Description = model.Description,
                        Duration = model.Duration,
                        DoctorId = model.DoctorId,
                        PatientId = model.PatientId,
                        IsDoctorApproved = false
                    };
                    await dbContext.Appointments.AddAsync(appointment);
                    await dbContext.SaveChangesAsync();
                } 
                return 2;
            }
        }

        public List<AppointmentVM> DoctorEventsById(string doctorId)
        {
           return dbContext.Appointments.Where(x=>x.DoctorId == doctorId).ToList().Select(c=> new AppointmentVM()
           {
               Id=c.Id,
               Description = c.Description,
               StartDate = c.StartDate.ToString("yyyy-MM-dd HH:mm:ss"),
               EndDate = c.EndDate.ToString("yyyy-MM-dd HH:mm:ss"),
               Title=c.Title,
               Duration=c.Duration,
               IsDoctorApproved=c.IsDoctorApproved
           }).ToList();
        }
        public async Task<int> ConfirmEvent(int id)
        {
            var appointment = dbContext.Appointments.FirstOrDefault(x => x.Id == id);
            if (appointment != null)
            {
                appointment.IsDoctorApproved = true;
                return await dbContext.SaveChangesAsync();
            }
            return 0;
        }

        public async Task<int> Delete(int id)
        {
            var appointment = dbContext.Appointments.FirstOrDefault(x => x.Id == id);
            if (appointment != null)
            {
                dbContext.Appointments.Remove(appointment);
                return await dbContext.SaveChangesAsync();
            }
            return 0;
        }
        public async Task<AppointmentVM> GetById(int id)
        {
            var doctors = await _userManager.GetUsersInRoleAsync(Utilities.Helper.Doctor);
            var patients = await _userManager.GetUsersInRoleAsync(Utilities.Helper.Patient);
            var data= dbContext.Appointments.Where(x => x.Id == id).ToList().Select(c => new AppointmentVM()
            {
                Id = c.Id,
                Description = c.Description,
                StartDate = c.StartDate.ToString("yyyy-MM-dd HH:mm:ss"),
                EndDate = c.EndDate.ToString("yyyy-MM-dd HH:mm:ss"),
                Title = c.Title,
                Duration = c.Duration,
                IsDoctorApproved = c.IsDoctorApproved,
                PatientId = c.PatientId,
                DoctorId = c.DoctorId,
                PatientName = patients.Where(x => x.Id == c.PatientId).Select(x => x.Name).FirstOrDefault(),
                DoctorName =  doctors.Where(x => x.Id == c.DoctorId).Select(x => x.Name).FirstOrDefault()
            }).SingleOrDefault();
            return data;
        }

        public async Task<List<DoctorVM>> GetDoctorlist()
        {
            var Doctors = await _userManager.GetUsersInRoleAsync(Utilities.Helper.Doctor);

            var doctorVM = Doctors.Select(user => new DoctorVM
            {
                Id = user.Id,
                Name = user.Name,
            }).ToList();


            return doctorVM;
        }

        public async Task<List<PatientVM>> GetPatientlist()
        {
            var patients = await _userManager.GetUsersInRoleAsync(Utilities.Helper.Patient);

            var patientVM = patients.Select(user => new PatientVM
            {
                Id = user.Id,
                Name = user.Name,
            }).ToList();


            return patientVM;
        }

        public List<AppointmentVM> PatientEventsById(string patientId)
        {
            return dbContext.Appointments.Where(x => x.PatientId == patientId).ToList()
                   .Select(c => new AppointmentVM()
                    {
                        Id = c.Id,
                        Description = c.Description,
                        StartDate = c.StartDate.ToString("yyyy-MM-dd HH:mm:ss"),
                        EndDate = c.EndDate.ToString("yyyy-MM-dd HH:mm:ss"),
                        Title = c.Title,
                        Duration = c.Duration,
                        IsDoctorApproved = c.IsDoctorApproved
                    }).ToList();
        }
    }
}
