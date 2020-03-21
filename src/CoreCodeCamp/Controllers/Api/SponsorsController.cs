using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CoreCodeCamp.Data;
using CoreCodeCamp.Data.Entities;
using CoreCodeCamp.Models;
using CoreCodeCamp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CoreCodeCamp.Controllers.Api
{
  [Route("{moniker}/api/sponsors")]
  [Authorize(Roles = Consts.ADMINROLE)]
  [ApiController]
  public class SponsorsController : Controller
  {
    private ICodeCampRepository _repo;
    private ILogger<SponsorsController> _logger;
    private readonly IMapper _mapper;

    public SponsorsController(ICodeCampRepository repo, ILogger<SponsorsController> logger, IMapper mapper)
    {
      _logger = logger;
      _mapper = mapper;
      _repo = repo;
    }

    [HttpGet("")]
    [AllowAnonymous]
    public async Task<IActionResult> GetSponsors(string moniker)
    {
      try
      {
        return Ok(_mapper.Map<IEnumerable<SponsorViewModel>>(await _repo.GetSponsorsAsync(moniker)));
      }
      catch (Exception ex)
      {
        _logger.LogError("Failed to get sponsors: {0}", ex);
        return BadRequest("Failed to get sponsors");
      }
    }

    [HttpPost("")]
    public async Task<IActionResult> UpsertSponsors(string moniker, [FromBody] SponsorViewModel vm)
    {
      if (ModelState.IsValid)
      {
        try
        {
          var sponsor = await _repo.GetSponsorAsync(vm.Id);

          if (sponsor == null) // new
          {
            sponsor = _mapper.Map<Sponsor>(vm);
            var eventInfo = await _repo.GetEventInfoAsync(moniker);
            sponsor.Event = eventInfo;
            _repo.AddOrUpdate(sponsor);

            await _repo.SaveChangesAsync();

            return Created($"/{moniker}/api/sponsors/{vm.Id}", _mapper.Map<SponsorViewModel>(sponsor));
          }
          else
          {
            _mapper.Map<SponsorViewModel, Sponsor>(vm, sponsor);
            _repo.AddOrUpdate(sponsor);
            await _repo.SaveChangesAsync();

            return Ok(vm);
          }
        }
        catch (Exception ex)
        {
          _logger.LogError("Failed to update sponsor: {0}", ex);
          ModelState.AddModelError("", $"Failed to Save: {ex.Message}");
        }
      }

      return BadRequest("Failed to save Sponsor");
    }

    [HttpPut("{id:int}/togglePaid")]
    public async Task<IActionResult> TogglePaid(string moniker, int id)
    {
      try
      {
        var sponsor = await _repo.GetSponsorAsync(id);
        if (sponsor == null) return NotFound();

        sponsor.Paid = !sponsor.Paid;
        await _repo.SaveChangesAsync();

        return Ok(true);
      }
      catch (Exception ex)
      {
        _logger.LogError("Failed to toggle the Sponsor's Paid flag: {0}", ex);
      }

      return BadRequest("Failed to toggle paid on sponsor");
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteSponsor(string moniker, int id)
    {
      try
      {
        var sponsor = await _repo.GetSponsorAsync(id);
        if (sponsor == null) return NotFound();

        _repo.Delete(sponsor);
        await _repo.SaveChangesAsync();

        return Ok(true);
      }
      catch (Exception ex)
      {
        _logger.LogError("Failed to delete sponsor: {0}", ex);
      }

      return BadRequest("Failed to delete sponsor");

    }

  }
}
