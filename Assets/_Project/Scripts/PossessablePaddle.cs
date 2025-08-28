using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class PossessablePaddle : MonoBehaviour
{
    [SerializeField] private float speed = 5;
    [SerializeField] private float yClamp = 4.5f;

    public PlayerInputHandler PlayerInputHandler => inputHandler;
    private PlayerInputHandler inputHandler;

    public void Move(PlayerInputHandler.KeyInput context)
    {
        Vector3 position = transform.position;
        position += Vector3.up * context.InputValue * speed * Time.deltaTime;
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
        {
            inputHandler.Move -= Move;
            inputHandler = null;
        }
    }

    //bool Validate(Key key)
    //{
    //    if (IsRight)
    //    {
    //        return key is Key.UpArrow or Key.DownArrow;
    //    }
    //    else
    //    {
    //        return key is Key.W or Key.S;
    //    }
    //}
}
