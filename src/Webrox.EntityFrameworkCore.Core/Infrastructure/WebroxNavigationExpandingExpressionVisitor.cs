using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Webrox.EntityFrameworkCore.Core.Infrastructure
{
    public class WebroxNavigationExpandingExpressionVisitor : NavigationExpandingExpressionVisitor
    {
        private readonly ISqlExpressionFactory _sqlExpressionFactory;
        private readonly QueryCompilationContext _queryCompilationContext;

        static Dictionary<string, List<MethodInfo>> queryableMethodGroups = typeof(Queryable)
            .GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
            .GroupBy(mi => mi.Name)
            .ToDictionary(e => e.Key, l => l.ToList());

        static MethodInfo GetMethod(string name, int genericParameterCount, Func<Type[], Type[]> parameterGenerator)
            => queryableMethodGroups[name].Single(
                mi => (genericParameterCount == 0 && !mi.IsGenericMethod
                        || mi.IsGenericMethod && mi.GetGenericArguments().Length == genericParameterCount)
                    && mi.GetParameters().Select(e => e.ParameterType).SequenceEqual(
                        parameterGenerator(mi.IsGenericMethod ? mi.GetGenericArguments() : Array.Empty<Type>())));

        static MethodInfo _methodSelectWithIndex;
        static MethodInfo _methodSelect;
        static MethodInfo _methodStringEqualsComparison;
        static MethodInfo _methodStringEquals;
        static MethodInfo _methodEFCollate;

        static WebroxNavigationExpandingExpressionVisitor()
        {
            _methodSelect = GetMethod(
             nameof(Queryable.Select), 2,
             types => new[]
             {
                typeof(IQueryable<>).MakeGenericType(types[0]),
                 typeof(Expression<>).MakeGenericType(typeof(Func<,>).MakeGenericType(types[0], types[1]))
             });

            _methodSelectWithIndex = GetMethod(
             nameof(Queryable.Select), 2,
             types => new[]
             {
                typeof(IQueryable<>).MakeGenericType(types[0]),
                typeof(Expression<>).MakeGenericType(typeof(Func<,,>).MakeGenericType(types[0], typeof(int), types[1]))
             });

            _methodStringEqualsComparison = typeof(string).GetMethod(nameof(string.Equals), BindingFlags.Instance | BindingFlags.Public,
                new Type[] { typeof(string), typeof(StringComparison) });
            _methodStringEquals = typeof(string).GetMethod(nameof(string.Equals), BindingFlags.Instance | BindingFlags.Public,
                new Type[] { typeof(string) });

            var allmethodsEFColate = typeof(RelationalDbFunctionsExtensions).GetMethods().Where(a => a.Name == nameof(RelationalDbFunctionsExtensions.Collate));
            _methodEFCollate = allmethodsEFColate.Where(a => a.IsGenericMethod).FirstOrDefault();

        }

        public WebroxNavigationExpandingExpressionVisitor(QueryTranslationPreprocessor queryTranslationPreprocessor,
            QueryCompilationContext queryCompilationContext,
            IEvaluatableExpressionFilter evaluatableExpressionFilter,
#if NET6_0_OR_GREATER
            INavigationExpansionExtensibilityHelper extensibilityHelper,
#endif
            ISqlExpressionFactory sqlExpressionFactory)
            : base(queryTranslationPreprocessor, queryCompilationContext, evaluatableExpressionFilter
#if NET6_0_OR_GREATER
                  , extensibilityHelper
#endif
                  )
        {
            
            _queryCompilationContext = queryCompilationContext;
            _sqlExpressionFactory = sqlExpressionFactory;
        }

        protected override Expression VisitMethodCall(MethodCallExpression methodCallExpression)
        {
            var method = methodCallExpression.Method;

            if (method.DeclaringType == typeof(Queryable)
                && method.Name == nameof(Queryable.Select)
                && method.IsGenericMethod)
            {
                var typesGenerics = method.GetGenericArguments();

                if (method == _methodSelectWithIndex?.MakeGenericMethod(typesGenerics))
                {
                    var firstArgument = Visit(methodCallExpression.Arguments[0]);
                    var lambda = methodCallExpression.Arguments[1].UnwrapLambdaFromQuote();

                    return ProcessSelect(firstArgument, lambda);
                }
            }

            if (method == _methodStringEqualsComparison)
            {
                return ProcessStringEquals(methodCallExpression);
            }

            return base.VisitMethodCall(methodCallExpression);
        }

        private Expression ProcessSubQuery(Expression source)//, ConstantExpression stringComparisonExpression)
        {
            var piPending = source.GetType().GetProperty("PendingSelector");
            var pendingSelector = piPending.GetValue(source) as Expression;


            return null;
        }

        static Dictionary<string, string> WindowsCollations = new Dictionary<string, string>
        {
            {"Windows locale","Collation Version 100"},
            {"Alsatian (France)","Latin1_General_100_"},
            {"Amharic (Ethiopia)","Latin1_General_100_"},
            {"Armenian (Armenia)","Cyrillic_General_100_"},
            {"Assamese (India)","Assamese_100_ 1"},
            {"Bangla (Bangladesh)","Bengali_100_1"},
            {"Bashkir (Russia)","Bashkir_100_"},
            {"Basque (Basque)","Latin1_General_100_"},
            {"Bengali (India)","Bengali_100_1"},
            {"Bosnian (Bosnia and Herzegovina, Cyrillic)","Bosnian_Cyrillic_100_"},
            {"Bosnian (Bosnia and Herzegovina, Latin)","Bosnian_Latin_100_"},
            {"Breton (France)","Breton_100_"},
            {"Chinese (Macao SAR)","Chinese_Traditional_Pinyin_100_"},
            {"Chinese (Singapore)","Chinese_Simplified_Stroke_Order_100_"},
            {"Corsican (France)","Corsican_100_"},
            {"Croatian (Bosnia and Herzegovina, Latin)","Croatian_100_"},
            {"Dari (Afghanistan)","Dari_100_"},
            {"English (India)","Latin1_General_100_"},
            {"English (Malaysia)","Latin1_General_100_"},
            {"English (Singapore)","Latin1_General_100_"},
            {"Filipino (Philippines)","Latin1_General_100_"},
            {"Frisian (Netherlands)","Frisian_100_"},
            {"Georgian (Georgia)","Cyrillic_General_100_"},
            {"Greenlandic (Greenland)","Danish_Greenlandic_100_"},
            {"Gujarati (India)","Indic_General_100_1"},
            {"Hausa (Nigeria, Latin)","Latin1_General_100_"},
            {"Hindi (India)","Indic_General_100_1"},
            {"Igbo (Nigeria)","Latin1_General_100_"},
            {"Inuktitut (Canada, Latin)","Latin1_General_100_"},
            {"Inuktitut (Syllabics) Canada","Latin1_General_100_"},
            {"Irish (Ireland)","Latin1_General_100_"},
            {"Japanese (Japan XJIS)","Japanese_XJIS_100_"},
            {"Japanese (Japan)","Japanese_Bushu_Kakusu_100_"},
            {"Kannada (India)","Indic_General_100_1"},
            {"Khmer (Cambodia)","Khmer_100_1"},
            {"K'iche (Guatemala)","Modern_Spanish_100_"},
            {"Kinyarwanda (Rwanda)","Latin1_General_100_"},
            {"Konkani (India)","Indic_General_100_1"},
            {"Lao (Lao PDR)","Lao_100_1"},
            {"Lower Sorbian (Germany)","Latin1_General_100_"},
            {"Luxembourgish (Luxembourg)","Latin1_General_100_"},
            {"Malayalam (India)","Indic_General_100_1"},
            {"Maltese (Malta)","Maltese_100_"},
            {"Maori (New Zealand)","Maori_100_"},
            {"Mapudungun (Chile)","Mapudungan_100_"},
            {"Marathi (India)","Indic_General_100_1"},
            {"Mohawk (Canada)","Mohawk_100_"},
            {"Mongolian (PRC)","Cyrillic_General_100_"},
            {"Nepali (Nepal)","Nepali_100_1"},
            {"Norwegian (Bokmål, Norway)","Norwegian_100_"},
            {"Norwegian (Nynorsk, Norway)","Norwegian_100_"},
            {"Occitan (France)","French_100_"},
            {"Odia (India)","Indic_General_100_1"},
            {"Pashto (Afghanistan)","Pashto_100_1"},
            {"Persian (Iran)","Persian_100_"},
            {"Punjabi (India)","Indic_General_100_1"},
            {"Quechua (Bolivia)","Latin1_General_100_"},
            {"Quechua (Ecuador)","Latin1_General_100_"},
            {"Quechua (Peru)","Latin1_General_100_"},
            {"Romansh (Switzerland)","Romansh_100_"},
            {"Sami (Inari, Finland)","Sami_Sweden_Finland_100_"},
            {"Sami (Lule, Norway)","Sami_Norway_100_"},
            {"Sami (Lule, Sweden)","Sami_Sweden_Finland_100_"},
            {"Sami (Northern, Finland)","Sami_Sweden_Finland_100_"},
            {"Sami (Northern, Norway)","Sami_Norway_100_"},
            {"Sami (Northern, Sweden)","Sami_Sweden_Finland_100_"},
            {"Sami (Skolt, Finland)","Sami_Sweden_Finland_100_"},
            {"Sami (Southern, Norway)","Sami_Norway_100_"},
            {"Sami (Southern, Sweden)","Sami_Sweden_Finland_100_"},
            {"Sanskrit (India)","Indic_General_100_1"},
            {"Serbian (Bosnia and Herzegovina, Cyrillic)","Serbian_Cyrillic_100_"},
            {"Serbian (Bosnia and Herzegovina, Latin)","Serbian_Latin_100_"},
            {"Serbian (Serbia, Cyrillic)","Serbian_Cyrillic_100_"},
            {"Serbian (Serbia, Latin)","Serbian_Latin_100_"},
            {"Sesotho sa Leboa/Northern Sotho (South Africa)","Latin1_General_100_"},
            {"Setswana/Tswana (South Africa)","Latin1_General_100_"},
            {"Sinhala (Sri Lanka)","Indic_General_100_1"},
            {"Swahili (Kenya)","Latin1_General_100_"},
            {"Syriac (Syria)","Syriac_100_1"},
            {"Tajik (Tajikistan)","Cyrillic_General_100_"},
            {"Tamazight (Algeria, Latin)","Tamazight_100_"},
            {"Tamil (India)","Indic_General_100_1"},
            {"Telugu (India)","Indic_General_100_1"},
            {"Tibetan (PRC)","Tibetan_100_1"},
            {"Turkmen (Turkmenistan)","Turkmen_100_"},
            {"Uighur (PRC)","Uighur_100_"},
            {"Upper Sorbian (Germany)","Upper_Sorbian_100_"},
            {"Urdu (Pakistan)","Urdu_100_"},
            {"Welsh (United Kingdom)","Welsh_100_"},
            {"Wolof (Senegal)","French_100_"},
            {"Xhosa/isiXhosa (South Africa)","Latin1_General_100_"},
            {"Sakha (Russia)","Yakut_100_"},
            {"Yi (PRC)","Latin1_General_100_"},
            {"Yoruba (Nigeria)","Latin1_General_100_"},
            {"Zulu/isiZulu (South Africa)","Latin1_General_100_"},
        };

        private Expression ProcessStringEquals(MethodCallExpression methodCallExpression)
        {
            var expression = methodCallExpression.Arguments[0];
            var stringOrdinal = methodCallExpression.Arguments[1] as ConstantExpression;
            var stringComparison = (StringComparison)stringOrdinal.Value;

            var productVersion = _queryCompilationContext.Model.GetProductVersion();

            string collation = null;


            if (productVersion.Contains("sqlite", StringComparison.OrdinalIgnoreCase))
            {
                collation = $"Webrox_{stringComparison}";
            }
            else
            {

                collation = "DATABASE_DEFAULT";

                switch (stringComparison)
                {
                    case StringComparison.Ordinal:
                        collation = "SQL_Latin1_General_CP1_CS_AS";
                        break;
                    case StringComparison.OrdinalIgnoreCase:
                        collation = "SQL_Latin1_General_CP1_CI_AS";
                        break;
                    case StringComparison.InvariantCulture:
                        collation = "SQL_Latin1_General_CP1_CS_AS";
                        break;
                    case StringComparison.InvariantCultureIgnoreCase:
                        collation = "SQL_Latin1_General_CP1_CI_AS";
                        break;
                    case StringComparison.CurrentCulture:
                        collation = GetCurrentCultureCollation(true, true);
                        break;
                    case StringComparison.CurrentCultureIgnoreCase:
                        collation = GetCurrentCultureCollation(false, true);
                        break;

                }
            }

            var miEFCollate = _methodEFCollate.MakeGenericMethod(expression.Type);

            var exprLeft = Expression.Call(miEFCollate, Expression.Constant(EF.Functions), expression, Expression.Constant(collation));

            // "variable".Equals(EF.Functions.Collate("obj","Collation"))
            var newMethodCallExpression = Expression.Call(methodCallExpression.Object, _methodStringEquals, exprLeft);

            return base.VisitMethodCall(newMethodCallExpression);
        }

        string GetCurrentCultureCollation(bool isCaseSensitive, bool isAccentSensitive)
        {
            string collation = "SQL_Latin1_General_CP1";



            var windowCollationName = System.Globalization.CultureInfo.CurrentCulture.DisplayName;
            var coll = WindowsCollations.FirstOrDefault(a => a.Value == windowCollationName);

            if (WindowsCollations.TryGetValue(windowCollationName, out var tmpCollation))
                collation = tmpCollation;

            collation = $"{collation}_{(isCaseSensitive ? "CI" : "CS")}_{(isAccentSensitive ? "AI" : "AS")}";

            return collation;
        }

        private Expression ProcessSelect(Expression source, LambdaExpression selector)
        {
            var piPending = source.GetType().GetProperty("PendingSelector");
            var pendingSelector = piPending.GetValue(source) as Expression;

            var selectorBody = ReplacingExpressionVisitor.Replace(
                selector.Parameters[0],
                pendingSelector,
                selector.Body);

            selectorBody = ReplacingExpressionVisitor.Replace(
                selector.Parameters[1],
                Expression.Convert(new RowNumberExpression(null,
                new List<OrderingExpression>(new[] { new OrderingExpression(_sqlExpressionFactory.Fragment("(SELECT NULL)"), true) }), RelationalTypeMapping.NullMapping), typeof(int)),
                selectorBody);

            var miApplySelector = source.GetType().GetMethod("ApplySelector");
            miApplySelector?.Invoke(source, new object[] { selectorBody });

            return source;
        }

        private Expression ProcessSelect2(Expression source, LambdaExpression selector)
        {
            //var piPending = source.GetType().GetProperty("PendingSelector");
            //var pendingSelector = piPending.GetValue(source) as Expression;

            //// var piType = pendingSelector.GetType().GetProperty("Type");

            //var selectorBody = ReplacingExpressionVisitor.Replace(
            //    selector.Parameters[0],
            //    pendingSelector,
            //    selector.Body);

            //var clrType = pendingSelector.Type;

            //MethodCallExpression predicateBody = null;

            //clrType = source.Type;
            // try
            // {
            //     var method = RelationalQueryableExtensions._asSubQuery.MakeGenericMethod(clrType.GenericTypeArguments[0]);

            //     var newSource = Expression.Call(
            //method,//QueryableMethods.Where.MakeGenericMethod(clrType),
            //Expression.Quote(Expression.Lambda(predicateBody, entityParameter)));


            //     predicateBody = Expression.Call(method,
            //         pendingSelector
            //         );
            // }
            // catch (Exception e)
            // {
            //     int uu = 0;
            //     uu++;
            // }


            //selectorBody = ReplacingExpressionVisitor.Replace(
            //    selector.Parameters[1],
            //    Expression.Convert(new RowNumberExpression(null,
            //    new List<OrderingExpression>(new[] { new OrderingExpression(_sqlExpressionFactory.Fragment("(SELECT NULL)"), true) }), RelationalTypeMapping.NullMapping), typeof(int)),
            //    selectorBody);

            //var miApplySelector = source.GetType().GetMethod("ApplySelector");
            //miApplySelector?.Invoke(source, new object[] { predicateBody });

            return source;
        }
    }

    public static class ExpressionExtensions
    {
        public static LambdaExpression UnwrapLambdaFromQuote(this Expression expression)
       => (LambdaExpression)(expression is UnaryExpression unary && expression.NodeType == ExpressionType.Quote
           ? unary.Operand
           : expression);
    }
}
