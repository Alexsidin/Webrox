using Microsoft.EntityFrameworkCore.Query;

namespace Webrox.EntityFrameworkCore.SqlServer.Query
{
    /// <inheritdoc />
    public class WebroxSqlServerParameterBasedSqlProcessorFactory : IRelationalParameterBasedSqlProcessorFactory
    {
        private readonly RelationalParameterBasedSqlProcessorDependencies _dependencies;

        /// <summary>
        /// Initializes new instance of <see cref="WebroxSqlServerParameterBasedSqlProcessorFactory"/>.
        /// </summary>
        /// <param name="dependencies">Dependencies.</param>
        public WebroxSqlServerParameterBasedSqlProcessorFactory(
           RelationalParameterBasedSqlProcessorDependencies dependencies)
        {
            _dependencies = dependencies;
        }

        /// <inheritdoc />
        public RelationalParameterBasedSqlProcessor Create(bool useRelationalNulls)
        {
            return new WebroxSqlServerParameterBasedSqlProcessor(_dependencies, useRelationalNulls);
        }
    }
}