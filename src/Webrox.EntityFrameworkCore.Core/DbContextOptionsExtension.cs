using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.DependencyInjection;

namespace Webrox.EntityFrameworkCore.Core
{
    public class DbContextOptionsExtension : IDbContextOptionsExtension
    {
        public bool AddRowNumberSupport { get; set; }
        DbContextOptionsExtensionInfo _info;
        public Microsoft.EntityFrameworkCore.Infrastructure.DbContextOptionsExtensionInfo Info => _info;

        public DbContextOptionsExtension()
        {
            _info = new(this);
        }

        public void ApplyServices(IServiceCollection services)
        {
            if (AddRowNumberSupport)
            {
                services.AddScoped<IMethodCallTranslatorPlugin, MethodCallTranslatorPlugin>();
            }
        }

        public void Validate(IDbContextOptions options)
        {
        }
    }
}