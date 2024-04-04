using UnityEngine;
using UnityEngine.UI;

namespace UI.Popups.Missions
{
    [RequireComponent(typeof(MultiTargetGraphics))]
    public class MultiImageButton : Button
    {
        private Graphic[] _graphics;

        private MultiTargetGraphics _targetGraphics;

        protected override void DoStateTransition(SelectionState state, bool instant)
        {
            //get the graphics, if it could not get the graphics, return here
            if (!GetGraphics()) return;

            var targetColor =
                state == SelectionState.Disabled ? colors.disabledColor :
                state == SelectionState.Highlighted ? colors.highlightedColor :
                state == SelectionState.Normal ? colors.normalColor :
                state == SelectionState.Pressed ? colors.pressedColor :
                state == SelectionState.Selected ? colors.selectedColor : Color.white;

            foreach (var graphic in _graphics)
            {
                var duration = instant ? 0f : colors.fadeDuration;
                graphic.CrossFadeColor(targetColor, duration, true, true);
            }
        }

        private bool GetGraphics()
        {
            if (!_targetGraphics) _targetGraphics = GetComponent<MultiTargetGraphics>();
            _graphics = _targetGraphics?.TargetGraphics;
            return _graphics != null && _graphics.Length > 0;
        }
    }
}