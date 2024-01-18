using Microsoft.EntityFrameworkCore;
using Webrox.EntityFrameworkCore.Core;
using System.Threading.Tasks;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using System.Text.Json;
using System.Text.Json.Serialization;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Linq.Expressions;

namespace Webrox.EntityFrameworkCore.Tests.Shared
{
    public class UnitTest
    {
        internal DbContextOptions<SampleDbContext> _options;

        protected ITestOutputHelper _output;
        static JsonSerializerOptions _jsonOptions;
        static UnitTest()
        {
            _jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true
            };
        }

        [Fact]
        public async Task TestRowNumber()
        {
            using var context = new SampleDbContext(_options);

            var count = await context.Users.CountAsync();
            Assert.Equal(10, count);

            var query = context.Users
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


                });

            var windowFunctions = await query.ToListAsync();

            Assert.NotNull(windowFunctions);
            Assert.Equal(10, windowFunctions.Count);

        }


        [Fact]
        public async Task TestRowNumberSubQuery()
        {
            using var context = new SampleDbContext(_options);

            var count = await context.Users.CountAsync();
            Assert.Equal(10, count);

            var query = context.Users
                .Select(a => new
                {
                    Id = a.Id,
                    RowNumber = EF.Functions.RowNumber(EF.Functions.OrderBy(a.Id)),
                    Sum = EF.Functions.Sum(a.Id, EF.Functions.OrderBy(a.Id)),
                })
                .AsSubQuery()
                .Where(a=>a.RowNumber > 5);

            _output.WriteLine(query.ToQueryString());


            var windowFunctions = await query.ToListAsync();

            Assert.Equal(windowFunctions.Count, 5);

        }



        [Fact]
        public async Task TestSelect()
        {
            using var context = new SampleDbContext(_options);

            //var count = await context.Users.CountAsync();
            //Assert.Equal(10, count);

            var query =  context.Users
                .Select((a, index) => new
                {
                    Id = a.Id,
                    Index = index,
                })
                ;

            _output.WriteLine(query.ToQueryString());

            var selectFunctions = await query.ToListAsync();

            Assert.NotNull(selectFunctions);
            Assert.Equal(10, selectFunctions.Count);

        }

        [Fact]
        public async Task TestCasts()
        {
            using var context = new SampleDbContext(_options);

            //var count = await context.Users.CountAsync();
            //Assert.Equal(10, count);

            var query = context.Users
                .Select(a => new
                {
                    Id = a.Id,

                    Sum_8 = EF.Functions.Sum(a.SubRoleId8, EF.Functions.OrderBy(a.Id)),
                    Sum_u8 = EF.Functions.Sum(a.SubRoleIdu8, EF.Functions.OrderBy(a.Id)),
                    Sum_16 = EF.Functions.Sum(a.SubRoleId16, EF.Functions.OrderBy(a.Id)),
                    Sum_u16 = EF.Functions.Sum(a.SubRoleIdu16, EF.Functions.OrderBy(a.Id)),
                    Sum_32 = EF.Functions.Sum(a.SubRoleId32, EF.Functions.OrderBy(a.Id)),
                    Sum_u32 = EF.Functions.Sum(a.SubRoleIdu32, EF.Functions.OrderBy(a.Id)),
                    Sum_64 = EF.Functions.Sum(a.SubRoleId64, EF.Functions.OrderBy(a.Id)),
                    Sum_u64 = EF.Functions.Sum(a.SubRoleIdu64, EF.Functions.OrderBy(a.Id)),
                    Sum_dec = EF.Functions.Sum(a.SubRoleIdDecimal, EF.Functions.OrderBy(a.Id)),
                    Sum_float = EF.Functions.Sum(a.SubRoleIdFloat, EF.Functions.OrderBy(a.Id)),
                    Sum_double = EF.Functions.Sum(a.SubRoleIdDouble, EF.Functions.OrderBy(a.Id)),

                    Average_8 = EF.Functions.Average(a.SubRoleId8, EF.Functions.OrderBy(a.Id)),
                    Average_u8 = EF.Functions.Average(a.SubRoleIdu8, EF.Functions.OrderBy(a.Id)),
                    Average_16 = EF.Functions.Average(a.SubRoleId16, EF.Functions.OrderBy(a.Id)),
                    Average_u16 = EF.Functions.Average(a.SubRoleIdu16, EF.Functions.OrderBy(a.Id)),
                    Average_32 = EF.Functions.Average(a.SubRoleId32, EF.Functions.OrderBy(a.Id)),
                    Average_u32 = EF.Functions.Average(a.SubRoleIdu32, EF.Functions.OrderBy(a.Id)),
                    Average_64 = EF.Functions.Average(a.SubRoleId64, EF.Functions.OrderBy(a.Id)),
                    Average_u64 = EF.Functions.Average(a.SubRoleIdu64, EF.Functions.OrderBy(a.Id)),
                    Average_dec = EF.Functions.Average(a.SubRoleIdDecimal, EF.Functions.OrderBy(a.Id)),
                    Average_float = EF.Functions.Average(a.SubRoleIdFloat, EF.Functions.OrderBy(a.Id)),
                    Average_double = EF.Functions.Average(a.SubRoleIdDouble, EF.Functions.OrderBy(a.Id)),

                    Min_8 = EF.Functions.Min(a.SubRoleId8, EF.Functions.OrderBy(a.Id)),
                    Min_u8 = EF.Functions.Min(a.SubRoleIdu8, EF.Functions.OrderBy(a.Id)),
                    Min_16 = EF.Functions.Min(a.SubRoleId16, EF.Functions.OrderBy(a.Id)),
                    Min_u16 = EF.Functions.Min(a.SubRoleIdu16, EF.Functions.OrderBy(a.Id)),
                    Min_32 = EF.Functions.Min(a.SubRoleId32, EF.Functions.OrderBy(a.Id)),
                    Min_u32 = EF.Functions.Min(a.SubRoleIdu32, EF.Functions.OrderBy(a.Id)),
                    Min_64 = EF.Functions.Min(a.SubRoleId64, EF.Functions.OrderBy(a.Id)),
                    Min_u64 = EF.Functions.Min(a.SubRoleIdu64, EF.Functions.OrderBy(a.Id)),
                    Min_dec = EF.Functions.Min(a.SubRoleIdDecimal, EF.Functions.OrderBy(a.Id)),
                    Min_float = EF.Functions.Min(a.SubRoleIdFloat, EF.Functions.OrderBy(a.Id)),
                    Min_double = EF.Functions.Min(a.SubRoleIdDouble, EF.Functions.OrderBy(a.Id)),

                    Max_8 = EF.Functions.Max(a.SubRoleId8, EF.Functions.OrderBy(a.Id)),
                    Max_u8 = EF.Functions.Max(a.SubRoleIdu8, EF.Functions.OrderBy(a.Id)),
                    Max_16 = EF.Functions.Max(a.SubRoleId16, EF.Functions.OrderBy(a.Id)),
                    Max_u16 = EF.Functions.Max(a.SubRoleIdu16, EF.Functions.OrderBy(a.Id)),
                    Max_32 = EF.Functions.Max(a.SubRoleId32, EF.Functions.OrderBy(a.Id)),
                    Max_u32 = EF.Functions.Max(a.SubRoleIdu32, EF.Functions.OrderBy(a.Id)),
                    Max_64 = EF.Functions.Max(a.SubRoleId64, EF.Functions.OrderBy(a.Id)),
                    Max_u64 = EF.Functions.Max(a.SubRoleIdu64, EF.Functions.OrderBy(a.Id)),
                    Max_dec = EF.Functions.Max(a.SubRoleIdDecimal, EF.Functions.OrderBy(a.Id)),
                    Max_float = EF.Functions.Min(a.SubRoleIdFloat, EF.Functions.OrderBy(a.Id)),
                    Max_double = EF.Functions.Min(a.SubRoleIdDouble, EF.Functions.OrderBy(a.Id)),

                });

            _output.WriteLine(query.ToQueryString());


            var windowFunctions = await query.ToListAsync();

            _output.WriteLine(JsonSerializer.Serialize(windowFunctions, _jsonOptions));

            Assert.NotNull(windowFunctions);
            Assert.Equal(10, windowFunctions.Count);

        }

        [Fact]
        public async Task TestStringEquals()
        {
            using var context = new SampleDbContext(_options);

            var query = context.Users
                .Where(a=>a.Email.Equals("sample1@Gm.com", System.StringComparison.OrdinalIgnoreCase)
                )
                ;

            _output.WriteLine(query.ToQueryString());

            var selectFunctions = await query.ToListAsync();

            Assert.NotNull(selectFunctions);
            Assert.Equal(selectFunctions.Count, 1);

            query = context.Users
                .Where(a => a.Email.Equals("sample1@Gm.com", System.StringComparison.Ordinal)
                )
                ;

            _output.WriteLine(query.ToQueryString());

            selectFunctions = await query.ToListAsync();

            Assert.NotNull(selectFunctions);
            Assert.Equal(selectFunctions.Count, 0);

            query = context.Users
               .Where(a => a.Email.Equals("sample1@Gm.com", System.StringComparison.InvariantCultureIgnoreCase)
               )
               ;

            _output.WriteLine(query.ToQueryString());

            selectFunctions = await query.ToListAsync();

            Assert.NotNull(selectFunctions);
            Assert.Equal(selectFunctions.Count, 1);

            query = context.Users
                .Where(a => a.Email.Equals("sample1@Gm.com", System.StringComparison.InvariantCulture)
                )
                ;

            _output.WriteLine(query.ToQueryString());

            selectFunctions = await query.ToListAsync();

            Assert.NotNull(selectFunctions);
            Assert.Equal(selectFunctions.Count, 0);

            query = context.Users
               .Where(a => a.Email.Equals("sample1@Gm.com", System.StringComparison.CurrentCultureIgnoreCase)
               )
               ;

            _output.WriteLine(query.ToQueryString());

            selectFunctions = await query.ToListAsync();

            Assert.NotNull(selectFunctions);
            Assert.Equal(selectFunctions.Count, 1);

            query = context.Users
                .Where(a => a.Email.Equals("sample1@Gm.com", System.StringComparison.CurrentCulture)
                )
                ;

            _output.WriteLine(query.ToQueryString());

            selectFunctions = await query.ToListAsync();

            Assert.NotNull(selectFunctions);
            Assert.Equal(selectFunctions.Count, 0);

        }
    }
}