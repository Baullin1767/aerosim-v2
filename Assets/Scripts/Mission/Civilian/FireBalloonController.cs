using Services;
using UnityEngine;
using UnityEngine.Events;

namespace Mission.Civilian
{
    public class FireBalloonController : MonoBehaviour
    {
        public UnityEvent<int> Drop = new();
        public int BalloonsCount { get; private set; }
        
        public void Awake()
        {
            BalloonsCount = 2;
        }

        public void RechargeBalloons(int count)
        {
            BalloonsCount = count;
        }

        private void OnDrop()
        {
            if (BalloonsCount > 0)
            {
                BalloonsCount--;
                var fireBalloonTemplate = AssetsService.Instance.GetAsset<FireBalloon>();
                var pos = transform.position - new Vector3(0f, 0.3f, 0f);
                var instance = Instantiate(fireBalloonTemplate, pos, Quaternion.identity, null);
                instance.Drop();
                Drop?.Invoke(BalloonsCount);
            }
        }
    }
}