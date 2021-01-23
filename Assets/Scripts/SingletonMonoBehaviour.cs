using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T>
{
    public static T Instance { get; private set; }
    public bool HasInstance { get; private set; }

    private void OnEnable()
    {
        if (!HasInstance)
        {
            Instance = (T)this;
            HasInstance = true;
        }
        else
        {
            Debug.LogError("Created more than one " + typeof(T));
        }
    }

    private void OnDisable()
    {
        Instance = null;
        HasInstance = false;
    }
}
