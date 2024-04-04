using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField]
    private float speed = 17;

    [SerializeField]
    private List<Vector3> points;

    [SerializeField]
    private bool backMove;
    
    [SerializeField]
    private float sphereSize;

    private readonly Vector3 _rotationRight = new(0, 30, 0);
    private readonly Vector3 _rotationLeft = new(0, -30, 0);

    private Rigidbody _rb;
    private Vector3 _currentPoint;
    private CancellationTokenSource _moveTokenSource;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _moveTokenSource?.Cancel();
        _moveTokenSource = new CancellationTokenSource();
        MoveLine(_moveTokenSource.Token).Forget();
    }

    private void OnDestroy()
    {
        _moveTokenSource?.Cancel();
    }

    // private void FixedUpdate()
    // {
    //     if (Input.GetKey("u"))
    //     {
    //         transform.Translate(Vector3.forward * speed * Time.deltaTime);
    //     }
    //
    //     if (Input.GetKey("j"))
    //     {
    //         transform.Translate(Vector3.back * speed * Time.deltaTime);
    //     }
    //
    //     if (Input.GetKey("k"))
    //     {
    //         var deltaRotationRight = Quaternion.Euler(_rotationRight * Time.deltaTime);
    //         _rb.MoveRotation(_rb.rotation * deltaRotationRight);
    //     }
    //
    //     if (Input.GetKey("h"))
    //     {
    //         var deltaRotationLeft = Quaternion.Euler(_rotationLeft * Time.deltaTime);
    //         _rb.MoveRotation(_rb.rotation * deltaRotationLeft);
    //     }
    // }

    // private IEnumerator MoveCircle()
    // {
    //     while (true)
    //     {
    //         transform.Translate(Vector3.forward * speed * Time.deltaTime);
    //         var deltaRotationLeft = Quaternion.Euler(_rotationLeft * Time.deltaTime);
    //         _rb.MoveRotation(_rb.rotation * deltaRotationLeft);
    //         yield return null;
    //     }
    // }

    public async UniTask MoveLine(CancellationToken cToken)
    {
        if (points.Count < 2) return;

        transform.position = points[0];
        var curPointIndex = 1;
        _currentPoint = points[curPointIndex];
        var damping = 6.0f;
        while (!cToken.IsCancellationRequested)
        {
            if (IsOnPointPosition(transform.position, _currentPoint))
            {
                curPointIndex++;
                if (curPointIndex == points.Count)
                {
                    if (backMove)
                    {
                        points.Reverse();
                        curPointIndex = 1;
                    }
                    else
                    {
                        curPointIndex = 0;
                    }
                }

                _currentPoint = points[curPointIndex];
            }

            var rotation = Quaternion.LookRotation(_currentPoint - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
            await UniTask.Yield();
        }
    }

    private bool IsOnPointPosition(Vector3 elementPosition, Vector3 pointPosition) =>
        Mathf.Abs(elementPosition.x - pointPosition.x) < 3 &&
        Mathf.Abs(elementPosition.y - pointPosition.y) < 3 &&
        Mathf.Abs(elementPosition.z - pointPosition.z) < 3;


    [ContextMenu("Point From Current Position")]
    public void AddPointToTarget()
    {
        points.Add(transform.position);
    }

    private void OnDrawGizmos()
    {
        if (sphereSize > 5f)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(transform.position, sphereSize);
        }
        else
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLineStrip(points.ToArray(), !backMove);
            for (var i = 0; i < points.Count; i++)
            {
                var point = points[i];
                Gizmos.DrawSphere(point, 3f);
#if UNITY_EDITOR
                var labelPoint = new Vector3(point.x, point.y + 5f, point.z);
                UnityEditor.Handles.Label(labelPoint, $"P:{i}");
#endif
            }
        }
    }
}