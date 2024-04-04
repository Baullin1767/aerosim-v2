using System;
using Drone2;
using UnityEngine;

namespace Mission.Civilian
{
    public class FireBalloon : MonoBehaviour
    {
        private Rigidbody _rb;
        private Rigidbody _droneRigidbody;

        private bool _isInitialized;

        private void Initialize()
        {
            _droneRigidbody = DroneBridge.Instance.GetComponent<Rigidbody>();
            _rb = GetComponent<Rigidbody>();
            _rb.isKinematic = true;

            _isInitialized = true;
        }

        private void Update()
        {
            if (!_rb.isKinematic)
            {
                var newVelocity = new Vector3(
                    _droneRigidbody.velocity.x,
                    _rb.velocity.y,
                    _droneRigidbody.velocity.z);
                _rb.velocity = newVelocity;
            }
        }

        public void Drop()
        {
            if (!_isInitialized) Initialize();
            transform.parent = null;
            _rb.isKinematic = false;
            _rb.WakeUp();
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.name.Contains("Drone", StringComparison.OrdinalIgnoreCase)) return;
            Destroy(gameObject);
        }
    }
}