using AppointmentScheduler.Data;
using AppointmentScheduler.Services;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentScheduler.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewBag.Duration = Utilities.Helper.GetTimeDropDown();
            ViewBag.DoctorList  = await _appointmentService.GetDoctorlist();
            ViewBag.PatientList = await _appointmentService.GetPatientlist();
            return View();
        }

    }
}
