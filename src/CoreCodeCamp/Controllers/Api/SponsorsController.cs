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
  public class SponsorsController : Controller
  {
    private ICodeCampRepository _repo;
    private ILogger<SponsorsController> _logger;

    public SponsorsController(ICodeCampRepository repo, ILogger<SponsorsController> logger)
    {
      _logger = logger;
      _repo = repo;
    }

    [HttpGet("")]
    [AllowAnonymous]
    public IActionResult GetSponsors(string moniker)
    {
      try
      {
        return Ok(Mapper.Map<IEnumerable<SponsorViewModel>>(_repo.GetSponsors(moniker)));
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
          var sponsor = _repo.GetSponsor(vm.Id);

          if (sponsor == null) // new
          {
            sponsor = Mapper.Map<Sponsor>(vm);
            var eventInfo = _repo.GetEventInfo(moniker);
            sponsor.Event = eventInfo;
            _repo.AddOrUpdate(sponsor);

            await _repo.SaveChangesAsync();

            return Created($"/{moniker}/api/sponsors/{vm.Id}", Mapper.Map<SponsorViewModel>(sponsor));
          }
          else
          {
            Mapper.Map<SponsorViewModel, Sponsor>(vm, sponsor);
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
        var sponsor = _repo.GetSponsor(id);
        if (sponsor == null) return NotFound();

        sponsor.Paid = !sponsor.Paid;
        await _repo.SaveChangesAsync();

        return Ok();
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
        var sponsor = _repo.GetSponsor(id);
        if (sponsor == null) return NotFound();

        _repo.Delete(sponsor);
        await _repo.SaveChangesAsync();

        return Ok();
      }
      catch (Exception ex)
      {
        _logger.LogError("Failed to delete sponsor: {0}", ex);
      }

      return BadRequest("Failed to delete sponsor");

    }

  }
}
