using backend.Data;
using backend.DTOs;
using backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers;

[ApiController]
[Route("api/events")]
public class EventsController(AppDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var events = await db.Events
            .Include(e => e.Participants)
            .OrderBy(e => e.StartDate)
            .Select(e => new
            {
                e.Id,
                e.Name,
                e.StartDate,
                e.EndDate,
                e.MaxParticipants,
                ParticipantCount = e.Participants.Count
            })
            .ToListAsync();

        return Ok(events);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateEventRequest req)
    {
        if (string.IsNullOrWhiteSpace(req.Name))
            return BadRequest(new { message = "Name is required" });
        if (req.MaxParticipants <= 0)
            return BadRequest(new { message = "Max capacity must be positive" });
        if (req.StartDate >= req.EndDate)
            return BadRequest(new { message = "Start date must be before end date" });

        var ev = new Event
        {
            Name = req.Name,
            StartDate = req.StartDate.ToUniversalTime(),
            EndDate = req.EndDate.ToUniversalTime(),
            MaxParticipants = req.MaxParticipants
        };

        db.Events.Add(ev);
        await db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetAll), new { id = ev.Id }, new
        {
            ev.Id,
            ev.Name,
            ev.StartDate,
            ev.EndDate,
            ev.MaxParticipants,
            ParticipantCount = 0
        });
    }
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var ev = await db.Events.FindAsync(id);
        if (ev is null)
            return NotFound(new { message = "Event not found" });

        db.Events.Remove(ev);
        await db.SaveChangesAsync();

        return NoContent();
    }
}
