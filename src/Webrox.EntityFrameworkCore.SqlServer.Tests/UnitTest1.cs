using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Xunit;
using Webrox.EntityFrameworkCore.Sqlite;
using System.Linq;
using Webrox.EntityFrameworkCore.Core;

namespace Webrox.EntityFrameworkCore.SqlServer.Tests
{
    public class UnitTest1 : IDisposable
    {
        private readonly SqliteConnection _connection;
        private readonly DbContextOptions<SampleDbContext> _options;

        public UnitTest1()
        {
            _connection = new SqliteConnection("datasource=:memory:");
            _connection.Open();

            _options = new DbContextOptionsBuilder<SampleDbContext>()
                .UseSqlite(_connection, opt =>
                {
                    opt.AddRowNumberSupport();
                })
                .Options;

            using (var context = new SampleDbContext(_options))
                context.Database.EnsureCreated();
        }

        public void Dispose()
        {
            _connection.Close();
        }
     
        [Fact]
        public async Task TestRowNumber_UsingInMemoryProvider()
        {
            using var context = new SampleDbContext(_options);

            var count = await context.Users.CountAsync();
            Assert.Equal(10, count);

            var windowFunctions = await context.Users
                .Select(a => new
                {
                    Id = a.Id,
                    RowNumber = EF.Functions.RowNumber(EF.Functions.OrderBy(a.Id)),
                    RowNumberPartition = EF.Functions.RowNumber(
                                            EF.Functions.PartitionBy(a.RoleId),
                                            EF.Functions.OrderBy(a.Id)),
                    Rank = EF.Functions.Rank(EF.Functions.OrderBy(a.Id)),
                    RankPartition = EF.Functions.RowNumber(
                                            EF.Functions.PartitionBy(a.RoleId),
                                            EF.Functions.OrderBy(a.Id)),
                }).ToListAsync();

            Assert.NotNull(windowFunctions);
            Assert.Equal(10, windowFunctions.Count);

        }
    }
}
