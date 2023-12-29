using Microsoft.EntityFrameworkCore.Infrastructure;
using MySql.EntityFrameworkCore.Infrastructure;

namespace Webrox.EntityFrameworkCore.MySql
{
    public static class DbContextOptionsBuilderExtensions
    {
        public static MySQLDbContextOptionsBuilder AddRowNumberSupport(
                   this MySQLDbContextOptionsBuilder optionsBuilder)
        {
            var infrastructure = (IRelationalDbContextOptionsBuilderInfrastructure)optionsBuilder;

            Core.DbContextOptionsBuilderExtensions.AddRowNumberSupport(infrastructure);

            return optionsBuilder;
        }
    }
}
