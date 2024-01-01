using Microsoft.EntityFrameworkCore.Query;

namespace Webrox.EntityFrameworkCore.Postgres.Query
{
    /// <inheritdoc />
    public class WebroxPostgreSqlParameterBasedSqlProcessorFactory : IRelationalParameterBasedSqlProcessorFactory
    {
        private readonly RelationalParameterBasedSqlProcessorDependencies _dependencies;

        /// <summary>
        /// Initializes new instance of <see cref="WebroxPostgreSqlParameterBasedSqlProcessorFactory"/>.
        /// </summary>
        /// <param name="dependencies">Dependencies.</param>
        public WebroxPostgreSqlParameterBasedSqlProcessorFactory(
           RelationalParameterBasedSqlProcessorDependencies dependencies)
        {
            _dependencies = dependencies;
        }

        /// <inheritdoc />
        public RelationalParameterBasedSqlProcessor Create(bool useRelationalNulls)
        {
            return new WebroxPostgreSqlParameterBasedSqlProcessor(_dependencies, useRelationalNulls);
        }
    }
}