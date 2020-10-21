using Player;
using UnityEngine;

namespace Utility
{
    [RequireComponent(typeof(Rigidbody))]
    public class DefaultGetShot : MonoBehaviour, IShootTarget
    {
        [SerializeField] private float shootPower = 20f;
        
        private Rigidbody _rb;
        private Transform _transform;

        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
            _transform = GetComponent<Transform>();
        }

        public void GetShot(Vector3 shotFrom, Vector3 shotPoint)
        {
            _rb.velocity = (_transform.position - shotFrom).normalized * shootPower;
        }
    }
}