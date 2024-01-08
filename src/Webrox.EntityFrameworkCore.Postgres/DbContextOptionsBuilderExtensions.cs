using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Query;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;
using Webrox.EntityFrameworkCore.Core.Infrastructure;
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
        public static NpgsqlDbContextOptionsBuilder AddWebroxFeatures(
                   this NpgsqlDbContextOptionsBuilder optionsBuilder)
        {
            var infrastructure = (IRelationalDbContextOptionsBuilderInfrastructure)optionsBuilder;


/* Unmerged change from project 'Webrox.EntityFrameworkCore.Postgres (net7.0)'
Before:
            Core.WebroxDbContextOptionsBuilderExtensions.AddWebroxFeatures(infrastructure);
After:
            Core.Infrastructure.WebroxDbContextOptionsBuilderExtensions.AddWebroxFeatures(infrastructure);
*/
            WebroxDbContextOptionsBuilderExtensions.AddWebroxFeatures(infrastructure);

            // Add custom functions Windowing
            infrastructure.OptionsBuilder.ReplaceService<IRelationalParameterBasedSqlProcessorFactory, WebroxPostgreSqlParameterBasedSqlProcessorFactory>();
            infrastructure.OptionsBuilder.ReplaceService<IQuerySqlGeneratorFactory, WebroxPostgreSqlQuerySqlGeneratorFactory>();

            //rewrite Linq/Select
            infrastructure.OptionsBuilder.ReplaceService<IQueryTranslationPreprocessorFactory, WebroxPostgresQueryTranslationPreprocessorFactory>();

            return optionsBuilder;
        }
    }
}
