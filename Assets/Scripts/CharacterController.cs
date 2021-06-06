using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Die() {
        animator.SetTrigger("Die");
    }

    public void GetHit() {
        animator.SetTrigger("GetHit");
    }
    
    public void Victory() {
        animator.SetTrigger("Victory");
    }

    public void MoveFoward() {
        animator.SetTrigger("Foward");
    }

    public void Attack() {
        animator.SetTrigger("Attack");
    }

    public void MoveBackward() {
        animator.SetTrigger("Backward");
    }

    public void StopMove() {
        animator.SetTrigger("Idle");
    }
}
