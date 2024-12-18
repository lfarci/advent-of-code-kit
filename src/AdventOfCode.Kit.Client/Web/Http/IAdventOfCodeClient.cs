﻿namespace AdventOfCode.Kit.Client.Web.Http
{
    internal interface IAdventOfCodeClient
    {
        Task<Stream> GetCalendarPageAsStreamAsync(int year);
        Task<Stream> GetDayPageAsStreamAsync(int year, int day);
        Task<Stream> GetPuzzleInputAsStreamAsync(int year, int day);
    }
}