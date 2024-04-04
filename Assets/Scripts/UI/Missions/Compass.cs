using System;
using Drone2;
using UnityEngine;

namespace UI.Missions
{
    public class Compass : MonoBehaviour
    {
        [SerializeField]
        private GameObject target;

        [SerializeField]
        private Vector3 startPosition;

        [SerializeField]
        private RectTransform compassRectTransform;

        [SerializeField]
        private float width = 720f;

        [SerializeField]
        private Vector3 forward = Vector3.forward;
        
        private void Start()
        {
            target = FindObjectOfType<DroneBridge>().gameObject;
        }

        private void Update()
        {
            var rationAngleToPixel = width / 360f;
            var targetForward = target.transform.forward;
            var forwardXZ = new Vector3(targetForward.x, 0f, targetForward.z);
            var perp = Vector3.Cross(forward, forwardXZ);
            var dir = Vector3.Dot(perp, Vector3.up);
            var angel = Vector3.Angle(forwardXZ, forward);
            var newX = angel * Mathf.Sign(dir) * rationAngleToPixel;
            compassRectTransform.localPosition = startPosition + (new Vector3(-newX, 0, 0));
            // Debug.Log($"p: {compassRectTransform.localPosition.x} nX: {newX} " +
            //           $"perp: {perp} dir: {dir} a: {angel} aa: {rationAngleToPixel} " +
            //           $" forward: {target.transform.forward}" +
            //           $" f2: {forwardXZ}");
        }
    }
}

