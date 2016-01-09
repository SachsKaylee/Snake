using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     Controller for an entire snake GameObject. Needs to be assigned
///     to a GameController to reccieve input.
/// </summary>
public class Snake : MonoBehaviour
{
    [SerializeField] private SnakeTail _firstTail;
    private GameController _gameController;

    /// <summary>
    ///     Unity event. Called once the behavior is instantiated.
    ///     Finds the GameController.
    /// </summary>
    private void Awake()
    {
        _gameController = FindObjectOfType<GameController>();
    }

    /// <summary>
    ///     Moves the snake up by one field. (If position is allowed or
    ///     forceMove true)
    /// </summary>
    public void MoveUp(bool forceMove = false)
    {
        Vector2 position = GetPosition();
        MoveTo(new Vector2(position.x, position.y + .5f), forceMove);
    }

    /// <summary>
    ///     Moves the snake down by one field. (If position is allowed or
    ///     forceMove true)
    /// </summary>
    public void MoveDown(bool forceMove = false)
    {
        Vector2 position = GetPosition();
        MoveTo(new Vector2(position.x, position.y - .5f), forceMove);
    }

    /// <summary>
    ///     Moves the snake left by one field. (If position is allowed or
    ///     forceMove true)
    /// </summary>
    public void MoveLeft(bool forceMove = false)
    {
        Vector2 position = GetPosition();
        MoveTo(new Vector2(position.x - .5f, position.y), forceMove);
    }

    /// <summary>
    ///     Moves the snake right by one field. (If position is allowed or
    ///     forceMove true)
    /// </summary>
    public void MoveRight(bool forceMove = false)
    {
        Vector2 position = GetPosition();
        MoveTo(new Vector2(position.x + .5f, position.y), forceMove);
    }

    public void Move(MoveDirection direction, bool forceMove = false)
    {
        switch (direction)
        {
            case MoveDirection.Left:
                MoveLeft(forceMove);
                break;
            case MoveDirection.Up:
                MoveUp(forceMove);
                break;
            case MoveDirection.Right:
                MoveRight(forceMove);
                break;
            case MoveDirection.Down:
                MoveDown(forceMove);
                break;
        }
    }

    /// <summary>
    ///     Moves the snake to the given position. (If position is allowed or
    ///     forceMove true)
    ///     If the movement fails the game is lost.
    /// </summary>
    public void MoveTo(Vector2 position, bool forceMove = false)
    {
        if (forceMove || _gameController.IsPositionAllowed(position))
        {
            _firstTail.MoveTo(position);
        }
        else
        {
            _gameController.LooseGame();
        }
    }

    /// <summary>
    ///     Adds a new tail to this snake.
    /// </summary>
    public void AddTail()
    {
        SnakeTail tail = _firstTail.AddTail();
        tail.transform.SetParent(transform);
    }

    /// <summary>
    ///     Returns the positions of all tails used by this snake.
    /// </summary>
    public IEnumerable<Vector2> GetTailPositions()
    {
        var targetList = new List<Vector2>();
        _firstTail.GetTailPositions(targetList);
        return targetList;
    }

    /// <summary>
    ///     Returns the amount of tails of this snake.
    /// </summary>
    public int GetSnakeLength()
    {
        return _firstTail.GetTailCount();
    }

    /// <summary>
    ///     Returns the current (head) posiiton of this snake.
    /// </summary>
    public Vector2 GetPosition()
    {
        return _firstTail.transform.position;
    }
}