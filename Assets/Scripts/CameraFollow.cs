using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform followedObject;
    [SerializeField] private Vector3 offset;

    private void Update()
    {
        transform.position = followedObject.transform.position + offset;
    }
}
