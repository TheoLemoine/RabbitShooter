using System.Collections.Generic;
using Player;
using UnityEngine;
using UnityEngine.AI;
using Utility;

namespace Rabbit
{
    public class RabbitGetShot : MonoBehaviour, IShootTarget
    {
        [SerializeField] private GameObject disassembleFrom;
        [SerializeField] private List<Behaviour> toDisable;
        [SerializeField] private float explodePower = 20f;

        public void GetShot(Vector3 shotFrom, Vector3 shotPoint)
        {
            foreach (var behaviour in toDisable)
            {
                behaviour.enabled = false;
            }
            
            foreach (var col in disassembleFrom.GetComponentsInChildren<Collider>())
            {
                var go = col.gameObject;
                
                if (!col.gameObject.TryGetComponent<Rigidbody>(out var _))
                {
                    // generating nav mesh obstacle box from collider box
                    if (col is BoxCollider boxCol)
                    {
                        var obst = go.AddComponent<NavMeshObstacle>();
                        obst.center = boxCol.center;
                        obst.size = boxCol.size;
                        obst.carving = true;
                        obst.carveOnlyStationary = true;
                        obst.carvingTimeToStationary = 1f;
                    }
                    
                    var rb = go.AddComponent<Rigidbody>();
                    rb.velocity = (go.transform.position - shotFrom).normalized * explodePower;
                    
                    // next time a piece is shot, it just flies away
                    go.AddComponent<DefaultGetShot>();
                }
            }
            
            // remove component, not object
            Destroy(this);
        }
    }
}