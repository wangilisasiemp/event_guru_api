using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using event_guru_api.models;
using event_guru_api.Services;
using event_guru_api.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;

namespace event_guru_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly ApplicationContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEventBudgetService _budgetService;
        public EventController(ApplicationContext db, UserManager<ApplicationUser> userManager, IEventBudgetService budgetService)
        {
            _db = db;
            _userManager = userManager;
            _budgetService = budgetService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Event>>> GetAll()
        {
            return await _db.Events.ToListAsync<Event>();
        }

        [HttpGet("/byOrganizer/{OrganizerID}")]
        public async Task<ActionResult<IEnumerable<Event>>> GetEventByUser(string OrganizerID)
        {
            return await _db.Events.Where(e => e.OrganizerID == OrganizerID).ToListAsync<Event>();
        }

        [HttpGet("/byAttendee/{AttendeeID}")]
        public async Task<ActionResult<IEnumerable<EventAttendance>>> GetEventByAttendee(string AttendeeID)
        {
            return await _db.EventAttendances.Where(ea => ea.AttendeeID == AttendeeID)
                .Include(ea => ea.Event)
                .ToListAsync();
        }


        [HttpGet]
        [Route("{ID:int}")]
        public async Task<ActionResult<Event>> Get(int ID)
        {
            Event? ev = await _db.Events.Where(e => e.ID == ID).FirstOrDefaultAsync();
            if (ev is null)
            {
                return Problem(title: "EventNotFound", statusCode: 404, detail: "The Event you are searching was not found");
            }
            return ev;
        }


        [HttpPost]
        public async Task<IActionResult> Add([FromBody] EventModel model)
        {
            //check if the model has all necessary data
            if (!ModelState.IsValid)
            {
                return ValidationProblem();
            }

            //check to see if the organizaer is available
            var orgExists = await _userManager.FindByIdAsync(model.OrganizerID);
            if (orgExists is null)
            {
                ModelState.AddModelError("OrganizerNotFound", "The organizer is not found");
                return ValidationProblem(ModelState);
            }

            var newEvent = new Event()
            {
                Title = model.Title,
                Description = model.Description,
                Location = model.Location,
                Contact = model.Contact,
                Type = model.Type,
                NoOfAttendees = model.NoOfAttendees,
                FinalizationDate = model.FinalizationDate ?? model.EventDate!.AddDays(-2),
                MinContribution = model.MinContribution,
                EventDate = model.EventDate,
                EventStartTime = model.EventStartTime,
                EventEndTime = model.EventEndTime,
                OrganizerID = model.OrganizerID,
                SecretCode = Guid.NewGuid().ToString(),

                //services from vendors
                Caterer = model.Caterer ?? false,
                Drinks = model.Drinks ?? true,
                MC = model.MC ?? false,
                ConferenceHall = model.ConferenceHall ?? true,
                RoyalTransport = model.RoyalTransport ?? false,
                Decoration = model.Decoration ?? true,
                OrdinaryTransport = model.OrdinaryTransport ?? false,
                Security = model.Security ?? false,
                Entertainment = model.Entertainment ?? false
            };
            await _db.Events.AddAsync(newEvent);
            await _db.SaveChangesAsync();
            return Ok(new Response { Status = "Success", Message = "The event was added successfully" });

        }


        [HttpPut]
        [Route("{EventID:int}")]
        public async Task<IActionResult> Edit(int EventID, [FromBody] EventModel model)
        {
            //check if the model has all necessary data
            if (!ModelState.IsValid)
            {
                return ValidationProblem();
            }

            //check to see if the event exists
            var ExistingEvent = await _db.Events.Where(ev => ev.ID == EventID).FirstOrDefaultAsync();
            if (ExistingEvent is null)
            {
                return Problem(
                    title: "EventNotFound",
                    detail: "The event you are trying to edit is not available",
                    statusCode: 404);
            }
            //check to see if the organizaer is available
            var orgExists = await _userManager.FindByIdAsync(model.OrganizerID);
            if (orgExists is null)
            {
                ModelState.AddModelError("OrganizerNotFound", "The organizer is not found");
                return ValidationProblem(ModelState);
            }

            ExistingEvent.Title = model.Title ?? ExistingEvent.Title;
            ExistingEvent.Description = model.Description ?? ExistingEvent.Description;
            ExistingEvent.Location = model.Location ?? ExistingEvent.Location;
            ExistingEvent.Contact = model.Contact ?? ExistingEvent.Contact;
            ExistingEvent.Type = model.Type ?? ExistingEvent.Type;
            ExistingEvent.NoOfAttendees = model.NoOfAttendees ?? ExistingEvent.NoOfAttendees;
            ExistingEvent.FinalizationDate = model.FinalizationDate ?? ExistingEvent.FinalizationDate;
            ExistingEvent.MinContribution = model.MinContribution ?? ExistingEvent.MinContribution;
            ExistingEvent.EventDate = model.EventDate;
            ExistingEvent.EventStartTime = model.EventStartTime ?? ExistingEvent.EventStartTime;
            ExistingEvent.EventEndTime = model.EventEndTime ?? ExistingEvent.EventEndTime;
            ExistingEvent.OrganizerID = model.OrganizerID ?? ExistingEvent.OrganizerID;

            //services from vendors
            ExistingEvent.Caterer = model.Caterer ?? ExistingEvent.Caterer;
            ExistingEvent.Drinks = model.Drinks ?? ExistingEvent.Drinks;
            ExistingEvent.MC = model.MC ?? ExistingEvent.MC;
            ExistingEvent.ConferenceHall = model.ConferenceHall ?? ExistingEvent.ConferenceHall;
            ExistingEvent.RoyalTransport = model.RoyalTransport ?? ExistingEvent.RoyalTransport;
            ExistingEvent.Decoration = model.Decoration ?? ExistingEvent.Decoration;
            ExistingEvent.OrdinaryTransport = model.OrdinaryTransport ?? ExistingEvent.OrdinaryTransport;
            ExistingEvent.Security = model.Security ?? ExistingEvent.Security;
            ExistingEvent.Entertainment = model.Entertainment ?? ExistingEvent.Entertainment;


            await _db.SaveChangesAsync();
            return Ok(new Response { Status = "Success", Message = "The event was updated successfully" });

        }

        [HttpGet]
        [Route("{EventID:int}/Budget")]
        public async Task<ActionResult<IEnumerable<Dictionary<string, Vendor>>>> GetBudget(int EventID)
        {
            try
            {
                ICollection<Budget> budgets = await _db.Budgets.Where(b => b.EventID == EventID).ToListAsync();
                if (budgets.Count == 0)
                {
                    //await GenerateBudgets(EventID);
                    //await _db.Budgets.Where(b => b.EventID == EventID).ToListAsync();
                    budgets = await _budgetService.getBudgetVendors(EventID);
                }
                //return await _db.Budgets.Where(b => b.EventID == EventID).ToListAsync();

                return Ok(budgets);
            }
            catch (Exception e)
            {
                return Problem(title: "Error", detail: e.Message, statusCode: 500);
            }
        }

        [HttpGet("/Budget/Vendors/{EventID}")]
        public async Task<ActionResult<IEnumerable<BudgetVendor>>> GetVendorsByEvent(int EventID)
        {
            try
            {
                var eventBudget = await _db.Budgets.Where(b => b.EventID == EventID).FirstOrDefaultAsync();
                if (eventBudget is null)
                {
                    return Problem(title: "Event not found", statusCode: 404, detail: "The Event you are looking for was not found");
                }
                var budgetVendors = await _db.BudgetVendors
                    .Where(bv => bv.BudgetID == eventBudget.ID)
                    .Include(bv => bv.Vendor)
                    .ToListAsync();
                return budgetVendors;
            }
            catch (Exception e)
            {
                return Problem(e.ToString());
            }
        }
        //private async Task GenerateBudgets(int EventID)
        //{
        //    /**Get the event to determin the type of event so as to exclude some vendors
        //     * Assuming that some events won't have some services
        //     **/
        //    var theEvent = await _db.Events.Where(e => e.ID == EventID).FirstOrDefaultAsync();
        //    if (theEvent is null)
        //    {
        //        throw new Exception("The event you are looking for was not found");
        //    }
        //    List<string> weddingVendors = new List<string>()
        //    {
        //        VendorTypes.Caterer,
        //        VendorTypes.ConferenceHall,
        //        VendorTypes.Decoration,
        //        VendorTypes.Drinks,
        //        VendorTypes.Entertainment,
        //        VendorTypes.MC,
        //        VendorTypes.RoyalTransport,
        //        VendorTypes.OrdinaryTransport,
        //        VendorTypes.Security
        //    };
        //    List<string> seminarVendors = new List<string>()
        //    {
        //        VendorTypes.Caterer,
        //        VendorTypes.ConferenceHall,
        //        VendorTypes.Decoration,
        //        VendorTypes.Drinks,
        //        VendorTypes.RoyalTransport,
        //        VendorTypes.Entertainment,
        //        VendorTypes.Security
        //    };
        //    List<string> congregationVendors = new List<string>()
        //    {
        //        VendorTypes.Caterer,
        //        VendorTypes.ConferenceHall,
        //        VendorTypes.Decoration,
        //        VendorTypes.Drinks,
        //        VendorTypes.Entertainment,
        //        VendorTypes.MC,
        //        VendorTypes.RoyalTransport,
        //        VendorTypes.Security
        //    };
        //    List<string> partyVendors = new List<string>()
        //    {
        //        VendorTypes.Caterer,
        //        VendorTypes.ConferenceHall,
        //        VendorTypes.Decoration,
        //        VendorTypes.Drinks,
        //        VendorTypes.Entertainment,
        //        VendorTypes.MC,
        //        VendorTypes.RoyalTransport,
        //        VendorTypes.Security
        //    };
        //    List<string> meetingVendors = new List<string>()
        //    {
        //        VendorTypes.Caterer,
        //        VendorTypes.ConferenceHall,
        //        VendorTypes.Decoration,
        //        VendorTypes.Drinks,
        //        VendorTypes.Entertainment,
        //    };
        //    List<string> showVendors = new List<string>()
        //    {
        //        VendorTypes.Caterer,
        //        VendorTypes.ConferenceHall,
        //        VendorTypes.Decoration,
        //        VendorTypes.Entertainment,
        //        VendorTypes.MC,
        //        VendorTypes.RoyalTransport,
        //        VendorTypes.Security
        //    };
        //    List<Vendor> minVendors = new List<Vendor>();
        //    List<Vendor> maxVendors = new List<Vendor>();
        //    List<Budget> budgets = new List<Budget>();
        //    List<BudgetVendor> budgetVendors = new List<BudgetVendor>();
        //    switch (theEvent.Type)
        //    {
        //        case EventTypes.Wedding:
        //            //Fetch the minimum bronze badge vendors according to their categories
        //            minVendors = await GetVendorList(false, weddingVendors, BudgetTypes.Bronze);
        //            maxVendors = await GetVendorList(true, weddingVendors, BudgetTypes.Bronze);
        //            budgets.Add(CalculateBudget(BudgetTypes.Bronze, theEvent, minVendors, maxVendors));
        //            //Fetch the minimum silver badge vendors according to their categories
        //            //Fetch the maximum silver badge vendors according to their categories
        //            minVendors = await GetVendorList(false, weddingVendors, BudgetTypes.Silver);
        //            maxVendors = await GetVendorList(true, weddingVendors, BudgetTypes.Silver);
        //            budgets.Add(CalculateBudget(BudgetTypes.Silver, theEvent, minVendors, maxVendors));

        //            minVendors = await GetVendorList(false, weddingVendors, BudgetTypes.Gold);
        //            maxVendors = await GetVendorList(true, weddingVendors, BudgetTypes.Gold);
        //            budgets.Add(CalculateBudget(BudgetTypes.Gold, theEvent, minVendors, maxVendors));
        //            break;

        //        case EventTypes.Seminar:
        //            //Fetch the minimum bronze badge vendors according to their categories
        //            minVendors = await GetVendorList(false, seminarVendors, BudgetTypes.Bronze);
        //            maxVendors = await GetVendorList(true, seminarVendors, BudgetTypes.Bronze);
        //            budgets.Add(CalculateBudget(BudgetTypes.Bronze, theEvent, minVendors, maxVendors));
        //            //Fetch the minimum silver badge vendors according to their categories
        //            //Fetch the maximum silver badge vendors according to their categories
        //            minVendors = await GetVendorList(false, seminarVendors, BudgetTypes.Silver);
        //            maxVendors = await GetVendorList(true, seminarVendors, BudgetTypes.Silver);
        //            budgets.Add(CalculateBudget(BudgetTypes.Silver, theEvent, minVendors, maxVendors));

        //            minVendors = await GetVendorList(false, seminarVendors, BudgetTypes.Gold);
        //            maxVendors = await GetVendorList(true, seminarVendors, BudgetTypes.Gold);
        //            budgets.Add(CalculateBudget(BudgetTypes.Gold, theEvent, minVendors, maxVendors));
        //            break;

        //        case EventTypes.Congregation:
        //            minVendors = await GetVendorList(false, congregationVendors, BudgetTypes.Bronze);
        //            maxVendors = await GetVendorList(true, congregationVendors, BudgetTypes.Bronze);
        //            budgets.Add(CalculateBudget(BudgetTypes.Bronze, theEvent, minVendors, maxVendors));
        //            //Fetch the minimum silver badge vendors according to their categories
        //            //Fetch the maximum silver badge vendors according to their categories
        //            minVendors = await GetVendorList(false, congregationVendors, BudgetTypes.Silver);
        //            maxVendors = await GetVendorList(true, congregationVendors, BudgetTypes.Silver);
        //            budgets.Add(CalculateBudget(BudgetTypes.Silver, theEvent, minVendors, maxVendors));

        //            minVendors = await GetVendorList(false, congregationVendors, BudgetTypes.Gold);
        //            maxVendors = await GetVendorList(true, congregationVendors, BudgetTypes.Gold);
        //            budgets.Add(CalculateBudget(BudgetTypes.Gold, theEvent, minVendors, maxVendors));
        //            break;

        //        case EventTypes.Party:
        //            minVendors = await GetVendorList(false, partyVendors, BudgetTypes.Bronze);
        //            maxVendors = await GetVendorList(true, partyVendors, BudgetTypes.Bronze);
        //            budgets.Add(CalculateBudget(BudgetTypes.Bronze, theEvent, minVendors, maxVendors));
        //            //Fetch the minimum silver badge vendors according to their categories
        //            //Fetch the maximum silver badge vendors according to their categories
        //            minVendors = await GetVendorList(false, partyVendors, BudgetTypes.Silver);
        //            maxVendors = await GetVendorList(true, partyVendors, BudgetTypes.Silver);
        //            budgets.Add(CalculateBudget(BudgetTypes.Silver, theEvent, minVendors, maxVendors));

        //            minVendors = await GetVendorList(false, partyVendors, BudgetTypes.Gold);
        //            maxVendors = await GetVendorList(true, partyVendors, BudgetTypes.Gold);
        //            budgets.Add(CalculateBudget(BudgetTypes.Gold, theEvent, minVendors, maxVendors));
        //            break;

        //        case EventTypes.Meeting:
        //            minVendors = await GetVendorList(false, meetingVendors, BudgetTypes.Bronze);
        //            maxVendors = await GetVendorList(true, meetingVendors, BudgetTypes.Bronze);
        //            budgets.Add(CalculateBudget(BudgetTypes.Bronze, theEvent, minVendors, maxVendors));
        //            //Fetch the minimum silver badge vendors according to their categories
        //            //Fetch the maximum silver badge vendors according to their categories
        //            minVendors = await GetVendorList(false, meetingVendors, BudgetTypes.Silver);
        //            maxVendors = await GetVendorList(true, meetingVendors, BudgetTypes.Silver);
        //            budgets.Add(CalculateBudget(BudgetTypes.Silver, theEvent, minVendors, maxVendors));

        //            minVendors = await GetVendorList(false, meetingVendors, BudgetTypes.Gold);
        //            maxVendors = await GetVendorList(true, meetingVendors, BudgetTypes.Gold);
        //            budgets.Add(CalculateBudget(BudgetTypes.Gold, theEvent, minVendors, maxVendors));
        //            break;

        //        case EventTypes.Show:
        //            minVendors = await GetVendorList(false, showVendors, BudgetTypes.Bronze);
        //            maxVendors = await GetVendorList(true, showVendors, BudgetTypes.Bronze);
        //            budgets.Add(CalculateBudget(BudgetTypes.Bronze, theEvent, minVendors, maxVendors));
        //            //Fetch the minimum silver badge vendors according to their categories
        //            //Fetch the maximum silver badge vendors according to their categories
        //            minVendors = await GetVendorList(false, showVendors, BudgetTypes.Silver);
        //            maxVendors = await GetVendorList(true, showVendors, BudgetTypes.Silver);
        //            budgets.Add(CalculateBudget(BudgetTypes.Silver, theEvent, minVendors, maxVendors));

        //            minVendors = await GetVendorList(false, showVendors, BudgetTypes.Gold);
        //            maxVendors = await GetVendorList(true, showVendors, BudgetTypes.Gold);
        //            budgets.Add(CalculateBudget(BudgetTypes.Gold, theEvent, minVendors, maxVendors));
        //            break;

        //        default:
        //            throw new Exception("The event type is not valid");
        //    }
        //    //save the list of budgets to the database
        //    await _db.Budgets.AddRangeAsync(budgets);
        //    var result = await _db.SaveChangesAsync();
        //}


        //private async Task<List<Vendor>> GetVendorList(bool isMax, List<string> vTypes, string budgetType)
        //{
        //    List<Vendor> minVendors = new List<Vendor>();
        //    List<Vendor> maxVendors = new List<Vendor>();
        //    if (isMax)
        //    {
        //        if (budgetType == BudgetTypes.Bronze)
        //        {
        //            foreach (var vType in vTypes)
        //            {
        //                var vMax = await _db.Vendors
        //                   .Where(
        //                    v => v.Type == vType &&
        //                    v.Bronze == _db.Vendors.Max(v2 => (int?)v2.Bronze))
        //                   //.OrderByDescending(v => v.Bronze)
        //                   .FirstOrDefaultAsync();
        //                if (vMax is not null) maxVendors.Add(vMax);
        //            }
        //        }
        //        else if (budgetType == BudgetTypes.Silver)
        //        {
        //            foreach (var vType in vTypes)
        //            {
        //                var vSilverMax = await _db.Vendors
        //                   .Where(v => v.Type == vType &&
        //                    v.Silver == _db.Vendors.Max(v2 => (int?)v2.Silver))
        //                   //.OrderByDescending(v => v.Silver)
        //                   .FirstOrDefaultAsync();
        //                if (vSilverMax is not null) maxVendors.Add(vSilverMax);
        //            }
        //        }
        //        else
        //        {
        //            foreach (var vType in vTypes)
        //            {
        //                var vGoldMax = await _db.Vendors
        //                   .Where(v => v.Type == vType &&
        //                    v.Gold == _db.Vendors.Max(v2 => (int?)v2.Gold))
        //                   //.OrderByDescending(v => v.Gold)
        //                   .FirstOrDefaultAsync();
        //                if (vGoldMax is not null) maxVendors.Add(vGoldMax);
        //            }

        //        }
        //        return maxVendors;

        //    }
        //    else
        //    {
        //        if (budgetType == BudgetTypes.Bronze)
        //        {
        //            foreach (var vType in vTypes)
        //            {
        //                var vMin = await _db.Vendors
        //                   .Where(
        //                    v => v.Type == vType &&
        //                    v.Bronze == _db.Vendors.Min(
        //                        v2 => (int?)v2.Bronze)
        //                    )
        //                   .FirstOrDefaultAsync();


        //                if (vMin is not null) minVendors.Add(vMin);
        //            }
        //        }
        //        else if (budgetType == BudgetTypes.Silver)
        //        {
        //            foreach (var vType in vTypes)
        //            {
        //                var vSilverMin = await _db.Vendors
        //                   .Where(v => v.Type == vType &&
        //                    v.Silver == _db.Vendors.Max(
        //                        v2 => (int?)v2.Silver))
        //                   //.OrderByDescending(v => v.Silver)
        //                   .FirstOrDefaultAsync();
        //                if (vSilverMin is not null) minVendors.Add(vSilverMin);
        //            }
        //        }
        //        else
        //        {
        //            foreach (var vType in vTypes)
        //            {
        //                var vGoldMin = await _db.Vendors
        //                   .Where(v => v.Type == vType &&
        //                    v.Gold == _db.Vendors.Min(
        //                        v2 => (int?)v2.Gold))
        //                   //.OrderByDescending(v => v.Gold)
        //                   .FirstOrDefaultAsync();
        //                if (vGoldMin is not null) minVendors.Add(vGoldMin);
        //            }

        //        }
        //        return minVendors;

        //    }
        //}

        //private Budget CalculateBudget(string Type, Event theEvent, List<Vendor> minVendors, List<Vendor> maxVendors)
        //{
        //    double? totalMin = 0;
        //    double? totalMax = 0;
        //    var budget = new Budget();
        //    if (Type == BudgetTypes.Bronze)
        //    {
        //        foreach (var minVendor in minVendors)
        //        {
        //            if (minVendor.Type == VendorTypes.Security ||
        //                minVendor.Type == VendorTypes.Entertainment ||
        //                minVendor.Type == VendorTypes.Decoration ||
        //                minVendor.Type == VendorTypes.RoyalTransport)
        //            {
        //                totalMin = totalMin + minVendor!.Bronze;
        //            }
        //            else
        //            {
        //                totalMin = totalMin + minVendor!.Bronze * theEvent.NoOfAttendees;
        //            }

        //        }
        //        foreach (var maxVendor in maxVendors)
        //        {
        //            if (maxVendor.Type == VendorTypes.Security ||
        //                maxVendor.Type == VendorTypes.Entertainment ||
        //                maxVendor.Type == VendorTypes.Decoration ||
        //                maxVendor.Type == VendorTypes.RoyalTransport)
        //            {
        //                totalMax = totalMax + maxVendor!.Bronze;
        //            }
        //            else
        //            {
        //                totalMax = totalMax + maxVendor!.Bronze * theEvent.NoOfAttendees;
        //            }
        //        }
        //        budget.BudgetType = BudgetTypes.Bronze;
        //        budget.MinValue = totalMin;
        //        budget.MaxValue = totalMax;
        //        budget.EventID = theEvent.ID;
        //    }
        //    else if (Type == BudgetTypes.Silver)
        //    {
        //        foreach (var minVendor in minVendors)
        //        {
        //            if (minVendor.Type == VendorTypes.Security ||
        //               minVendor.Type == VendorTypes.Entertainment ||
        //               minVendor.Type == VendorTypes.Decoration ||
        //               minVendor.Type == VendorTypes.RoyalTransport)
        //            {
        //                totalMin = totalMin + minVendor!.Silver;
        //            }
        //            else
        //            {
        //                totalMin = totalMin + minVendor!.Silver * theEvent.NoOfAttendees;

        //            }
        //        }
        //        foreach (var maxVendor in maxVendors)
        //        {
        //            if (maxVendor.Type == VendorTypes.Security ||
        //                maxVendor.Type == VendorTypes.Entertainment ||
        //                maxVendor.Type == VendorTypes.Decoration ||
        //                maxVendor.Type == VendorTypes.RoyalTransport)
        //            {
        //                totalMax = totalMax + maxVendor!.Silver;
        //            }
        //            else
        //            {
        //                totalMax = totalMax + maxVendor!.Silver * theEvent.NoOfAttendees;
        //            }
        //        }
        //        budget.BudgetType = BudgetTypes.Silver;
        //        budget.MinValue = totalMin;
        //        budget.MaxValue = totalMax;
        //        budget.EventID = theEvent.ID;
        //    }
        //    else
        //    {
        //        foreach (var minVendor in minVendors)
        //        {
        //            if (minVendor.Type == VendorTypes.Security ||
        //               minVendor.Type == VendorTypes.Entertainment ||
        //               minVendor.Type == VendorTypes.Decoration ||
        //               minVendor.Type == VendorTypes.RoyalTransport)
        //            {
        //                totalMin = totalMin + minVendor!.Gold;
        //            }
        //            else
        //            {
        //                totalMin = totalMin + minVendor!.Gold * theEvent.NoOfAttendees;
        //            }
        //        }
        //        foreach (var maxVendor in maxVendors)
        //        {
        //            if (maxVendor.Type == VendorTypes.Security ||
        //                maxVendor.Type == VendorTypes.Entertainment ||
        //                maxVendor.Type == VendorTypes.Decoration ||
        //                maxVendor.Type == VendorTypes.RoyalTransport)
        //            {
        //                totalMax = totalMax + maxVendor!.Gold;
        //            }
        //            else
        //            {
        //                totalMax = totalMax + maxVendor!.Gold * theEvent.NoOfAttendees;
        //            }
        //        }
        //        budget.BudgetType = BudgetTypes.Gold;
        //        budget.MinValue = totalMin;
        //        budget.MaxValue = totalMax;
        //        budget.EventID = theEvent.ID;
        //    }
        //    return budget;
        //}

    }
}
