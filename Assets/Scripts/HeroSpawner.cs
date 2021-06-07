using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroSpawner : MonoBehaviour
{
    public int locationNumber; // 012 전열 345 후열 
    void Start()
    {
        GameObject unit = CharacterFactory.CreateHero().Prefab;
        Instantiate(unit, transform.position, Quaternion.Euler(0, 180, 0));
    }
}
