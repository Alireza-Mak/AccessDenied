using UnityEngine;

public class EnemyStateMachineBehaviour : StateMachineBehaviour
{
    protected Enemy enemy;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemy = animator.gameObject.GetComponent<Enemy>();
    }
}