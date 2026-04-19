using UnityEngine;

public class EnemyChaseState : EnemyStateMachineBehaviour
{
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemy.Agent.SetDestination(enemy.Player.transform.position);
        if (enemy.GetDistanceFromPlayer() <= enemy.AttackRange)
        {
            animator.SetTrigger("attack");
        }
        else if (enemy.GetDistanceFromPlayer() > enemy.ChaseRange)
        {
            animator.SetTrigger("idle");
        }
    }
}
