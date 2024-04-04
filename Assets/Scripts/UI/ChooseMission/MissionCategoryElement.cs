using UnityEngine;
using UnityEngine.UI;

namespace UI.ChooseMission
{
    public class MissionCategoryElement : MonoBehaviour
    {
        [SerializeField]
        private Text categoryText;

        public void Initialize(string categoryName)
        {
            categoryText.text = categoryName;
        }
    }
}