using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonBattleMonsterController : MonoBehaviour
{
    public GameObject slider;

    void OnEnable()
    {
        slider.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
