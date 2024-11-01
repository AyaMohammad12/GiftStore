using GiftStore.Data;
using GiftStore.Models;
using GiftStore.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GiftStore.Controllers
{
	public class AccountController : Controller
	{
		#region ConfigurationCode
		MyDbContext db;
		UserManager<ApplicationUser> userManager;
		SignInManager<ApplicationUser> signInManager;
		RoleManager<IdentityRole> roleManager;
		public AccountController(MyDbContext _db, UserManager<ApplicationUser> _userManager, SignInManager<ApplicationUser> _signInManager, RoleManager<IdentityRole> _roleManager)
		{
			db = _db;
			userManager = _userManager;
			signInManager = _signInManager;
			roleManager = _roleManager;
		}
		#endregion
		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Register()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Register(RegisterViewModel model)
		{
			if (ModelState.IsValid)
			{
				ApplicationUser user = new ApplicationUser
				{
					UserName = model.Email,
					Email = model.Email,
					PhoneNumber = model.Mobile
				};
				var result = await userManager.CreateAsync(user, model.Password);
				if (result.Succeeded)
				{
					return RedirectToAction("Login");
				}
				else
				{
					foreach (var err in result.Errors)
					{
						ModelState.AddModelError(err.Code, err.Description);

					}
				}
			}
			return View();
		}
		public IActionResult login()
		{
			return View();
		}
        //[HttpPost]
        //public async Task<IActionResult> login(LoginViewModel model)
        //{
        //	if (ModelState.IsValid)
        //	{
        //		//IdentityResult r = null;

        //		var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
        //		if (result.Succeeded)
        //		{

        //			//return RedirectToAction("Index", "CPanel");
        //			//return RedirectToAction("Index", "CPanel", new { area = "Administrator" });
        //			return RedirectToAction("Index", "Home");

        //		}
        //		ModelState.AddModelError("", "Invalid login attempt.");
        //	}
        //	return View(model);
        //}


        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
                if (result.Succeeded)
                {
                    // الحصول على المستخدم الحالي
                    var user = await userManager.FindByEmailAsync(model.Email);

                    if (user != null)
                    {
                        // التحقق مما إذا كان المستخدم لديه دور "Admin"
                        var isAdmin = await userManager.IsInRoleAsync(user, "Admin");

                        if (isAdmin)
                        {
                            return RedirectToAction("Index", "Gifts", new { area = "Administrator" });
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                }
                ModelState.AddModelError("", "Invalid login attempt.");
            }
            return View(model);
        }



        //public async Task<IActionResult> Login(LoginViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        // التحقق من بيانات تسجيل الدخول
        //        var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
        //        if (result.Succeeded)
        //        {
        //            // الحصول على المستخدم الحالي
        //            var user = await userManager.FindByEmailAsync(model.Email);

        //            if (user != null)
        //            {
        //                // التحقق من دور المستخدم
        //                var roles = await userManager.GetRolesAsync(user);

        //                // إذا كان المستخدم لديه الدور المطلوب (مثلاً "Admin")
        //                if (roles.Contains("Admin"))
        //                {
        //                    return RedirectToAction("Index", "CPanel", new { area = "Administrator" });
        //                }
        //                else
        //                {
        //                    return RedirectToAction("Index", "Home");
        //                }
        //            }
        //        }

        //        ModelState.AddModelError("", "Invalid login attempt.");
        //    }

        //    return View(model);
        //}









        //[HttpPost]
        public async Task<IActionResult> Logout()
		{
			await signInManager.SignOutAsync();
			return RedirectToAction("Login");
		}
	}
}
