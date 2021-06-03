using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    private Animator animator;

    private int hp; 
    private bool isDead;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        hp = 1000;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead) return;
    }

    private void Die() {
        isDead = true;
    }

    public void GetHit(int damage) {
        animator.SetTrigger("GetHit");
        hp -= damage;
        animator.SetInteger("HP", hp);
        if (hp < 1) Die();
    }
}
