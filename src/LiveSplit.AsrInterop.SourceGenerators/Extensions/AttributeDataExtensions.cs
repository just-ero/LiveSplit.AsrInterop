using System;

using Microsoft.CodeAnalysis;

namespace LiveSplit.AsrInterop.SourceGenerators.Extensions;

internal static class AttributeDataExtensions
{
    public static T? GetConstructorArgument<T>(this AttributeData attribute, int index)
    {
        return (T?)attribute.ConstructorArguments[index].Value;
    }

    public static T? GetNamedArgument<T>(this AttributeData attribute, string name)
    {
        foreach (var arg in attribute.NamedArguments)
        {
            if (arg.Key == name)
            {
                return (T?)arg.Value.Value;
            }
        }

        throw new ArgumentException(
            $"The attribute does not have a named argument with the name '{name}'.");
    }
}
