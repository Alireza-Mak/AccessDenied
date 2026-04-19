using UnityEngine;

public class EnemyAtackState : EnemyStateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        enemy.Agent.SetDestination(enemy.transform.position);
    }
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemy.transform.LookAt(enemy.Player.transform);

        if (enemy.GetDistanceFromPlayer() > enemy.AttackRange)
        {
            animator.SetTrigger("chase");
        }
    }
}
