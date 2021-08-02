using System;

namespace Baseline.Validate.Internal.Extensions
{
    public static class TypeExtensions
    {
        public static bool IsRegisterableValidator(this Type typeToCheck)
        {
            return !typeToCheck.IsAbstract && typeToCheck.ValidatorBaseType() != null;
        }

        public static Type? ValidatorBaseType(this Type typeToCheck)
        {
            while (true)
            {
                if (typeToCheck.BaseType == null)
                {
                    return null;
                }

                if (
                    !typeToCheck.BaseType.IsGenericType || 
                    (
                        typeToCheck.BaseType.GetGenericTypeDefinition() != typeof(SyncValidator<>) && 
                        typeToCheck.BaseType.GetGenericTypeDefinition() != typeof(AsyncValidator<>)
                    )
                )
                {
                    typeToCheck = typeToCheck.BaseType;
                    continue;
                }

                return typeToCheck.BaseType;
            }
        }
    }
}