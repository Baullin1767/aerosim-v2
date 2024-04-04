using System;
using UnityEngine;

namespace Drone2
{
    public class BasicControl : MonoBehaviour
    {
        [Header("Control")]
        public Controller.Controller controller;

        public float throttleIncrease;

        [Header("Motors")]
        public Drone2.Motors.Motor[] motors;

        public float throttleValue;

        [Header("Internal")]
        public ComputerModule computer;

        public Rigidbody droneRigidbody;
        
        private void Awake()
        {
            DroneSettings.SetSettingsToControl(this, droneRigidbody);
        }

        private void FixedUpdate()
        {
            computer.UpdateComputer(controller.pitch, controller.roll, controller.throttle * throttleIncrease);
            throttleValue = computer.heightCorrection;
            ComputeMotors();
            if (computer != null)
                computer.UpdateGyro();
            ComputeMotorSpeeds();
        }

        private void ComputeMotors()
        {
            var yaw = 0.0f;
            var rb = GetComponent<Rigidbody>();
            foreach (var motor in motors)
            {
                motor.UpdateForceValues();
                yaw += motor.sideForce;
                var t = motor.GetComponent<Transform>();
                rb.AddForceAtPosition(transform.up * motor.upForce, t.position, ForceMode.Impulse);
            }

            rb.AddTorque(Vector3.up * yaw, ForceMode.Force);
        }

        private void ComputeMotorSpeeds()
        {
            foreach (var motor in motors)
            {
                if (computer.gyro.altitude < 0.1)
                    motor.UpdatePropeller(0.0f);
                else
                    motor.UpdatePropeller(1200.0f);
            }
        }
    }
}