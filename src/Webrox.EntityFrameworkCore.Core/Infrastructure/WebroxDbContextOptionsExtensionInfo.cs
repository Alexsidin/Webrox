using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Globalization;
using System.Text;

namespace Webrox.EntityFrameworkCore.Core.Infrastructure
{
    class WebroxDbContextOptionsExtensionInfo : DbContextOptionsExtensionInfo
    {
        private readonly WebroxDbContextOptionsExtension _extension;
        public override bool IsDatabaseProvider => false;

        private string? _logFragment;

        public override string LogFragment => _logFragment ??= CreateLogFragment();

        private string CreateLogFragment()
        {
            var sb = new StringBuilder();

            return sb.ToString();
        }

        /// <inheritdoc />
        public WebroxDbContextOptionsExtensionInfo(WebroxDbContextOptionsExtension extension)
           : base(extension)
        {
            _extension = extension ?? throw new ArgumentNullException(nameof(extension));
        }


#if NETSTANDARD || NET5_0
        /// <inheritdoc />
        public override long GetServiceProviderHashCode()
#else
        /// <inheritdoc />
        public override int GetServiceProviderHashCode()
#endif
        {
            var hashCode = new HashCode();
            hashCode.Add(base.GetHashCode());

            return hashCode.ToHashCode();
        }

#if NET6_0_OR_GREATER
        /// <inheritdoc />
        public override bool ShouldUseSameServiceProvider(DbContextOptionsExtensionInfo other)
        {
            return other is WebroxDbContextOptionsExtensionInfo otherSqlServerInfo
                   && _extension.AddWebroxFeatures == otherSqlServerInfo._extension.AddWebroxFeatures
                   ;
        }
#endif
        /// <inheritdoc />
        public override void PopulateDebugInfo(IDictionary<string, string> debugInfo)
        {
        }
    }
}
