using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Sqlite.Query.Internal;
using Webrox.EntityFrameworkCore.Core.Infrastructure;
using Webrox.EntityFrameworkCore.Sqlite.Query;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Webrox.EntityFrameworkCore.Sqlite
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
        /// <returns><see cref="SqliteDbContextOptionsBuilder"/></returns>
        public static SqliteDbContextOptionsBuilder AddWebroxFeatures(
                   this SqliteDbContextOptionsBuilder optionsBuilder, SqliteConnection sqliteConnection)
        {
            var infrastructure = (IRelationalDbContextOptionsBuilderInfrastructure)optionsBuilder;

/* Unmerged change from project 'Webrox.EntityFrameworkCore.Sqlite (net7.0)'
Before:
            Core.WebroxDbContextOptionsBuilderExtensions.AddWebroxFeatures(infrastructure);
After:
            Core.Infrastructure.WebroxDbContextOptionsBuilderExtensions.AddWebroxFeatures(infrastructure);
*/
            WebroxDbContextOptionsBuilderExtensions.AddWebroxFeatures(infrastructure, "sqlite");

            // Add custom functions Windowing
            infrastructure.OptionsBuilder.ReplaceService<IRelationalParameterBasedSqlProcessorFactory, WebroxSqliteParameterBasedSqlProcessorFactory>();
            infrastructure.OptionsBuilder.ReplaceService<IQuerySqlGeneratorFactory, WebroxSqliteQuerySqlGeneratorFactory>();
            
            //rewrite Linq/Select
            infrastructure.OptionsBuilder.ReplaceService<IQueryTranslationPreprocessorFactory, WebroxSqliteQueryTranslationPreprocessorFactory>();

            //SubQuery
            infrastructure.OptionsBuilder.ReplaceService<IQueryableMethodTranslatingExpressionVisitorFactory, WebroxSqliteQueryableMethodTranslatingExpressionVisitorFactory>();

            //create collations for String.Equals(column, StringComparison) translation
            sqliteConnection.CreateCollation("Webrox_CurrentCulture", (x, y) => string.Compare(x, y, StringComparison.CurrentCulture));
            sqliteConnection.CreateCollation("Webrox_CurrentCultureIgnoreCase", (x, y) => string.Compare(x, y, StringComparison.CurrentCultureIgnoreCase));
            sqliteConnection.CreateCollation("Webrox_InvariantCulture", (x, y) => string.Compare(x, y, StringComparison.InvariantCulture));
            sqliteConnection.CreateCollation("Webrox_InvariantCultureIgnoreCase", (x, y) => string.Compare(x, y, StringComparison.InvariantCultureIgnoreCase));
            sqliteConnection.CreateCollation("Webrox_Ordinal", (x, y) => string.Compare(x, y, StringComparison.Ordinal));
            sqliteConnection.CreateCollation("Webrox_OrdinalIgnoreCase", (x, y) => string.Compare(x, y, StringComparison.OrdinalIgnoreCase));


            return optionsBuilder;
        }
    }
}