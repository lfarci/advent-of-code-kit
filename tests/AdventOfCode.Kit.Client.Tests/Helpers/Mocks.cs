﻿using AdventOfCode.Kit.Client;
using AdventOfCode.Kit.Client.Web;
using AdventOfCode.Kit.Client.Web.Resources;
using Moq;
using System;
using System.Linq.Expressions;

namespace AdventOfCodeConsole.Tests.Helpers
{
    internal static class Mocks
    {
        public static IResourceRepository GetRepositoryThatReturns(CalendarPage calendarPage)
        {
            var mock = MockReturnValue<CalendarPage, ICalendarPageRepository>(
                calendarPage,
                r => r.FindByYearAsync(Fixtures.DefaultYear).Result
            );
            return new AdventOfCodeWebsite(mock, new Mock<IDayPageRepository>().Object, new Mock<IPuzzleInputRepository>().Object);
        }

        public static IResourceRepository GetRepositoryThatReturns(DayPage dayPage)
        {
            var mock = MockReturnValue<DayPage, IDayPageRepository>(
                dayPage,
                r => r.FindByYearAndDayAsync(Fixtures.DefaultYear, Fixtures.DefaultDay).Result
            );
            return new AdventOfCodeWebsite(new Mock<ICalendarPageRepository>().Object, mock, new Mock<IPuzzleInputRepository>().Object);
        }

        public static IResourceRepository GetRepositoryThatReturns(string[] inputLines)
        {
            var mock = MockReturnValue<string[], IPuzzleInputRepository>(
                inputLines,
                r => r.FindByYearAndDayAsync(Fixtures.DefaultYear, Fixtures.DefaultDay).Result
            );
            return new AdventOfCodeWebsite(new Mock<ICalendarPageRepository>().Object, new Mock<IDayPageRepository>().Object, mock);
        }

        public static IResourceRepository GetRepositoryThatThrows<TThrown>(Expression<Action<ICalendarPageRepository>> call)
            where TThrown : Exception, new()
        {
            var mock = MockThrown<TThrown, ICalendarPageRepository>(call);
            return new AdventOfCodeWebsite(mock, new Mock<IDayPageRepository>().Object, new Mock<IPuzzleInputRepository>().Object);

        }

        public static IResourceRepository GetRepositoryThatThrows<TThrown>(Expression<Action<IDayPageRepository>> call)
            where TThrown : Exception, new()
        {
            var mock = MockThrown<TThrown, IDayPageRepository>(call);
            return new AdventOfCodeWebsite(new Mock<ICalendarPageRepository>().Object, mock, new Mock<IPuzzleInputRepository>().Object);
        }

        public static IResourceRepository GetRepositoryThatThrows<TThrown>(Expression<Action<IPuzzleInputRepository>> call)
            where TThrown : Exception, new()
        {
            var mock = MockThrown<TThrown, IPuzzleInputRepository>(call);
            return new AdventOfCodeWebsite(new Mock<ICalendarPageRepository>().Object, new Mock<IDayPageRepository>().Object, mock);
        }

        private static TRepository MockReturnValue<TResult, TRepository>(TResult value, Expression<Func<TRepository, TResult>> call)
            where TRepository : class
            where TResult : class
        {
            var mock = new Mock<TRepository>();
            mock.Setup(call).Returns(value);
            return mock.Object;
        }

        private static TRepository MockThrown<TThrown, TRepository>(Expression<Action<TRepository>> call)
            where TRepository : class
            where TThrown : Exception, new()
        {
            var mock = new Mock<TRepository>();
            mock.Setup(call).Throws<TThrown>();
            return mock.Object;
        }
    }
}
