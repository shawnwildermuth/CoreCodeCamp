using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CoreCodeCamp.Data;
using CoreCodeCamp.Data.Entities;
using CoreCodeCamp.Models.Admin;
using CoreCodeCamp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CoreCodeCamp.Controllers.Api
{
  [Route("api/users")]
  [Authorize(Roles = Consts.AdminRole)]
  public class UserController : Controller
  {
    private ICodeCampRepository _repo;
    private SignInManager<CodeCampUser> _signInMgr;
    private UserManager<CodeCampUser> _userMgr;

    public UserController(ICodeCampRepository repo, UserManager<CodeCampUser> userMgr, SignInManager<CodeCampUser> signInMgr)
    {
      _repo = repo;
      _userMgr = userMgr;
      _signInMgr = signInMgr;
    }

    [HttpGet("")]
    public async Task<IActionResult> Get()
    {
      var users = _repo.GetUsers().Where(u => u.UserName != User.Identity.Name); // Don't include the current user
      var vms = Mapper.Map<IEnumerable<CodeCampUserViewModel>>(users);

      for (var x = 0; x < users.Count(); ++x)
      {
        var element = vms.ElementAt(x);
        var usr = users.ElementAt(x);
        element.IsAdmin = await _userMgr.IsInRoleAsync(usr, Consts.AdminRole);
        element.IsEmailConfirmed = usr.EmailConfirmed;
      }

      return Ok(vms);
    }

    [HttpPut("toggleAdmin")]
    public async Task<IActionResult> ToggleAdmin([FromBody]CodeCampUserViewModel vm)
    {
      if (ModelState.IsValid)
      {
        var user = await _userMgr.FindByNameAsync(vm.UserName);

        if (user != null)
        {
          var isAdmin = await _userMgr.IsInRoleAsync(user, Consts.AdminRole);
          IdentityResult result;
          if (isAdmin)
          {
            result = await _userMgr.RemoveFromRoleAsync(user, Consts.AdminRole);
          }
          else
          {
            result = await _userMgr.AddToRoleAsync(user, Consts.AdminRole);
          }

          if (result.Succeeded)
          {
            return Ok(!isAdmin);
          }
        }
      }

      return BadRequest("Could not update roles.");
    }

  }
}
