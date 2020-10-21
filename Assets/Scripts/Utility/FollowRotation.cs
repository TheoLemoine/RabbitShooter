using System;
using UnityEngine;

namespace Utility
{
    public class FollowRotation : MonoBehaviour
    {
        [SerializeField] private Transform followTarget;
        [SerializeField] private Vector3 eulerOffset;
        private Transform _transform;
        private Quaternion _quaternionOffset;

        private void Start()
        {
            _transform = GetComponent<Transform>();
            _quaternionOffset = Quaternion.Euler(eulerOffset);
        }

        private void Update()
        {
            _transform.rotation = followTarget.rotation * _quaternionOffset;
        }
    }
}