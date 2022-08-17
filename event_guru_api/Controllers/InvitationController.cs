using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using event_guru_api.models;
using event_guru_api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace event_guru_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvitationController : ControllerBase
    {
        private readonly ApplicationContext _db;
        private readonly ILogger _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ISMSSenderService _smsSender;
        public InvitationController(
            ApplicationContext db,
            ILogger<InvitationController> logger,
            UserManager<ApplicationUser> userManager,
            ISMSSenderService smsSender)
        {
            _db = db;
            _logger = logger;
            _userManager = userManager;
            _smsSender = smsSender;
        }

        [HttpPost]
        public async Task<IActionResult> InviteUserToEvent([FromForm] InvitationModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return ValidationProblem();
                }
                var theEvent = await _db.Events.Where(e => e.ID == model.EventID).FirstOrDefaultAsync();
                var theAttendee = await _userManager.FindByIdAsync(model.AttendeeID);
                var smsModel = new SMSModel()
                {
                    from = theEvent!.Contact,
                    to = theAttendee!.UserName,
                    text = "Welcome to the event we have created",
                };
                var result = await _smsSender.SendSMS(smsModel);
                if (result)
                {
                    return Ok("The message was sent successfully");
                }
                else
                {
                    return Problem("The message sending failed");
                }
            }
            catch (Exception e)
            {
                return Problem(e.ToString());
            }



        }

        [HttpPost]
        [Route("{eventID}")]
        public async Task<IActionResult> InviteMultipleUsersToEvent(int EventID)
        {
            return Ok();
        }
    }
}
