using System;

using Microsoft.CodeAnalysis;

namespace LiveSplit.AsrInterop.SourceGenerators.Extensions;

internal static class AttributeDataExtensions
{
    public static T? GetConstructorArgument<T>(this AttributeData attribute, int index)
    {
        return (T?)attribute.ConstructorArguments[index].Value;
    }

    public static T? GetConstructorArgumentOrDefault<T>(this AttributeData attribute, int index)
    {
        if (index >= attribute.ConstructorArguments.Length)
        {
            return default;
        }

        if (attribute.ConstructorArguments[index].Value is not T value)
        {
            return default;
        }

        return value;
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

    public static T? GetNamedArgumentOrDefault<T>(this AttributeData attribute, string name)
    {
        foreach (var arg in attribute.NamedArguments)
        {
            if (arg.Key == name
                && arg.Value.Value is T value)
            {
                return value;
            }
        }

        return default;
    }
}
