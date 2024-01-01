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

            Core.WebroxDbContextOptionsBuilderExtensions.AddRowNumberSupport(infrastructure);

            infrastructure.OptionsBuilder.ReplaceService<IRelationalParameterBasedSqlProcessorFactory, WebroxSqlServerParameterBasedSqlProcessorFactory>();
            infrastructure.OptionsBuilder.ReplaceService<IQuerySqlGeneratorFactory, WebroxSqlServerQuerySqlGeneratorFactory>();

            return optionsBuilder;
        }
    }
}