using System;
using Cysharp.Threading.Tasks;
using TMPro;
using UI.Settings;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class RebindAxis : MonoBehaviour
{
    [SerializeField]
    private RebindMenu _menu;

    [SerializeField]
    private InputActionReference _action;

    [SerializeField]
    private Button _button;

    [SerializeField]
    private TextMeshProUGUI _name;

    [SerializeField]
    private TextMeshProUGUI _value;

    [SerializeField]
    private Transform _progressValue;

    [SerializeField]
    private Transform _progressValuePoint;

    [SerializeField]
    private bool _isButton;
    
    [SerializeField]
    private TextMeshProUGUI _nameText;

    [SerializeField]
    private AxisList _axisList;

    [SerializeField]
    private InputList _inputList;

    [Header("Toggle")]
    [SerializeField]
    private Toggle _toggle;

    [SerializeField]
    private GameObject _bgToggle;

    public bool HasChanges => _toggleOldValue != null || _bindingPathOldValue != null;

    public InputDevice Device;
    public InputActionReference Action => _action;
    
    private InputActionRebindingExtensions.RebindingOperation _rebindingOperation;
    private bool? _toggleOldValue;
    private string _bindingPathOldValue;

    private void Start()
    {
        SetValue(0);
        _button.onClick.AddListener(StartRebinding);
        _toggle.onValueChanged.AddListener(OnToggle);
        _action.action.Enable();
        Load();
    }

    private void OnEnable()
    {
        _action.action.performed += UpdateValue;
    }

    private void OnDisable()
    {
        _action.action.performed -= UpdateValue;
        _toggleOldValue = null;
        _bindingPathOldValue = null;
    }

    private void StartRebinding()
    {
        // if (_isButton)
        // {
        //     _button.enabled = false;
        //     _name.text = "...";
        //
        //     var bindingIndex = BindingIndex;
        //     var layout = _menu.Device?.layout;
        //
        //     _action.action.Disable();
        //     _rebindingOperation = _action.action.PerformInteractiveRebinding(bindingIndex)
        //         .WithControlsExcluding("Mouse")
        //         .WithControlsExcluding("Keyboard")
        //         .WithExpectedControlType(_isButton ? "Button" : "Axis")
        //         .WithControlsHavingToMatchPath($"<{layout}>")
        //         .WithMagnitudeHavingToBeGreaterThan(0.5f)
        //         .OnMatchWaitForAnother(0.1f)
        //         .OnCancel(_ => CompleteRebind(bindingIndex))
        //         .OnComplete(_ => CompleteRebind(bindingIndex))
        //         .Start();
        //     return;
        // }
        // _action.action.performed += (context) =>
        // {
        //     var val = context.ReadValue<float>();
        //     var bindStr = "";
        //     foreach (var bind in context.action.bindings)
        //     {
        //         bindStr += bind.path + " ";
        //     }
        //     var ctrlStr = "";
        //     foreach (var bind in context.action.controls)
        //     {
        //         ctrlStr += bind.path + " ";
        //     }
        //     Debug.Log($"{context.action.name} \n {bindStr} \n{ctrlStr}");
        // };

        StartRebindingAsync().Forget();

        async UniTask StartRebindingAsync()
        {
            var path = string.Empty;
            if (_isButton)
            {
                var binding = _action.action.bindings[BindingIndex];
                _bindingPathOldValue ??= binding.overridePath;
                var inputControl = await _inputList.GetAxisControl(_nameText.text);
                if (inputControl != null)
                {
                    path = inputControl.path;
                    Debug.Log(inputControl.displayName);

                    binding = _action.action.bindings[BindingIndex];
                    binding.overridePath = path;
                    _action.action.ApplyBindingOverride(BindingIndex, binding);
                    UpdateName(BindingIndex);
                    Save();
                }
                else
                {
                    _bindingPathOldValue = null;
                }
            }
            else
            {
                var binding = _action.action.bindings[BindingIndex];
                _bindingPathOldValue ??= binding.overridePath;
                var layoutName = _isButton ? "Button" : "Axis";
                var axisControl = await _axisList.GetAxisControl(layoutName, _nameText.text);
                if (axisControl != null)
                {
                    path = axisControl.path;
                    Debug.Log(axisControl.displayName);

                    binding = _action.action.bindings[BindingIndex];
                    binding.overridePath = path;
                    _action.action.ApplyBindingOverride(BindingIndex, binding);
                    UpdateName(BindingIndex);
                    Save();
                }
                else
                {
                    _bindingPathOldValue = null;
                }
            }

            // var layoutName = _isButton ? "Button" : "Axis";
            // var axisControl = await _axisList.GetAxisControl(layoutName);
            // Debug.Log(axisControl.displayName);
        }
    }

    private void CompleteRebind(int bindingIndex)
    {
        UpdateName(bindingIndex);
        _button.enabled = true;
        _action.action.Enable();
        _rebindingOperation.Dispose();
        Save();
    }

    private void OnToggle(bool isOn)
    {
        _toggleOldValue ??= !isOn;
        _bgToggle.SetActive(isOn);
        var bindingIndex = BindingIndex;
        var binding = _action.action.bindings[bindingIndex];
        binding.overrideProcessors = isOn ? "Invert" : "";
        _action.action.ApplyBindingOverride(bindingIndex, binding);
        Save();
    }

    private void UpdateName(int bindingIndex)
    {
        if (_action == null ||
            _action.action == null ||
            bindingIndex >= _action.action.bindings.Count ||
            bindingIndex == -1) return;
        // _name.text = InputControlPath.ToHumanReadableString(
        //     _action.action.bindings[bindingIndex].effectivePath,
        //     InputControlPath.HumanReadableStringOptions.UseShortNames);
        var bind = _action.action.bindings[bindingIndex];
        var inputName = InputControlPath.ToHumanReadableString(bind.effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice);
        var buttonName = _isButton
            ? $"КНОПКА {inputName}"
            : $"ОСЬ {inputName}";
        _name.text = buttonName;
    }

    private void UpdateValue(InputAction.CallbackContext context)
    {
        var value = context.ReadValue<float>();
        SetValue(value);
    }

    private void SetValue(float value)
    {
        if (_value != null) _value.text = value.ToString("0.00");
        _progressValue.localScale = new Vector3(value, 1, 1);
        var pointValue = value == 0f ? 1f : 1 / value;
        _progressValuePoint.localScale = new Vector3(pointValue, 1, 1);
    }

    private int BindingIndex => _action.action.bindings.IndexOf(
        x => x.path.Contains("Joystick", StringComparison.OrdinalIgnoreCase) ||
             x.path.Contains("Gamepad", StringComparison.OrdinalIgnoreCase));

    private void Save() => BindPrefs.SaveBind(_action.action);

    public void Load()
    {
        BindPrefs.LoadBind(_action.action);
        var bindingIndex = BindingIndex;
        UpdateName(bindingIndex);
        if (bindingIndex == -1) return;
        _toggle.SetIsOnWithoutNotify(!string.IsNullOrEmpty(_action.action.bindings[bindingIndex].overrideProcessors));
    }

    public string GetInfo()
    {
        var binding = _action.action.bindings[BindingIndex];
        return $"{_action.action.name}\n" +
               $"{_action.action}\n" +
               $"{binding.overridePath}\n" +
               $"{binding.overrideProcessors}";
    }

    public void DropSetting()
    {
        var binding = _action.action.bindings[BindingIndex];
        if (_bindingPathOldValue != null) binding.overridePath = _bindingPathOldValue;
        if (_toggleOldValue != null)
        {
            binding.overrideProcessors = _toggleOldValue.Value ? "Invert" : "";
            _toggle.SetIsOnWithoutNotify(_toggleOldValue.Value);
            _bgToggle.SetActive(_toggleOldValue.Value);
        }

        _action.action.ApplyBindingOverride(BindingIndex, binding);
        UpdateName(BindingIndex);
        Save();
        _bindingPathOldValue = null;
        _toggleOldValue = null;
    }
}