using System;

public static class DebugUtils
{
    /// <summary>
    ///     Does a specific action a specific amount of times.
    /// </summary>
    public static void Do(int count, Action action)
    {
        for (int i = 0; i < count; i++)
        {
            action.Invoke();
        }
    }
}
