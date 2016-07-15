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

namespace CoreCodeCamp.Controllers.Api
{
  [Route("api/sponsors")]
  [Authorize(Roles = Consts.ADMINROLE)]
  public class SponsorsApiController : Controller
  {
    private ICodeCampRepository _repo;

    public SponsorsApiController(ICodeCampRepository repo)
    {
      _repo = repo;
    }

    [HttpGet("events")]
    public IActionResult GetEvents()
    {
      try
      {
        return Ok(_repo.GetAllEventInfo()
          .OrderByDescending(e => e.Moniker)
          .Select(e => new { Name = e.Name, Moniker = e.Moniker })
          .ToArray());
      }
      catch
      {
        return BadRequest("Failed to get events");
      }
    }

    [HttpGet("{moniker}")]
    public IActionResult GetSponsors(string moniker)
    {
      try
      {
        return Ok(Mapper.Map<IEnumerable<SponsorViewModel>>(_repo.GetSponsors(moniker)));
      }
      catch
      {
        return BadRequest("Failed to get sponsors");
      }
    }

    [HttpPost("{moniker}")]
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
          ModelState.AddModelError("", $"Failed to Save: {ex.Message}");
        }
      }

      return BadRequest("Failed to save Sponsor");
    }

    [HttpPut("{moniker}/togglePaid/{id:int}")]
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
      catch
      {
      }

      return BadRequest("Failed to toggle paid on sponsor");
    }

    [HttpDelete("{moniker}/{id:int}")]
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
      catch
      {
      }

      return BadRequest("Failed to delete sponsor");

    }

  }
}
