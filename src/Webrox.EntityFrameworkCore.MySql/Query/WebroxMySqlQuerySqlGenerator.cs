//using Microsoft.EntityFrameworkCore.Query;
//using MySqlLib = MySql.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Storage;
//using MySql.EntityFrameworkCore.Infrastructure.Internal;
//using System.Diagnostics.CodeAnalysis;
//using System.Linq.Expressions;
//using System.Reflection;
//using Webrox.EntityFrameworkCore.Core;
//using Webrox.EntityFrameworkCore.Core.SqlExpressions;
//using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
//using System.Reflection.Emit;

//namespace Webrox.EntityFrameworkCore.MySql.Query
//{
//    /// <inheritdoc />
//    [SuppressMessage("Usage", "EF1001", MessageId = "Internal EF Core API usage.")]
//    public class WebroxMySqlQuerySqlGenerator : 
//        QuerySqlGenerator
//    {
//        //private readonly ITenantDatabaseProvider _databaseProvider;
//        private readonly WebroxQuerySqlGenerator _webroxQuerySqlGenerator;
//        private readonly IMySQLOptions _mySQLOptions;
//        object _MySQLQuerySqlGenerator;
//        MethodInfo _VisitExtension, _VisitCrossApply,
//            _VisitOuterApply, _VisitSqlBinary,
//            _VisitSqlFunction, _VisitSqlUnary
//            ;

//        void createDYnamicAssembly()
//        {
//            var ddd = this.GetType().Assembly.GetName();


//            string name = "Mocks";
//            AssemblyName assemblyName = new AssemblyName();
//            assemblyName.Name = name;
//            var assembly = typeof(MySqlLib.Query.Internal.MySQLCommandParser).Assembly;
//            var type = assembly.GetType("MySql.EntityFrameworkCore.Query.MySQLQuerySqlGenerator");

//            // Grab the public key of the assembly which has the internal class.

//            byte[] publicKey = type.Assembly.GetName().GetPublicKey();

//            // Assign the public key to the new assembly.
//            // Assign the strong name key to the new assembly.
            
//            assemblyName.SetPublicKey(publicKey);

//            // Get the public key to be added to "AssemblyInfo.cs".
//            string publicKeyString = BitConverter.ToString(publicKey).Replace("-", "");
//            System.Diagnostics.Debug.WriteLine(
//                "Put this in AssemblyInfo.cs --> [assembly: InternalsVisibleTo(\"Mocks, PublicKey=" +
//                publicKeyString + "\")]");

//            AssemblyBuilder assemblyBuilder =
//                  AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);

//            //AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
//            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule(name);

//            TypeBuilder typeBuilder =
//                moduleBuilder.DefineType(
//                    "MockFoo",
//                    TypeAttributes.NotPublic | TypeAttributes.Class,
//                    type);


//            var ctorbuilder = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Any,
//                new Type[] { typeof(QuerySqlGeneratorDependencies) });

//            var ilGenerator = ctorbuilder.GetILGenerator();
//            var baseCtor = type.GetConstructor(new Type[] { typeof(QuerySqlGeneratorDependencies) });

//            // i want to call the constructor of the baseclass with eventName as param
//            ilGenerator.Emit(OpCodes.Ldarg_0); // push "this"
//            ilGenerator.Emit(OpCodes.Ldarg_1); // push dependencies
//            ilGenerator.Emit(OpCodes.Call, baseCtor);
//            ilGenerator.Emit(OpCodes.Ret);

//            Type myMockFoo = typeBuilder.CreateType();
//        }


//        /// <inheritdoc />
//        public WebroxMySqlQuerySqlGenerator(
//           QuerySqlGeneratorDependencies dependencies,
//           IRelationalTypeMappingSource typeMappingSource,
//           IMySQLOptions mySQLOptions,
//           WebroxQuerySqlGenerator webroxQuerySqlGenerator
//            //ITenantDatabaseProvider databaseProvider
//            )
//           : base(dependencies)//, typeMappingSource, mySQLOptions.ReverseNullOrderingEnabled, mySQLOptions.PostgresVersion)
//        {
//            _webroxQuerySqlGenerator = webroxQuerySqlGenerator;
//            _mySQLOptions = mySQLOptions;

//            createDYnamicAssembly();
//            return;
//            var assembly = typeof(MySqlLib.Query.Internal.MySQLCommandParser).Assembly;
//            // Grab the public key of the assembly which has the internal class.
//            byte[] publicKey = assembly.GetName().GetPublicKey();
//            ////
//            var type = assembly.GetType("MySql.EntityFrameworkCore.Query.MySQLQuerySqlGenerator");
            
