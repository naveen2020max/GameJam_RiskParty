using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class PlayerInputHandler : MonoBehaviour
{
    PlayerInputActions inputs;

    public event Action<KeyInput> Move;
    bool keyPressed = false;
    Vector2 inputVector;
    KeyInput inputContext;

    [Header("Debug")]
    [SerializeField] private bool isRight;

    void Start()
    {
        inputs = new PlayerInputActions();
        inputs.Enable();

        InputAction inputAction = GetInputActionByPriority();

        inputAction.performed += UpDownPerformed;
        inputAction.canceled += UpDownCancelled;

        GameManager.Instance.OnPlayerJoin(this);
    }

    private void UpDownPerformed(InputAction.CallbackContext context)
    {
        keyPressed = true;
        float inputValue = context.ReadValue<Vector2>().y;
        Key keyCode = (context.control as KeyControl).keyCode;
        inputContext = new KeyInput(inputValue, keyCode);
    }

    private void UpDownCancelled(InputAction.CallbackContext context)
    {
        keyPressed = false;
    }

    void OnDestroy()
    {
        GameManager.Instance.OnPlayerDisconnect(this);

        if (inputs != null)
        {
            InputAction inputAction = GetInputActionByPriority();
            inputAction.performed -= UpDownPerformed;
            inputAction.canceled -= UpDownCancelled;
        }
    }

    void Update()
    {
        if (keyPressed)
            Move?.Invoke(inputContext);
    }

    public struct KeyInput
    {
        public float InputValue;
        public Key PressedKeyType;

        public KeyInput(float value, Key key)
        {
            InputValue = value;
            PressedKeyType = key;
        }
    }

    InputAction GetInputActionByPriority()
    {
        return isRight ? inputs.Player2.UPDOWN : inputs.Player.UPDOWN;
    }
}
