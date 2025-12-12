namespace CloudCare.Data.Models;

public enum BillingCycle
{
    None = 0,       // For non-recurring expenses
    Weekly = 1,
    BiWeekly = 2,   // Every two weeks
    Monthly = 3,
    Quarterly = 4,  // Every 3 months
    BiAnnually = 5, // Every 6 months (not currently being used)
    Annually = 6    // Yearly
}
