using System;

namespace Mushka.Application.Analytics.Providers;

public sealed class AnalyticsPeriodProvider : IAnalyticsPeriodProvider
{
    public DateTime StartOfWindow(DateTime now, int periodInMonth)
    {
        var limitDate = now.AddMonths(-periodInMonth + 1);
        return new DateTime(limitDate.Year, limitDate.Month, 1);
    }
}
