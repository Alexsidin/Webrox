using Microsoft.EntityFrameworkCore.Query;
namespace Webrox.EntityFrameworkCore.MySql.Query
{
    /// <summary>
    /// Factory for creation of the <see cref="WebroxMySqlQueryableMethodTranslatingExpressionVisitor"/>.
    /// </summary>
    public class WebroxMySqlQueryableMethodTranslatingExpressionVisitorFactory : IQueryableMethodTranslatingExpressionVisitorFactory
    {
        private readonly RelationalSqlTranslatingExpressionVisitorDependencies _dependencies;
        private readonly QueryableMethodTranslatingExpressionVisitor _queryableMethodTranslatingExpressionVisitor;
        /// <summary>
        /// Initializes new instance of <see cref="WebroxMySqlQueryableMethodTranslatingExpressionVisitorFactory"/>.
        /// </summary>
        /// <param name="dependencies">Dependencies.</param>
        /// <param name="relationalDependencies">Relational dependencies.</param>
        public WebroxMySqlQueryableMethodTranslatingExpressionVisitorFactory(
            RelationalSqlTranslatingExpressionVisitorDependencies dependencies,
            QueryableMethodTranslatingExpressionVisitor queryableMethodTranslatingExpressionVisitor
            )
        {
            _dependencies = dependencies ?? throw new ArgumentNullException(nameof(dependencies));
            _queryableMethodTranslatingExpressionVisitor= queryableMethodTranslatingExpressionVisitor ?? throw new ArgumentNullException(nameof(queryableMethodTranslatingExpressionVisitor));
        }

        /// <inheritdoc />
  //      public QueryableMethodTranslatingExpressionVisitor Create(QueryCompilationContext model)
		//{
  //          return new WebroxMySqlQueryableMethodTranslatingExpressionVisitor(this._dependencies, model, _queryableMethodTranslatingExpressionVisitor);

  //      }

        public QueryableMethodTranslatingExpressionVisitor Create(QueryCompilationContext queryCompilationContext)
        {
            throw new NotImplementedException();
        }

        //public QueryableMethodTranslatingExpressionVisitor Create(QueryCompilationContext queryCompilationContext)
        //    {
        //        return new WebroxMySqlQueryableMethodTranslatingExpressionVisitor(_dependencies);
        //    }
    }
}