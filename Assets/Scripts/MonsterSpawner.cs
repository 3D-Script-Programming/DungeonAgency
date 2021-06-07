using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public GameObject unit;

    void Start()
    {
        Instantiate(unit, transform.position, Quaternion.identity);
    }
}
