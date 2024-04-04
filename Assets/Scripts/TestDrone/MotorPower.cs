using UnityEngine;

public class MotorPower : MonoBehaviour
{
    [SerializeField]
    private Transform view;

    [SerializeField]
    private float rotationSpeed = 1;

    private QuadDrone _drone;

    private float _pitchRatio;
    private float _rollRatio;
    private float _yawRatio;

    private float _currentSpeed;

    public void Initialize(QuadDrone drone)
    {
        _drone = drone;

        var xSign = Mathf.Sign(transform.localPosition.x);
        var zSign = Mathf.Sign(transform.localPosition.z);

        _pitchRatio = _drone.PitchRatio * zSign;
        _rollRatio = _drone.RollRatio * xSign;
        _yawRatio = _drone.YawRatio * xSign * zSign;
    }

    public void UpdateMotor()
    {
        var xSign = Mathf.Sign(transform.localPosition.x);
        var zSign = Mathf.Sign(transform.localPosition.z);

        _pitchRatio = _drone.PitchRatio * zSign;
        _rollRatio = _drone.RollRatio * xSign;
        _yawRatio = _drone.YawRatio * xSign * zSign;
        
        var pitch = _pitchRatio * _drone.Controller.Pitch;
        var roll = _rollRatio * _drone.Controller.Roll;
        var yaw = _yawRatio * _drone.Controller.Yaw;
        _currentSpeed = _drone.IdlePower + (_drone.Controller.Throttle + pitch + roll + yaw) * _drone.MotorPower;
        var force = _currentSpeed * Time.fixedDeltaTime;
        _drone.Rb.AddForceAtPosition(transform.up * force, transform.position, ForceMode.Impulse);
        Debug.Log($"{gameObject.name} {force} {transform.up * force} {transform.position}\n" +
                  $"{_drone.Controller.Throttle} {_drone.Controller.Pitch} " +
                  $"{_drone.Controller.Roll} {_drone.Controller.Yaw}");
        //GetComponent<Rigidbody>().AddForceAtPosition(transform.up * force, transform.position, ForceMode.Impulse);
    }

    private void Update()
    {
        if (view != null)
            RotateView();
    }

    private void RotateView()
    {
        view.transform.Rotate(new Vector3(0, 0, rotationSpeed * _currentSpeed) * Time.deltaTime);
    }
}