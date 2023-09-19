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
        public IActionResult Index()
        {
            ViewBag.DoctorList  = _appointmentService.GetDoctorlist();
            ViewBag.PatientList = _appointmentService.GetPatientlist();
            return View();
        }
    }
}
