using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;
using MySql.EntityFrameworkCore.Design.Tests;
using System.Diagnostics.CodeAnalysis;
using MySqlLib = MySql.EntityFrameworkCore;

namespace Webrox.EntityFrameworkCore.MySql.Query
{
    /// <summary>
    /// Factory for creation of the <see cref="WebroxSqliteRelationalSqlTranslatingExpressionVisitor"/>.
    /// </summary>
    public sealed class WebroxMySqlTranslatingExpressionVisitorFactory :
      IRelationalSqlTranslatingExpressionVisitorFactory
    {
        private readonly RelationalSqlTranslatingExpressionVisitorDependencies _dependencies;
        private readonly ISqlExpressionFactory _sqlExpressionFactory;

        public WebroxMySqlTranslatingExpressionVisitorFactory(
            [NotNull] RelationalSqlTranslatingExpressionVisitorDependencies dependencies, ISqlExpressionFactory sqlExpressionFactory)
        {
            _dependencies = dependencies;
            _sqlExpressionFactory = sqlExpressionFactory;
        }

        public RelationalSqlTranslatingExpressionVisitor Create(
            QueryCompilationContext queryCompilationContext, 
            QueryableMethodTranslatingExpressionVisitor queryableMethodTranslatingExpressionVisitor)
              => new WebroxMySqlSqlTranslatingExpressionVisitorProxy().Create(
                _dependencies,
                queryCompilationContext,
                queryableMethodTranslatingExpressionVisitor,
                _sqlExpressionFactory);
    }
}