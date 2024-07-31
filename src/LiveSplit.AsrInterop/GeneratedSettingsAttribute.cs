using System;

namespace LiveSplit.AsrInterop;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public sealed class GeneratedSettingsAttribute : Attribute;
