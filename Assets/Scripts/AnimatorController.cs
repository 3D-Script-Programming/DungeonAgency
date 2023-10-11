using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorController
{
    private Animator animator;

    // 생성자: AnimatorController 클래스의 인스턴스를 초기화할 때 호출됨
    public AnimatorController(Animator animator)
    {
        this.animator = animator;
    }

    // Die 애니메이션을 재생하는 메서드
    public void Die()
    {
        animator.SetTrigger("Die");
    }

    // GetHit 애니메이션을 재생하는 메서드
    public void GetHit()
    {
        animator.SetTrigger("GetHit");
    }

    // Victory 애니메이션을 재생하는 메서드
    public void Victory()
    {
        animator.SetTrigger("Victory");
    }

    // Forward 애니메이션을 재생하는 메서드
    public void MoveForward()
    {
        animator.SetTrigger("Forward");
    }

    // Attack 애니메이션을 재생하는 메서드
    public void Attack()
    {
        animator.SetTrigger("Attack");
    }

    // Critical 애니메이션을 재생하는 메서드
    public void Critical()
    {
        animator.SetTrigger("Critical");
    }

    // Backward 애니메이션을 재생하는 메서드
    public void MoveBackward()
    {
        animator.SetTrigger("Backward");
    }

    // Idle 애니메이션을 재생하는 메서드
    public void StopMove()
    {
        animator.SetTrigger("Idle");
    }
}
