using System;

namespace LiveSplit.AsrInterop.Settings;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class HeadingAttribute : Attribute
{
    public HeadingAttribute(string title, uint level)
    {
        Title = title;
        Level = level;
    }

    public string Title { get; }
    public uint Level { get; }
}

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public sealed class H1Attribute(string title) : HeadingAttribute(title, 0);

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public sealed class H2Attribute(string title) : HeadingAttribute(title, 1);

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public sealed class H3Attribute(string title) : HeadingAttribute(title, 2);

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public sealed class H4Attribute(string title) : HeadingAttribute(title, 3);

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public sealed class H5Attribute(string title) : HeadingAttribute(title, 4);

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public sealed class H6Attribute(string title) : HeadingAttribute(title, 5);
