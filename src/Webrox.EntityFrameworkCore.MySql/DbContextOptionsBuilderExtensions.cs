using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Query;
using MySql.EntityFrameworkCore.Design.Tests;
using Webrox.EntityFrameworkCore.Core;
using Webrox.EntityFrameworkCore.MySql.Query;
using MySqlLib = MySql.EntityFrameworkCore.Infrastructure;

namespace Webrox.EntityFrameworkCore.MySql
{
    /// <summary>
    /// DbContextOptionsBuilder Extensions
    /// </summary>
    public static class DbContextOptionsBuilderExtensions
    {
        /// <summary>
        /// Add RowNumber support
        /// </summary>
        /// <param name="optionsBuilder">options Builder</param>
        /// <returns><see cref="MySQLDbContextOptionsBuilder"/></returns>
        public static MySqlLib.MySQLDbContextOptionsBuilder AddRowNumberSupport(
                   this MySqlLib.MySQLDbContextOptionsBuilder optionsBuilder)
        {
            var infrastructure = (IRelationalDbContextOptionsBuilderInfrastructure)optionsBuilder;

            WebroxDbContextOptionsBuilderExtensions.AddRowNumberSupport(infrastructure);

            // Add custom functions Windowing
            infrastructure.OptionsBuilder.ReplaceService<IRelationalParameterBasedSqlProcessorFactory, WebroxMySqlParameterBasedSqlProcessorFactory>();
            infrastructure.OptionsBuilder.ReplaceService<IQuerySqlGeneratorFactory, WebroxMySqlQuerySqlGeneratorFactory>();

            //rewrite Linq/Select
            infrastructure.OptionsBuilder.ReplaceService<IRelationalSqlTranslatingExpressionVisitorFactory,
                WebroxMySqlTranslatingExpressionVisitorFactory
                >();
            //infrastructure.OptionsBuilder.ReplaceService<IQueryTranslationPreprocessorFactory, WebroxPostgresQueryTranslationPreprocessorFactory>();


            return optionsBuilder;
        }
    }
}
