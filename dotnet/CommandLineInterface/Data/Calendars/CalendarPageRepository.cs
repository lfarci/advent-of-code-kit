﻿using AdventOfCode2021.CommandLineInterface.WebClient;

namespace CommandLineInterface.Data
{
    public class CalendarPageRepository : ICalendarPageRepository
    {
        private static ICalendarPageRepository? instance;
        private readonly IAdventOfCodeClient _client = AdventOfCodeClient.Instance;
        public static ICalendarPageRepository Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CalendarPageRepository();
                }
                return instance;
            }
        }

        protected CalendarPageRepository() : this(AdventOfCodeClient.Instance)
        { }

        public CalendarPageRepository(IAdventOfCodeClient client)
        {
            _client = client;
        }

        public static string GetParseErrorMessage(int year) => $"Could not parse calendar for year {year}.";
        public static string GetNotFoundErrorMessage(int year) => $"Could not find cannot calendar for year {year}.";

        public async Task<CalendarPage> FindByYearAsync(int year)
        {
            try
            {
                using Stream stream = await _client.GetCalendarPageAsStreamAsync(year);
                using StreamReader reader = new(stream);
                return CalendarPage.Parse(await reader.ReadToEndAsync());
            }
            catch (FormatException e)
            {
                throw new InvalidOperationException(GetParseErrorMessage(year), e);
            }
            catch (IOException e)
            {
                throw new IOException(GetNotFoundErrorMessage(year), e);
            }
        }
    }
}
