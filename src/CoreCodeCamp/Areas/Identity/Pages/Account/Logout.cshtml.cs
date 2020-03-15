using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreCodeCamp.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoreCodeCamp
{
  public class LogoutModel : PageModel
  {
    private readonly SignInManager<CodeCampUser> _signInManager;

    public LogoutModel(SignInManager<CodeCampUser> signInManager)
    {
      _signInManager = signInManager;
    }
    public void OnGet()
    {
      _signInManager.SignOutAsync();
      this.Redirect("/");
    }
  }
}