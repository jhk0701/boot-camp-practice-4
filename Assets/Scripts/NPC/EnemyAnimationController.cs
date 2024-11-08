using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EnemyAnimationController : MonoBehaviour
{
    readonly int move = Animator.StringToHash("Moving");
    readonly int attack = Animator.StringToHash("Attack");

    Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void SetSpeed(float speed)
    {
        animator.speed = speed;
    }

    public void SetMove(bool isMove)
    {
        animator.SetBool(move, isMove);
    }

    public void SetAttack()
    {
        animator.SetTrigger(attack);   
    }
}
