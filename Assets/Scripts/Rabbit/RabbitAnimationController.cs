using UnityEngine;
using UnityEngine.AI;

namespace Rabbit
{
    public class RabbitAnimationController : MonoBehaviour
    {

        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private Animator animator;

        private static readonly int IsMovingID = Animator.StringToHash("isMoving");

        private void Update()
        {
            animator.SetBool(IsMovingID, agent.remainingDistance > agent.stoppingDistance);
        }

        // animation event, stopping agent to prevent sliding
        private void OnLanding()
        {
            agent.isStopped = true;
        }

        private void OnTakingOff()
        {
            agent.isStopped = false;
        }

    }
}
