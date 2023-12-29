using Microsoft.EntityFrameworkCore.Infrastructure;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;

namespace Webrox.EntityFrameworkCore.Postgres
{
    public static class DbContextOptionsBuilderExtensions
    {
        public static NpgsqlDbContextOptionsBuilder AddRowNumberSupport(
                   this NpgsqlDbContextOptionsBuilder optionsBuilder)
        {
            var infrastructure = (IRelationalDbContextOptionsBuilderInfrastructure)optionsBuilder;

            Core.DbContextOptionsBuilderExtensions.AddRowNumberSupport(infrastructure);

            return optionsBuilder;
        }
    }
}
