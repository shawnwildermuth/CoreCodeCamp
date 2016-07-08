using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CoreCodeCamp.Models;
using CoreCodeCamp.Models.ManageViewModels;
using CoreCodeCamp.Data;
using CoreCodeCamp.Data.Entities;

namespace CoreCodeCamp.Controllers.Web
{
  [Authorize]
  [Route("/you/")]
  public class ManageController : Controller
  {
    private readonly UserManager<CodeCampUser> _userManager;
    private readonly SignInManager<CodeCampUser> _signInManager;
    private readonly ILogger _logger;

    public ManageController(
    UserManager<CodeCampUser> userManager,
    SignInManager<CodeCampUser> signInManager,
    ILoggerFactory loggerFactory)
    {
      _userManager = userManager;
      _signInManager = signInManager;
      _logger = loggerFactory.CreateLogger<ManageController>();
    }

    //
    // GET: /Manage/Index
    [HttpGet("")]
    public async Task<IActionResult> Index(ManageMessageId? message = null)
    {
      ViewData["StatusMessage"] =
          message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
          : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
          : message == ManageMessageId.SetTwoFactorSuccess ? "Your two-factor authentication provider has been set."
          : message == ManageMessageId.Error ? "An error has occurred."
          : message == ManageMessageId.AddPhoneSuccess ? "Your phone number was added."
          : message == ManageMessageId.RemovePhoneSuccess ? "Your phone number was removed."
          : "";

      var user = await GetCurrentUserAsync();
      if (user == null)
      {
        return View("Error");
      }
      var model = new IndexViewModel
      {
        HasPassword = await _userManager.HasPasswordAsync(user),
        PhoneNumber = await _userManager.GetPhoneNumberAsync(user),
        TwoFactor = await _userManager.GetTwoFactorEnabledAsync(user),
        Logins = await _userManager.GetLoginsAsync(user),
        BrowserRemembered = await _signInManager.IsTwoFactorClientRememberedAsync(user)
      };
      return View(model);
    }

