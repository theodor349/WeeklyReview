﻿namespace Shared.Services;

internal class TimeService : ITimeService
{
    public DateTime Current => DateTime.Now;
}
