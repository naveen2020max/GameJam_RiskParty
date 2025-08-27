using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    PlayerInputActions inputs;

    public event Action<float> Move;
    bool keyPressed = false;
    Vector2 inputVector;


    void Start()
    {
        inputs = new PlayerInputActions();
        inputs.Enable();

        inputs.Player.UPDOWN.performed += UpDownPerformed;
        inputs.Player.UPDOWN.canceled += UpDownCancelled;
    }

    private void UpDownPerformed(InputAction.CallbackContext context)
    {
        Debug.Log("Key Pressed");
        keyPressed = true;
        inputVector = context.ReadValue<Vector2>();
    }

    private void UpDownCancelled(InputAction.CallbackContext context)
    {
        keyPressed = false;
    }

    void OnDestroy()
    {
        inputs.Player.UPDOWN.performed -= UpDownPerformed;
        inputs.Player.UPDOWN.canceled -= UpDownCancelled;
    }

    void Update()
    {
        if(keyPressed)
        Move?.Invoke(inputVector.y);
    }
}
