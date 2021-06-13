using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonBattleMonsterController : MonoBehaviour
{
    public GameObject slider;
    public AnimatorController animatorController;

    void OnEnable()
    {
        slider.SetActive(false);
        animatorController = new AnimatorController(GetComponent<Animator>());
        animatorController.Victory();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
