using UnityEngine;

public class EnemyPatrolState : EnemyStateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        enemy.SetNextWaypoint();
        enemy.Agent.SetDestination(enemy.GetCurrentWaypoint());
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (enemy.Agent.remainingDistance <= enemy.Agent.stoppingDistance)
        {
            animator.SetTrigger("idle");
        }
        else if (enemy.GetDistanceFromPlayer() < enemy.ChaseRange)
        {
            animator.SetTrigger("chase");
        }
    }
}
