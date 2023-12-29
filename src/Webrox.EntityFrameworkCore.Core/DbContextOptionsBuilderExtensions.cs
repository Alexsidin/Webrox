using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Webrox.EntityFrameworkCore.Core
{
    public class DbContextOptionsBuilderExtensions
    {
        /// <summary>
        /// Add RowNumber extension
        /// </summary>
        /// <param name="infrastructure">infrastructure</param>
        public static void AddRowNumberSupport(
                   IRelationalDbContextOptionsBuilderInfrastructure infrastructure)
        {
            ArgumentException.ThrowIfNullOrEmpty(nameof(infrastructure));

            var optionsBuilder = (IDbContextOptionsBuilderInfrastructure)infrastructure.OptionsBuilder;

            var extension = infrastructure.OptionsBuilder.Options
                                          .FindExtension<DbContextOptionsExtension>()
                                          ?? new DbContextOptionsExtension();
            
            extension.AddRowNumberSupport = true;

            optionsBuilder.AddOrUpdateExtension(extension);
        }
    }
}
