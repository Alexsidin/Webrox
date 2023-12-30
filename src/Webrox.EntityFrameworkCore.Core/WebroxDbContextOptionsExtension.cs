using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.DependencyInjection;

namespace Webrox.EntityFrameworkCore.Core
{
    /// <summary>
    /// Webrox DbContextOptionsExtension
    /// </summary>
    public class WebroxDbContextOptionsExtension : IDbContextOptionsExtension
    {
        WebroxDbContextOptionsExtensionInfo _info;
        /// <summary>
        /// Row Number Support
        /// </summary>
        public bool AddRowNumberSupport { get; set; }
        
        /// <inheritdoc/>
        public DbContextOptionsExtensionInfo Info => _info;

        /// <summary>
        /// Constructor
        /// </summary>
        public WebroxDbContextOptionsExtension()
        {
            _info = new(this);
        }

        /// <inheritdoc/>
        public void ApplyServices(IServiceCollection services)
        {
            if (AddRowNumberSupport)
            {
                services.AddScoped<IMethodCallTranslatorPlugin, WebroxMethodCallTranslatorPlugin>();
            }
        }

        /// <inheritdoc/>
        public void Validate(IDbContextOptions options)
        {
        }
    }
}