﻿using AdventOfCode.Kit.Console.Core;
using AdventOfCode.Kit.Console.Commands;
using Spectre.Console.Cli;
using AdventOfCode.Kit.Console.View;
using AdventOfCode.Kit.Client;

namespace AdventOfCode.Kit.Console
{
    public class AdventOfCodeConsole
    {
        private static AdventOfCodeConsole? instance;
        public static AdventOfCodeConsole Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AdventOfCodeConsole();
                }
                return instance;
            }
        }

        internal IAdventOfCodeView Console { get; init; }
        internal IAdventOfCodeClient Client { get; init;  }
        internal IDictionary<int, IPuzzleSubmitter> Submitters { get; init; }

        internal AdventOfCodeConsole()
            : this(new AdventOfCodeClient(), new AdventOfCodeView())
        {}

        internal AdventOfCodeConsole(IAdventOfCodeClient client, IAdventOfCodeView console)
        {
            Client = client;
            Console = console;
            Submitters = new Dictionary<int, IPuzzleSubmitter>();
        }

        private bool HasSubmitterFor(int year) => Submitters.ContainsKey(year);

        internal IPuzzleSubmitter FindSubmitter(int year)
        {
            if (!HasSubmitterFor(year))
            {
                throw new InvalidOperationException($"No context for year: {year}");
            }
            return Submitters[year];
        }

        public void StartYear(int year, Action<IPuzzleSubmitter> onYearInitialized)
        {
            if (HasSubmitterFor(year))
            {
                throw new InvalidOperationException($"Trying to add the same year twice: {year}.");
            }

            try
            {
                IPuzzleSubmitter context = new AdventOfCodeContext(year, Client);
                Submitters[year] = context;
                Console.Status($"Initializing year {year}...", () =>
                {
                    ((AdventOfCodeContext) context).Initialize(onYearInitialized).Wait();
                });
            }
            catch (Exception)
            {
                Console.ShowError($"Failed to add year {year}.");
            }
        }

        internal void ShowPuzzleAnswers(int year, int dayIndex)
        {
            Console.Status($"Downloading input for year {year} and day {dayIndex}...", () =>
            {
                try
                {
                    string[] lines = Client.FindPuzzleInputByYearAndDayAsync(year, dayIndex).Result;
                    var submitter = FindSubmitter(year);

                    if (submitter?.Calendar != null)
                    {
                        var day = submitter.Calendar[dayIndex];
                        Console.ShowPuzzleAnswers(day, lines);
                    }
                    else
                    {
                        Console.ShowError($"Could not run submitted puzzle because year {year} isn't initialized.");
                    }
                }
                catch (IOException)
                {
                    Console.ShowError($"Failed to fetch the input data for the requested puzzle."); 
                }
            });
        }

        public int Run(string[] args)
        {
            var app = new CommandApp();
            app.Configure(config =>
            {
                config.AddCommand<ShowPuzzleAnswers>("run")
                    .WithDescription("Runs the submitted puzzle and shows its answers.")
                    .WithExample(new[] { "run", "2020", "1" });
            });
            return app.Run(args);
        }
    }
}