//            AssemblyName an = new AssemblyName();
//            an.Name = "CheatingInternal";
//            an.SetPublicKey(publicKey);
//            AssemblyBuilder ab = AssemblyBuilder.DefineDynamicAssembly(an, AssemblyBuilderAccess.Run);
//            var mb = ab.DefineDynamicModule("Mod");//, "CheatingInternal.dll");
//            //AppDomain currentDom = Thread.GetDomain();
//            //currentDom.TypeResolve += (a, e)=>
//            //{
//            //    Assembly ret = null;
//            //    return ret;
//            //};

//            TypeBuilder tb = mb.DefineType("Cheat", TypeAttributes.NotPublic | TypeAttributes.Class, type);

//    //        var baseNonGenericCtor = type.GetConstructor(
//    //BindingFlags.NonPublic | BindingFlags.Instance, null,
//    //new[] { typeof(string) }, null);
//    //        var baseCtor = TypeBuilder.GetConstructor(type, baseNonGenericCtor);

//            var ctorbuilder = tb.DefineConstructor(MethodAttributes.Public, CallingConventions.Any,
//                new Type[] { typeof(QuerySqlGeneratorDependencies) });

//            var ilGenerator = ctorbuilder.GetILGenerator();
//            var baseCtor = type.GetConstructor(new Type[] { typeof(QuerySqlGeneratorDependencies) });

//            // i want to call the constructor of the baseclass with eventName as param
//            ilGenerator.Emit(OpCodes.Ldarg_0); // push "this"
//            ilGenerator.Emit(OpCodes.Ldarg_1); // push dependencies
//            ilGenerator.Emit(OpCodes.Call, baseCtor);
//            ilGenerator.Emit(OpCodes.Ret);

//            Type cheatType = tb.CreateType();








            
//            _MySQLQuerySqlGenerator = Activator.CreateInstance(type, new object[] { dependencies });
//            var fieldRelationalCommandBuilder = type.BaseType.GetField("_relationalCommandBuilder",BindingFlags.Instance | BindingFlags.NonPublic);
//            var relationalCommandBuilder = dependencies.RelationalCommandBuilderFactory.Create();
//            fieldRelationalCommandBuilder?.SetValue(_MySQLQuerySqlGenerator, relationalCommandBuilder);


//            _VisitExtension = type.GetMethod(nameof(VisitExtension), BindingFlags.Instance | BindingFlags.NonPublic);
//            _VisitCrossApply = type.GetMethod(nameof(VisitCrossApply), BindingFlags.Instance | BindingFlags.NonPublic);
//            _VisitOuterApply = type.GetMethod(nameof(VisitOuterApply), BindingFlags.Instance | BindingFlags.NonPublic);
//            _VisitSqlBinary = type.GetMethod(nameof(VisitSqlBinary), BindingFlags.Instance | BindingFlags.NonPublic);
//            _VisitSqlFunction = type.GetMethod(nameof(VisitSqlFunction), BindingFlags.Instance | BindingFlags.NonPublic);
//            _VisitSqlUnary = type.GetMethod(nameof(VisitSqlUnary), BindingFlags.Instance | BindingFlags.NonPublic);
//        }

//        /// <inheritdoc />
//        protected override Expression VisitExtension(Expression extensionExpression)
//        {
//            switch (extensionExpression)
//            {
//                case WindowExpression windowExpression:
//                    return _webroxQuerySqlGenerator.VisitWindowFunction(Sql, windowExpression, Visit, VisitOrdering);
//                default:
//                    return _VisitExtension?.Invoke(_MySQLQuerySqlGenerator, new[] { extensionExpression }) as Expression;
//            }
//        }

//        protected override Expression VisitCrossApply(CrossApplyExpression crossApplyExpression)
//        {
//            return _VisitCrossApply?.Invoke(_MySQLQuerySqlGenerator, new[] { crossApplyExpression }) as Expression;
//        }

//        protected override Expression VisitOuterApply(OuterApplyExpression outerApplyExpression)
//        {
//            return _VisitOuterApply?.Invoke(_MySQLQuerySqlGenerator, new[] { outerApplyExpression }) as Expression;
//        }
//        protected override Expression VisitSqlBinary(SqlBinaryExpression sqlBinaryExpression)
//        {
//            return _VisitSqlBinary?.Invoke(_MySQLQuerySqlGenerator, new[] { sqlBinaryExpression }) as Expression;
//        }
//        protected override Expression VisitSqlFunction(SqlFunctionExpression sqlFunctionExpression)
//        {
//            return _VisitSqlFunction?.Invoke(_MySQLQuerySqlGenerator, new[] { sqlFunctionExpression }) as Expression;
//        }
//        protected override Expression VisitSqlUnary(SqlUnaryExpression sqlUnaryExpression)
//        {
//            return _VisitSqlUnary?.Invoke(_MySQLQuerySqlGenerator, new[] { sqlUnaryExpression }) as Expression;
//        }
//    }
//}