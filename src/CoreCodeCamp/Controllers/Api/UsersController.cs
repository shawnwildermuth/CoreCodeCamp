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
using Microsoft.Extensions.Logging;

namespace CoreCodeCamp.Controllers.Api
{
  [Route("api/users")]
  [Authorize(Roles = Consts.ADMINROLE)]
  public class UsersController : Controller
  {
    private ILogger<UsersController> _logger;
    private ICodeCampRepository _repo;
    private SignInManager<CodeCampUser> _signInMgr;
    private UserManager<CodeCampUser> _userMgr;

    public UsersController(ICodeCampRepository repo, 
      UserManager<CodeCampUser> userMgr, 
      SignInManager<CodeCampUser> signInMgr,
      ILogger<UsersController> logger)
    {
      _logger = logger;
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
        element.IsAdmin = await _userMgr.IsInRoleAsync(usr, Consts.ADMINROLE);
        element.IsEmailConfirmed = usr.EmailConfirmed;
      }

      return Ok(vms);
    }

    [HttpGet("{name}")]
    public async Task<IActionResult> Get(string name)
    {
      var user = await _userMgr.FindByNameAsync(name);
      var vm = Mapper.Map<CodeCampUserViewModel>(user);
      vm.IsAdmin = await _userMgr.IsInRoleAsync(user, Consts.ADMINROLE);

      return Ok(vm);
    }

    [HttpPut("{name}")]
    public async Task<IActionResult> Put(string name, [FromBody]CodeCampUserViewModel vm)
    {
      var user = await _userMgr.FindByNameAsync(name);
      Mapper.Map<CodeCampUserViewModel, CodeCampUser>(vm, user);
      await _repo.SaveChangesAsync();

      return Ok(Mapper.Map<CodeCampUserViewModel>(user));
    }

    [HttpPut("{name}/toggleAdmin")]
    public async Task<IActionResult> ToggleAdmin(string name, [FromBody]CodeCampUserViewModel vm)
    {
      if (ModelState.IsValid)
      {
        var user = await _userMgr.FindByNameAsync(name);

        if (user != null)
        {
          var isAdmin = await _userMgr.IsInRoleAsync(user, Consts.ADMINROLE);
          IdentityResult result;
          if (isAdmin)
          {
            result = await _userMgr.RemoveFromRoleAsync(user, Consts.ADMINROLE);
          }
          else
          {
            result = await _userMgr.AddToRoleAsync(user, Consts.ADMINROLE);
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
