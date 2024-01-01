using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System;
using System.Linq;
using System.Threading.Tasks;
using Webrox.EntityFrameworkCore.Core;
using Webrox.EntityFrameworkCore.MySql;
using Webrox.EntityFrameworkCore.Sqlite;
using Xunit;

namespace Webrox.EntityFrameworkCore.Tests
{
    public class UnitTestMySql : IDisposable
    {
        private readonly MySqlConnection _connection;
        private readonly DbContextOptions<SampleDbContext> _options;

        public UnitTestMySql()
        {
            _connection = new MySqlConnection("server=localhost;user id=root;password=Clef2Cqrit100%;persistsecurityinfo=True;database=efcore;port=3306;");
            _connection.Open();

            _options = new DbContextOptionsBuilder<SampleDbContext>()
                .UseMySQL(_connection, opt =>
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
        public async Task TestRowNumber_UsingSqliteInMemoryProvider()
        {
            using var context = new SampleDbContext(_options);

            //var count = await context.Users.CountAsync();
            //Assert.Equal(10, count);

            var windowFunctions = await context.Users
                .Select(a => new
                {
                    Id = a.Id,
                    RowNumber = EF.Functions.RowNumber(EF.Functions.OrderBy(a.Id)),
                    RowNumberPartition = EF.Functions.RowNumber(
                                            EF.Functions.PartitionBy(a.RoleId),
                                            EF.Functions.OrderBy(a.Id)),
                    Rank = EF.Functions.Rank(EF.Functions.OrderBy(a.Id)),
                    RankPartition = EF.Functions.Rank(
                                            EF.Functions.PartitionBy(a.RoleId),
                                            EF.Functions.OrderBy(a.Id)),
                    DenseRank = EF.Functions.DenseRank(EF.Functions.OrderBy(a.Id)),
                    DenseRankPartition = EF.Functions.DenseRank(
                                            EF.Functions.PartitionBy(a.RoleId),
                                            EF.Functions.OrderBy(a.Id)),
                    Average = EF.Functions.Average(a.Id, EF.Functions.OrderBy(a.Id)),
                    AveragePartition = EF.Functions.Average(a.Id,
                                            EF.Functions.PartitionBy(a.RoleId),
                                            EF.Functions.OrderBy(a.Id)),
                    Sum = EF.Functions.Sum(a.Id, EF.Functions.OrderBy(a.Id)),
                    SumPartition = EF.Functions.Sum(a.Id,
                                            EF.Functions.PartitionBy(a.RoleId),
                                            EF.Functions.OrderBy(a.Id)),
                    Min = EF.Functions.Min(a.Id, EF.Functions.OrderBy(a.Id)),
                    MinPartition = EF.Functions.Min(a.Id,
                                            EF.Functions.PartitionBy(a.RoleId),
                                            EF.Functions.OrderBy(a.Id)),

                    Max = EF.Functions.Max(a.Id, EF.Functions.OrderBy(a.Id)),
                    MaxPartition = EF.Functions.Max(a.Id,
                                            EF.Functions.PartitionBy(a.RoleId),
                                            EF.Functions.OrderBy(a.Id)),


                }).ToListAsync();

            Assert.NotNull(windowFunctions);
            Assert.Equal(10, windowFunctions.Count);

        }
    }
}
