using System;
using Unity.Mathematics;
using UnityEngine;

public class HandController : MonoBehaviour
{
    public Vector3 clampAreaMin = new Vector2(-1.2f, -.6f), clampAreaMax = new Vector2(1.2f, .6f);
    public Transform target;
    public float speed = 20;
    public float lerpSpeed = .2f;

    Vector3 input;
    float inputX, inputY;
    bool rightClicked;
    bool cursorEnabled = true;
    Vector3 calculatedPosition;
    RaycastHit hit;

    GameObject currentHeldObject;

    void Start()
    {
        ToggleCursor();
    }

    void Update()
    {
        inputX = Input.GetAxis("Mouse X");
        inputY = Input.GetAxis("Mouse Y");

        rightClicked = Input.GetMouseButton(1);

        input.x = inputX;
        input.y = rightClicked ? inputY : 0;
        input.z = rightClicked ? 0 : inputY;

        calculatedPosition = target.position;
        calculatedPosition += input * speed * Time.deltaTime;
        calculatedPosition.x = math.clamp(calculatedPosition.x, clampAreaMin.x, clampAreaMax.x);
        calculatedPosition.y = math.clamp(calculatedPosition.y, clampAreaMin.y, clampAreaMax.y);
        calculatedPosition.z = math.clamp(calculatedPosition.z, clampAreaMin.z, clampAreaMax.z);

        target.position = Vector3.Lerp(target.position, calculatedPosition, lerpSpeed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Tab)) ToggleCursor();
    }

    void ToggleCursor()
    {
        cursorEnabled = !cursorEnabled;

        Cursor.lockState = cursorEnabled ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = cursorEnabled;
    }
}
