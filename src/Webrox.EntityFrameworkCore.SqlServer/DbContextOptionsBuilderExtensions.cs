using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Query;
using Webrox.EntityFrameworkCore.SqlServer.Query;

namespace Webrox.EntityFrameworkCore.SqlServer
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
        /// <returns><see cref="SqlServerDbContextOptionsBuilder"/></returns>
        public static SqlServerDbContextOptionsBuilder AddRowNumberSupport(
                   this SqlServerDbContextOptionsBuilder optionsBuilder)
        {
            var infrastructure = (IRelationalDbContextOptionsBuilderInfrastructure)optionsBuilder;


/* Unmerged change from project 'Webrox.EntityFrameworkCore.SqlServer (net8.0)'
Before:
            Core.WebroxDbContextOptionsBuilderExtensions.AddRowNumberSupport(infrastructure);
After:
            WebroxDbContextOptionsBuilderExtensions.AddRowNumberSupport(infrastructure);
*/
            Core.Infrastructure.WebroxDbContextOptionsBuilderExtensions.AddRowNumberSupport(infrastructure);

            // Add custom functions Windowing
            infrastructure.OptionsBuilder.ReplaceService<IRelationalParameterBasedSqlProcessorFactory, WebroxSqlServerParameterBasedSqlProcessorFactory>();
            infrastructure.OptionsBuilder.ReplaceService<IQuerySqlGeneratorFactory, WebroxSqlServerQuerySqlGeneratorFactory>();

            //rewrite Linq/Select
            infrastructure.OptionsBuilder.ReplaceService<IQueryTranslationPreprocessorFactory, WebroxSqlServerQueryTranslationPreprocessorFactory>();

            return optionsBuilder;
        }
    }
}