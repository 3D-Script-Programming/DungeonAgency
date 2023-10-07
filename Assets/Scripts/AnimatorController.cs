using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorController
{
    private Animator animator;

    public AnimatorController(Animator animator)
    {
        this.animator = animator;
    }

    public void Die()
    {
        animator.SetTrigger("Die");
    }

    public void GetHit()
    {
        animator.SetTrigger("GetHit");
    }

    public void Victory()
    {
        animator.SetTrigger("Victory");
    }

    public void MoveFoward()
    {
        animator.SetTrigger("Forward");
    }

    public void Attack()
    {
        animator.SetTrigger("Attack");
    }

    public void Critical()
    {
        animator.SetTrigger("Critical");
    }

    public void MoveBackward()
    {
        animator.SetTrigger("Backward");
    }

    public void StopMove()
    {
        animator.SetTrigger("Idle");
    }
}
