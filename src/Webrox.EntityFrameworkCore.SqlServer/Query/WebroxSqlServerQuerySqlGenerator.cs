using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using Webrox.EntityFrameworkCore.Core;
using Webrox.EntityFrameworkCore.Core.SqlExpressions;

namespace Webrox.EntityFrameworkCore.SqliSqlServerte.Query
{
    /// <inheritdoc />
    [SuppressMessage("Usage", "EF1001", MessageId = "Internal EF Core API usage.")]
    public class WebroxSqlServerQuerySqlGenerator : SqlServerQuerySqlGenerator
    {
        //private readonly ITenantDatabaseProvider _databaseProvider;
        private readonly WebroxQuerySqlGenerator _webroxQuerySqlGenerator;
        /// <inheritdoc />
        public WebroxSqlServerQuerySqlGenerator(
           QuerySqlGeneratorDependencies dependencies,
           IRelationalTypeMappingSource typeMappingSource,
           ISqlServerSingletonOptions sqlServerSingletonOptions,
           WebroxQuerySqlGenerator webroxQuerySqlGenerator
            //ITenantDatabaseProvider databaseProvider
            )
           : base(dependencies, typeMappingSource, sqlServerSingletonOptions)
        {
            _webroxQuerySqlGenerator = webroxQuerySqlGenerator;
           // _databaseProvider = databaseProvider ?? throw new ArgumentNullException(nameof(databaseProvider));
        }

        /// <inheritdoc />
        protected override Expression VisitExtension(Expression extensionExpression)
        {
            switch (extensionExpression)
            {
                case WindowExpression windowExpression:
                    return _webroxQuerySqlGenerator.VisitWindowFunction(Sql, windowExpression, Visit, VisitOrdering);
                default:
                    return base.VisitExtension(extensionExpression);
            }
        }
    }
}
