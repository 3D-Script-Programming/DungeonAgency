using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    private Animator animator;

    private bool isDead;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead) return;
    }

    public void Die() {
        animator.SetTrigger("Die");
        isDead = true;
    }

    public void GetHit(int damage) {
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
