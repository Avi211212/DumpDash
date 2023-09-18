using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class CharacterRotation : MonoBehaviour
{
    public float rotationSpeed = 360f;

    private float initialRotation;
    private Vector3 initialMousePosition;

    private bool isMouseDown = false; 

    private void Update()
    {
        RotateCharacter();
    }

    private void RotateCharacter()
    {
        if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
        {
            initialMousePosition = GetInputPosition();
        }
        else if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved))
        {
            Vector2 touchPosition = GetInputPosition();

            float deltaTouchPosition = touchPosition.x - initialMousePosition.x;

            transform.rotation = Quaternion.Euler(0, -deltaTouchPosition * rotationSpeed, 0) * transform.rotation;
        }

#if UNITY_EDITOR
        
        if (Input.GetMouseButtonDown(0))
        {
            isMouseDown = true;
            initialMousePosition = GetInputPosition();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isMouseDown = false;
        }

        if(isMouseDown)
        {
            Vector2 mousePosition = GetInputPosition();

            float deltaMousePosition = mousePosition.x - initialMousePosition.x;

            transform.rotation = Quaternion.Euler(0, -deltaMousePosition * rotationSpeed, 0) * transform.rotation;
        }

#endif
    }

    private Vector2 GetInputPosition()
    {
        if (Input.touchCount > 0)
        {
            return Input.GetTouch(0).position;
        }
        else
        {
            return Input.mousePosition;
        }
    }

}
