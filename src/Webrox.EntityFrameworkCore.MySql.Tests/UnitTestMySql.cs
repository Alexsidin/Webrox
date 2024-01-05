using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using MySql.Data.MySqlClient;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Webrox.EntityFrameworkCore.Tests.Shared;
using Xunit;
using Xunit.Abstractions;

namespace Webrox.EntityFrameworkCore.MySql.Tests
{
    public class UnitTestMySql : UnitTest, IDisposable
    {
        private readonly MySqlConnection _connection;

        public void DeleteDatabase()
        {
            var connection = new MySqlConnection("server=localhost;user id=root;password=Clef2Cqrit100%;persistsecurityinfo=True;database=efcore;port=3306;");
            connection.Open();

            var options = new DbContextOptionsBuilder<SampleDbContext>()
                .UseMySQL(connection)
                .Options;

            using (var context = new SampleDbContext(options))
            {
                context.Database.ExecuteSqlRaw("DROP TABLE IF EXISTS  `users`");//.EnsureDeleted();
            }
        }
        public UnitTestMySql(ITestOutputHelper output)
        {
            _output = output;
            DeleteDatabase();

            _connection = new MySqlConnection("server=localhost;user id=root;password=Clef2Cqrit100%;persistsecurityinfo=True;database=efcore;port=3306;");
            _connection.Open();

            _options = new DbContextOptionsBuilder<SampleDbContext>()
                .UseMySQL(_connection, opt =>
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
