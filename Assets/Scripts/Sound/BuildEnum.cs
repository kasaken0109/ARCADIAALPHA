using System;
using System.Reflection;
using System.Reflection.Emit;

//#pragma warning disable CS8321 // ローカル関数は宣言されていますが、一度も使用されていません
//static Type BuildSystem(string[] keys)
//#pragma warning restore CS8321 // ローカル関数は宣言されていますが、一度も使用されていません
//{
//    AssemblyName assemblyName = new AssemblyName { Name = "MyAssembly" };
//    AppDomain domain = AppDomain.CurrentDomain;
//    AssemblyBuilder assemblyBuilder = domain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
//    ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule("MyModule");
//    EnumBuilder enumBuilder = moduleBuilder.DefineEnum("MyNamespace.MyEnum", TypeAttributes.Public, typeof(int));

//    for (int i = 0; i < keys.Length; ++i)
//    {
//        enumBuilder.DefineLiteral(keys[i], i + 1);
//    }
//    return enumBuilder.CreateType();
//}
