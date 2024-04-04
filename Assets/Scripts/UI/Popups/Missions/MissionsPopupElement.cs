using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Popups.Missions
{
    public class MissionsPopupElement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField]
        private TextMeshProUGUI headerText;

        [SerializeField]
        private TextMeshProUGUI descriptionText;

        [SerializeField]
        private Button startButton;

        [SerializeField]
        private Image mainImage;

        [SerializeField]
        private Image shadowImage;

        [SerializeField]
        private Outline bgOutline;

        [SerializeField]
        private GameObject hardSticker;
        
        private MissionsPopupElementModel _model;
        private Color _bgOutlineColor;

        public string Name => _model.Header;
        public string Category => _model.Category;

        // private void Awake()
        // {
        //     var model = new MissionsPopupElementModel(
        //         "взлет и посадка оооО",
        //         "тут тупо описание",
        //         null,
        //         Color.green,
        //         null);
        //     
        //     Initialize(model);
        // }

        public void Initialize(MissionsPopupElementModel model)
        {
            _model = model;

            headerText.text = model.Header;
            descriptionText.text = model.Description;
            mainImage.sprite = model.MainSprite;
            shadowImage.color = model.MainColor;
            shadowImage.gameObject.SetActive(false);
            
            _bgOutlineColor = bgOutline.effectColor;
            
            startButton.onClick.RemoveAllListeners();
            startButton.onClick.AddListener(OnButtonClick);
            //colors

            var colors = startButton.colors;
            colors.highlightedColor = model.MainColor;
            colors.pressedColor = model.MainColor;
            startButton.colors = colors;
            
            hardSticker.SetActive(model.IsHard);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            shadowImage.gameObject.SetActive(true);
            bgOutline.effectColor = _model.MainColor;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            shadowImage.gameObject.SetActive(false);
            bgOutline.effectColor = _bgOutlineColor;
        }

        private void OnButtonClick()
        {
            _model?.ButtonCallback?.Invoke(this);
        }
    }

    public record MissionsPopupElementModel(
        string Header,
        string Description,
        string Category,
        Sprite MainSprite,
        bool IsHard,
        Color MainColor,
        UnityAction<MissionsPopupElement> ButtonCallback)
    {
        public Color MainColor { get; } = MainColor;
        public string Header { get; } = Header;
        public string Description { get; } = Description;
        public string Category { get; } = Category;
        public Sprite MainSprite { get; } = MainSprite;
        public UnityAction<MissionsPopupElement> ButtonCallback { get; } = ButtonCallback;
        public bool IsHard { get; } = IsHard;
    }
}