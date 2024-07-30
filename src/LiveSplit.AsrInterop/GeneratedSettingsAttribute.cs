using System;

namespace LiveSplit.AsrInterop.Settings;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public sealed class GeneratedSettingsAttribute : Attribute;
