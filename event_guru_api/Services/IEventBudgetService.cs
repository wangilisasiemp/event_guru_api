using System;
using event_guru_api.models;

namespace event_guru_api.Services
{
    public interface IEventBudgetService
    {
        public Task<ICollection<Budget>> getBudgetVendors(int EventID);
    }
}

