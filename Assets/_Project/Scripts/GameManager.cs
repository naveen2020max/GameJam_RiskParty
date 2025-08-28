using Unity.Netcode;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public PossessionHandler PossessionHandler;

    public static GameManager Instance;

    private PlayerPaddleData[] playerPaddleData;

    void Awake()
    {
        Instance = this;
        playerPaddleData = new PlayerPaddleData[2] { new PlayerPaddleData(), new PlayerPaddleData()};
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
}
