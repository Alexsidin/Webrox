using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Text;

namespace Webrox.EntityFrameworkCore.Sqlite.Query
{
    /// <inheritdoc />
    public class WebroxSqliteParameterBasedSqlProcessorFactory : IRelationalParameterBasedSqlProcessorFactory
    {
        private readonly RelationalParameterBasedSqlProcessorDependencies _dependencies;

        /// <summary>
        /// Initializes new instance of <see cref="WebroxSqliteParameterBasedSqlProcessorFactory"/>.
        /// </summary>
        /// <param name="dependencies">Dependencies.</param>
        public WebroxSqliteParameterBasedSqlProcessorFactory(
           RelationalParameterBasedSqlProcessorDependencies dependencies)
        {
            _dependencies = dependencies;
        }

        /// <inheritdoc />
        public RelationalParameterBasedSqlProcessor Create(bool useRelationalNulls)
        {
            return new WebroxSqliteParameterBasedSqlProcessor(_dependencies, useRelationalNulls);
        }
    }
}