    //
    // POST: /Manage/RemoveLogin
    [HttpPost("removelogin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RemoveLogin(RemoveLoginViewModel account)
    {
      ManageMessageId? message = ManageMessageId.Error;
      var user = await GetCurrentUserAsync();
      if (user != null)
      {
        var result = await _userManager.RemoveLoginAsync(user, account.LoginProvider, account.ProviderKey);
        if (result.Succeeded)
        {
          await _signInManager.SignInAsync(user, isPersistent: false);
          message = ManageMessageId.RemoveLoginSuccess;
        }
      }
      return RedirectToAction(nameof(ManageLogins), new { Message = message });
    }

    //
    // GET: /Manage/AddPhoneNumber
    [HttpGet("addphonenumber")]
    public IActionResult AddPhoneNumber()
    {
      return View();
    }

    //
    // POST: /Manage/AddPhoneNumber
    [HttpPost("addphonenumber")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddPhoneNumber(AddPhoneNumberViewModel model)
    {
      if (!ModelState.IsValid)
      {
        return View(model);
      }
      // Generate the token and send it
      var user = await GetCurrentUserAsync();
      if (user == null)
      {
        return View("Error");
      }
      var code = await _userManager.GenerateChangePhoneNumberTokenAsync(user, model.PhoneNumber);
      //await _smsSender.SendSmsAsync(model.PhoneNumber, "Your security code is: " + code);
      return RedirectToAction(nameof(VerifyPhoneNumber), new { PhoneNumber = model.PhoneNumber });
    }

    //
    // POST: /Manage/EnableTwoFactorAuthentication
    [HttpPost("EnableTwoFactorAuthentication")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EnableTwoFactorAuthentication()
    {
      var user = await GetCurrentUserAsync();
      if (user != null)
      {
        await _userManager.SetTwoFactorEnabledAsync(user, true);
        await _signInManager.SignInAsync(user, isPersistent: false);
        _logger.LogInformation(1, "User enabled two-factor authentication.");
      }
      return RedirectToAction(nameof(Index), "Manage");
    }

    //
    // POST: /Manage/DisableTwoFactorAuthentication
    [HttpPost("DisableTwoFactorAuthentication")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DisableTwoFactorAuthentication()
    {
      var user = await GetCurrentUserAsync();
      if (user != null)
      {
        await _userManager.SetTwoFactorEnabledAsync(user, false);
        await _signInManager.SignInAsync(user, isPersistent: false);
        _logger.LogInformation(2, "User disabled two-factor authentication.");
      }
      return RedirectToAction(nameof(Index), "Manage");
    }

    //
    // GET: /Manage/VerifyPhoneNumber
    [HttpGet("VerifyPhoneNumber")]
    public async Task<IActionResult> VerifyPhoneNumber(string phoneNumber)
    {
      var user = await GetCurrentUserAsync();
      if (user == null)
      {
        return View("Error");
      }
      var code = await _userManager.GenerateChangePhoneNumberTokenAsync(user, phoneNumber);
      // Send an SMS to verify the phone number
      return phoneNumber == null ? View("Error") : View(new VerifyPhoneNumberViewModel { PhoneNumber = phoneNumber });
    }

    //
    // POST: /Manage/VerifyPhoneNumber
    [HttpPost("VerifyPhoneNumber")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> VerifyPhoneNumber(VerifyPhoneNumberViewModel model)
    {
      if (!ModelState.IsValid)
      {
        return View(model);
      }
      var user = await GetCurrentUserAsync();
      if (user != null)
      {
        var result = await _userManager.ChangePhoneNumberAsync(user, model.PhoneNumber, model.Code);
        if (result.Succeeded)
        {
          await _signInManager.SignInAsync(user, isPersistent: false);
          return RedirectToAction(nameof(Index), new { Message = ManageMessageId.AddPhoneSuccess });
        }
      }
      // If we got this far, something failed, redisplay the form
      ModelState.AddModelError(string.Empty, "Failed to verify phone number");
      return View(model);
    }

    //
    // POST: /Manage/RemovePhoneNumber
    [HttpPost("RemovePhoneNumber")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RemovePhoneNumber()
    {
      var user = await GetCurrentUserAsync();
      if (user != null)
      {
        var result = await _userManager.SetPhoneNumberAsync(user, null);
        if (result.Succeeded)
        {
          await _signInManager.SignInAsync(user, isPersistent: false);
          return RedirectToAction(nameof(Index), new { Message = ManageMessageId.RemovePhoneSuccess });
        }
      }
      return RedirectToAction(nameof(Index), new { Message = ManageMessageId.Error });
    }

    //
    // GET: /Manage/ChangePassword
    [HttpGet("ChangePassword")]
    public IActionResult ChangePassword()
    {
      return View();
    }

    //
    // POST: /Manage/ChangePassword
    [HttpPost("ChangePassword")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
    {
      if (!ModelState.IsValid)
      {
        return View(model);
      }
      var user = await GetCurrentUserAsync();
      if (user != null)
      {
        var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
        if (result.Succeeded)
        {
          await _signInManager.SignInAsync(user, isPersistent: false);
          _logger.LogInformation(3, "User changed their password successfully.");
          return RedirectToAction(nameof(Index), new { Message = ManageMessageId.ChangePasswordSuccess });
        }
        AddErrors(result);
        return View(model);
      }
      return RedirectToAction(nameof(Index), new { Message = ManageMessageId.Error });
    }

    //
    // GET: /Manage/SetPassword
    [HttpGet("SetPassword")]
    public IActionResult SetPassword()
    {
      return View();
    }

    //
    // POST: /Manage/SetPassword
    [HttpPost("SetPassword")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SetPassword(SetPasswordViewModel model)
    {
      if (!ModelState.IsValid)
      {
        return View(model);
      }

      var user = await GetCurrentUserAsync();
      if (user != null)
      {
        var result = await _userManager.AddPasswordAsync(user, model.NewPassword);
        if (result.Succeeded)
        {
          await _signInManager.SignInAsync(user, isPersistent: false);
          return RedirectToAction(nameof(Index), new { Message = ManageMessageId.SetPasswordSuccess });
        }
        AddErrors(result);
        return View(model);
      }
      return RedirectToAction(nameof(Index), new { Message = ManageMessageId.Error });
    }

    //GET: /Manage/ManageLogins
    [HttpGet("ManageLogins")]
    public async Task<IActionResult> ManageLogins(ManageMessageId? message = null)
    {
      ViewData["StatusMessage"] =
          message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
          : message == ManageMessageId.AddLoginSuccess ? "The external login was added."
          : message == ManageMessageId.Error ? "An error has occurred."
          : "";
      var user = await GetCurrentUserAsync();
      if (user == null)
      {
        return View("Error");
      }
      var userLogins = await _userManager.GetLoginsAsync(user);
      var otherLogins = _signInManager.GetExternalAuthenticationSchemes().Where(auth => userLogins.All(ul => auth.AuthenticationScheme != ul.LoginProvider)).ToList();
      ViewData["ShowRemoveButton"] = user.PasswordHash != null || userLogins.Count > 1;
      return View(new ManageLoginsViewModel
      {
        CurrentLogins = userLogins,
        OtherLogins = otherLogins
      });
    }

    //
    // POST: /Manage/LinkLogin
    [HttpPost("LinkLogin")]
    [ValidateAntiForgeryToken]
    public IActionResult LinkLogin(string provider)
    {
      // Request a redirect to the external login provider to link a login for the current user
      var redirectUrl = Url.Action("LinkLoginCallback", "Manage");
      var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl, _userManager.GetUserId(User));
      return Challenge(properties, provider);
    }

    //
    // GET: /Manage/LinkLoginCallback
    [HttpGet("LinkLoginCallback")]
    public async Task<ActionResult> LinkLoginCallback()
    {
      var user = await GetCurrentUserAsync();
      if (user == null)
      {
        return View("Error");
      }
      var info = await _signInManager.GetExternalLoginInfoAsync(await _userManager.GetUserIdAsync(user));
      if (info == null)
      {
        return RedirectToAction(nameof(ManageLogins), new { Message = ManageMessageId.Error });
      }
      var result = await _userManager.AddLoginAsync(user, info);
      var message = result.Succeeded ? ManageMessageId.AddLoginSuccess : ManageMessageId.Error;
      return RedirectToAction(nameof(ManageLogins), new { Message = message });
    }

    #region Helpers

    private void AddErrors(IdentityResult result)
    {
      foreach (var error in result.Errors)
      {
        ModelState.AddModelError(string.Empty, error.Description);
      }
    }

    public enum ManageMessageId
    {
      AddPhoneSuccess,
      AddLoginSuccess,
      ChangePasswordSuccess,
      SetTwoFactorSuccess,
      SetPasswordSuccess,
      RemoveLoginSuccess,
      RemovePhoneSuccess,
      Error
    }

    private Task<CodeCampUser> GetCurrentUserAsync()
    {
      return _userManager.GetUserAsync(HttpContext.User);
    }

    #endregion
  }
}
