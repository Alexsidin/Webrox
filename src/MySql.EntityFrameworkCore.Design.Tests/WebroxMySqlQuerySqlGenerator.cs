using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using Webrox.EntityFrameworkCore.Core;
using Webrox.EntityFrameworkCore.Core.SqlExpressions;

namespace MySql.EntityFrameworkCore.Design.Tests
{
    public class WebroxMySqlQuerySqlGeneratorProxy
    {
        public QuerySqlGenerator Create(QuerySqlGeneratorDependencies dependencies,
            WebroxQuerySqlGenerator webroxQuerySqlGenerator)
        {
            return new WebroxMySqlQuerySqlGenerator(dependencies,
             webroxQuerySqlGenerator);
        }
    }

    /// <summary>
    /// Pretend to be a Test lib (check InternalsVisibleTo on MySql.EFCore)
    /// </summary>
    internal class WebroxMySqlQuerySqlGenerator : Query.MySQLQuerySqlGenerator
    {
        private readonly WebroxQuerySqlGenerator _webroxQuerySqlGenerator;
        public WebroxMySqlQuerySqlGenerator(
            QuerySqlGeneratorDependencies dependencies,
            WebroxQuerySqlGenerator webroxQuerySqlGenerator) 
            
            : base(dependencies)
        {
            _webroxQuerySqlGenerator = webroxQuerySqlGenerator;
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