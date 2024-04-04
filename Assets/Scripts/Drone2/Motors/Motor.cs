using System;
using UnityEngine;

// Basic motor class.  Have to be applied to a BasicControl class.  The motor only compute its force individualy.  The force application must be done by the Rigidbody class.
namespace Drone2.Motors
{
    public class Motor : MonoBehaviour
    {
        // Total force to be applied by this motor.  This may be transfered to the parent RigidBody
        public float upForce = 0.0f;

        // Torque or side force applied by this motor.  This may be transfered to the parent RigidBody and get computed with others motors
        public float sideForce = 0.0f;

        // A power multiplier.  An easy way to create more potent motors
        public float power = 2;

        // Negative force value when Upforce gets below 0
        public float exceedForce = 0.0f;

        // A factor to be applied to the side force.  Higher values get a faster Yaw movement
        public float yawFactor = 0.0f;

        // Whether the direction of the motor is counter or counterclockwise
        public bool invertDirection;

        // A factor to be applied to the pitch correction
        public float pitchFactor = 0.0f;

        // A factor to be applied to the roll correction
        public float rollFactor = 0.0f;

        public float mass = 0.0f;

        public PropellerAngle propellerAngle;

        // Parent main controller.  Where usualy may be found the RigidBody
        public BasicControl mainController; 
        // The propeller object.  Annimation will be done here.
        public GameObject propeller; 
        private float _speedPropeller = 0;

        // Method called by BasicControl class to calculate force value of this specific motor.  The force application itself will be done at BasicControl class
        public void UpdateForceValues()
        {
            var upForceThrottle = Mathf.Clamp(mainController.throttleValue, 0, 1) * power;
            var upForceTotal = upForceThrottle;

            upForceTotal -= mainController.computer.pitchCorrection * pitchFactor;
            upForceTotal -= mainController.computer.rollCorrection * rollFactor;

            upForce = upForceTotal;

            sideForce = PreNormalize(mainController.controller.yaw, yawFactor);

            _speedPropeller = Mathf.Lerp(_speedPropeller, upForce * 2500.0f, Time.deltaTime);
            UpdatePropeller(_speedPropeller);
        }

        public void UpdatePropeller(float speed)
        {
            switch (propellerAngle)
            {
                case PropellerAngle.X:
                    propeller.transform.Rotate(0.0f, _speedPropeller * 2 * Time.deltaTime, 0.0f);
                    break;
                case PropellerAngle.Y:
                    propeller.transform.Rotate(_speedPropeller * 2 * Time.deltaTime, 0.0f, 0.0f);
                    break;
                case PropellerAngle.Z:
                    propeller.transform.Rotate(0.0f, 0.0f, _speedPropeller * 2 * Time.deltaTime);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
        }

        // Method to apply the factor and clamp the torque to its limit
        private float PreNormalize(float input, float factor)
        {
            var finalValue = input;

            finalValue = invertDirection 
                ? Mathf.Clamp(finalValue, -1, 0) 
                : Mathf.Clamp(finalValue, 0, 1);

            return finalValue * (yawFactor);
        }
    }

    public enum PropellerAngle
    {
        X,
        Y,
        Z
    }
}