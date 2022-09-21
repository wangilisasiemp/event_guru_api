using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using event_guru_api.models;
using event_guru_api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Utilities;
using PortalSDK;

namespace event_guru_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContributionController : ControllerBase
    {
        private readonly ApplicationContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration Config;
        private readonly IMPesaService mPesaService;
        public ContributionController(ApplicationContext db, UserManager<ApplicationUser> userManager, IMPesaService mpesaService, IConfiguration config)
        {
            _db = db;
            _userManager = userManager;
            Config = config;
            mPesaService = mpesaService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Contribution>>> GetAll()
        {
            try
            {
                List<ContributionResultModel> contributions = new List<ContributionResultModel>();
                List<Contribution> contrList = await _db.Contributions
                               .Include(ct => ct.Event)
                               .Include(ct => ct.Attendee)
                               .ToListAsync<Contribution>();
                contrList.ForEach(con =>
                {
                    ContributionResultModel cont = new ContributionResultModel()
                    {
                        Completed = con.Completed,
                        EventID = con.EventID,
                        Amount = con.Amount,
                        EventName = con.Event.Title,
                        ID = con.ID,
                        TransactionID = con.TransactionID,
                        Contributor = con.Attendee.FirstName + " " + con.Attendee.LastName

                    };
                    contributions.Add(cont);
                });
                return Ok(contributions);
            }
            catch (Exception err)
            {
                return Problem(err.Message);
            }

        }

        [HttpGet]
        [Route("{EventID:int}")]
        public async Task<ActionResult<IEnumerable<Contribution>>> GetEventContributions(int EventID)
        {
            try
            {
                return await _db.Contributions.Where(c => c.EventID == EventID)
                                .Include(c => c.Event)
                                .Include(c => c.Attendee)
                                .ToListAsync();
            }
            catch (Exception err)
            {
                return Problem(err.Message);
            }

        }

        [HttpGet]
        [Route("{UserID}")]
        public async Task<ActionResult<IEnumerable<Contribution>>> GetUserContributions(string UserID)
        {
            try
            {
                return await _db.Contributions.Where(c => c.AttendeeID == UserID)
                                .Include(c => c.Event)
                                .Include(c => c.Attendee)
                                .ToListAsync();
            }
            catch (Exception err)
            {
                return Problem(err.Message);
            }

        }
        [HttpPost]
        [Route("pay/{EventID:int}")]
        public async Task<ActionResult<PaymentResponse>> Pay(int EventID, PaymentRequestModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return ValidationProblem();
                }
                var eventExists = await _db.Events.Where(e => e.ID == EventID).FirstOrDefaultAsync();
                if (eventExists is null)
                {
                    ModelState.AddModelError("EventNotFound", "The event you are contributing two does not exist");
                    return ValidationProblem();
                }
                var attendee = await _userManager.FindByIdAsync(model.input_ThirdPartyConversationID);
                if (attendee is null)
                {
                    ModelState.AddModelError("AttendeeNotFound", "The attendee you supplied does not exist");
                    return ValidationProblem();
                }
                PaymentResponse response = await this.mPesaService.pay(model);
                var PrevContribution = await _db.Contributions.Where(c => c.EventID == EventID && c.AttendeeID == model.input_ThirdPartyConversationID).FirstOrDefaultAsync();
                if (PrevContribution is null)
                {
                    Contribution newContribution = new Contribution()
                    {
                        EventID = EventID,
                        AttendeeID = model.input_ThirdPartyConversationID,
                        Amount = model.input_Amount,
                        Completed = true,
                        CustomerMSISDN = model.input_CustomerMSISDN,
                        TransactionID = response.output_TransactionID,
                        ConversationID = response.output_ConversationID,
                        ThirdPartyConversationID = response.output_ThirdPartyConversationID,
                        ResponseCode = response.output_ResponseCode,
                        ResponseDesc = response.output_ResponseDesc
                    };
                    await _db.Contributions.AddAsync(newContribution);
                    await _db.SaveChangesAsync();
                    return Ok(new Response { Status = "Success", Message = "Contribution was added successfully" });
                }
                var totalContributed = PrevContribution.Amount + model.input_Amount;
                PrevContribution.Amount = PrevContribution.Amount + model.input_Amount;
                PrevContribution.Completed = totalContributed >= eventExists.MinContribution;
                await _db.SaveChangesAsync();
                return Ok(new Response { Status = "Success", Message = "The contribution was added successfully" });
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] ContributionModel model)
        {
            try
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
            catch (Exception err)
            {
                return Problem(err.Message);
            }


        }

        [HttpDelete]
        [Route("{ID:int}")]
        public async Task<IActionResult> Delete(int ID)
        {
            try
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
            catch (Exception err)
            {
                return Problem(err.Message);
            }
        }
    }
}
