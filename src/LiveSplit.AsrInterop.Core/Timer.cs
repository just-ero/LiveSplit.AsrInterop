using System;
using System.Text;

namespace LiveSplit.AsrInterop.Core;

/// <summary>
///     Provides access to marshalled asr timer functions.
/// </summary>
public static unsafe class Timer
{
    /// <summary>
    ///     Gets the current <see cref="TimerState"/> of the timer.
    /// </summary>
    public static TimerState GetState()
    {
        return (TimerState)sys.timer_get_state();
    }

    /// <summary>
    ///     Starts the timer.
    /// </summary>
    public static void Start()
    {
        sys.timer_start();
    }

    /// <summary>
    ///     Splits the timer's current segment.
    /// </summary>
    public static void Split()
    {
        sys.timer_split();
    }

    /// <summary>
    ///     Resets the timer.
    /// </summary>
    public static void Reset()
    {
        sys.timer_reset();
    }

    /// <summary>
    ///     Skips the timer's current segment.
    /// </summary>
    public static void SkipSplit()
    {
        sys.timer_skip_split();
    }

    /// <summary>
    ///     Undoes the timer's previous split.
    /// </summary>
    public static void UndoSplit()
    {
        sys.timer_undo_split();
    }

    /// <summary>
    ///     Sets the timer's game time to the specified <paramref name="time"/>.
    /// </summary>
    public static void SetGameTime(TimeSpan time)
    {
        SetGameTime(time.Seconds, time.Nanoseconds);
    }

    /// <summary>
    ///     Sets the timer's game time to the specified <paramref name="seconds"/> and <paramref name="nanoseconds"/>.
    /// </summary>
    public static void SetGameTime(long seconds, int nanoseconds)
    {
        sys.timer_set_game_time(seconds, nanoseconds);
    }

    /// <summary>
    ///     Pauses the timer's game time.
    /// </summary>
    public static void PauseGameTime()
    {
        sys.timer_pause_game_time();
    }

    /// <summary>
    ///     Resumes the timer's game time.
    /// </summary>
    public static void ResumeGameTime()
    {
        sys.timer_resume_game_time();
    }

    /// <summary>
    ///     Sets a custom key-value pair for visualization on the timer.
    /// </summary>
    /// <param name="key">
    ///     The name of the variable.
    /// </param>
    /// <param name="value">
    ///     The value of the variable.
    /// </param>
    public static void SetVariable(string key, string value)
    {
        SetVariable(Encoding.UTF8.GetBytes(key), Encoding.UTF8.GetBytes(value));
    }

    /// <summary>
    ///     Sets a custom key-value pair for visualization on the timer.
    /// </summary>
    /// <param name="key">
    ///     The name of the variable.
    /// </param>
    /// <param name="value">
    ///     The value of the variable.
    /// </param>
    public static void SetVariable(ReadOnlySpan<byte> key, ReadOnlySpan<byte> value)
    {
        fixed (byte* pKey = key, pValue = value)
        {
            sys.timer_set_variable(pKey, (nuint)key.Length, pValue, (nuint)value.Length);
        }
    }
}
