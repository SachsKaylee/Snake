using UnityEngine;

/// <summary>
///     Used for creating pickups useable by the snake.
/// </summary>
public class Pickup : MonoBehaviour
{
    [SerializeField] private int _tailCount = 1;

    /// <summary>
    ///     By how many tails will this pickup extend the snake?
    /// </summary>
    public int TailCount
    {
        get { return _tailCount; }
    }
}
