using System;
using UnityEngine;

public class PossessablePaddle : MonoBehaviour
{
    [SerializeField] private float speed = 5;
    [SerializeField] private float yClamp = 4.5f;

    private PlayerInputHandler inputHandler;

    public void Move(float dir)
    {
        Debug.Log("moving");
        Vector3 position = transform.position;
        position += Vector3.up * dir * speed * Time.deltaTime;
        position.y = Mathf.Clamp(position.y, -yClamp, yClamp);
        transform.position = position;
    }

    public void EnableControl(PlayerInputHandler inputHandler)
    {
        this.inputHandler = inputHandler;
        inputHandler.Move += Move;
    }

    public void DisableControl()
    {
        if (inputHandler)
            inputHandler.Move -= Move;
    }
}
