using backend.Data;
using backend.DTOs;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers;
[ApiController]
[Route("api/events/{eventId:int}/register")]
public class RegistrationsController(AppDbContext db) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Register(int eventId, [FromBody] RegisterRequest req)
    {
        if (string.IsNullOrWhiteSpace(req.FirstName) || string.IsNullOrWhiteSpace(req.LastName) || string.IsNullOrWhiteSpace(req.PersonalIdCode))
            return BadRequest(new { message = "First name, last name, and personal ID code are required" });

        var ev = await db.Events.Include(e => e.Participants).FirstOrDefaultAsync(e => e.Id == eventId);
        if (ev is null)
            return NotFound(new { message = "Event not found" });

        if (ev.Participants.Count >= ev.MaxParticipants)
            return Conflict(new { message = "Event is at full capacity" });

        if (ev.Participants.Any(p => p.PersonalIdCode == req.PersonalIdCode))
            return Conflict(new { message = "You are already registered for this event" });

        var participant = new Participant
        {
            EventId = eventId,
            FirstName = req.FirstName,
            LastName = req.LastName,
            PersonalIdCode = req.PersonalIdCode
        };

        db.Participants.Add(participant);
        await db.SaveChangesAsync();

        return Ok(new { message = "Registration successful" });
    }
}
