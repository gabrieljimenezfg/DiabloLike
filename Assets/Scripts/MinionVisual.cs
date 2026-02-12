using System;
using UnityEngine;

public class MinionVisual : MonoBehaviour
{
    private Animator animator;
    private Minion minion;
    private Vector3 lastPosition;
    //[SerializeField] private float stopMovingDistance;

    private const string AnimatorAttackKey = "Attack";
    private const string AnimatorMovingKey = "isMoving";

    void Start()
    {
        animator = GetComponent<Animator>();
        minion = GetComponentInParent<Minion>();

        minion.IsAttacking += HandleAttackAnimation;
    }

    private void FixedUpdate()
    {
        /*var distanceToPositionSqr = (transform.position - minion.TargetPosition).sqrMagnitude;
        if (distanceToPositionSqr <= stopMovingDistance)
        {
            animator.SetBool(AnimatorMovingKey, false);
        }
        else
        {
            animator.SetBool(AnimatorMovingKey, true);
        }*/
        lastPosition = transform.position;
    }

    private void LateUpdate()
    {
        if (transform.position == lastPosition)
        {
            animator.SetBool(AnimatorMovingKey, false);
        }
        else
        {
            animator.SetBool(AnimatorMovingKey, true);
        }
    }

    private void HandleAttackAnimation(object sender, EventArgs e)
    {
        animator.SetTrigger(AnimatorAttackKey);
    }

    public void AttackDuringAnimation()
    {
        minion.Attack();
    }
}
