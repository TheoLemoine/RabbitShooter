using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Rabbit
{
    /*
     * not really an AI, the rabbit just wanders around randomly
     */
    public class RabbitAI : MonoBehaviour
    {
    
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private float newTargetRange = 5f;
    
        private void Start()
        {
            StartCoroutine(NewTargetAfterRandomTime());
        }

        private IEnumerator NewTargetAfterRandomTime()
        {
            for /* ever */ (;;)
            {
                if (agent.isOnNavMesh)
                {
                    var randomCircle = Random.insideUnitCircle;
                    agent.SetDestination(transform.TransformPoint(new Vector3(randomCircle.x, 0, randomCircle.y) * newTargetRange));
                }
        
                yield return new WaitForSeconds(Random.Range(3f, 5f));
            }
        }
    }
}
