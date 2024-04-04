using Drone2;
using UnityEngine;
using UnityEngine.UI;

public class DroneSettings : MonoBehaviour
{
    [Header("Physics")]
    [SerializeField]
    private InputField rigidbodyMassInputField;

    [SerializeField]
    private InputField rigidbodyDragInputField;

    [SerializeField]
    private InputField rigidbodyAngularDragInputField;

    [Header("Controller")]
    [SerializeField]
    private InputField pitchLimitInputField;

    [SerializeField]
    private InputField rollLimitInputField;

    [SerializeField]
    private InputField pidThrottlePInputField;

    [SerializeField]
    private InputField pidThrottleIInputField;

    [SerializeField]
    private InputField pidThrottleDInputField;

    [SerializeField]
    private InputField pidPitchPInputField;

    [SerializeField]
    private InputField pidPitchIInputField;

    [SerializeField]
    private InputField pidPitchDInputField;

    [SerializeField]
    private InputField pidRollPInputField;

    [SerializeField]
    private InputField pidRollIInputField;

    [SerializeField]
    private InputField pidRollDInputField;

    [Header("Motors")]
    [Header("FL")]
    [SerializeField]
    private InputField upForceMflInputField;

    [SerializeField]
    private InputField sideForceMflInputField;

    [SerializeField]
    private InputField powerInputMflField;

    [SerializeField]
    private InputField yawFactorMflInputField;

    [SerializeField]
    private InputField pitchFactorMflInputField;
    
    [SerializeField]
    private InputField rollFactorMflInputField;

    [SerializeField]
    private InputField invertDirectionMflInputField;

    [Header("FR")]
    [SerializeField]
    private InputField upForceMfrInputField;

    [SerializeField]
    private InputField sideForceMfrInputField;

    [SerializeField]
    private InputField powerInputMfrField;

    [SerializeField]
    private InputField yawFactorMfrInputField;

    [SerializeField]
    private InputField pitchFactorMfrInputField;
    
    [SerializeField]
    private InputField rollFactorMfrInputField;

    [SerializeField]
    private InputField invertDirectionMfrInputField;
    

    [Header("RR")]
    [SerializeField]
    private InputField upForceMrrInputField;

    [SerializeField]
    private InputField sideForceMrrInputField;

    [SerializeField]
    private InputField powerInputMrrField;

    [SerializeField]
    private InputField yawFactorMrrInputField;

    [SerializeField]
    private InputField pitchFactorMrrInputField;
    
    [SerializeField]
    private InputField rollFactorMrrInputField;

    [SerializeField]
    private InputField invertDirectionMrrInputField;

    [Header("RL")]
    [SerializeField]
    private InputField upForceMrlInputField;

    [SerializeField]
    private InputField sideForceMrlInputField;

    [SerializeField]
    private InputField powerInputMrlField;

    [SerializeField]
    private InputField yawFactorMrlInputField;

    [SerializeField]
    private InputField pitchFactorMrlInputField;
    
    [SerializeField]
    private InputField rollFactorMrlInputField;

    [SerializeField]
    private InputField invertDirectionMrlInputField;

    [Header("Drone")]
    [SerializeField]
    private BasicControl basicDroneControl;

    [SerializeField]
    private Rigidbody droneRigidbody;

    private const string FirstLoadStr = "first_drone_load";
    
    private void Awake()
    {
        if (!PlayerPrefs.HasKey(FirstLoadStr))
        {
            SetDefaultValues();
            PlayerPrefs.SetInt(FirstLoadStr, 1);
            PlayerPrefs.Save();
        }

        LoadValues();
    }

