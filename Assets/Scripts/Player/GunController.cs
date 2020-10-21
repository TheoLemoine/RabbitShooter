using System;
using UnityEngine;
using UnityEngine.VFX;

namespace Player
{
    public class GunController : MonoBehaviour
    {
        [SerializeField] private GameObject explosionPrefab;
        [SerializeField] private GameObject impactPrefab;
        [SerializeField] private Animator animator;
        [SerializeField] private Transform shootOrigin;
        [SerializeField] private Transform explosionOrigin;
        [SerializeField] private LayerMask shootLayer;
        [SerializeField] private float shootCoolDown;
        private float _currentShootCoolDown = 0f;
        
        private static readonly int ShootID = Animator.StringToHash("Shoot");
        private Rigidbody _referringRb;

        private void Start()
        {
            _referringRb = GetComponentInParent<Rigidbody>();
        }

        private void Update()
        {
            if (Input.GetButton("Fire1") && _currentShootCoolDown < 0f)
            {
                _currentShootCoolDown = shootCoolDown;
                
                // boom effect
                var explosion = Instantiate(explosionPrefab, explosionOrigin.position, explosionOrigin.rotation);
                var vfx = explosion.GetComponent<VisualEffect>();
                vfx.SetVector3("BaseSpeed", vfx.transform.InverseTransformDirection(_referringRb.velocity));
                
                // start anim
                animator.SetTrigger(ShootID);
                
                if (Physics.Raycast(shootOrigin.position, shootOrigin.forward, out var ray, Mathf.Infinity, shootLayer.value))
                {
                    if (ray.collider.TryGetComponent(out IShootTarget target))
                    {
                        target.GetShot(shootOrigin.position, ray.point);
                    }

                    // impact on the ground (show the player where hit landed)
                    Instantiate(impactPrefab, ray.point, Quaternion.FromToRotation(Vector3.forward, ray.normal));
                }
            }
            else
            {
                animator.ResetTrigger(ShootID);
            }

            _currentShootCoolDown -= Time.deltaTime;
        }
    }
}