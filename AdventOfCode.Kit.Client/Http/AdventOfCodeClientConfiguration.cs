﻿namespace AdventOfCode.Kit.Client.Http
{
    internal class AdventOfCodeClientConfiguration : IConfiguration
    {
        internal static readonly string hostEnvironmentVariableName = "AOC_HOST";
        internal static readonly string sessionIdEnvironmentVariableName = "AOC_SESSION_ID";
        internal static readonly string defaultHost = "adventofcode.com";
        internal static readonly string defaultSessionId = "";

        public string Host { get; init; }
        public string SessionId { get; init; }

        public AdventOfCodeClientConfiguration(string? host, string? sessionId)
        {
            Host = RequireNotNullOrEmpty(host, "Host");
            SessionId = RequireNotNullOrEmpty(sessionId, "Session");
        }

        public AdventOfCodeClientConfiguration() 
            : this(
                  Environment.GetEnvironmentVariable(hostEnvironmentVariableName) ?? defaultHost,
                  Environment.GetEnvironmentVariable(sessionIdEnvironmentVariableName) ?? defaultSessionId)
        { }

        internal static string RequireNotNullOrEmpty(string? text, string? fieldName = "text")
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentException($"A value is required for '{fieldName}'.");
            }
            return text;
        }
    }
}