    public void SaveValues()
    {
        var prefix = "_drone_";

        // rigidbody
        var rigidbodyMass = $"{prefix}rigidbody.mass";
        var floatValue = float.Parse(rigidbodyMassInputField.text);
        SetValue(rigidbodyMass, floatValue);

        var rigidbodyDrag = $"{prefix}rigidbody.drag";
        floatValue = float.Parse(rigidbodyDragInputField.text);
        SetValue(rigidbodyDrag, floatValue);

        var rigidbodyAngularDrag = $"{prefix}rigidbody.angularDrag";
        floatValue = float.Parse(rigidbodyAngularDragInputField.text);
        SetValue(rigidbodyAngularDrag, floatValue);

        //computer
        var pitchLimitName = $"{prefix}basicControl.computer.pitchLimit";
        floatValue = float.Parse(pitchLimitInputField.text);
        SetValue(pitchLimitName, floatValue);

        var rollLimitName = $"{prefix}basicControl.computer.rollLimit";
        floatValue = float.Parse(rollLimitInputField.text);
        SetValue(rollLimitName, floatValue);

        //pid Throttle
        var pidThrottlePFactorName = $"{prefix}basicControl.computer.pidThrottle.pFactor";
        floatValue = float.Parse(pidThrottlePInputField.text);
        SetValue(pidThrottlePFactorName, floatValue);

        var pidThrottleIFactorName = $"{prefix}basicControl.computer.pidThrottle.iFactor";
        floatValue = float.Parse(pidThrottleIInputField.text);
        SetValue(pidThrottleIFactorName, floatValue);

        var pidThrottleDFactorName = $"{prefix}basicControl.computer.pidThrottle.dFactor";
        floatValue = float.Parse(pidThrottleDInputField.text);
        SetValue(pidThrottleDFactorName, floatValue);

        //pid Pitch
        var pidPitchPFactorName = $"{prefix}basicControl.computer.pidPitch.pFactor";
        floatValue = float.Parse(pidPitchPInputField.text);
        SetValue(pidPitchPFactorName, floatValue);

        var pidPitchIFactorName = $"{prefix}basicControl.computer.pidPitch.iFactor";
        floatValue = float.Parse(pidPitchIInputField.text);
        SetValue(pidPitchIFactorName, floatValue);

        var pidPitchDFactorName = $"{prefix}basicControl.computer.pidPitch.dFactor";
        floatValue = float.Parse(pidPitchDInputField.text);
        SetValue(pidPitchDFactorName, floatValue);

        //pid Roll
        var pidRollPFactorName = $"{prefix}basicControl.computer.pidRoll.pFactor";
        floatValue = float.Parse(pidRollPInputField.text);
        SetValue(pidRollPFactorName, floatValue);

        var pidRollIFactorName = $"{prefix}basicControl.computer.pidRoll.iFactor";
        floatValue = float.Parse(pidRollIInputField.text);
        SetValue(pidRollIFactorName, floatValue);

        var pidRollDFactorName = $"{prefix}basicControl.computer.pidRoll.dFactor";
        floatValue = float.Parse(pidRollDInputField.text);
        SetValue(pidRollDFactorName, floatValue);

        //motor
        //FL
        prefix = "_drone_motor_fl";
        var upForceName = $"{prefix}motor.upForce";
        floatValue = float.Parse(upForceMflInputField.text);
        SetValue(upForceName, floatValue);

        var sideForceName = $"{prefix}motor.sideForce";
        floatValue = float.Parse(sideForceMflInputField.text);
        SetValue(sideForceName, floatValue);

        var powerName = $"{prefix}motor.power";
        floatValue = float.Parse(powerInputMflField.text);
        SetValue(powerName, floatValue);

        var yawFactorName = $"{prefix}motor.yawFactor";
        floatValue = float.Parse(yawFactorMflInputField.text);
        SetValue(yawFactorName, floatValue);

        var pitchFactorName = $"{prefix}motor.pitchFactor";
        floatValue = float.Parse(pitchFactorMflInputField.text);
        SetValue(pitchFactorName, floatValue);

        var invertDirectionName = $"{prefix}motor.invertDirection";
        floatValue = float.Parse(invertDirectionMflInputField.text);
        SetValue(invertDirectionName, floatValue);

        //FR
        prefix = "_drone_motor_fr";
        upForceName = $"{prefix}motor.upForce";
        floatValue = float.Parse(upForceMfrInputField.text);
        SetValue(upForceName, floatValue);

        sideForceName = $"{prefix}motor.sideForce";
        floatValue = float.Parse(sideForceMfrInputField.text);
        SetValue(sideForceName, floatValue);

        powerName = $"{prefix}motor.power";
        floatValue = float.Parse(powerInputMfrField.text);
        SetValue(powerName, floatValue);

        yawFactorName = $"{prefix}motor.yawFactor";
        floatValue = float.Parse(yawFactorMfrInputField.text);
        SetValue(yawFactorName, floatValue);

        pitchFactorName = $"{prefix}motor.pitchFactor";
        floatValue = float.Parse(pitchFactorMfrInputField.text);
        SetValue(pitchFactorName, floatValue);

        invertDirectionName = $"{prefix}motor.invertDirection";
        floatValue = float.Parse(invertDirectionMfrInputField.text);
        SetValue(invertDirectionName, floatValue);

        //RR
        prefix = "_drone_motor_rr";
        upForceName = $"{prefix}motor.upForce";
        floatValue = float.Parse(upForceMrrInputField.text);
        SetValue(upForceName, floatValue);

        sideForceName = $"{prefix}motor.sideForce";
        floatValue = float.Parse(sideForceMrrInputField.text);
        SetValue(sideForceName, floatValue);

        powerName = $"{prefix}motor.power";
        floatValue = float.Parse(powerInputMrrField.text);
        SetValue(powerName, floatValue);

        yawFactorName = $"{prefix}motor.yawFactor";
        floatValue = float.Parse(yawFactorMrrInputField.text);
        SetValue(yawFactorName, floatValue);

        pitchFactorName = $"{prefix}motor.pitchFactor";
        floatValue = float.Parse(pitchFactorMrrInputField.text);
        SetValue(pitchFactorName, floatValue);

        invertDirectionName = $"{prefix}motor.invertDirection";
        floatValue = float.Parse(invertDirectionMrrInputField.text);
        SetValue(invertDirectionName, floatValue);

        //RL
        prefix = "_drone_motor_rl";
        upForceName = $"{prefix}motor.upForce";
        floatValue = float.Parse(upForceMrlInputField.text);
        SetValue(upForceName, floatValue);

        sideForceName = $"{prefix}motor.sideForce";
        floatValue = float.Parse(sideForceMrlInputField.text);
        SetValue(sideForceName, floatValue);

        powerName = $"{prefix}motor.power";
        floatValue = float.Parse(powerInputMrlField.text);
        SetValue(powerName, floatValue);

        yawFactorName = $"{prefix}motor.yawFactor";
        floatValue = float.Parse(yawFactorMrlInputField.text);
        SetValue(yawFactorName, floatValue);

        pitchFactorName = $"{prefix}motor.pitchFactor";
        floatValue = float.Parse(pitchFactorMrlInputField.text);
        SetValue(pitchFactorName, floatValue);

        invertDirectionName = $"{prefix}motor.invertDirection";
        floatValue = float.Parse(invertDirectionMrlInputField.text);
        SetValue(invertDirectionName, floatValue);
        
        PlayerPrefs.Save();
    }

