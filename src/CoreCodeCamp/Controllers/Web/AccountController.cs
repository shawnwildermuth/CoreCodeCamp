using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using CoreCodeCamp.Models;
using CoreCodeCamp.Models.AccountViewModels;
using CoreCodeCamp.Data;
using CoreCodeCamp.Data.Entities;
using CoreCodeCamp.Services;
using CoreCodeCamp.Models.Emails;
using AutoMapper;

namespace CoreCodeCamp.Controllers.Web
{
  [Authorize]
  public class AccountController : MonikerControllerBase
  {
    private readonly UserManager<CodeCampUser> _userManager;
    private readonly SignInManager<CodeCampUser> _signInManager;
    private IMailService _mailService;

    public AccountController(
        UserManager<CodeCampUser> userManager,
        SignInManager<CodeCampUser> signInManager,
        ILogger<AccountController> logger,
        IMailService mailService,
        ICodeCampRepository repo,
        IMapper mapper) : base(repo, logger, mapper)
    {
      _userManager = userManager;
      _signInManager = signInManager;
      _mailService = mailService;
    }

    //
    // GET: /Account/Login
    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login(string returnUrl = null)
    {
      ViewData["ReturnUrl"] = returnUrl;
      return View();
    }

    //
    // POST: /Account/Login
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
    {
      ViewData["ReturnUrl"] = returnUrl;

      if (ModelState.IsValid)
      {
        var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
        if (result.Succeeded)
        {
          _logger.LogInformation(1, "User logged in.");


          return RedirectToLocal(returnUrl);
        }
        else
        {
          var user = await _userManager.FindByEmailAsync(model.Email);
          if (user != null && !user.EmailConfirmed)
          {
            ModelState.AddModelError(string.Empty, "You must confirm your email address before logging in.");
          }
          else
          {
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
          }
        }
      }

      // If we got this far, something failed, redisplay form
      return View(model);
    }

    //
    // GET: /Account/Join
    [HttpGet]
    [AllowAnonymous]
    public IActionResult Join(string returnUrl = null)
    {
      ViewData["ReturnUrl"] = returnUrl;
      return View();
    }

    //
    // POST: /Account/Join
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Join(RegisterViewModel model, string returnUrl = null)
    {
      ViewData["ReturnUrl"] = returnUrl;
      if (ModelState.IsValid)
      {
        var user = new CodeCampUser { UserName = model.Email, Email = model.Email, Name = model.Name };
        var result = await _userManager.CreateAsync(user, model.Password);
        if (result.Succeeded)
        {
          var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
          var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);
          if (!(await _mailService.SendTemplateMailAsync("ConfirmEmail", new AccountConfirmModel()
          {
            Name = model.Name,
            Email = model.Email,
            Subject = "Confirm your account",
            Callback = callbackUrl
          })))
          {
            _logger.LogError($"Failed to send out confirmation email, user created but can't confirm the account.");
            ModelState.AddModelError("", "Could not send out confirmation email, please contact us at codecamp@live.com for help.");
          }
          _logger.LogInformation(3, "User created a new account with password.");
          return View("ResendConfirmEmailSent");
        }
        AddErrors(result);
      }

      // If we got this far, something failed, redisplay form
      return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> LogOff()
    {
      await _signInManager.SignOutAsync();
      _logger.LogInformation(4, "User logged out.");
      return RedirectToAction(nameof(RootController.Index), "Root");
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult ResendConfirmEmail()
    {
      return View();
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> ResendConfirmEmail(string email)
    {
      if (ModelState.IsValid)
      {
        var user = await _userManager.FindByEmailAsync(email);

        if (user != null)
        {
          if (user.EmailConfirmed)
          {
            ModelState.AddModelError("", "Account already confirmed, please just login");
            return View();
          }
          else
          {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);
            await _mailService.SendTemplateMailAsync("ConfirmEmail", new AccountConfirmModel()
            {
              Email = email,
              Subject = "Confirm your account",
              Callback = callbackUrl
            });

            return View("ResendConfirmEmailSent");
          }
        }
        else
        {
          ModelState.AddModelError("", "No such email, please register.");
          return View();
        }
      }

      return View("Error");
    }


    // GET: /Account/ConfirmEmail
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> ConfirmEmail(string userId, string code)
    {
      if (userId == null || code == null)
      {
        return View("ResendConfirmEmail");
      }
      var user = await _userManager.FindByIdAsync(userId);
      if (user == null)
      {
        return View("ResendConfirmEmail");
      }
      var result = await _userManager.ConfirmEmailAsync(user, code);
      return View(result.Succeeded ? "ConfirmEmail" : "ResendConfirmEmail");
    }

    //
    // GET: /Account/ForgotPassword
    [HttpGet]
    [AllowAnonymous]
    public IActionResult ForgotPassword()
    {
      return View();
    }

    //
    // POST: /Account/ForgotPassword
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
    {
      if (ModelState.IsValid)
      {
        var user = await _userManager.FindByNameAsync(model.Email);
        if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
        {
          // Don't reveal that the user does not exist or is not confirmed
          return View("ResendConfirmEmail");
        }

        // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=532713
        // Send an email with this link
        var code = await _userManager.GeneratePasswordResetTokenAsync(user);
        var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);
        if (!await _mailService.SendTemplateMailAsync("ResetPassword", new AccountConfirmModel()
        {
          Email = model.Email,
          Subject = "Forgot Your Password",
          Callback = callbackUrl
        }))
        {
          ModelState.AddModelError("", "Failed to send email. Please send an email to codecamp@live.com and we'll fix the issue.");
          return View();
        }
        return View("ForgotPasswordConfirmation");
      }

      // If we got this far, something failed, redisplay form
      return View(model);
    }

    //
    // GET: /Account/ForgotPasswordConfirmation
    [HttpGet]
    [AllowAnonymous]
    public IActionResult ForgotPasswordConfirmation()
    {
      return View();
    }

    //
    // GET: /Account/ResetPassword
    [HttpGet]
    [AllowAnonymous]
    public IActionResult ResetPassword(string code = null)
    {
      return code == null ? View("Error") : View();
    }

    //
    // POST: /Account/ResetPassword
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
    {
      if (!ModelState.IsValid)
      {
        return View(model);
      }
      var user = await _userManager.FindByNameAsync(model.Email);
      if (user == null)
      {
        // Don't reveal that the user does not exist
        return RedirectToAction(nameof(AccountController.ResetPasswordConfirmation), "Account");
      }
      var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
      if (result.Succeeded)
      {
        return RedirectToAction(nameof(AccountController.ResetPasswordConfirmation), "Account");
      }
      AddErrors(result);
      return View();
    }

    //
    // GET: /Account/ResetPasswordConfirmation
    [HttpGet]
    [AllowAnonymous]
    public IActionResult ResetPasswordConfirmation()
    {
      return View();
    }


    #region Helpers

    private void AddErrors(IdentityResult result)
    {
      foreach (var error in result.Errors)
      {
        ModelState.AddModelError(string.Empty, error.Description);
      }
    }

    private Task<CodeCampUser> GetCurrentUserAsync()
    {
      return _userManager.GetUserAsync(HttpContext.User);
    }

    private IActionResult RedirectToLocal(string returnUrl)
    {
      if (Url.IsLocalUrl(returnUrl))
      {
        return Redirect(returnUrl);
      }
      else
      {
        return RedirectToAction(nameof(RootController.Index), "Root");
      }
    }

    #endregion
  }
}
