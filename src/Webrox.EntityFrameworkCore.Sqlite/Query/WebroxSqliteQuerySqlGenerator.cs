using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Sqlite.Query.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using Webrox.EntityFrameworkCore.Core.SqlExpressions;

namespace Webrox.EntityFrameworkCore.Sqlite.Query
{
    /// <inheritdoc />
    [SuppressMessage("Usage", "EF1001", MessageId = "Internal EF Core API usage.")]
    public class WebroxSqliteQuerySqlGenerator : SqliteQuerySqlGenerator
    {
        //private readonly ITenantDatabaseProvider _databaseProvider;

        /// <inheritdoc />
        public WebroxSqliteQuerySqlGenerator(
           QuerySqlGeneratorDependencies dependencies,
           IRelationalTypeMappingSource typeMappingSource
           //ISqliteSingletonOptions sqlServerSingletonOptions,
           //ITenantDatabaseProvider databaseProvider
            )
           : base(dependencies)//, typeMappingSource, sqlServerSingletonOptions)
        {
           // _databaseProvider = databaseProvider ?? throw new ArgumentNullException(nameof(databaseProvider));
        }

        /// <inheritdoc />
        protected override Expression VisitExtension(Expression extensionExpression)
        {
            switch (extensionExpression)
            {
                case WindowExpression windowExpression:
                    return VisitWindowFunction(windowExpression);
                default:
                    return base.VisitExtension(extensionExpression);
            }
        }

        private Expression VisitWindowFunction(WindowExpression windowExpression)
        {
            Sql.Append(windowExpression.AggregateFunction).Append("(");
                
            if(windowExpression.ColumnExpression != null)
            {
                Visit(windowExpression.ColumnExpression);
            }

            Sql.Append(")").Append(" ").Append("OVER (");

            if (windowExpression.Partitions.Count != 0)
            {
                Sql.Append("PARTITION BY ");

                for (var i = 0; i < windowExpression.Partitions.Count; i++)
                {
                    if (i != 0)
                        Sql.Append(", ");

                    var partition = windowExpression.Partitions[i];
                    Visit(partition);
                }
            }

            if (windowExpression.Orderings.Count != 0)
            {
                Sql.Append(" ORDER BY ");

                for (var i = 0; i < windowExpression.Orderings.Count; i++)
                {
                    if (i != 0)
                        Sql.Append(", ");

                    var ordering = windowExpression.Orderings[i];
                    VisitOrdering(ordering);
                }
            }

            Sql.Append(")");

            return windowExpression;
        }
    }
}
