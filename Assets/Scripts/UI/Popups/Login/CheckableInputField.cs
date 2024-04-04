using System;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Popups.Login
{
    public class CheckableInputField : MonoBehaviour
    {
        [SerializeField]
        private TMP_InputField inputField;

        [SerializeField]
        private TextMeshProUGUI nameText;

        [Header("Colors")]
        [Header("Default")]
        [SerializeField]
        private ColorBlock defaultColors;

        [Header("Good")]
        [SerializeField]
        private ColorBlock goodColors;

        [Header("Wrong")]
        [SerializeField]
        private ColorBlock wrongColors;

        [Header("Button eye")]
        [SerializeField]
        private Button showButton;

        [SerializeField]
        private GameObject onState;

        [SerializeField]
        private GameObject offState;

        [SerializeReference, SubclassSelector]
        private InputValidation validation;

        private string _defaultName;

        public bool IsValid { get; private set; }
        public string Text => inputField.text;

        public void SetError(string errorText)
        {
            nameText.text = $"{_defaultName} ({errorText})";
        }

        private void Awake()
        {
            if (showButton != null)
            {
                showButton.onClick.RemoveAllListeners();
                showButton.onClick.AddListener(OnShowButton);

                var isPasswordType = inputField.contentType == TMP_InputField.ContentType.Standard;
                onState.SetActive(!isPasswordType);
                offState.SetActive(isPasswordType);
                inputField.ForceLabelUpdate();
            }

            inputField.onValueChanged.RemoveAllListeners();
            inputField.onValueChanged.AddListener(OnValueChange);

            _defaultName = nameText.text;
        }

        private void OnShowButton()
        {
            bool isPasswordType;
            if (inputField.contentType == TMP_InputField.ContentType.Standard)
            {
                isPasswordType = false;
                inputField.contentType = TMP_InputField.ContentType.Password;
            }
            else
            {
                isPasswordType = true;
                inputField.contentType = TMP_InputField.ContentType.Standard;
            }

            onState.SetActive(!isPasswordType);
            offState.SetActive(isPasswordType);

            inputField.ForceLabelUpdate();
        }

        private void ChangeInputColor(ColorBlock newColors)
        {
            inputField.colors = newColors;
        }

        private void OnValueChange(string text)
        {
            if (validation == null)
            {
                IsValid = true;
                return;
            }

            IsValid = validation.IsValid(text);
            var newColor = IsValid ? goodColors : wrongColors;
            ChangeInputColor(newColor);
        }

        private void OnValidate()
        {
            if (inputField != null)
            {
                ChangeInputColor(defaultColors);
            }
        }
    }

    [Serializable]
    public abstract class InputValidation
    {
        public abstract bool IsValid(string text);
    }

    [Serializable]
    public class NickNameValidation : InputValidation
    {
        public const string NickNamePattern = @"^\s*[a-zA-Z\d]{7,16}$";

        public override bool IsValid(string text) => Regex.IsMatch(text, NickNamePattern);
    }

    [Serializable]
    public class PasswordValidation : InputValidation
    {
        public const string PasswordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{8,}$";

        [SerializeField]
        private CheckableInputField secondField;

        public override bool IsValid(string text)
        {
            var patternValid = Regex.IsMatch(text, PasswordPattern);
            var secondFieldSame = IsSecondFieldSame(text);
            return patternValid && secondFieldSame;
        }

        private bool IsSecondFieldSame(string text)
        {
            if (secondField == null) return false;
            return secondField.Text == text;
        }
    }

    [Serializable]
    public class EmailValidation : InputValidation
    {
        public const string MatchEmailPattern =
            @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
            + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
            + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
            + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$";

        public override bool IsValid(string text) => Regex.IsMatch(text, MatchEmailPattern);
    }
}