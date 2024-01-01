using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Query;
using Webrox.EntityFrameworkCore.MySql.Query;
using MySqlLib = MySql.EntityFrameworkCore.Infrastructure;

namespace Webrox.EntityFrameworkCore.MySql
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
        /// <returns><see cref="MySQLDbContextOptionsBuilder"/></returns>
        public static MySqlLib.MySQLDbContextOptionsBuilder AddRowNumberSupport(
                   this MySqlLib.MySQLDbContextOptionsBuilder optionsBuilder)
        {
            var infrastructure = (IRelationalDbContextOptionsBuilderInfrastructure)optionsBuilder;

            Core.WebroxDbContextOptionsBuilderExtensions.AddRowNumberSupport(infrastructure);
            
            infrastructure.OptionsBuilder.ReplaceService<IRelationalParameterBasedSqlProcessorFactory, WebroxMySqlParameterBasedSqlProcessorFactory>();
            infrastructure.OptionsBuilder.ReplaceService<IQuerySqlGeneratorFactory, WebroxMySqlQuerySqlGeneratorFactory>();

            return optionsBuilder;
        }
    }
}
