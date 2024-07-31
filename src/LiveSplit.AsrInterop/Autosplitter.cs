using System;

namespace LiveSplit.AsrInterop;

public abstract class Autosplitter
{
    public abstract string[] ProcessNames { get; }

    public virtual IAutosplitterSettings Settings { get; } = new DefaultSettings();

    public virtual void Startup() { }

    public virtual bool Init(ExternalProcess game)
    {
        return true;
    }

    public virtual bool Update(ExternalProcess game)
    {
        return true;
    }

    public virtual bool Start(ExternalProcess game)
    {
        return false;
    }

    public virtual bool Split(ExternalProcess game)
    {
        return false;
    }

    public virtual bool Reset(ExternalProcess game)
    {
        return false;
    }

    public virtual TimeSpan? GameTime(ExternalProcess game)
    {
        return null;
    }

    public virtual bool IsLoading(ExternalProcess game)
    {
        return false;
    }

    public virtual void Exit() { }
}