    public void LoadValues()
    {
        var prefix = "_drone_";

        // rigidbody
        var rigidbodyMass = $"{prefix}rigidbody.mass";
        rigidbodyMassInputField.text = GetValue(rigidbodyMass).ToString();

        var rigidbodyDrag = $"{prefix}rigidbody.drag";
        rigidbodyDragInputField.text = GetValue(rigidbodyDrag).ToString();

        var rigidbodyAngularDrag = $"{prefix}rigidbody.angularDrag";
        rigidbodyAngularDragInputField.text = GetValue(rigidbodyAngularDrag).ToString();

        //computer
        var pitchLimitName = $"{prefix}basicControl.computer.pitchLimit";
        pitchLimitInputField.text = GetValue(pitchLimitName).ToString();

        var rollLimitName = $"{prefix}basicControl.computer.rollLimit";
        rollLimitInputField.text = GetValue(rollLimitName).ToString();

        //pid Throttle
        var pidThrottlePFactorName = $"{prefix}basicControl.computer.pidThrottle.pFactor";
        pidThrottlePInputField.text = GetValue(pidThrottlePFactorName).ToString();

        var pidThrottleIFactorName = $"{prefix}basicControl.computer.pidThrottle.iFactor";
        pidThrottleIInputField.text = GetValue(pidThrottleIFactorName).ToString();

        var pidThrottleDFactorName = $"{prefix}basicControl.computer.pidThrottle.dFactor";
        pidThrottleDInputField.text = GetValue(pidThrottleDFactorName).ToString();

        //pid Pitch
        var pidPitchPFactorName = $"{prefix}basicControl.computer.pidPitch.pFactor";
        pidPitchPInputField.text = GetValue(pidPitchPFactorName).ToString();

        var pidPitchIFactorName = $"{prefix}basicControl.computer.pidPitch.iFactor";
        pidPitchIInputField.text = GetValue(pidPitchIFactorName).ToString();

        var pidPitchDFactorName = $"{prefix}basicControl.computer.pidPitch.dFactor";
        pidPitchDInputField.text = GetValue(pidPitchDFactorName).ToString();

        //pid Roll
        var pidRollPFactorName = $"{prefix}basicControl.computer.pidRoll.pFactor";
        pidRollPInputField.text = GetValue(pidRollPFactorName).ToString();

        var pidRollIFactorName = $"{prefix}basicControl.computer.pidRoll.iFactor";
        pidRollIInputField.text = GetValue(pidRollIFactorName).ToString();

        var pidRollDFactorName = $"{prefix}basicControl.computer.pidRoll.dFactor";
        pidRollDInputField.text = GetValue(pidRollDFactorName).ToString();

        //motor
        //FL
        prefix = "_drone_motor_fl";
        var upForceName = $"{prefix}motor.upForce";
        upForceMflInputField.text = GetValue(upForceName).ToString();

        var sideForceName = $"{prefix}motor.sideForce";
        sideForceMflInputField.text = GetValue(sideForceName).ToString();

        var powerName = $"{prefix}motor.power";
        powerInputMflField.text = GetValue(powerName).ToString();

        var yawFactorName = $"{prefix}motor.yawFactor";
        yawFactorMflInputField.text = GetValue(yawFactorName).ToString();

        var pitchFactorName = $"{prefix}motor.pitchFactor";
        pitchFactorMflInputField.text = GetValue(pitchFactorName).ToString();
        
        var rollFactorName = $"{prefix}motor.rollFactor";
        rollFactorMflInputField.text = GetValue(rollFactorName).ToString();

        var invertDirectionName = $"{prefix}motor.invertDirection";
        invertDirectionMflInputField.text = GetValue(invertDirectionName).ToString();

        //FR
        prefix = "_drone_motor_fr";
        upForceName = $"{prefix}motor.upForce";
        upForceMfrInputField.text = GetValue(upForceName).ToString();

        sideForceName = $"{prefix}motor.sideForce";
        sideForceMfrInputField.text = GetValue(sideForceName).ToString();

        powerName = $"{prefix}motor.power";
        powerInputMfrField.text = GetValue(powerName).ToString();

        yawFactorName = $"{prefix}motor.yawFactor";
        yawFactorMfrInputField.text = GetValue(yawFactorName).ToString();

        pitchFactorName = $"{prefix}motor.pitchFactor";
        pitchFactorMfrInputField.text = GetValue(pitchFactorName).ToString();
        
        rollFactorName = $"{prefix}motor.rollFactor";
        rollFactorMfrInputField.text = GetValue(rollFactorName).ToString();

        invertDirectionName = $"{prefix}motor.invertDirection";
        invertDirectionMfrInputField.text = GetValue(invertDirectionName).ToString();

        //RR
        prefix = "_drone_motor_rr";
        upForceName = $"{prefix}motor.upForce";
        upForceMrrInputField.text = GetValue(upForceName).ToString();

        sideForceName = $"{prefix}motor.sideForce";
        sideForceMrrInputField.text = GetValue(sideForceName).ToString();

        powerName = $"{prefix}motor.power";
        powerInputMrrField.text = GetValue(powerName).ToString();

        yawFactorName = $"{prefix}motor.yawFactor";
        yawFactorMrrInputField.text = GetValue(yawFactorName).ToString();

        pitchFactorName = $"{prefix}motor.pitchFactor";
        pitchFactorMrrInputField.text = GetValue(pitchFactorName).ToString();
        
        rollFactorName = $"{prefix}motor.rollFactor";
        rollFactorMrrInputField.text = GetValue(rollFactorName).ToString();

        invertDirectionName = $"{prefix}motor.invertDirection";
        invertDirectionMrrInputField.text = GetValue(invertDirectionName).ToString();

        //RL
        prefix = "_drone_motor_rl";
        upForceName = $"{prefix}motor.upForce";
        upForceMrlInputField.text = GetValue(upForceName).ToString();

        sideForceName = $"{prefix}motor.sideForce";
        sideForceMrlInputField.text = GetValue(sideForceName).ToString();

        powerName = $"{prefix}motor.power";
        powerInputMrlField.text = GetValue(powerName).ToString();

        yawFactorName = $"{prefix}motor.yawFactor";
        yawFactorMrlInputField.text = GetValue(yawFactorName).ToString();

        pitchFactorName = $"{prefix}motor.pitchFactor";
        pitchFactorMrlInputField.text = GetValue(pitchFactorName).ToString();
        
        pitchFactorName = $"{prefix}motor.pitchFactor";
        pitchFactorMrlInputField.text = GetValue(pitchFactorName).ToString();
        
        rollFactorName = $"{prefix}motor.rollFactor";
        rollFactorMrlInputField.text = GetValue(rollFactorName).ToString();

        invertDirectionName = $"{prefix}motor.invertDirection";
        invertDirectionMrlInputField.text = GetValue(invertDirectionName).ToString();
    }

