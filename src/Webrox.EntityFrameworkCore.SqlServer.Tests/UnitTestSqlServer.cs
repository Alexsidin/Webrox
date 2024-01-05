using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Webrox.EntityFrameworkCore.SqlServer;
using Xunit;
using Webrox.EntityFrameworkCore.Tests.Shared;

namespace Webrox.EntityFrameworkCore.SqlServer.Tests
{
    public class UnitTestSqlServer : UnitTest, IDisposable
    {
        private readonly SqlConnection _connection;

        public void DeleteDatabase()
        {
            var connection = new SqlConnection("Server=poppyto6;Integrated Security=true;Initial Catalog=efcore;TrustServerCertificate=true;");
            connection.Open();

            var options = new DbContextOptionsBuilder<SampleDbContext>()
                .UseSqlServer(connection)
                .Options;

            using (var context = new SampleDbContext(options))
            {
                context.Database.ExecuteSqlRaw("DROP TABLE IF EXISTS  users");//.EnsureDeleted();
            }
        }

        public UnitTestSqlServer()
        {
            DeleteDatabase();

            _connection = new SqlConnection("Server=poppyto6;Integrated Security=true;Initial Catalog=efcore;TrustServerCertificate=true;");
            _connection.Open();

            _options = new DbContextOptionsBuilder<SampleDbContext>()
                .UseSqlServer(_connection, opt =>
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
                context.Database.EnsureCreated();
        }

        public void Dispose()
        {
            _connection.Close();
        }
    }
}