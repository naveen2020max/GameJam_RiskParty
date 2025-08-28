using Unity.Netcode;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public PossessionHandler PossessionHandler;

    public static GameManager Instance;

    [SerializeField,Min(0f)]
    Vector2 arenaExtents = new Vector2(10f, 10f);
    public Ball Ball;

    private PlayerPaddleData[] playerPaddleData;

    void Awake()
    {
        Instance = this;
        playerPaddleData = new PlayerPaddleData[2] { new PlayerPaddleData(), new PlayerPaddleData()};
        Ball.StartNewGame();
    }

    public void OnPlayerJoin(PlayerInputHandler playerInputHandler)
    {
        foreach (var item in playerPaddleData)
        {
            if (item.InputHandler == null || item.InputHandler != playerInputHandler)
            {
                item.InputHandler = playerInputHandler;
                item.Paddle = PossessionHandler.AssignPlayerInput(playerInputHandler);
                break;
            }
        }
    }

    public void OnPlayerDisconnect(PlayerInputHandler playerInputHandler)
    {
        foreach (var item in playerPaddleData)
        {
            if (item.InputHandler != null && item.InputHandler == playerInputHandler)
            {
                item.InputHandler = null;
                PossessionHandler.UnAssignPlayerInput(item.Paddle);
            }
        }
    }

    private void Update()
    {
        Ball.Move();
        BounceYIfNeeded();
        BounceXIfNeeded();
        Ball.UpdateVisualization();
    }

    void BounceYIfNeeded()
    {
        float yExtents = arenaExtents.y - Ball.Extents;
        if(Ball.Position.y < -yExtents)
            Ball.BounceY(-yExtents);
        else if (Ball.Position.y > yExtents)
            Ball.BounceY(yExtents);
    }

    void BounceXIfNeeded()
    {
        float xExtents = arenaExtents.x - Ball.Extents;
        if (Ball.Position.x < -xExtents)
            Ball.BounceX(-xExtents);
        else if (Ball.Position.x > xExtents)
            Ball.BounceX(xExtents);
    }
}