    public void SetDefaultValues()
    {
        var prefix = "_drone_";

        // rigidbody
        var rigidbodyMass = $"{prefix}rigidbody.mass";
        SetValue(rigidbodyMass, droneRigidbody.mass);

        var rigidbodyDrag = $"{prefix}rigidbody.drag";
        SetValue(rigidbodyDrag, droneRigidbody.drag);

        var rigidbodyAngularDrag = $"{prefix}rigidbody.angularDrag";
        SetValue(rigidbodyAngularDrag, droneRigidbody.angularDrag);

        //computer
        var pitchLimitName = $"{prefix}basicControl.computer.pitchLimit";
        SetValue(pitchLimitName, basicDroneControl.computer.pitchLimit);

        var rollLimitName = $"{prefix}basicControl.computer.rollLimit";
        SetValue(rollLimitName, basicDroneControl.computer.rollLimit);

        //pid Throttle
        var pidThrottlePFactorName = $"{prefix}basicControl.computer.pidThrottle.pFactor";
        SetValue(pidThrottlePFactorName, basicDroneControl.computer.pidThrottle.pFactor);

        var pidThrottleIFactorName = $"{prefix}basicControl.computer.pidThrottle.iFactor";
        SetValue(pidThrottleIFactorName, basicDroneControl.computer.pidThrottle.iFactor);

        var pidThrottleDFactorName = $"{prefix}basicControl.computer.pidThrottle.dFactor";
        SetValue(pidThrottleDFactorName, basicDroneControl.computer.pidThrottle.dFactor);

        //pid Pitch
        var pidPitchPFactorName = $"{prefix}basicControl.computer.pidPitch.pFactor";
        SetValue(pidPitchPFactorName, basicDroneControl.computer.pidPitch.pFactor);

        var pidPitchIFactorName = $"{prefix}basicControl.computer.pidPitch.iFactor";
        SetValue(pidPitchIFactorName, basicDroneControl.computer.pidPitch.iFactor);

        var pidPitchDFactorName = $"{prefix}basicControl.computer.pidPitch.dFactor";
        SetValue(pidPitchDFactorName, basicDroneControl.computer.pidPitch.dFactor);

        //pid Roll
        var pidRollPFactorName = $"{prefix}basicControl.computer.pidRoll.pFactor";
        SetValue(pidRollPFactorName, basicDroneControl.computer.pidRoll.pFactor);

        var pidRollIFactorName = $"{prefix}basicControl.computer.pidRoll.iFactor";
        SetValue(pidRollIFactorName, basicDroneControl.computer.pidRoll.iFactor);

        var pidRollDFactorName = $"{prefix}basicControl.computer.pidRoll.dFactor";
        SetValue(pidRollDFactorName, basicDroneControl.computer.pidRoll.dFactor);

        //motor
        //FL
        prefix = "_drone_motor_fl";
        var motor = basicDroneControl.motors[0];
        SetDefaultMotor(prefix, motor);

        //FR
        prefix = "_drone_motor_fr";
        motor = basicDroneControl.motors[1];
        SetDefaultMotor(prefix, motor);

        //RR
        prefix = "_drone_motor_rr";
        motor = basicDroneControl.motors[2];
        SetDefaultMotor(prefix, motor);

        //RL
        prefix = "_drone_motor_rl";
        motor = basicDroneControl.motors[3];
        SetDefaultMotor(prefix, motor);
        
        void SetDefaultMotor(string motorPrefix, Drone2.Motors.Motor defMotor)
        {
            var upForceName = $"{motorPrefix}motor.upForce";
            SetValue(upForceName, defMotor.upForce);

            var sideForceName = $"{motorPrefix}motor.sideForce";
            SetValue(sideForceName, defMotor.sideForce);

            var powerName = $"{motorPrefix}motor.power";
            SetValue(powerName, defMotor.power);

            var yawFactorName = $"{motorPrefix}motor.yawFactor";
            SetValue(yawFactorName, defMotor.yawFactor);

            var pitchFactorName = $"{motorPrefix}motor.pitchFactor";
            SetValue(pitchFactorName, defMotor.pitchFactor);
            
            var rollFactorName = $"{motorPrefix}motor.rollFactor";
            SetValue(rollFactorName, defMotor.rollFactor);

            var invertDirectionName = $"{motorPrefix}motor.invertDirection";
            SetValue(invertDirectionName, defMotor.invertDirection ? 1f : 0f);
        }

        PlayerPrefs.Save();
    }

