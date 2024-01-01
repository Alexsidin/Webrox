using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Query;

namespace Webrox.EntityFrameworkCore.Core
{
    /// <summary>
    /// Webrox DbContextOptionsBuilderExtensions
    /// </summary>
    public class WebroxDbContextOptionsBuilderExtensions
    {
        /// <summary>
        /// Add RowNumber extension
        /// </summary>
        /// <param name="infrastructure">infrastructure</param>
        public static void AddRowNumberSupport(
                   IRelationalDbContextOptionsBuilderInfrastructure infrastructure)
        {
            if (infrastructure == null) throw new ArgumentNullException(nameof(infrastructure));

            var optionsBuilder = (IDbContextOptionsBuilderInfrastructure)infrastructure.OptionsBuilder;

            var extension = infrastructure.OptionsBuilder.Options
                                          .FindExtension<WebroxDbContextOptionsExtension>()
                                          ?? new WebroxDbContextOptionsExtension();
            
            extension.AddRowNumberSupport = true;

            optionsBuilder.AddOrUpdateExtension(extension);
        }
    }
}