using AspNetCoreHero.ToastNotification.Abstractions;
using DentaEquip.BL.IRepositories;
using DentaEquip.BL.Repositories.EmailService;
using DentaEquip.BL.ViewModels.Account;
using DentaEquip.DAL.Constant;
using DentaEquip.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DentaEquip.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IEmailSender _emailSender;
        private readonly INotyfService notyf;
        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IEmailSender emailSender, INotyfService notyf)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this._emailSender = emailSender;
            this.notyf = notyf;
        }
        #region Registration (Sign Up) 
        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registration(RegisterViewModel modelvm)
        {

            try
            {

                if (ModelState.IsValid)
                {

                    var user = new ApplicationUser()
                    {

                        UserName = modelvm.Username,
                        Email = modelvm.Email,
                        Address = modelvm.Address
                    };

                    var result = await userManager.CreateAsync(user, modelvm.Password);

                    if (result.Succeeded)
                    {
                        var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
                        var confirmationLink = Url.Action(nameof(ConfirmEmail), "Account", new { token, email = user.Email }, Request.Scheme);

                        string body = System.IO.File.ReadAllText("wwwroot/Templates/EmailTemplate.html");
                        body = body.Replace("[Name]", modelvm.Username);
                        body = body.Replace("[Callback]", confirmationLink);

                        var message = new Message(new string[]
                        {
                            user.Email
                        }, "Activate Account", body, null);
                        await _emailSender.SendEmailAsync(message);
                        var log = await userManager.AddToRoleAsync(user, Roles.userrole);
                        if (log is not null)
                        {
                            notyf.Success("Register is Done, Please Activate Your Account To Login Check Gmail ", 10);
                        }
                        return RedirectToAction(nameof(Login));

                    }
                    else
                    {
                        foreach (var item in result.Errors)
                        {
                            ModelState.AddModelError("", item.Description);
                        }
                    }


                }

                return View(modelvm);

            }
            catch (Exception)
            {
                ApplicationUser user = await userManager.FindByNameAsync(modelvm.Username);
                if (user is not null)
                {
                  await userManager.DeleteAsync(user);
                }
                notyf.Error("Check Internet Access", 10);
                return View(modelvm);
            }
        }
        #endregion


        #region EmailConfirm (Sign Up) 
        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
                return View("Error");
            var result = await userManager.ConfirmEmailAsync(user, token);
            if (result is not null)
            {
                notyf.Success("Activate Account Done,You Can Login", 10);
            }
            return View(result.Succeeded ? nameof(Login) : "Error");
        }

        [HttpGet]
        public IActionResult Error()
        {
            return View();
        }
        #endregion


        #region Login (Sign In)

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel modelvm)
        {

            try
            {

                if (ModelState.IsValid)
                {
                    var user = await userManager.FindByNameAsync(modelvm.Username);
                    if (user is not null)
                    {
                        if (!await userManager.IsEmailConfirmedAsync(user))
                        {
                            ModelState.AddModelError("", "You Must Have Confirmed Email To Login");
                            return View();
                        }
                        else
                        {

                            bool found = await userManager.CheckPasswordAsync(user, modelvm.Password);
                            if (found)
                            {
                                await signInManager.SignInAsync(user, modelvm.RememberMe);
                                notyf.Success("Loged In", 10);
                                return RedirectToAction("Index", "Home");

                            }
                        }

                    }
                    ModelState.AddModelError("", "Invalid UserName or Password");

                }

                return View(modelvm);

            }
            catch (Exception)
            {
                return View(modelvm);
            }
        }

        #endregion


        #region LogOff (Sign Out)

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            notyf.Success("LogOut", 10);
            return RedirectToAction("Login");
        }


        #endregion


        #region Forgot and Reset Password
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordModel forgotPasswordModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await userManager.FindByEmailAsync(forgotPasswordModel.Email);
                    if (user is null)
                    {

                        ModelState.AddModelError("", "Email Not Exist");
                    }
                    else
                    {
                        var token = await userManager.GeneratePasswordResetTokenAsync(user);
                        var callback = Url.Action(nameof(ResetPassword), "Account", new { token, email = user.Email }, Request.Scheme);

                        string body = System.IO.File.ReadAllText("wwwroot/Templates/EmailTemplate.html");
                        body = body.Replace("[Name]", user.UserName);
                        body = body.Replace("[Callback]", callback);

                        var message = new Message(new string[] { user.Email }, "Reset password", body, null);
                        await _emailSender.SendEmailAsync(message);

                        return RedirectToAction(nameof(ForgotPasswordConfirmation));
                    }


                }
                return View(forgotPasswordModel);

            }
            catch (Exception)
            {
                notyf.Error("Check Internet Access", 10);
                return View(forgotPasswordModel);
            }
        }

        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ResetPassword(string token, string email)
        {

            var model = new ResetPasswordModel { Token = token, Email = email };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel resetPasswordModel)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    var user = await userManager.FindByEmailAsync(resetPasswordModel.Email);
                    if (user is null)
                    {

                        ModelState.AddModelError("", "Email Not Exist");
                    }

                    var resetPassResult = await userManager.ResetPasswordAsync(user, resetPasswordModel.Token, resetPasswordModel.Password);
                    if (!resetPassResult.Succeeded)
                    {
                        foreach (var error in resetPassResult.Errors)
                        {
                            ModelState.TryAddModelError(error.Code, error.Description);
                        }

                        return View();
                    }
                    string name = User.Identity.Name;
                    if (string.IsNullOrWhiteSpace(name) == false)
                    {
                        await signInManager.SignOutAsync();
                        notyf.Success("Reset Password Done,Login By New Password", 10);
                        return RedirectToAction(nameof(Login));

                    }
                    notyf.Success("Reset Password Done,Login By New Password", 10);
                    return RedirectToAction(nameof(Login));
                }
                else
                {
                    return View(resetPasswordModel);
                }
            }
            catch
            {
                notyf.Error("Check Internet Access", 10);
                return View(resetPasswordModel);
            }

        }
        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            else
                return RedirectToAction(nameof(HomeController.Index), "Home");
        }
        #endregion
    }
}