    public static void SetSettingsToControl(BasicControl basicControl, Rigidbody rigidbody)
    {
        if (!PlayerPrefs.HasKey(FirstLoadStr)) return;
     
        var prefix = "_drone_";

        // rigidbody
        var rigidbodyMass = $"{prefix}rigidbody.mass";
        rigidbody.mass = GetValue(rigidbodyMass);

        var rigidbodyDrag = $"{prefix}rigidbody.drag";
        rigidbody.drag = GetValue(rigidbodyDrag);

        var rigidbodyAngularDrag = $"{prefix}rigidbody.angularDrag";
        rigidbody.angularDrag = GetValue(rigidbodyAngularDrag);

        //computer
        var pitchLimitName = $"{prefix}basicControl.computer.pitchLimit";
        basicControl.computer.pitchLimit = GetValue(pitchLimitName);

        var rollLimitName = $"{prefix}basicControl.computer.rollLimit";
        basicControl.computer.rollLimit = GetValue(rollLimitName);

        //pid Throttle
        var pidThrottlePFactorName = $"{prefix}basicControl.computer.pidThrottle.pFactor";
        basicControl.computer.pidThrottle.pFactor = GetValue(pidThrottlePFactorName);

        var pidThrottleIFactorName = $"{prefix}basicControl.computer.pidThrottle.iFactor";
        basicControl.computer.pidThrottle.iFactor = GetValue(pidThrottleIFactorName);

        var pidThrottleDFactorName = $"{prefix}basicControl.computer.pidThrottle.dFactor";
        basicControl.computer.pidThrottle.dFactor = GetValue(pidThrottleDFactorName);

        //pid Pitch
        var pidPitchPFactorName = $"{prefix}basicControl.computer.pidPitch.pFactor";
        basicControl.computer.pidPitch.pFactor = GetValue(pidPitchPFactorName);

        var pidPitchIFactorName = $"{prefix}basicControl.computer.pidPitch.iFactor";
        basicControl.computer.pidPitch.iFactor = GetValue(pidPitchIFactorName);

        var pidPitchDFactorName = $"{prefix}basicControl.computer.pidPitch.dFactor";
        basicControl.computer.pidPitch.dFactor = GetValue(pidPitchDFactorName);

        //pid Roll
        var pidRollPFactorName = $"{prefix}basicControl.computer.pidRoll.pFactor";
        basicControl.computer.pidRoll.pFactor = GetValue(pidRollPFactorName);

        var pidRollIFactorName = $"{prefix}basicControl.computer.pidRoll.iFactor";
        basicControl.computer.pidRoll.iFactor = GetValue(pidRollIFactorName);

        var pidRollDFactorName = $"{prefix}basicControl.computer.pidRoll.dFactor";
        basicControl.computer.pidRoll.dFactor = GetValue(pidRollDFactorName);

        //motor
        //FL
        prefix = "_drone_motor_fl";
        var motor = basicControl.motors[0];
        SetMotorValues(prefix, motor);

        //FR
        prefix = "_drone_motor_fr";
        motor = basicControl.motors[1];
        SetMotorValues(prefix, motor);

        //RR
        prefix = "_drone_motor_rr";
        motor = basicControl.motors[2];
        SetMotorValues(prefix, motor);

        //RL
        prefix = "_drone_motor_rl";
        motor = basicControl.motors[3];
        SetMotorValues(prefix, motor);
    }

