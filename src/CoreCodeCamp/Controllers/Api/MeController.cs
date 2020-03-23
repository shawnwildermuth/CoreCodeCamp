using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CoreCodeCamp.Authentication;
using CoreCodeCamp.Data;
using CoreCodeCamp.Data.Entities;
using CoreCodeCamp.Models;
using CoreCodeCamp.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CoreCodeCamp.Controllers.Api
{
  [Route("{moniker}/api/me")]
  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
  [ApiController]
  public class MeController : Controller
  {
    private ILogger<MeController> _logger;
    private readonly IMapper _mapper;
    private readonly UserManager<CodeCampUser> _userManager;
    private readonly SignInManager<CodeCampUser> _signInManager;
    private readonly CoreCodeCampTokenFactory _tokenFactory;
    private ICodeCampRepository _repo;

    public MeController(ICodeCampRepository repo, 
      ILogger<MeController> logger, 
      IMapper mapper,
      UserManager<CodeCampUser> userManager,
      SignInManager<CodeCampUser> signInManager,
      CoreCodeCampTokenFactory tokenFactory)
    {
      _repo = repo;
      _logger = logger;
      _mapper = mapper;
      _userManager = userManager;
      _signInManager = signInManager;
      _tokenFactory = tokenFactory;
    }

    [AllowAnonymous]
    [HttpPost("token")]
    public async Task<IActionResult> CreateToken(TokenRequestModel model)
    {
      try
      {
        // Allow by username or email
        var user = await _userManager.FindByNameAsync(model.Username);
        if (user == null) user = await _userManager.FindByEmailAsync(model.Username);

        if (user == null) return BadRequest("Invalid User");

        // Check Password
        if ((await _signInManager.CheckPasswordSignInAsync(user, model.Password, false)) == Microsoft.AspNetCore.Identity.SignInResult.Success)
        {
          // Generate the token
          var token = await _tokenFactory.GenerateForUser(user);

          return Created("", token);
        }
      }
      catch (Exception ex)
      {
        _logger.LogError($"Failed to login for Token: {ex}");
      }

      return BadRequest();

    }

    [HttpGet("favorites")]
    public async Task<IActionResult> GetFavorites(string moniker)
    {
      var favs = await _repo.GetUserWithFavoriteTalksForEventAsync(User.Identity.Name, moniker);
      return Ok(_mapper.Map<IEnumerable<FavoriteTalkViewModel>>(favs));
    }

    [HttpPut("favorites/{talkId:int}")]
    public async Task<IActionResult> ToggleStar(string moniker, int talkId)
    {
      try
      {
        var state = await _repo.ToggleTalkForUserAsync(moniker, User.Identity.Name, talkId);
        await _repo.SaveChangesAsync();
        return Ok(state);
      }
      catch (Exception ex)
      {
        _logger.LogError("Failed to save favorite talk: {0}", ex);
      }

      return BadRequest("Failed to toggle star");
    }

  }
}
