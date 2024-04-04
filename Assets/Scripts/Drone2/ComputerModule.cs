using UnityEngine;

// Module reuniting computing parts of a drone.  Used by a BasicControl.
namespace Drone2
{
    public class ComputerModule : MonoBehaviour
    {
        [Header("Settings")]
        public float pitchLimit;
        public float rollLimit;

        [Header("Parts")]
        public Pid pidThrottle;

        public Pid pidPitch;
        public Pid pidRoll;
        public BasicGyro gyro;


        [Header("Values")]
        public float pitchCorrection;

        public float rollCorrection;
        public float heightCorrection;

        public void UpdateComputer(float controlPitch, float controlRoll, float controlHeight)
        {
            UpdateGyro();
            pitchCorrection = pidPitch.Update(controlPitch * pitchLimit, gyro.pitch, Time.deltaTime);
            rollCorrection = pidRoll.Update(gyro.roll, controlRoll * rollLimit, Time.deltaTime);
            heightCorrection = pidThrottle.Update(controlHeight, gyro.velocityVector.y, Time.deltaTime);
        }

        public void UpdateGyro()
        {
            gyro.UpdateGyro(transform);
        }
    }
}