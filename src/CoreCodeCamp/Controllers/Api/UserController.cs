using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CoreCodeCamp.Data;
using CoreCodeCamp.Data.Entities;
using CoreCodeCamp.Models.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CoreCodeCamp.Controllers.Api
{
  [Route("api/users")]
  [Authorize(Roles = "Admin")]
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
        vms.ElementAt(x).IsAdmin = await _userMgr.IsInRoleAsync(users.ElementAt(x), "Admin");
      }            

      return Ok(vms);
    }

  }
}
