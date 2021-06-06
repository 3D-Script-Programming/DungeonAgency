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

    private void Die() {
        animator.SetTrigger("Die");
        isDead = true;
    }

    public void GetHit(int damage) {
        animator.SetTrigger("GetHit");
    }
    
    public void Victory() {
        animator.SetBool("Victory", true);
    }
}
