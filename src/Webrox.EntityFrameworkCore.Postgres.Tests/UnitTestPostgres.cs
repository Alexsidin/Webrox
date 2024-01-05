using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Npgsql;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Webrox.EntityFrameworkCore.Core;
using Webrox.EntityFrameworkCore.Postgres;
using Xunit;
using Webrox.EntityFrameworkCore.Tests.Shared;
using Xunit.Abstractions;

namespace Webrox.EntityFrameworkCore.Postgres.Tests
{
    public class UnitTestPostgres : UnitTest, IDisposable
    {
        private readonly NpgsqlConnection _connection;

        public void DeleteDatabase()
        {
            using var connection = new NpgsqlConnection("Host=localhost;Port=5432;Username=postgres;Password=Popome690$;Database=efcore;");
            connection.Open();
            var options = new DbContextOptionsBuilder<SampleDbContext>()
                .UseNpgsql(connection)
                .Options;

            using (var context = new SampleDbContext(options))
            {
                context.Database.ExecuteSqlRaw("DROP TABLE IF EXISTS users;");//.EnsureDeleted();
            }
        }
        public UnitTestPostgres(ITestOutputHelper output)
        {
            _output = output;
            DeleteDatabase();

            _connection = new NpgsqlConnection("Host=localhost;Port=5432;Username=postgres;Password=Popome690$;Database=efcore;");
            _connection.Open();

            _options = new DbContextOptionsBuilder<SampleDbContext>()
                .UseNpgsql(_connection, opt =>
                {
                    opt.AddRowNumberSupport();

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
            {
                context.Database.EnsureCreated();
            }

        }

        public void Dispose()
        {
            _connection.Close();
        }
    }
}
