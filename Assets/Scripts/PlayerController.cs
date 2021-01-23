using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : SingletonMonoBehaviour<PlayerController>
{
    public float Speed;
    
    [SerializeField] private float spinTime;
    [SerializeField] private float minSpeed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float deltaSpeedPerSpin;
    [SerializeField] private float deltaSpeedPerSecond;
    
    private float _rotationSpeed;
    private bool _isRotating;
    private bool _isTurningTo0 = true;
    private float _zRotation;
    private float _ditancePassedThisSpin;

    private float DesiredZRotation => _isTurningTo0 ? -360 : -180;
    private float DistancePassedPerSpin => Speed * spinTime;

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
        Speed = minSpeed;
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
                Speed += deltaSpeedPerSpin;
                if (Speed > maxSpeed)
                {
                    Speed = maxSpeed;
                }
            }
            else
            {
                ZRotation += deltaZRotationThisFrame;
            }


            float deltaX = Speed * Time.deltaTime;
            float distanceLeft = DistancePassedPerSpin - _ditancePassedThisSpin;
            if (deltaX > distanceLeft)
            {
                deltaX = distanceLeft;
            }
            transform.Translate(new Vector3(deltaX, 0, 0), Space.World);
            _ditancePassedThisSpin += deltaX;
        }
        
        
        Speed += deltaSpeedPerSecond * Time.fixedDeltaTime;
        if (Speed < minSpeed)
        {
            Speed = minSpeed;
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
