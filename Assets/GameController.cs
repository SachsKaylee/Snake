using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
///     Main GameController managing player input and pickups.
/// </summary>
public class GameController : MonoBehaviour
{
	/// <summary>
    ///     Is the game paused? The snake cannot be moved while the game is paused.
    /// </summary>
    public static bool IsPaused { get; set; }

    private static Pickup _pickupPrefab;

    // The borders limiting the game world. The snake can't "leave" 
    // their boundaries.
    [SerializeField] private Transform _borderBottomLeft;
    [SerializeField] private Transform _borderTopRight;
    private MoveDirection _currentDirection;
    private MoveDirection _lastDirection;
    private Pickup _currentPickup;
    [SerializeField] private float _gameSpeed;
    [SerializeField] private Snake _snake;
    [SerializeField] private Interface _interface;
    private float _timeUntilNextMove;

    /// <summary>
    ///     The snake game object.
    /// </summary>
    public Snake Snake
    {
        get { return _snake; }
    }

    /// <summary>
    ///     Unity event. Called once the behavior is instantiated.
    ///     Initializes some time stuff and loads prefabs.
    /// </summary>
    private void Awake()
    {
        Debug.Log("Data Path " + Application.dataPath);
        _timeUntilNextMove = _gameSpeed;
        _pickupPrefab = Resources.Load<Pickup>("Tail Pickup (x1)");
        StartNewGame();
        //DebugUtils.Do(5, Snake.AddTail);
        //DebugUtils.Do(15, delegate { Debug.Log(GetRandomPositionInBoundaries()); });
    }

    /// <summary>
    ///     Checks if the given position is withing the boundaries of the
    ///     map.
    /// </summary>
    public bool IsPositionAllowed(Vector2 position)
    {
        return position.x > _borderBottomLeft.position.x && position.x < _borderTopRight.position.x &&
               position.y > _borderBottomLeft.position.y && position.y < _borderTopRight.position.y &&
               !Snake.GetTailPositions().Contains(position);
    }

    /// <summary>
    ///     Generates a random position within the boundaries.
    /// </summary>
    public Vector2 GetRandomPositionInBoundaries()
    {
        float x = Random.Range(_borderBottomLeft.position.x, _borderTopRight.position.x);
        float y = Random.Range(_borderBottomLeft.position.y, _borderTopRight.position.y);
        return new Vector2(x - x%.5f, y - y%.5f);
    }

    /// <summary>
    ///     Unity event. Called once every frame.
    ///     Gets player input in order to move the snake.
    /// </summary>
    private void Update()
    {
        if (IsPaused)
            return;

        MoveDirection wantedDirection = _currentDirection;
        if (Input.GetButton("Move Left"))
        {
            wantedDirection = MoveDirection.Left;
        }
        if (Input.GetButton("Move Right"))
        {
            wantedDirection = MoveDirection.Right;
        }
        if (Input.GetButton("Move Down"))
        {
            wantedDirection = MoveDirection.Down;
        }
        if (Input.GetButton("Move Up"))
        {
            wantedDirection = MoveDirection.Up;
        }

        if (Snake.GetSnakeLength() > 1)
        {
            // If adding a move direction from another is 0 the move
            // directions are opposite to each other.
            // We are checking this since we want to avoid the player to
            // randomly loose by pressing the opposite direction key during
            // a long game.
            int directionCheck = (int)_lastDirection + (int) wantedDirection;
            if (directionCheck != 0)
            {
                _currentDirection = wantedDirection;
            }
            else
            {
                _currentDirection = _lastDirection;
            }
        }
        else
        {
            _currentDirection = wantedDirection;
        }

        _timeUntilNextMove -= Time.deltaTime;
        if (_timeUntilNextMove > 0)
            return;

        _timeUntilNextMove = _gameSpeed;


        Snake.Move(_currentDirection);
        if (_currentPickup && VectorUtils.Vector2EqualsVector3(Snake.GetPosition(), _currentPickup.transform.position))
        {
            for (var i = 0; i < _currentPickup.TailCount; i++)
            {
                Snake.AddTail();
            }
            CreateNewPickup();
        }
        _lastDirection = _currentDirection;
    }

    /// <summary>
    ///     Losses the game and writes the highscore.
    /// </summary>
    public void LooseGame()
    {
        IsPaused = true;
        int current = PlayerPrefs.GetInt("Highscore", 0);
        int length = Snake.GetSnakeLength();
        if (length > current)
        {
            PlayerPrefs.SetInt("Highscore", length);
        }
        _interface.ShowGameOverScreen(length);
    }

    /// <summary>
    ///     Starts a new game.
    /// </summary>
    public void StartNewGame(bool allowCheat = true)
    {
        CreateNewSnake();
        CreateNewPickup();
        // Some cheats for testing purposes.
        if (allowCheat)
        {
            if (File.Exists(Application.dataPath + "/cheat.txt"))
            {
                Debug.Log("Found cheat.txt file ... Parsing now.");
                string cheat = File.ReadAllText(Application.dataPath + "/cheat.txt");
                int cheatNumber;
                int.TryParse(cheat, out cheatNumber);
                DebugUtils.Do(cheatNumber, Snake.AddTail);
            }
        }
    }

    /// <summary>
    ///     Destroys the old snake and creates a new one.
    /// </summary>
    public void CreateNewSnake()
    {
        if (Snake)
        {
            Destroy(_snake.gameObject);
        }
        _snake = (Snake)Instantiate(Resources.Load<Snake>("Snake"), new Vector3(1, 0, 0), Quaternion.identity);
    }

    /// <summary>
    ///     Creates a new pickup in the world which can be picked up by the snake in order to extends in size.
    /// </summary>
    private void CreateNewPickup()
    {
        if (_currentPickup)
        {
            Destroy(_currentPickup.gameObject);
        }
        _currentPickup = (Pickup) Instantiate(_pickupPrefab, GetRandomPositionInBoundaries(), Quaternion.identity);
        _currentPickup.transform.SetParent(transform, true);
    }

    /// <summary>
    ///     Unity event. Used to draw gizmos to the scene view.
    ///     Draws gizmos for the boundaries if they are set.
    /// </summary>
    private void OnDrawGizmos()
    {
        if (_borderBottomLeft && _borderTopRight)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(new Vector3(_borderBottomLeft.position.x, _borderTopRight.position.y),
                _borderTopRight.position);
            Gizmos.DrawLine(_borderTopRight.position,
                new Vector3(_borderTopRight.position.x, _borderBottomLeft.position.y));
            Gizmos.DrawLine(new Vector3(_borderTopRight.position.x, _borderBottomLeft.position.y),
                _borderBottomLeft.position);
            Gizmos.DrawLine(_borderBottomLeft.position,
                new Vector3(_borderBottomLeft.position.x, _borderTopRight.position.y));
        }
    }
}