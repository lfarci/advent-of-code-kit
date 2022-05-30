﻿using AdventOfCode2021.Challenges;
using AdventOfCode2021.CommandLineInterface.Data;
using CommandLineInterface.Data;

namespace AdventOfCode2021
{
    public static class Program
    {

        private static readonly string ProgramFriendlyName = AppDomain.CurrentDomain.FriendlyName;
        private static readonly IChallengeInputRepository inputRepository = ChallengeInputRepository.Instance;
        private static readonly ICalendarRepository calendarRepository = CalendarRepository.Instance;

        static async Task Main(string[] args)
        {
            await calendarRepository.FindByYear(2020);
            /*if (args.Length == 1)
            {
                DailyChallenge? challenge = DailyChallengeRepository.FindByDay(args[0]);
                if (challenge != null)
                {
                    string[] lines = await inputRepository.FindInputLinesByYearAndDayAsync(DailyChallenge.Year, challenge.Day);
                    IEnumerable<DailyChallenge.PuzzleAnswer> answers = challenge.Run(lines);
                    Console.WriteLine($"Day {challenge.Day}: {challenge.Title}");
                    foreach (DailyChallenge.PuzzleAnswer answer in answers)
                    {
                        Console.WriteLine($"\t- {answer.Description}: {answer.Value}.");
                    }
                }
                else
                {
                    Console.WriteLine($"No daily challenge could be resolved with key: \"{args[0]}\".");
                }
            }
            else
            {
                Console.WriteLine($"Usage: {ProgramFriendlyName} ## (Daily challenge key).");
            }*/
        }
    }
}