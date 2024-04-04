using System;
using UnityEngine;

public class Motor : MonoBehaviour
{
    [SerializeField] private Transform _view;
    [SerializeField] private float _rotationSpeed = 1;
    
    private Drone _drone;

    private float _pitchRatio;
    private float _rollRatio;
    private float _yawRatio;

    private float _currentSpeed;

    public void Initialize(Drone drone)
    {
        _drone = drone;

        var xSign = Mathf.Sign(transform.localPosition.x);
        var zSign = Mathf.Sign(transform.localPosition.z);
        
        _pitchRatio = _drone.PitchRatio * zSign;
        _rollRatio = _drone.RollRatio * xSign;
        _yawRatio = _drone.YawRatio * xSign * zSign;
    }

    public void UpdateMotor()
    {
        // Debug.Log($"{_drone.Controller.Throttle} {_drone.Controller.Pitch} " +
        //           $"{_drone.Controller.Roll} {_drone.Controller.Yaw}");
        var pitch = _pitchRatio * _drone.Controller.Pitch;
        var roll = _rollRatio * _drone.Controller.Roll;
        var yaw = _yawRatio * _drone.Controller.Yaw;
        _currentSpeed = _drone.IdlePower + (_drone.Controller.Throttle + pitch + roll + yaw) * _drone.MotorPower;
        var force = _currentSpeed * Time.fixedDeltaTime;
        _drone.Rb.AddForceAtPosition(transform.up * force, transform.position, ForceMode.Impulse);
    }
    
    private void Update()
    {
        if (_view != null)
            RotateView();
    }

    private void RotateView()
    {
        _view.transform.Rotate(new Vector3(0, 0, _rotationSpeed * _currentSpeed) * Time.deltaTime);
    }
}

