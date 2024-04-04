using UnityEngine.Events;
using UnityEngine.UI;

namespace Extensions
{
    public static class CommonExtensions
    {
        public static void SetClickListener(this Button button, UnityAction callback)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(callback);
        }
    }
}
