namespace LiveSplit.AsrInterop.Core;

/// <summary>
///     Represents the state of the LiveSplit timer.
/// </summary>
public enum TimerState
{
    /// <summary>
    ///     The timer is not running.
    /// </summary>
    NotRunning = 0,

    /// <summary>
    ///     The timer is running.
    /// </summary>
    Running = 1,

    /// <summary>
    ///     The timer is paused.
    /// </summary>
    Paused = 2,

    /// <summary>
    ///     The timer has ended.
    /// </summary>
    Ended = 3
}
