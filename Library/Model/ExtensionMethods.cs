using System;
using System.Reflection;

namespace Library.Model
{
    internal static class ExtensionMethods
    {
        internal static bool GetVisible(this Type type)
        {
            if (type == null)
                throw new ArgumentNullException("Type can't be null.");
            return type.IsPublic || type.IsNestedPublic || type.IsNestedFamily || type.IsNestedFamANDAssem;
        }

        internal static bool GetVisible(this MethodBase method)
        {
            if (method == null)
                throw new ArgumentNullException("Type can't be null.");
            return method != null && (method.IsPublic || method.IsFamily || method.IsFamilyAndAssembly);
        }

        internal static string GetNamespace(this Type type)
        {
            if (type == null)
                throw new ArgumentNullException("Type can't be null.");
            return type.Namespace ?? string.Empty;
        }
    }
}