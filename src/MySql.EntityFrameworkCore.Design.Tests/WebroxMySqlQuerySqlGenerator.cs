using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;
using System.Reflection;
using Webrox.EntityFrameworkCore.Core.Infrastructure;
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
#if NET6_0 // hack for Convert.ToXXX bug in MySQL.EF6

        private static readonly Dictionary<string, string[]> _castMappings = new Dictionary<string, string[]>
    {
        {
            "signed",
            new string[6] { "tinyint", "smallint", "mediumint", "int", "bigint", "bit" }
        },
        {
            "decimal(65,30)",
            new string[1] { "decimal" }
        },
        {
            "double",
            new string[1] { "double" }
        },
        {
            "float",
            new string[1] { "float" }
        },
        {
            "binary",
            new string[6] { "binary", "varbinary", "tinyblob", "blob", "mediumblob", "longblob" }
        },
        {
            "datetime(6)",
            new string[1] { "datetime(6)" }
        },
        {
            "datetime",
            new string[1] { "datetime" }
        },
        {
            "date",
            new string[1] { "date" }
        },
        {
            "timestamp(6)",
            new string[1] { "timestamp(6)" }
        },
        {
            "timestamp",
            new string[1] { "timestamp" }
        },
        {
            "time(6)",
            new string[1] { "time(6)" }
        },
        {
            "time",
            new string[1] { "time" }
        },
        {
            "json",
            new string[1] { "json" }
        },
        {
            "char",
            new string[6] { "char", "varchar", "text", "tinytext", "mediumtext", "longtext" }
        },
        {
            "nchar",
            new string[2] { "nchar", "nvarchar" }
        }
    };

        protected override Expression VisitSqlUnary(SqlUnaryExpression sqlUnaryExpression)
        {
            if (sqlUnaryExpression.OperatorType != ExpressionType.Convert)
            {
                return base.VisitSqlUnary(sqlUnaryExpression);
            }
            return VisitConvert(sqlUnaryExpression);
        }

        private SqlUnaryExpression VisitConvert(SqlUnaryExpression sqlUnaryExpression)
        {
            string castStoreType = GetCastStoreType(sqlUnaryExpression.TypeMapping);
            if (castStoreType == "binary")
            {
                Sql.Append("UNHEX(HEX(");
                Visit(sqlUnaryExpression.Operand);
                Sql.Append("))");
                return sqlUnaryExpression;
            }
            if (sqlUnaryExpression.Operand is SqlUnaryExpression sqlUnaryExpression2 && sqlUnaryExpression2.OperatorType == ExpressionType.Convert)
            {
                castStoreType.Equals(GetCastStoreType(sqlUnaryExpression2.TypeMapping), StringComparison.OrdinalIgnoreCase);
            }
            else
                _ = 0;
            Visit(sqlUnaryExpression.Operand);
            return sqlUnaryExpression;
        }
        private string GetCastStoreType(RelationalTypeMapping typeMapping)
        {
            string text = typeMapping.StoreType.ToLower();
            string text2 = null;

            foreach (KeyValuePair<string, string[]> castMapping in _castMappings)
            {
                string[] value = castMapping.Value;
                foreach (string value2 in value)
                {
                    if (text.StartsWith(value2))
                    {
                        text2 = castMapping.Key;
                        break;
                    }
                }
                if (text2 != null)
                {
                    break;
                }
            }
            if (text2 == null)
            {
                throw new InvalidOperationException("Invalid cast '" + typeMapping.StoreType + "'");
            }
            if (text2 == "signed" && text.Contains("unsigned"))
            {
                text2 = "unsigned";
            }
            return text2;
        }
#endif
    }
}