using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Query;
using Npgsql;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
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
                   this NpgsqlDbContextOptionsBuilder optionsBuilder,
                   NpgsqlConnection connection)
        {
            var infrastructure = (IRelationalDbContextOptionsBuilderInfrastructure)optionsBuilder;

            WebroxDbContextOptionsBuilderExtensions.AddWebroxFeatures(infrastructure,"postgres");
       
            // Add custom functions Windowing
            infrastructure.OptionsBuilder.ReplaceService<IRelationalParameterBasedSqlProcessorFactory, WebroxPostgreSqlParameterBasedSqlProcessorFactory>();
            infrastructure.OptionsBuilder.ReplaceService<IQuerySqlGeneratorFactory, WebroxPostgreSqlQuerySqlGeneratorFactory>();

            //rewrite Linq/Select
            infrastructure.OptionsBuilder.ReplaceService<IQueryTranslationPreprocessorFactory, WebroxPostgresQueryTranslationPreprocessorFactory>();

            //SubQuery
            infrastructure.OptionsBuilder.ReplaceService<IQueryableMethodTranslatingExpressionVisitorFactory, WebroxPostgresQueryableMethodTranslatingExpressionVisitorFactory>();

            //create collations for String.Equals(column, StringComparison) translation
            CreateCollation(connection, "webrox_ignore_accent_case", "icu", false, "und-u-ks-level1");
            CreateCollation(connection, "webrox_accent_case", "icu", true, "und-u-ks-level1");

            return optionsBuilder;
        }

        static void CreateCollation(NpgsqlConnection connection,
            string collationName,
            string provider,
            bool deterministic,
            string locale
            )
        {
            var cmd = connection.CreateCommand();
            cmd.CommandText = $"CREATE COLLATION IF NOT EXISTS {collationName} (provider = {provider}, deterministic = {deterministic}, locale = '{locale}');";
            cmd.ExecuteNonQuery();
        }
    }
}
