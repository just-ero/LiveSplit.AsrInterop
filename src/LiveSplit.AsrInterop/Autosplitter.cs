using System;

namespace LiveSplit.AsrInterop;

public abstract class Autosplitter
{
    public abstract string[] ProcessNames { get; }

    public virtual void Startup() { }

    public virtual bool Init(Process game)
    {
        return true;
    }

    public virtual bool Update(Process game)
    {
        return true;
    }

    public virtual bool Start(Process game)
    {
        return false;
    }

    public virtual bool Split(Process game)
    {
        return false;
    }

    public virtual bool Reset(Process game)
    {
        return false;
    }

    public virtual TimeSpan? GameTime(Process game)
    {
        return null;
    }

    public virtual bool IsLoading(Process game)
    {
        return false;
    }

    public virtual void Exit() { }
}
