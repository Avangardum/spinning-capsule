using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float spinTime;
    [SerializeField] private float speed;

    private float _rotationSpeed;
    private bool _isRotating;
    private bool _isTurningTo0;
    private float _zRotation;
    private float _ditancePassedThisSpin;

    private float DesiredZRotation => _isTurningTo0 ? -360 : -180;
    private float DistancePassedPerSpin => speed * spinTime;

    private float ZRotation
    {
        get => _zRotation;
        set
        {
            _zRotation = value;
            if (_zRotation <= -360)
            {
                _zRotation += 360;
            }
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, _zRotation);
        }
    }
    
    private void Awake()
    {
        _rotationSpeed = -180 / spinTime;
    }

    private void FixedUpdate()
    {
        if (_isRotating)
        {
            float deltaZRotationToFinishRotation = DesiredZRotation - _zRotation;
            float deltaZRotationThisFrame = _rotationSpeed * Time.fixedDeltaTime;
            if (deltaZRotationThisFrame <= deltaZRotationToFinishRotation)
            {
                ZRotation = DesiredZRotation;
                _isRotating = false;
            }
            else
            {
                ZRotation += deltaZRotationThisFrame;
            }



            float deltaX = speed * Time.deltaTime;
            float distanceLeft = DistancePassedPerSpin - _ditancePassedThisSpin;
            if (deltaX > distanceLeft)
            {
                deltaX = distanceLeft;
            }
            transform.Translate(new Vector3(deltaX, 0, 0), Space.World);
            _ditancePassedThisSpin += deltaX;
        }
    }

    private void Update()
    {
        if (!_isRotating)
        {
            if (InputManager.Instance.Tap)
            {
                _isRotating = true;
                _isTurningTo0 = !_isTurningTo0;
                _ditancePassedThisSpin = 0;
            }
        }
    }
}
