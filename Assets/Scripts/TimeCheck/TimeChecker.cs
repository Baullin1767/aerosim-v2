using UnityEngine;

namespace TimeCheck
{
    public class TimeChecker : MonoBehaviour
    {
        [SerializeField]
        private TimeConfig timeConfig;

        [SerializeField]
        private GameObject blockedWindow;

        private void Awake()
        {
            // blockedWindow.SetActive(false);
            // if (timeConfig.IsEndTime()) blockedWindow.SetActive(true);
        }
    }
}