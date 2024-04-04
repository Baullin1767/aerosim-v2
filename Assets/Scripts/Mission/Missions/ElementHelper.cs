using TMPro;
using UnityEngine;

namespace Mission.Missions
{
    public class ElementHelper : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI mainText;

        [SerializeField]
        private TextMeshProUGUI numberText;

        [SerializeField]
        private int number;

        [TextArea]
        [SerializeField]
        private string main;

        private void OnValidate()
        {
            mainText.text = main;
            numberText.text = number.ToString();
        }
    }
}