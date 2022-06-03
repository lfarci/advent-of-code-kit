﻿using AdventOfCode2021.CommandLineInterface.WebClient;
using CommandLineInterface.Data;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Tests.CommandLineInterface.Data
{
    public class DayPageRepositoryTests
    {
        private static readonly int defaultYear = 2021;
        private static readonly int defaultDay = 1;

        [Fact]
        public async Task FindByYearAndDayAsync_ClientThrowsIOException_ThrowsIOException()
        {
            var repository = GetRepositoryThatThrows<IOException>();
            var thrown = await Assert.ThrowsAsync<IOException>(async () => {
                await repository.FindByYearAndDayAsync(defaultYear, defaultDay);
            });
            Assert.Equal(DayPageRepository.GetNotFoundErrorMessage(defaultYear, defaultDay), thrown.Message);
        }

        [Theory]
        [InlineData("")]
        [InlineData("Invalid page")]
        [InlineData("<body><main></main></body>")]
        public async Task FindByYearAndDayAsync_ParserThrowsFormatException_ThrowsInvalidOperation(string invalidPage)
        {
            var repository = GetRepositoryThatReturns(invalidPage);
            var thrown = await Assert.ThrowsAsync<InvalidOperationException>(async () => {
                await repository.FindByYearAndDayAsync(defaultYear, defaultDay);
            });
            Assert.Equal(DayPageRepository.GetParseErrorMessage(defaultYear, defaultDay), thrown.Message);
        }

        [Theory]
        [InlineData("Tests.Resources.NotStartedDayPage.html")]
        [InlineData("Tests.Resources.CompleteDayPage.html")]
        [InlineData("Tests.Resources.VeryCompleteDayPage.html")]
        public async Task FindByYearAndDayAsync_ValidPage_ReturnsDay(string resourceName)
        {
            var content = Helpers.ReadResourceContentAsString(resourceName);
            var repository = GetRepositoryThatReturns(content);
            Assert.NotNull(await repository.FindByYearAndDayAsync(defaultYear, defaultDay));
        }

        private static IDayPageRepository GetRepositoryThatThrows<TException>() where TException : Exception, new()
        {
            var client = Helpers.GetClientThatThrows<IOException>(c => c.GetDayPageAsStreamAsync(defaultYear, defaultDay));
            return new DayPageRepository(client);
        }

        private static IDayPageRepository GetRepositoryThatReturns(string result)
        {
            var client = Helpers.GetClientThatReturns(result, c => c.GetDayPageAsStreamAsync(defaultYear, defaultDay).Result);
            return new DayPageRepository(client);
        }
    }
}
