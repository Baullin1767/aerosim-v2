using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Cargo : MonoBehaviour
{
    [SerializeField]
    private GameObject target;

    [SerializeField]
    private Vector3 cargoSize = Vector3.one;

    [SerializeField]
    private GameObject cargoCamera;

    [SerializeField]
    private Rigidbody droneRigidbody;

    [SerializeField]
    private Vector3 cameraOffset = new (0f, 5f, 0f);
    
    private Rigidbody _rb;
    private Vector3 _prevPos;
    private Vector3 _forward;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.isKinematic = true;
        _rb.detectCollisions = false;
    }

    private void Update()
    {
        if (transform.position.y < -50)
        {
            MissionManager.Fail();
        }

        if (cargoCamera.activeSelf)
        {
            var curPosition = transform.position;
            cargoCamera.transform.position = curPosition + cameraOffset;
        }

        if (!_rb.isKinematic)
        {
            var newVelocity = new Vector3(
                droneRigidbody.velocity.x,
                _rb.velocity.y,
                droneRigidbody.velocity.z);
            _rb.velocity = newVelocity;
        }

        // if (Input.GetKey(KeyCode.B))
        // {
        //     var newVelocity = new Vector3(droneRigidbody.velocity.x, 
        //         droneRigidbody.velocity.y /2, droneRigidbody.velocity.z);
        //     Debug.Log($"B {droneRigidbody.velocity} {newVelocity}");
        //     _rb.velocity = newVelocity;
        // }
    }

    public void Drop()
    {
        transform.parent = null;
        _rb.isKinematic = false;
        _rb.WakeUp();
        _rb.detectCollisions = true;
        transform.rotation = Quaternion.identity;
        transform.localScale = cargoSize;
        cargoCamera.transform.parent = null;
        cargoCamera.transform.eulerAngles = new Vector3(90f, 0f, 0f);
        cargoCamera.SetActive(true);
        gameObject.SetActive(true);
        //TODO: make force
        // speed an move vector
        //_rb.AddForce(droneCamera.forward, ForceMode.Impulse);
        //_rb.AddForce(_forward, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.name.Contains("Drone")) return;
        switch (other.gameObject.name)
        {
            case "target":
                MissionManager.Success();
                return;
            default:
                MissionManager.Fail();
                return;
        }
    }
}