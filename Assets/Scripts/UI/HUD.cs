using System.Collections;
using Drone2;
using Mission.Tutorial;
using UI.Settings;
using UnityEngine;
using UnityEngine.UI;
using YueUltimateDronePhysics;

public class HUD : MonoBehaviour
{
    [SerializeField]
    private Text _speed;

    [SerializeField]
    private Text _time;

    [SerializeField]
    private Text _height;

    [SerializeField]
    private Transform _skyline;

    [SerializeField]
    private Image _grain;

    [SerializeField]
    private Text _flyMode;

    [SerializeField]
    private Text _laps;

    [SerializeField]
    private GameObject _fps;

    private Drone _drone;
    private DroneBridge _droneBridge;
    private TutorialMission _mission;

    private IEnumerator Start()
    {
        yield return null;
        yield return null;
        yield return null;
        yield return null;
        yield return null;
        _drone = FindObjectOfType<Drone>();
        _droneBridge = FindObjectOfType<DroneBridge>();
        _mission = FindObjectOfType<TutorialMission>();
        if (_laps != null) _laps.gameObject.SetActive(_mission != null && _mission.Laps > 0);
        SetGrain(0f);
        _fps.SetActive(GraphicsSettings.IsShowFps);
    }

    private void LateUpdate()
    {
        if (_drone != null)
        {
            _speed.text = _drone.GetSpeed();
            _time.text = GetTime();
            _height.text = _drone.GetHeight();
        }

        if (_droneBridge != null)
        {
            _speed.text = _droneBridge.GetSpeed();
            _time.text = GetTime();
            _height.text = _droneBridge.GetHeight();
            _skyline.eulerAngles = new Vector3(0f, 0f, -_droneBridge.GetDroneZRotation());
            _flyMode.text = _droneBridge.GetFlyMode();
        }

        if (_mission != null && _laps != null)
        {
            _laps.text = $"{_mission.CurrentLap}/{_mission.Laps}";
        }
    }

    private string GetTime()
    {
        var minutes = Mathf.Floor(Time.timeSinceLevelLoad / 60);
        var seconds = Mathf.Floor(Time.timeSinceLevelLoad % 60);
        return $"{minutes:00}:{seconds:00}";
    }

    public void SetGrain(float alpha)
    {
        if (_grain != null)
        {
            var color = _grain.color;
            _grain.color = new Color(color.r, color.g, color.b, alpha);
            _grain.gameObject.SetActive(alpha > 0.01f);
        }
    }

    public void SetHeightColor(Color color)
    {
        _height.color = color;
    }
}