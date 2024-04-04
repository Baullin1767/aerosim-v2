using UnityEngine;

[ExecuteInEditMode]
public class Billboard : MonoBehaviour
{
    private void LateUpdate()
    {
        transform.LookAt(Camera.main.transform);
        transform.Rotate(0, 180, 0);
        var angles = transform.localEulerAngles;
        transform.rotation = Quaternion.Euler(0, angles.y, 0);
    }
}
