using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Query;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;
using Webrox.EntityFrameworkCore.Postgres.Query;

namespace Webrox.EntityFrameworkCore.Postgres
{
    /// <summary>
    /// DbContextOptionsBuilder Extensions
    /// </summary>
    public static class DbContextOptionsBuilderExtensions
    {
        /// <summary>
        /// Add RowNumber support
        /// </summary>
        /// <param name="optionsBuilder">options Builder</param>
        /// <returns><see cref="NpgsqlDbContextOptionsBuilder"/></returns>
        public static NpgsqlDbContextOptionsBuilder AddRowNumberSupport(
                   this NpgsqlDbContextOptionsBuilder optionsBuilder)
        {
            var infrastructure = (IRelationalDbContextOptionsBuilderInfrastructure)optionsBuilder;

            Core.WebroxDbContextOptionsBuilderExtensions.AddRowNumberSupport(infrastructure);

            infrastructure.OptionsBuilder.ReplaceService<IRelationalParameterBasedSqlProcessorFactory, WebroxPostgreSqlParameterBasedSqlProcessorFactory>();
            infrastructure.OptionsBuilder.ReplaceService<IQuerySqlGeneratorFactory, WebroxPostgreSqlQuerySqlGeneratorFactory>();

            return optionsBuilder;
        }
    }
}
