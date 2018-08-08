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
  [ApiController]
  public class UsersController : Controller
  {
    private ILogger<UsersController> _logger;
    private readonly IMapper _mapper;
    private ICodeCampRepository _repo;
    private SignInManager<CodeCampUser> _signInMgr;
    private UserManager<CodeCampUser> _userMgr;

    public UsersController(ICodeCampRepository repo,
      UserManager<CodeCampUser> userMgr,
      SignInManager<CodeCampUser> signInMgr,
      ILogger<UsersController> logger,
      IMapper mapper)
    {
      _logger = logger;
      _mapper = mapper;
      _repo = repo;
      _userMgr = userMgr;
      _signInMgr = signInMgr;
    }

    [HttpGet("")]
    public async Task<IActionResult> Get()
    {
      var users = _repo.GetUsers().Where(u => u.UserName != User.Identity.Name); // Don't include the current user
      var vms = _mapper.Map<IEnumerable<CodeCampUserViewModel>>(users);

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
      var vm = _mapper.Map<CodeCampUserViewModel>(user);
      vm.IsAdmin = await _userMgr.IsInRoleAsync(user, Consts.ADMINROLE);

      return Ok(vm);
    }

    [HttpPut("{name}")]
    public async Task<IActionResult> Put(string name, [FromBody]CodeCampUserViewModel vm)
    {
      var user = await _userMgr.FindByNameAsync(name);
      _mapper.Map<CodeCampUserViewModel, CodeCampUser>(vm, user);
      await _repo.SaveChangesAsync();

      return Ok(_mapper.Map<CodeCampUserViewModel>(user));
    }

    [HttpPut("{name}/toggleAdmin")]
    public async Task<IActionResult> ToggleAdmin(string name, [FromBody]CodeCampUserViewModel vm)
    {
      try
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
      }
      catch (Exception ex)
      {
        _logger.LogError("Exception thrown while toggling admin: {0}", ex);
      }

      return BadRequest("Could not update roles.");
    }

    [HttpPut("{name}/toggleConfirmation")]
    public async Task<IActionResult> ToggleConfirmation(string name, [FromBody]CodeCampUserViewModel vm)
    {
      try
      {
        if (ModelState.IsValid)
        {
          var user = await _userMgr.FindByNameAsync(name);

          if (user != null)
          {
            user.EmailConfirmed = !user.EmailConfirmed;
            await _userMgr.UpdateAsync(user);
            return Ok(user.EmailConfirmed);
          }
        }
      }
      catch (Exception ex)
      {
        _logger.LogError("Exception thrown while toggling confirmation: {0}", ex);
      }

      return BadRequest("Could not toggle email confirmation.");
    }

  }
}
