using System;
using UnityEngine;
using UnityEngine.Events;

namespace Mission
{
    public class StopTrigger : MonoBehaviour
    {
        public UnityEvent<bool> trigger = new ();
        
        private void OnTriggerEnter(Collider other)
        {
            if (!other.name.Contains("drone", StringComparison.OrdinalIgnoreCase)) return;
            trigger?.Invoke(true);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.name.Contains("drone", StringComparison.OrdinalIgnoreCase)) return;
            trigger?.Invoke(false);
        }
    }
}