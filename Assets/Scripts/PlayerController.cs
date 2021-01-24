using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : SingletonMonoBehaviour<PlayerController>
{
    private const float groundedY = 1.1f;
    
    public float Speed;
    
    [SerializeField] private float spinTime;
    [SerializeField] private float minSpeed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float deltaSpeedPerSpin;
    [SerializeField] private float deltaSpeedPerSecond;
    
    private float _rotationSpeed;
    private bool _isRotating;
    private bool _isTurningTo0 = true;
    private bool _isJumping;
    private bool _isTopPointPassed;
    private float _zRotation;
    private float _ditancePassedThisSpin;
    private float _jumpingTopY;
    private float _verticalSpeedForThisJump;

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

        if (_isJumping)
        {
            // вертикальное движение
            if (!_isTopPointPassed)
            {
                float yMovementToTopPointLeft = _jumpingTopY - transform.position.y;
                float thisFrameYMovement = _verticalSpeedForThisJump * Time.fixedDeltaTime;
                if (thisFrameYMovement < yMovementToTopPointLeft)
                {
                    transform.Translate(Vector3.up * thisFrameYMovement, Space.World);
                }
                else
                {
                    transform.Translate(Vector3.up * yMovementToTopPointLeft + 
                                        Vector3.down * (thisFrameYMovement - yMovementToTopPointLeft), Space.World);
                    _isTopPointPassed = true;
                }
            }
            else
            {
                float yMovementToGroundLeft = groundedY - transform.position.y;
                float thisFrameYMovement = -_verticalSpeedForThisJump * Time.fixedDeltaTime;
                if (thisFrameYMovement > yMovementToGroundLeft)
                {
                    transform.Translate(Vector3.up * thisFrameYMovement, Space.World);
                }
                else
                {
                    transform.Translate(Vector3.up * yMovementToGroundLeft, Space.World);
                    _isJumping = false;
                }
            }
            
            // горизонтальное движение
            transform.Translate(Vector3.right * (Speed * Time.fixedDeltaTime), Space.World);
            
            // вращение
            ZRotation -= 180 * Time.fixedDeltaTime * _verticalSpeedForThisJump > 5 ? 3.7f : 2.85f;
            if (!_isJumping)
            {
                if (ZRotation < -160 && ZRotation > -200)
                {
                    ZRotation = -180;
                    _isTurningTo0 = false;
                }
                else
                {
                    ZRotation = 0;
                    _isTurningTo0 = true;
                }
            }
        }
        
        Speed += deltaSpeedPerSecond * Time.fixedDeltaTime;
        if (Speed < minSpeed)
        {
            Speed = minSpeed;
        }
    }

    private void Update()
    {
        if (!_isRotating && !_isJumping)
        {
            if (InputManager.Instance.Tap)
            {
                _isRotating = true;
                _isTurningTo0 = !_isTurningTo0;
                _ditancePassedThisSpin = 0;
            }

            if (InputManager.Instance.SwipeUp)
            {
                _isJumping = true;
                _verticalSpeedForThisJump = Speed;
                _jumpingTopY = groundedY + _verticalSpeedForThisJump / 2;
                _isTopPointPassed = false;
            }
        }
    }

    private float CalculateYWhileJumping(Vector2 top)
    {
        return Mathf.Pow(transform.position.x - top.x, 2) + top.y;
    }
}
