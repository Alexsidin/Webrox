using Microsoft.EntityFrameworkCore.Query;

namespace Webrox.EntityFrameworkCore.MySql.Query
{
    /// <inheritdoc />
    public class WebroxMySqlParameterBasedSqlProcessorFactory : IRelationalParameterBasedSqlProcessorFactory
    {
        private readonly RelationalParameterBasedSqlProcessorDependencies _dependencies;

        /// <summary>
        /// Initializes new instance of <see cref="WebroxMySqlParameterBasedSqlProcessorFactory"/>.
        /// </summary>
        /// <param name="dependencies">Dependencies.</param>
        public WebroxMySqlParameterBasedSqlProcessorFactory(
           RelationalParameterBasedSqlProcessorDependencies dependencies)
        {
            _dependencies = dependencies;
        }

        /// <inheritdoc />
        public RelationalParameterBasedSqlProcessor Create(bool useRelationalNulls)
        {
            return new WebroxMySqlParameterBasedSqlProcessor(_dependencies, useRelationalNulls);
        }
    }
}