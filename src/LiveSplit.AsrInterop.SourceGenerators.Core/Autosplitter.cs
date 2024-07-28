namespace LiveSplit.AsrInterop.SourceGenerators.Core;

public abstract class Autosplitter
{
    public abstract string[] ProcessNames { get; }

    public virtual void Startup() { }

    public virtual bool Init(LiveSplit.AsrInterop.Process game) => true;
    public virtual bool Update(LiveSplit.AsrInterop.Process game) => true;

    public virtual bool Start(LiveSplit.AsrInterop.Process game) => false;
    public virtual bool Split(LiveSplit.AsrInterop.Process game) => false;
    public virtual bool Reset(LiveSplit.AsrInterop.Process game) => false;

    public virtual global::System.TimeSpan? GameTime(LiveSplit.AsrInterop.Process game) => null;
    public virtual bool IsLoading(LiveSplit.AsrInterop.Process game) => false;

    public virtual void Exit() { }
}
