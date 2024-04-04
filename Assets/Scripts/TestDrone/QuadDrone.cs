using System;
using UnityEngine;

public class QuadDrone : MonoBehaviour
{
    [SerializeField]
    private Cargo _cargo;

    [Header("Properties")]
    public float IdlePower;

    public float MotorPower;
    public float PitchRatio;
    public float RollRatio;
    public float YawRatio;
    public float YawSpeed;

    [HideInInspector]
    public Rigidbody Rb;

    [HideInInspector]
    public DroneController Controller;

    private MotorPower[] _motors;

    private float Speed => Rb.velocity.magnitude;

    private void Start()
    {
        Application.targetFrameRate = 60;

        _motors = GetComponentsInChildren<MotorPower>();

        Rb = GetComponent<Rigidbody>();
        Controller = GetComponent<DroneController>();

        foreach (var motor in _motors)
            motor.Initialize(this);
    }

    private void Update()
    {
        Shader.SetGlobalVector("_Position", transform.position);
    }

    private void FixedUpdate()
    {
        foreach (var motor in _motors)
        {
            if (motor != null) motor.UpdateMotor();
        }


        // var yaw = Controller.Yaw * YawSpeed * Time.fixedDeltaTime;
        // var rotation = Rb.rotation * Quaternion.Euler(0, yaw, 0);
        // Rb.MoveRotation(rotation);
    }

    private void OnCollisionEnter(Collision other)
    {
        switch (other.gameObject.name)
        {
            case "finish":
                MissionManager.Success();
                return;
            case "buildings":
                MissionManager.Fail();
                return;
            default:
                if (Speed > 5f)
                    MissionManager.Fail();
                return;
        }
    }

    private void OnDrop()
    {
        if (_cargo != null) _cargo.Drop();
    }

    public string GetHeight()
    {
        var height = Mathf.Max(0, Mathf.RoundToInt(Rb.position.y) + 22);
        return $"{height} m";
    }

    public string GetSpeed()
    {
        var speed = Mathf.RoundToInt(Speed * 3.6f); // 3.6 is m/s to km/h
        return $"{speed} km/h";
    }
}