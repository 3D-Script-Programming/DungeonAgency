using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroSpawner : MonoBehaviour
{
    public GameObject unit;

    void Start()
    {
        Instantiate(unit, transform.position, Quaternion.Euler(0, 180, 0));
    }
}
