using UnityEngine;
using UnityEngine.UI;

namespace UI.Popups.Missions
{
    public class MultiTargetGraphics : MonoBehaviour
    {
        [field: SerializeField]
        public Graphic[] TargetGraphics { get; private set; }
    }
}