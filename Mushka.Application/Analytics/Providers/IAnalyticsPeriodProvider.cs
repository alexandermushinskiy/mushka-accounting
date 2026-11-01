using System;

namespace Mushka.Application.Analytics.Providers;

public interface IAnalyticsPeriodProvider
{
    DateTime StartOfWindow(DateTime now, int periodInMonth);
}
