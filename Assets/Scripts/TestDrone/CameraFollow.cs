using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private Transform follow;
    
    [SerializeField]
    private Vector3 offset = new (0, 1f, -1.2f);

    private void Update()
    {
        transform.position = follow.transform.position + offset;
    }
}
