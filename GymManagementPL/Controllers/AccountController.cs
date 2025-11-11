using GymManagementBLL.Service.InterFaces;
using GymManagementBLL.ViewModels.AccountViewModel;
using GymManagementDAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;

namespace GymManagementPL.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public AccountController(IAccountService accountService , SignInManager<ApplicationUser> signInManager)
        {
            _accountService = accountService;
            _signInManager = signInManager;
        }
        #region Login
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(AccountViewModel accountView)
        {
           if(!ModelState.IsValid)  return View(accountView);

            var user = _accountService.ValidateUser(accountView);

            if(user is null)
            {
               ModelState.AddModelError("InvalidLogin", "Invalid Email or Password");
                return View(accountView);
            }
            var result = _signInManager.PasswordSignInAsync(user, accountView.Password, accountView.RememberMe, false).Result;
            if(result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError(string.Empty, "Invalid Email or Password");
            return View(accountView);
        }
        #endregion

        #region Login out
        public ActionResult Logout()
        {
            _signInManager.SignOutAsync().GetAwaiter().GetResult();
            return RedirectToAction(nameof(Login));
        }


        #endregion

        #region AccessDenied

        public ActionResult AccessDenied()
        {
            return View();
        }
        #endregion
    }
}
