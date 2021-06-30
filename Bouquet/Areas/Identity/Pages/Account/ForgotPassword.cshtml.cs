using System;
using System.Collections.Generic;
using System.IO;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.AspNetCore.Hosting;
using Bouquet.DataAccess.Data;
using System.Linq;

namespace Bouquet.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly ApplicationDbContext _db;

        public ForgotPasswordModel(UserManager<IdentityUser> userManager, IEmailSender emailSender, IWebHostEnvironment hostEnvironment, ApplicationDbContext db)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _hostEnvironment = hostEnvironment;
            _db = db;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(Input.Email);
                var userDb = _db.ApplicationUsers.FirstOrDefault(u => u.Email == Input.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return RedirectToPage("./ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please 
                // visit https://go.microsoft.com/fwlink/?LinkID=532713
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                    "/Account/ResetPassword",
                    pageHandler: null,
                    values: new { area = "Identity", code },
                    protocol: Request.Scheme);

                var pathToFile = _hostEnvironment.WebRootPath + Path.DirectorySeparatorChar.ToString()
                      + "Templates" + Path.DirectorySeparatorChar.ToString() + "EmailTemplates"
                      + Path.DirectorySeparatorChar.ToString() + "Password_Resend.html";


                var subject = "Password Reset";
                string HtmlBody = "";
                using (StreamReader streamReader = System.IO.File.OpenText(pathToFile))
                {
                    HtmlBody = streamReader.ReadToEnd();
                }

                string message = $"Please click to reset password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.";
                string messageBody = string.Format(HtmlBody,
                    subject,
                    String.Format("{0:dddd, d MMMM yyyy}", DateTime.Now),
                    userDb.Name,
                    user.Email,
                    message,
                    callbackUrl
                    );
                await _emailSender.SendEmailAsync(Input.Email, "Reset Password", messageBody);            

                return RedirectToPage("./ForgotPasswordConfirmation");
            }

            return Page();
        }
    }
}
