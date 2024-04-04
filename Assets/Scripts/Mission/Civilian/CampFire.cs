using System;
using UnityEngine;
using UnityEngine.Events;

namespace Mission.Civilian
{
    public class CampFire : MonoBehaviour
    {
        public UnityEvent<int> PutOutFire { get; private set; }

        private bool _isPutOut;
        private int _pos;

        public void Initialize(int pos)
        {
            _pos = pos;
            PutOutFire = new UnityEvent<int>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_isPutOut) return;
            if (!other.gameObject.name.Contains("FireBalloon", StringComparison.OrdinalIgnoreCase)) return;
            PutOutFire?.Invoke(_pos);
            gameObject.SetActive(false);
            Destroy(other.gameObject);
            _isPutOut = true;
        }
    }
}