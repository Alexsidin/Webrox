using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Webrox.EntityFrameworkCore.SqlServer
{
    public static class DbContextOptionsBuilderExtensions
    {
        public static SqlServerDbContextOptionsBuilder AddRowNumberSupport(
                   this SqlServerDbContextOptionsBuilder optionsBuilder)
        {
            var infrastructure = (IRelationalDbContextOptionsBuilderInfrastructure)optionsBuilder;

            Core.DbContextOptionsBuilderExtensions.AddRowNumberSupport(infrastructure);

            return optionsBuilder;
        }
    }
}
