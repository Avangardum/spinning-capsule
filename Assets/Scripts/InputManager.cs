using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : SingletonMonoBehaviour<InputManager>
{
    public bool Tap { get; private set; }

    public bool SwipeUp { get; private set; }

    [SerializeField] private bool useKeyboard;

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
        throw new NotImplementedException();
    }
}
