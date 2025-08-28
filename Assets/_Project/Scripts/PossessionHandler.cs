using UnityEngine;

public class PossessionHandler : MonoBehaviour
{
    [SerializeField] private PossessablePaddle paddleLeft, paddleRight;

    public PossessablePaddle AssignPlayerInput(PlayerInputHandler inputHandler)
    {
        if (paddleLeft.PlayerInputHandler == null)
        {
            paddleLeft.EnableControl(inputHandler);
            return paddleLeft;
        }
        else if (paddleRight.PlayerInputHandler == null)
        {
            paddleRight.EnableControl(inputHandler);
            return paddleRight;
        }
        return null;
    }

    public void UnAssignPlayerInput(PossessablePaddle paddle)
    {
        paddle.DisableControl();
        
    }
}
