using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using BotDetect.Web.Mvc;
using Maple2.AdminLTE.Bel;
using Maple2.AdminLTE.Bll;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Maple2.AdminLTE.Uil.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(SignInManager<ApplicationUser> signInManager, ILogger<LoginModel> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }

            [Required]
            public string CaptchaCode { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        [CaptchaValidation("CaptchaCode", "ExampleCaptcha", "Incorrect CAPTCHA code!")]
        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;

            if (ModelState.IsValid)
            {
                // init mvcCaptcha instance with captchaId 
                MvcCaptcha mvcCaptcha = new MvcCaptcha("ExampleCaptcha");

                // get validatingInstanceId from HttpContext.Request.Form 
                string validatingInstanceId = HttpContext.Request.Form[mvcCaptcha.ValidatingInstanceKey];

                if (mvcCaptcha.Validate(Input.CaptchaCode, validatingInstanceId))
                {
                    // or you can use static Validate method of MvcCaptcha class 
                    MvcCaptcha.ResetCaptcha("ExampleCaptcha");

                    var user = await _signInManager.UserManager.FindByEmailAsync(Input.Email);


                    if (user != null)
                    {
                        // This doesn't count login failures towards account lockout
                        // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                        var result = await _signInManager.PasswordSignInAsync(user, Input.Password, Input.RememberMe, lockoutOnFailure: true);
                        if (result.Succeeded)
                        {
                            _logger.LogInformation("User logged in.");

                            //set user id for Application user
                            var appUser = await _signInManager.UserManager.FindByIdAsync(user.Id);
                            if(appUser != null)
                            {
                                using (var userBll = new UserBLL())
                                {
                                    appUser.UserId = await userBll.GetSystemUserId(user.Id);  //
                                }

                                await _signInManager.UserManager.UpdateAsync(appUser);
                            }
                            

                            return LocalRedirect(Url.GetLocalUrl(returnUrl));
                        }
                        if (result.RequiresTwoFactor)
                        {
                            return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                        }
                        if (result.IsLockedOut)
                        {
                            _logger.LogWarning("User account locked out.");
                            return RedirectToPage("./Lockout");
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Email or password is incorrect.");
                            return Page();
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "User with this email not found");
                        return Page();
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Picture Code Incorrect!");
                    return Page();
                }

                
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
