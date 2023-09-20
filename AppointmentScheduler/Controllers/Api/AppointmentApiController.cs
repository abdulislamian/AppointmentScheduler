using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AppointmentScheduler.Services;
using System.Security.Claims;
using AppointmentScheduler.Models.ViewModels;
using AppointmentScheduler.Utilities;

namespace AppointmentScheduler.Controllers.Api
{
    [Route("api/Appointment")]
    [ApiController]
    public class AppointmentApiController : Controller
    {
        private readonly IAppointmentService _appointmentService;
        public IHttpContextAccessor _HttpContextAccessor { get; }
        private readonly string loginUserId;
        private readonly string role;

        public AppointmentApiController(IAppointmentService appointmentService,IHttpContextAccessor httpContextAccessor)
        {
            _appointmentService = appointmentService;
            _HttpContextAccessor = httpContextAccessor;
            loginUserId = _HttpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            role        = _HttpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);
        }


        [HttpPost]
        [Route("SaveCalendarData")]
        public async Task<IActionResult> SaveCalendarData(AppointmentVM data)
        {
            CommonResponse<int> commonResponse = new CommonResponse<int>();
            try
            {
                commonResponse.status =  _appointmentService.AddUpdate(data).Result;
                if(commonResponse.status == 1)
                {
                    commonResponse.message = Helper.appointmentUpdated;
                }
                if(commonResponse.status == 2)
                {
                    commonResponse.message = Helper.appointmentAdded;
                }
            }
            catch(Exception ex)
            {
                commonResponse.message = ex.Message;
                commonResponse.status = Helper.failure_code;
            }
            return Ok(commonResponse);
        }
        [HttpGet]
        [Route("GetCalendarData")]
        public IActionResult GetCalendarData(string doctorId)
        {
            CommonResponse<List<AppointmentVM>> commonResponse = new CommonResponse<List<AppointmentVM>>();
            try
            {
                if (role == Helper.Patient)
                {
                    commonResponse.dataenum = _appointmentService.PatientEventsById(loginUserId);
                    commonResponse.status = Helper.success_code;
                }
                else if (role == Helper.Doctor)
                {
                    commonResponse.dataenum = _appointmentService.DoctorEventsById(loginUserId);
                    commonResponse.status = Helper.success_code;
                }
                else
                {
                    commonResponse.dataenum = _appointmentService.DoctorEventsById(doctorId);
                    commonResponse.status = Helper.success_code;
                }
            }
            catch (Exception e)
            {
                commonResponse.message = e.Message;
                commonResponse.status = Helper.failure_code;
            }
            return Ok(commonResponse);
        }

        [HttpGet]
        [Route("GetCalendarData/{id}")]
        public async Task<IActionResult> GetCalendarDataById(int id)
        {
            CommonResponse<AppointmentVM> commonResponse = new CommonResponse<AppointmentVM>();
            try
            {
                    commonResponse.dataenum =await _appointmentService.GetById(id);
                    commonResponse.status = Helper.success_code;
            }
            catch (Exception e)
            {
                commonResponse.message = e.Message;
                commonResponse.status = Helper.failure_code;
            }
            return Ok(commonResponse);
        }
        [HttpGet]
        [Route("DeleteAppoinment/{id}")]
        public async Task<IActionResult> DeleteAppoinment(int id)
        {
            CommonResponse<int> commonResponse = new CommonResponse<int>();
            try
            {
                commonResponse.status = await _appointmentService.Delete(id);
                commonResponse.message = commonResponse.status == 1 ? Helper.appointmentDeleted : Helper.somethingWentWrong;

            }
            catch (Exception e)
            {
                commonResponse.message = e.Message;
                commonResponse.status = Helper.failure_code;
            }
            return Ok(commonResponse);
        }

        [HttpGet]
        [Route("ConfirmEvent/{id}")]
        public IActionResult ConfirmEvent(int id)
        {
            CommonResponse<int> commonResponse = new CommonResponse<int>();
            try
            {
                var result = _appointmentService.ConfirmEvent(id).Result;
                if (result > 0)
                {
                    commonResponse.status = Helper.success_code;
                    commonResponse.message = Helper.meetingConfirm;
                }
                else
                {

                    commonResponse.status = Helper.failure_code;
                    commonResponse.message = Helper.meetingConfirmError;
                }

            }
            catch (Exception e)
            {
                commonResponse.message = e.Message;
                commonResponse.status = Helper.failure_code;
            }
            return Ok(commonResponse);
        }
    }
}
