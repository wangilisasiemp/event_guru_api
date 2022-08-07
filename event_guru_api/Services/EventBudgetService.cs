
using event_guru_api.models;
using event_guru_api.Utils;
using Microsoft.EntityFrameworkCore;

namespace event_guru_api.Services
{
    public class EventBudgetService : IEventBudgetService
    {
        private readonly ApplicationContext _db;
        private readonly ILogger _logger;
        const string Min = "minimum";
        const string Max = "maximum";
        public EventBudgetService(ApplicationContext db, ILogger<EventBudgetService> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<ICollection<Budget>> getBudgetVendors(int EventID)
        {
            var theEvent = await _db.Events.Where(e => e.ID == EventID).FirstOrDefaultAsync();
            if (theEvent is null)
            {
                throw new Exception("The event was not found");
            }
            //create a list to store each of the maps
            List<Dictionary<string, Vendor>> vendorList = new List<Dictionary<string, Vendor>>();

            if (theEvent.Caterer) { vendorList.Add(await getVendorByType(VendorTypes.Caterer)); }
            if (theEvent.MC) { vendorList.Add(await getVendorByType(VendorTypes.MC)); }
            if (theEvent.Drinks) { vendorList.Add(await getVendorByType(VendorTypes.Drinks)); }
            if (theEvent.ConferenceHall) { vendorList.Add(await getVendorByType(VendorTypes.ConferenceHall)); }
            if (theEvent.Entertainment) { vendorList.Add(await getVendorByType(VendorTypes.Entertainment)); }
            if (theEvent.Security) { vendorList.Add(await getVendorByType(VendorTypes.Security)); }
            if (theEvent.RoyalTransport) { vendorList.Add(await getVendorByType(VendorTypes.RoyalTransport)); }
            if (theEvent.OrdinaryTransport) { vendorList.Add(await getVendorByType(VendorTypes.OrdinaryTransport)); }
            if (theEvent.Decoration) { vendorList.Add(await getVendorByType(VendorTypes.Decoration)); }

            //calculate and allocate Budget
            await CalculateAndAllocateBudget(vendorList, EventID);
            return await _db.Budgets.Where(bd => bd.EventID == EventID).ToListAsync();
        }

        public async Task<Dictionary<string, Vendor>> getVendorByType(string Type)
        {
            Dictionary<string, Vendor> vendorDict = new Dictionary<string, Vendor>();

            var vendorMin = await _db.Vendors.Where(v => v.Type == Type)
              .OrderBy(v => v.Price)
              .FirstOrDefaultAsync<Vendor>();

            var vendorMax = await _db.Vendors
                .Where(v => v.Type == Type)
                .OrderByDescending(v => v.Price)
              .FirstOrDefaultAsync<Vendor>();

            if (vendorMin != null) vendorDict.Add(Min, vendorMin);
            if (vendorMax != null) vendorDict.Add(Max, vendorMax);
            return vendorDict;
        }

        public async Task CalculateAndAllocateBudget(List<Dictionary<string, Vendor>> vendorList, int EventID)
        {
            double? sumMin = 0.0;
            double? sumMax = 0.0;


            vendorList.ForEach(vDict =>
            {
                sumMin = sumMin + vDict[Min].Price;
                sumMax = sumMax + vDict[Max].Price;
            });

            //create budget
            Budget budgetMin = new Budget()
            {
                BudgetType = Min,
                Value = sumMin,
                EventID = EventID,
            };
            Budget budgetMax = new Budget()
            {
                BudgetType = Max,
                Value = sumMax,
                EventID = EventID,
            };

            await _db.Budgets.AddAsync(budgetMin);
            await _db.Budgets.AddAsync(budgetMax);
            await _db.SaveChangesAsync();

            foreach (var vendorDict in vendorList)
            {
                //get the Budget and add associate it to the event
                BudgetVendor bvMin = new BudgetVendor()
                {
                    VendorID = vendorDict[Min].ID,
                    BudgetID = budgetMin.ID,
                };
                BudgetVendor bvMax = new BudgetVendor()
                {
                    VendorID = vendorDict[Max].ID,
                    BudgetID = budgetMax.ID,
                };

                await _db.AddAsync(bvMin);
                await _db.AddAsync(bvMax);
                await _db.SaveChangesAsync();
            }

        }

    }
}

