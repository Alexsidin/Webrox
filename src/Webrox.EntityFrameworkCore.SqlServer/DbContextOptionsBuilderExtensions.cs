using Microsoft.EntityFrameworkCore.Infrastructure;

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

            return optionsBuilder;
        }
    }
}