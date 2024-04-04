using UnityEngine;

namespace Drone2
{
    public class CollisionController : MonoBehaviour
    {
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
                    return;
            }
        }
    }
}

