using Microsoft.AspNetCore.Mvc.Rendering;

namespace AppointmentScheduler.Utilities
{
    public static class Helper
    {
        public static string Admin = "Admin";
        public static string Patient = "Patient";
        public static string Doctor = "Doctor";

        public static List<SelectListItem> GetRoleforDropDown()
        {
            return new List<SelectListItem> 
            { 
                new SelectListItem{Value = Helper.Admin,Text=Helper.Admin }, 
                new SelectListItem{Value = Helper.Patient,Text=Helper.Patient }, 
                new SelectListItem{Value = Helper.Doctor,Text=Helper.Doctor } 
            }; 
        }
    }
}
