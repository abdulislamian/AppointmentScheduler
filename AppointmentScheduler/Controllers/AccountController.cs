using AppointmentScheduler.Data;
using AppointmentScheduler.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentScheduler.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        SignInManager<ApplicationUser> _signInManager;
        UserManager<ApplicationUser> _userManager;
        RoleManager<IdentityRole>  _roleManager;

        public AccountController(ApplicationDbContext dbContext, 
        SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager)
        {
            this.dbContext = dbContext;
            _signInManager= signInManager;
            _userManager= userManager;
            _roleManager= roleManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        public  IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(loginModel.Email, loginModel.Password, loginModel.RememberMe,false);
                if(result.Succeeded)
                {
                    return RedirectToAction("Index", "Appointment");
                }
            }
            ModelState.AddModelError("", "Invalid Login attempt");
            return View(loginModel);
        }
        public async Task<IActionResult> Register()
        {
            if(!_roleManager.RoleExistsAsync(Utilities.Helper.Admin).GetAwaiter().GetResult())
            {
                await _roleManager.CreateAsync(new IdentityRole(Utilities.Helper.Admin));
                await _roleManager.CreateAsync(new IdentityRole(Utilities.Helper.Patient));
                await _roleManager.CreateAsync(new IdentityRole(Utilities.Helper.Doctor));
            }
            
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterAsync(RegisterModel registerModel)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = registerModel.EmailAddress,
                    Email = registerModel.EmailAddress,
                    Name = registerModel.Name
                };

                var result = await _userManager.CreateAsync(user,registerModel.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, registerModel.RoleName);
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index","Appointment");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login","Account");
        }

    }
}
