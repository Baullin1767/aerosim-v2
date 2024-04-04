using UnityEngine;

namespace Drone2
{
    public class KamikadzeController : MonoBehaviour
    {
        [HideInInspector]
        public bool IsActive;
        
        private void OnCollisionEnter(Collision other)
        {
            if (!IsActive)
            {
                return;
            }
            
            switch (other.gameObject.name)
            {
                case "finish":
                    MissionManager.Success();
                    return;
                case "buildings":
                    MissionManager.Fail();
                    return;
            }
        }
    }
}
