using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Webrox.EntityFrameworkCore.Sqlite
{
    public static class DbContextOptionsBuilderExtensions
    {
        public static SqliteDbContextOptionsBuilder AddRowNumberSupport(
                   this SqliteDbContextOptionsBuilder optionsBuilder)
        {
            var infrastructure = (IRelationalDbContextOptionsBuilderInfrastructure)optionsBuilder;

            Core.DbContextOptionsBuilderExtensions.AddRowNumberSupport(infrastructure);

            return optionsBuilder;
        }
    }
}
