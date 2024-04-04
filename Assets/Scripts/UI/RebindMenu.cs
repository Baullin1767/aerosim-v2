using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class RebindMenu : MonoBehaviour
{
    [SerializeField]
    private Button _deleteButton;

    [SerializeField]
    private Text _device;

    [SerializeField]
    private InputField _log;

    [SerializeField]
    private Button _logButton;

    [SerializeField]
    private Button _copyButton;

    [SerializeField]
    private RebindAxis[] _axis;

    [SerializeField]
    private GameObject axisList;

    [SerializeField]
    private GameObject graphicsSettings;

    [SerializeField]
    private GameObject inputSettings;

    [SerializeField]
    private GameObject droneSettings;

    [SerializeField]
    private GameObject acceptMenu;

    public InputDevice Device;

    private void Awake()
    {
        _deleteButton.onClick.AddListener(DeleteAll);
        _logButton.onClick.AddListener(RefreshLog);
        _copyButton.onClick.AddListener(CopyLog);

        Device = InputSystem.devices
            .Where(d => d is Joystick)
            .FirstOrDefault();

        _device.text = Device?.displayName ?? string.Empty;

        RefreshLog();
    }

    private void OnEnable()
    {
        axisList.SetActive(false);
        graphicsSettings.SetActive(false);
        droneSettings.SetActive(false);
        acceptMenu.SetActive(false);

        inputSettings.SetActive(true);
    }

    private void DeleteAll()
    {
        BindPrefs.DeleteAll();
        foreach (var axis in _axis)
            axis.Load();
    }

    private void RefreshLog()
    {
        _log.text = GetDeviceInfo();
        foreach (var axis in _axis)
            _log.text += "\n==========\n" + axis.GetInfo();
    }

    private void CopyLog()
    {
        GUIUtility.systemCopyBuffer = _log.text;
    }

    private string GetDeviceInfo()
    {
        if (Device is null)
            return "<>";

        var result = Device.name;

        foreach (var control in Device.allControls)
        {
            result += $"\n{control.layout}:{control.path}={control.ReadValueAsObject()}";
        }

        return result;
    }
}