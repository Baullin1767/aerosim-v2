using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;

namespace Drone2
{
    public class DroneMoveHelper : MonoBehaviour
    {
        [SerializeField]
        private Transform drone;

        [SerializeField]
        private float moveSpeed = 1f;
        
        [SerializeField]
        private float rotationSpeed = 1f;

        [SerializeField]
        private float epsilon = 0.1f;

        [SerializeField]
        private float yOffset = 1.5f;

        [SerializeField]
        private List<Vector3> points;

        [SerializeField]
        private LineRenderer lineRenderer;

        private CancellationTokenSource _moveTs;

        [ContextMenu("Get Points From Line Renderer")]
        private void GetPointsFromLineRenderer()
        {
            points = new List<Vector3>();
            for (var i = 0; i < lineRenderer.positionCount; i++)
            {
                var point = lineRenderer.GetPosition(i);
                points.Add(point);
            }
        }

        [ContextMenu("Play")]
        private void Play()
        {
            if (points.Count < 1) return;
            PlayAsync().Forget();
        }

        [ContextMenu("Stop")]
        private void Stop()
        {
            _moveTs?.Cancel();
        }

        private async UniTask PlayAsync()
        {
            var currentPosPoints = 0;
            var targetPosition = points[currentPosPoints];
            _moveTs = new CancellationTokenSource();
            while (!_moveTs.IsCancellationRequested && drone != null)
            {
                var nextPosition = new Vector3(targetPosition.x, targetPosition.y + yOffset, targetPosition.z);
                if (Near(drone.position, nextPosition, epsilon))
                {
                    currentPosPoints++;
                    if (currentPosPoints >= points.Count) currentPosPoints = 0;
                    targetPosition = points[currentPosPoints];
                    nextPosition = new Vector3(targetPosition.x, targetPosition.y + yOffset, targetPosition.z);
                }

                // var rotation = Vector3.RotateTowards(drone.position, targetPosition,
                //     100f, 0f);
                // drone.eulerAngles = rotation;
                // var step = moveSpeed * Time.deltaTime;
                // drone.position = Vector3.MoveTowards(drone.position, targetPosition, step);

                var dir = (nextPosition - drone.position).normalized;
                //transform.Translate(dir * moveSpeed * Time.deltaTime);
                drone.position += dir * moveSpeed * Time.deltaTime;

                var rotateStep = rotationSpeed * Time.deltaTime;
                var newDirection = Vector3.RotateTowards(transform.forward, dir,
                    rotateStep, 0.0f);
                drone.rotation = Quaternion.LookRotation(newDirection);

                await UniTask.Yield(PlayerLoopTiming.FixedUpdate);
            }
        }

        private bool Near(Vector3 from, Vector3 point, float eps)
        {
            if (Mathf.Abs(from.x - point.x) < eps &&
                Mathf.Abs(from.y - point.y) < eps &&
                Mathf.Abs(from.z - point.z) < eps)
            {
                return true;
            }

            return false;
        }

        private void OnValidate()
        {
            drone = transform;
            lineRenderer = FindObjectOfType<LineRenderer>();
            if (lineRenderer != null)
            {
                GetPointsFromLineRenderer();
            }
        }
    }
}