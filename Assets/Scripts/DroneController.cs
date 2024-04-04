using UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class DroneController : MonoBehaviour
{
    public float Throttle { private set; get; }
    public float Pitch { private set; get; }
    public float Yaw { private set; get; }
    public float Roll { private set; get; }

    private void Start()
    {
        var input = GetComponent<PlayerInput>();
        foreach (var action in input.actions)
            BindPrefs.LoadBind(action);
    }

    private void OnThrottle(InputValue value)
    {
        Throttle = (value.Get<float>() + 1) / 2;
    }

    private void OnPitch(InputValue value)
    {
        Pitch = value.Get<float>();
    }

    private void OnYaw(InputValue value)
    {
        Yaw = value.Get<float>();
    }

    private void OnRoll(InputValue value)
    {
        Roll = value.Get<float>();
    }

    private void OnDrop()
    {
        Debug.Log("Drop");
    }

    private void OnRestart()
    {
        var gameMenu = FindObjectOfType<GameMenu>(true);
        gameMenu.Open();
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}