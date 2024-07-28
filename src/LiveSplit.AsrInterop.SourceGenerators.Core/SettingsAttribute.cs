using System;

namespace LiveSplit.AsrInterop.SourceGenerators.Core;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
public sealed class SettingsAttribute : Attribute;
