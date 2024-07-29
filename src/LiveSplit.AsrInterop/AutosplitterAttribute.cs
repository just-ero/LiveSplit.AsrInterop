using System;

namespace LiveSplit.AsrInterop;

[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false)]
public sealed class AutosplitterAttribute<TSplitter> : Attribute
    where TSplitter : Autosplitter, new();
