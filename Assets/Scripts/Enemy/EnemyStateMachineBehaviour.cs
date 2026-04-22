using UnityEngine;

public class EnemyStateMachineBehaviour : StateMachineBehaviour
{
    protected Enemy enemy;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemy = animator.gameObject.GetComponent<Enemy>();
        if (enemy == null)
        {
            Debug.LogError("Enemy is NULL");
            return;
        }

        if (enemy.Agent == null)
        {
            Debug.LogError("Enemy.Agent is NULL");
            return;
        }
    }
}