    private static void SetMotorValues(string prefix, Drone2.Motors.Motor motor)
    {
        var upForceName = $"{prefix}motor.upForce";
        motor.upForce = GetValue(upForceName);

        var sideForceName = $"{prefix}motor.sideForce";
        motor.sideForce = GetValue(sideForceName);

        var powerName = $"{prefix}motor.power";
        motor.power = GetValue(powerName);

        var yawFactorName = $"{prefix}motor.yawFactor";
        motor.yawFactor = GetValue(yawFactorName);

        var pitchFactorName = $"{prefix}motor.pitchFactor";
        motor.pitchFactor = GetValue(pitchFactorName);
        
        var rollFactorName = $"{prefix}motor.rollFactor";
        SetValue(rollFactorName, motor.rollFactor);

        var invertDirectionName = $"{prefix}motor.invertDirection";
        motor.invertDirection = GetValue(invertDirectionName) > 0.5f;
    }

    public static void MakeDefaultSettings()
    {
    }

    private static float GetValue(string valueName)
    {
        return PlayerPrefs.GetFloat(valueName);
    }

    private static void SetValue(string valueName, float value)
    {
        PlayerPrefs.SetFloat(valueName, value);
    }

    public void SetDefaultValuesButtonClick()
    {
        SetDefaultValues();
        LoadValues();
    }
}