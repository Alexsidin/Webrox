using System.Globalization;
using System.Text;

namespace Webrox.EntityFrameworkCore.Core
{
    class DbContextOptionsExtensionInfo : Microsoft.EntityFrameworkCore.Infrastructure.DbContextOptionsExtensionInfo
    {
        private readonly DbContextOptionsExtension _extension;
        public override bool IsDatabaseProvider => false;

        private string? _logFragment;

        public override string LogFragment => _logFragment ??= CreateLogFragment();

        private string CreateLogFragment()
        {
            var sb = new StringBuilder();

            if (_extension.AddRowNumberSupport)
                sb.Append("RowNumberSupport ");

            return sb.ToString();
        }

        /// <inheritdoc />
        public DbContextOptionsExtensionInfo(DbContextOptionsExtension extension)
           : base(extension)
        {
            _extension = extension ?? throw new ArgumentNullException(nameof(extension));
        }

        /// <inheritdoc />
        public override int GetServiceProviderHashCode()
        {
            var hashCode = new HashCode();
            hashCode.Add(base.GetHashCode());
            hashCode.Add(_extension.AddRowNumberSupport);

            return hashCode.ToHashCode();
        }

        /// <inheritdoc />
        public override bool ShouldUseSameServiceProvider(Microsoft.EntityFrameworkCore.Infrastructure.DbContextOptionsExtensionInfo other)
        {
            return other is DbContextOptionsExtensionInfo otherSqlServerInfo
                   && _extension.AddRowNumberSupport == otherSqlServerInfo._extension.AddRowNumberSupport
                   ;
        }

        /// <inheritdoc />
        public override void PopulateDebugInfo(IDictionary<string, string> debugInfo)
        {
            debugInfo["Webrox:RowNumberSupport"] = _extension.AddRowNumberSupport.ToString(CultureInfo.InvariantCulture);
        }
    }
}
