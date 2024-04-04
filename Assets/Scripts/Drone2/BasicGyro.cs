using UnityEngine;

// Basic gyroscope simulator.  Uses the zero and identity to calculate.  This one suffre from gimball lock effect
namespace Drone2
{
    [System.Serializable]
    public class BasicGyro
    {
        // The current pitch for the given transform
        public float pitch;

        // The current roll for the given transform
        public float roll;

        // The current Yaw for the given transform
        public float yaw;

        // The current altitude from the zero position
        public float altitude;

        // Velocity vector
        public Vector3 velocityVector;

        // Velocity scalar value
        public float velocityScalar;

        public void UpdateGyro(Transform transform)
        {
            pitch = transform.eulerAngles.x;
            pitch = (pitch > 180) ? pitch - 360 : pitch;

            roll = transform.eulerAngles.z;
            roll = (roll > 180) ? roll - 360 : roll;

            yaw = transform.eulerAngles.y;
            yaw = (yaw > 180) ? yaw - 360 : yaw;

            altitude = transform.position.y;

            velocityVector = transform.GetComponent<Rigidbody>().velocity;
            velocityScalar = velocityVector.magnitude;
        }
    }
}