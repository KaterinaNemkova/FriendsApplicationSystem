// <copyright file="IDateNotificationJobService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace EventsService.Application.Contracts;

public interface IDateNotificationJobService
{
    Task CheckImportantDates();
}