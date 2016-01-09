using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     Class used to control the tails of a snake.
/// </summary>
public class SnakeTail : MonoBehaviour
{
    private static SnakeTail _snakeTailPrefab;
    [SerializeField] private SnakeTail _nextTail;

    /// <summary>
    ///     The next tail assigned to this tail. null if none.
    /// </summary>
    public SnakeTail NextTail
    {
        get { return _nextTail; }
    }

    /// <summary>
    ///     The snake this tail belongs to.
    /// </summary>
    //public Snake AssignedSnake { get; set; }

    /// <summary>
    ///     Unity event. Called once the behavior is instantiated.
    ///     Gets the snake tail prefab from the game resources.
    /// </summary>
    private void Awake()
    {
        if (!_snakeTailPrefab)
        {
            _snakeTailPrefab = Resources.Load<SnakeTail>("Snake Tail");
        }
    }

    /// <summary>
    ///     Recursively adds a tail to this or one of the next tails,
    ///     dependat on which tail needs a next tail.
    /// </summary>
    public SnakeTail AddTail()
    {
        if (NextTail)
        {
            return NextTail.AddTail();
        }
        _nextTail = (SnakeTail) Instantiate(_snakeTailPrefab, transform.position, Quaternion.identity);
        return _nextTail;
    }

    /// <summary>
    ///     Moves this tail to the given position. Recursively moves the next
    ///     tails with it.
    /// </summary>
    public void MoveTo(Vector2 position)
    {
        if (NextTail)
        {
            NextTail.MoveTo(transform.position);
        }
        transform.position = position;
    }

    /// <summary>
    ///     Recursively adds the position of this tail and the following
    ///     tails to a given List.
    /// </summary>
    public void GetTailPositions(IList<Vector2> targetList)
    {
        targetList.Add(transform.position);
        if (NextTail)
        {
            NextTail.GetTailPositions(targetList);
        }
    }

    /// <summary>
    ///     Returns the amount of tails attached to this tail after itself (included).
    /// </summary>
    public int GetTailCount(int alreadyCounted = 0)
    {
        alreadyCounted++;
        if (NextTail)
        {
            return NextTail.GetTailCount(alreadyCounted);
        }
        return alreadyCounted;
    }
}