using Microsoft.EntityFrameworkCore.Query;
using MySql.EntityFrameworkCore.Design.Tests;
using MySql.EntityFrameworkCore.Infrastructure.Internal;

namespace Webrox.EntityFrameworkCore.MySql.Query
{
    /// <inheritdoc />
    public class WebroxMySqlParameterBasedSqlProcessorFactory : IRelationalParameterBasedSqlProcessorFactory
    {
        private readonly IMySQLOptions _mySQLOptions;
        private readonly RelationalParameterBasedSqlProcessorDependencies _dependencies;

        /// <summary>
        /// Initializes new instance of <see cref="WebroxMySqlParameterBasedSqlProcessorFactory"/>.
        /// </summary>
        /// <param name="dependencies">Dependencies.</param>
        public WebroxMySqlParameterBasedSqlProcessorFactory(
           RelationalParameterBasedSqlProcessorDependencies dependencies,
           IMySQLOptions mySQLOptions)
        {
            _dependencies = dependencies;
            _mySQLOptions = mySQLOptions;
        }

        /// <inheritdoc />
        public RelationalParameterBasedSqlProcessor Create(bool useRelationalNulls)
        {
            return new WebroxMySqlParameterBasedSqlProcessorProxy().Create(_dependencies, useRelationalNulls, _mySQLOptions);
        }
    }
}