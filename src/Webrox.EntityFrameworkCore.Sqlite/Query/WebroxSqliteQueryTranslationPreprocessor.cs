using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Webrox.EntityFrameworkCore.Sqlite.Query
{
    public class WebroxSqliteQueryTranslationPreprocessor : QueryTranslationPreprocessor
    {
        private readonly ISqlExpressionFactory _sqlExpressionFactory;
        /// <summary>
        ///     Creates a new instance of the <see cref="QueryTranslationPreprocessor" /> class.
        /// </summary>
        /// <param name="dependencies">Parameter object containing dependencies for this class.</param>
        /// <param name="queryCompilationContext">The query compilation context object to use.</param>
        public WebroxSqliteQueryTranslationPreprocessor(
            QueryTranslationPreprocessorDependencies dependencies,
            QueryCompilationContext queryCompilationContext,
            ISqlExpressionFactory sqlExpressionFactory)
            : base(dependencies, queryCompilationContext)
        {
            _sqlExpressionFactory = sqlExpressionFactory;
        }

        /// <summary>
        ///     Applies preprocessing transformations to the query.
        /// </summary>
        /// <param name="query">The query to process.</param>
        /// <returns>A query expression after transformations.</returns>
        public override Expression Process(Expression query)
        {
            query = new InvocationExpressionRemovingExpressionVisitor().Visit(query);
            query = NormalizeQueryableMethod(query);
            query = new CallForwardingExpressionVisitor().Visit(query);
            query = new NullCheckRemovingExpressionVisitor().Visit(query);
            query = new SubqueryMemberPushdownExpressionVisitor(QueryCompilationContext.Model).Visit(query);
            query = new WebroxNavigationExpandingExpressionVisitor(
                    this,
                    QueryCompilationContext,
                    Dependencies.EvaluatableExpressionFilter,
                    Dependencies.NavigationExpansionExtensibilityHelper,
                    _sqlExpressionFactory)
                .Expand(query);
            query = new QueryOptimizingExpressionVisitor().Visit(query);
            query = new NullCheckRemovingExpressionVisitor().Visit(query);

            return query;
        }

        ///// <summary>
        /////     Normalizes queryable methods in the query.
        ///// </summary>
        ///// <remarks>
        /////     This method extracts query metadata information like tracking, ignore query filters.
        /////     It also converts potential enumerable methods on navigation to queryable methods.
        /////     It flattens patterns of GroupJoin-SelectMany patterns to appropriate Join/LeftJoin.
        ///// </remarks>
        ///// <param name="expression">The query expression to normalize.</param>
        ///// <returns>A query expression after normalization has been done.</returns>
        //public virtual Expression NormalizeQueryableMethod(Expression expression)
        //{
        //    expression = new QueryableMethodNormalizingExpressionVisitor(QueryCompilationContext).Normalize(expression);
        //    expression = ProcessQueryRoots(expression);

        //    return expression;
        //}

        ///// <summary>
        /////     Adds additional query root nodes to the query.
        ///// </summary>
        ///// <param name="expression">The query expression to process.</param>
        ///// <returns>A query expression after query roots have been added.</returns>
        //protected virtual Expression ProcessQueryRoots(Expression expression)
        //    => new QueryRootProcessor(Dependencies, QueryCompilationContext).Visit(expression);
    }
}
