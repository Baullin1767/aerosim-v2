using UnityEngine;
using UnityEngine.UI;

namespace Mission.Tutorial
{
    public class Timer : MonoBehaviour
    {
        [SerializeField]
        private Text timerText;

        private void Awake()
        {
            gameObject.SetActive(false);
        }

        public void SetText(string text)
        {
            if (timerText != null) timerText.text = text;
        }
    }
}