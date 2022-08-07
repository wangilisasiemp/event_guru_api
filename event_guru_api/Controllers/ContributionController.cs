using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using event_guru_api.models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace event_guru_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContributionController : ControllerBase
    {
        private readonly ApplicationContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        public ContributionController(ApplicationContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Contribution>>> GetAll()
        {
            return await _db.Contributions
                .Include(ct => ct.Event)
                .Include(ct => ct.Attendee)
                .ToListAsync<Contribution>();
        }

        [HttpGet]
        [Route("{EventID:int}")]
        public async Task<ActionResult<IEnumerable<Contribution>>> GetEventContributions(int EventID)
        {
            return await _db.Contributions.Where(c => c.EventID == EventID)
                .Include(c => c.Event)
                .Include(c => c.Attendee)
                .ToListAsync();
        }

        [HttpGet]
        [Route("{UserID}")]
        public async Task<ActionResult<IEnumerable<Contribution>>> GetUserContributions(string UserID)
        {
            return await _db.Contributions.Where(c => c.AttendeeID == UserID)
                .Include(c => c.Event)
                .Include(c => c.Attendee)
                .ToListAsync();
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromForm] ContributionModel model)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem();
            }
            var eventExists = await _db.Events.Where(e => e.ID == model.EventID).FirstOrDefaultAsync();
            if (eventExists is null)
            {
                ModelState.AddModelError("EventNotFound", "The event you are contributing two does not exist");
                return ValidationProblem();
            }
            var attendee = await _userManager.FindByIdAsync(model.AttendeeID);
            if (attendee is null)
            {
                ModelState.AddModelError("AttendeeNotFound", "The attendee you supplied does not exist");
                return ValidationProblem();
            }

            var PrevContribution = await _db.Contributions.Where(c => c.EventID == model.EventID && c.AttendeeID == model.AttendeeID).FirstOrDefaultAsync();
            if (PrevContribution is null)
            {
                var newContribution = new Contribution()
                {
                    Amount = model.Amount,
                    AttendeeID = model.AttendeeID,
                    Completed = model.Amount >= eventExists.MinContribution,
                    EventID = model.EventID,
                };
                await _db.Contributions.AddAsync(newContribution);
                await _db.SaveChangesAsync();
                return Ok(new Response { Status = "Success", Message = "Contribution was added successfully" });
            }
            var totalContributed = PrevContribution.Amount + model.Amount;
            PrevContribution.Amount += model.Amount;
            PrevContribution.Completed = totalContributed >= eventExists.MinContribution;
            await _db.SaveChangesAsync();
            return Ok(new Response { Status = "Success", Message = "The contribution was added successfully" });

        }

        [HttpDelete]
        [Route("{ID:int}")]
        public async Task<IActionResult> Delete(int ID)
        {
            var contribution = await _db.Contributions.Where(c => c.ID == ID).FirstOrDefaultAsync();
            if (contribution is null)
            {

                return NotFound("The contribution tyou are trying to delete could not be found");
            }
            _db.Remove(contribution);
            var res = await _db.SaveChangesAsync();
            return Ok(new Response { Status = "Success", Message = "The contribution was successfully deleted" });
        }
    }
}