namespace LiveSplit.AsrInterop.SourceGenerators;

public static class Sources
{
    public const string Implementation = $$"""
        // <auto-generated />

        namespace {{Tokens.SplitterNamespace}} {
            partial class {{Tokens.SplitterClassName}} : LiveSplit.AsrInterop.SourceGenerators.Autosplitter { }
        }
        """;

    public const string GlobalNamespaceImplementation = $$"""
        // <auto-generated />

        partial class {{Tokens.SplitterClassName}} : LiveSplit.AsrInterop.SourceGenerators.Autosplitter { }
        """;

    public const string AbstractAutosplitter = $$"""
        // <auto-generated />

        #nullable enable

        namespace LiveSplit.AsrInterop.SourceGenerators.Core {
            public abstract class Autosplitter {
                private static LiveSplit.AsrInterop.SourceGenerators.Autosplitter _instance = new {{Tokens.SplitterFullName}}();
                private static LiveSplit.AsrInterop.Process? _process;

                static Autosplitter() {
                    _instance.Startup();
                }

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

                [global::System.Diagnostics.StackTraceHiddenAttribute]
                [global::System.Runtime.InteropServices.UnmanagedCallersOnly(EntryPoint = "update")]
                private static void UpdateInternal() {
                    if (_process is null) {
                        foreach (string processName in _instance.ProcessNames) {
                            if (LiveSplit.AsrInterop.Process.TryGetProcessByName(processName, out LiveSplit.AsrInterop.Process? process)
                                && _instance.Init(process)) {
                                _process = process;
                                break;
                            }
                        }

                        if (_process is null) {
                            return;
                        }
                    }

                    if (_process.HasExited) {
                        LiveSplit.AsrInterop.Core.Process.Detach(_process.Handle);

                        _instance.Exit();
                        _process = null;
                        return;
                    }

                    if (!_instance.Update(_process)) {
                        return;
                    }

                    switch (LiveSplit.AsrInterop.Core.Timer.GetState()) {
                        case LiveSplit.AsrInterop.Core.TimerState.NotRunning:
                            if (_instance.Start(_process)) {
                                LiveSplit.AsrInterop.Core.Timer.Start();
                                goto case LiveSplit.AsrInterop.Core.TimerState.Running;
                            }
                            break;

                        case LiveSplit.AsrInterop.Core.TimerState.Running:
                        case LiveSplit.AsrInterop.Core.TimerState.Paused:
                            if (_instance.IsLoading(_process)) {
                                LiveSplit.AsrInterop.Core.Timer.PauseGameTime();
                            }
                            else {
                                LiveSplit.AsrInterop.Core.Timer.ResumeGameTime();
                            }

                            if (_instance.GameTime(_process) is global::System.TimeSpan gameTime) {
                                LiveSplit.AsrInterop.Core.Timer.SetGameTime(gameTime.Seconds, gameTime.Nanoseconds);
                            }

                            if (_instance.Reset(_process)) {
                                LiveSplit.AsrInterop.Core.Timer.Reset();
                            }
                            else if (_instance.Split(_process)) {
                                LiveSplit.AsrInterop.Core.Timer.Split();
                            }
                            break;
                    }
                }
            }
        }
        """;
}
