using System;

namespace LiveSplit.AsrInterop;

[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false, Inherited = false)]
public sealed class AutosplitterAttribute<TSplitter> : Attribute
    where TSplitter : class, new();
