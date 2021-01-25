using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : SingletonMonoBehaviour<InputManager>
{
    public bool Tap { get; private set; }

    public bool SwipeUp { get; private set; }

    [SerializeField] private bool useKeyboard;
    [SerializeField] private float minimalSwipeDistance;

    private Vector2 _fingerDown;
    private Vector2 _fingerUp;

    private void Update()
    {
        if (useKeyboard)
        {
            CheckKeyboardInput();
        }
        else
        {
            CheckMobileInput();
        }
    }

    private void CheckKeyboardInput()
    {
        Tap = Input.GetKeyDown(KeyCode.RightArrow);
        SwipeUp = Input.GetKeyDown(KeyCode.UpArrow);
    }

    private void CheckMobileInput()
    {
        Tap = false;
        SwipeUp = false;
        if (Input.touches.Length == 0)
        {
            return;
        }
        Touch touch = Input.touches[0];
        switch (touch.phase)
        {
            case TouchPhase.Began:
                _fingerDown = touch.position;
                break;
            case TouchPhase.Ended:
                _fingerUp = touch.position;
                Vector2 fingerMovement = _fingerUp - _fingerDown;
                if (fingerMovement.sqrMagnitude < minimalSwipeDistance * minimalSwipeDistance)
                {
                    Tap = true;
                }
                else
                {
                    if (Math.Abs(fingerMovement.y) / Math.Abs(fingerMovement.x) >= 2 &&           // если свайп вертикален и
                        fingerMovement.y > 0)                                                     // направлен вверх
                    {
                        SwipeUp = true;
                    }
                }
                break;
        }
    }
}
