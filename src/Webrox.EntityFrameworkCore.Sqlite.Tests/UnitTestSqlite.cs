using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Webrox.EntityFrameworkCore.Sqlite;
using Xunit;
using Webrox.EntityFrameworkCore.Tests.Shared;
using Xunit.Abstractions;

namespace Webrox.EntityFrameworkCore.Sqlite.Tests
{
    public class UnitTestSqlite : UnitTest, IDisposable
    {
        private readonly SqliteConnection _connection;
        public UnitTestSqlite(ITestOutputHelper output)
        {
            _output = output;
            _connection = new SqliteConnection("datasource=:memory:");
            _connection.Open();

            _options = new DbContextOptionsBuilder<SampleDbContext>()
                .UseSqlite(_connection, opt =>
                {
                    opt.AddWebroxFeatures();
                })
                .LogTo(logText =>
                {
                    bool isMySQL = true;
                    var splittedLogText = logText.Split(Environment.NewLine).ToList();
                    splittedLogText[0] = $"{new string('-', 2)}{(isMySQL ? "MySQL" : "SQLServer")}{new string('-', 80)}";
                    splittedLogText.Insert(0, string.Empty);
                    splittedLogText.Insert(2, string.Empty);

                    logText = string.Join(Environment.NewLine, splittedLogText);

                    //logger?.LogTrace(logText);

                    var fgColor = Console.ForegroundColor;
                    Console.ForegroundColor = isMySQL ? ConsoleColor.Blue : ConsoleColor.Yellow;
                    Console.WriteLine(logText);
                    Debug.WriteLine(logText);
                    Console.ForegroundColor = fgColor;
                },
                (b, c) =>
                {
                    return (b.Id == RelationalEventId.CommandExecuting); //only SQL Queries
                })
                .Options;

            using (var context = new SampleDbContext(_options))
                context.Database.EnsureCreated();
        }

        public void Dispose()
        {
            _connection.Close();
        }
    }
}